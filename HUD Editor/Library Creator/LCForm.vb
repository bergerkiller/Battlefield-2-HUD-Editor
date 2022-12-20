Public Class LCForm
    Dim images(-1) As ImageData

    Public Function CutOffPath(ByVal OldPath As String, Optional ByVal Wdir As String = "") As String
        If Wdir.Trim = "" Then Wdir = Application.StartupPath
        Wdir = Wdir.ToLower.Trim
        If OldPath.ToLower.Contains("objects") Then
            OldPath = OldPath.Remove(0, OldPath.ToLower.IndexOf("objects") + 7).Trim("\")
        ElseIf OldPath.ToLower.Contains("menu") Then
            OldPath = OldPath.Remove(0, OldPath.ToLower.IndexOf("menu") + 4).Trim("\")
            If OldPath.ToLower.StartsWith("hud\texture\") Then OldPath = OldPath.Remove(0, 12)
        ElseIf OldPath.ToLower.Contains("common") Then
            OldPath = OldPath.Remove(0, OldPath.ToLower.IndexOf("common") + 6).Trim("\")
        ElseIf OldPath.ToLower.Contains("fonts") Then
            OldPath = OldPath.Remove(0, OldPath.ToLower.IndexOf("fonts") + 5).Trim("\")
        End If
        OldPath = OldPath.Trim("\")
        If OldPath.ToLower.StartsWith(Wdir) Then OldPath = OldPath.Remove(0, Wdir.Length).Trim("\")
        Return OldPath
    End Function
    Public Function ListFolderFiles(ByVal Path As String, ByVal Filter As String) As String()
        Dim rval As New List(Of String)
        For Each file As String In IO.Directory.GetFiles(Path)
            '--Filter--
            Dim add As Boolean = False
            For Each f As String In Filter.Split("|")
                f = f.ToLower.Trim("*")
                If file.EndsWith(f) Then add = True
                If f = "." Then add = True
            Next
            '--end--
            If add Then rval.Add(file)
        Next
        For Each directory As String In IO.Directory.GetDirectories(Path)
            For Each file As String In ListFolderFiles(directory, Filter)
                rval.Add(file)
            Next
        Next
        Dim frval(rval.Count - 1) As String
        For i As Integer = 0 To rval.Count - 1
            frval(i) = rval(i)
        Next
        Return frval
    End Function

    Public Class ImagePointer
        Sub New(ByVal Path As String, ByVal SourceFile As String, ByVal ImageOffset As Long, ByVal ImageLength As Long)
            Me.Path = Path
            Me.SourceFile = SourceFile
            Me.ImageOffset = ImageOffset
            Me.ImageLength = ImageLength
        End Sub
        Public Path As String = ""
        Public SourceFile As String = ""
        Public ImageOffset As Long = 0
        Public ImageLength As Long = 0
        Public Function GetImage() As Image
            Dim s As New System.IO.FileStream(Me.SourceFile, IO.FileMode.Open)
            Dim imgbytes(Me.ImageLength) As Byte
            s.Position = Me.ImageOffset
            s.Read(imgbytes, 0, Me.ImageLength)
            s.Close()
            Dim ms As New System.IO.MemoryStream(imgbytes, 0, imgbytes.Length)
            ms.Write(imgbytes, 0, imgbytes.Length)
            GetImage = Image.FromStream(ms)
            ms.Close()
        End Function
    End Class
    Public Class ImageData
        Sub New(ByVal Path As String, ByVal Image As Image)
            Me.Image = Image
            Me.Path = Path
        End Sub
        Public Path As String = ""
        Private _Image As Image
        Public Property Image() As Image
            Get
                Return Me._Image
            End Get
            Set(ByVal value As Image)
                Me._Image = value
                Dim ms As New System.IO.MemoryStream()
                value.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                Me.imageBytes = ms.ToArray
                ms.Close()
            End Set
        End Property
        Public imageBytes() As Byte
        Public Function GetHeader() As String
            Return Me.imageBytes.Length & "|" & Me.Path
        End Function
    End Class
    Public Sub SaveImageData(ByVal DestFile As String, ByVal Images() As ImageData)
        If System.IO.File.Exists(DestFile) Then
            SetAttr(DestFile, FileAttribute.Normal)
            System.IO.File.Delete(DestFile)
        End If
        Dim s As New System.IO.FileStream(DestFile, System.IO.FileMode.OpenOrCreate)
        Dim headerstring As String = ""
        For Each img As ImageData In Images
            headerstring &= img.GetHeader() & vbCrLf
        Next
        headerstring &= "==================================================================="
        Dim header() As Byte = New System.Text.ASCIIEncoding().GetBytes(headerstring)
        s.Write(header, 0, header.Length)
        For Each img As ImageData In Images
            s.Write(img.imageBytes, 0, img.imageBytes.Length)
        Next
        s.Close()
    End Sub
    Public Function LoadImageData(ByVal SourceFile As String) As ImagePointer()
        Dim s As New System.IO.FileStream(SourceFile, System.IO.FileMode.Open)
        Dim header As String = ""
        Dim hid As String = "==================================================================="
        Dim libdata(s.Length) As Byte
        s.Read(libdata, 0, s.Length)
        header = New System.Text.ASCIIEncoding().GetString(libdata)
        header = header.Substring(0, header.IndexOf(hid) + hid.Length)

        'Do While s.Position < s.Length
        '    Dim b() As Byte = {s.ReadByte}
        '    header &= New System.Text.ASCIIEncoding().GetString(b)
        '    If header.EndsWith("===================================================================") Then Exit Do
        'Loop
        s.Close()
        Dim posoffset As Long = header.Length
        Dim img(-1) As ImagePointer
        For Each line As String In header.Split(vbCrLf)
            If line.Split("|").Count = 2 Then
                Dim bytelength As Long = line.Split("|")(0)
                Dim path As String = line.Split("|")(1)
                ReDim Preserve img(img.Count)
                img(img.Count - 1) = New ImagePointer(path, SourceFile, posoffset, bytelength)
                posoffset += bytelength
            End If
        Next
        Return img
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If System.IO.Directory.Exists(Application.StartupPath & "\Textures") Then SaveFileDialog1.InitialDirectory = Application.StartupPath & "\Textures"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            LoadPaths.Clear()
            LoadPaths.Add(SaveFileDialog1.FileName)
            LoadThread = New Threading.Thread(AddressOf SaveData)
            LoadThread.IsBackground = True
            LoadThread.Start()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        OpenFileDialog1.Title = "Select an image"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim img As Image = Nothing
            Try
                img = FreeImageAPI.FreeImage.LoadBitmap(OpenFileDialog1.FileName, 0, -1)
            Catch
            End Try
            Try
                If IsNothing(img) Then img = Image.FromFile(OpenFileDialog1.FileName)
            Catch
            End Try
            If IsNothing(img) Then
                MsgBox("Failed to load image.", MsgBoxStyle.Critical)
            Else
                AddItem(New ImageData(CutOffPath(OpenFileDialog1.FileName), img))
            End If
        End If
    End Sub
    Private Sub ListView1_AfterLabelEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LabelEditEventArgs) Handles ListView1.AfterLabelEdit
        Try
            If e.Label.Trim = "" Then
                e.CancelEdit = True

            ElseIf e.Label.Contains("|") Then
                MsgBox("Invalid character: " & Chr(34) & "|" & Chr(34), MsgBoxStyle.Information)
                e.CancelEdit = True
            ElseIf e.Label.Contains("===================================================================") Then
                MsgBox("Internal header type error; you can't use an header identifier in a path name.", MsgBoxStyle.Information)
                e.CancelEdit = True

            ElseIf e.Label.Trim.ToLower <> ListView1.Items(e.Item).Text.Trim.ToLower Then
                For Each img As ImageData In images
                    If img.Path.ToLower.Trim = e.Label.ToLower.Trim Then
                        MsgBox(e.Label & " already exists.", MsgBoxStyle.Information)
                        e.CancelEdit = True
                        Exit For
                    End If
                Next
            End If
        Catch
            e.CancelEdit = True
        End Try
        If e.CancelEdit = False Then images(ListView1.SelectedIndices(0)).Path = e.Label
    End Sub
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 0 Then
            PictureBox1.Image = Nothing
        Else
            PictureBox1.Image = images(ListView1.SelectedIndices(0)).Image
            If PictureBox1.Image.Width > PictureBox1.Width Or PictureBox1.Image.Height > PictureBox1.Height Then
                PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
            Else
                PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
            End If
        End If
        RemoveImageToolStripMenuItem.Enabled = ListView1.SelectedItems.Count <> 0
        Button4.Enabled = ListView1.SelectedItems.Count <> 0
        EditPathToolStripMenuItem.Enabled = ListView1.SelectedItems.Count = 1
        If ListView1.SelectedItems.Count = 1 Then
            RemoveImageToolStripMenuItem.Text = "Remove image"
            Button4.Text = "Remove image"
        ElseIf ListView1.SelectedItems.Count > 1 Then
            RemoveImageToolStripMenuItem.Text = "Remove images"
            Button4.Text = "Remove images"
        End If
    End Sub
    Private Sub LCForm_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub
    Private Sub LCForm_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop
        dragdata = e.Data.GetData(DataFormats.FileDrop)
        LoadThread = New Threading.Thread(AddressOf LoadDragged)
        LoadThread.IsBackground = True
        LoadThread.Start()
    End Sub
    Dim dragdata() As String
    Private Sub LoadDragged()
        ProgressbarAdapter(-1, "Indexing...")
        Dim files As New List(Of String)
        Dim parts As New List(Of String)
        LoadPaths.Clear()
        For Each part As String In dragdata
            If System.IO.File.Exists(part) Then files.Add(part)
            If System.IO.Directory.Exists(part) Then
                parts.Add(part.ToLower)
                For Each file As String In ListFolderFiles(part, "*.*")
                    files.Add(file)
                Next
            End If
        Next
        Dim i As Integer = 0
        For Each File As String In files
            ProgressbarAdapter(i * 100 / files.Count, "loading: " & File & "...")
            Dim part As String = ""
            For Each p As String In parts
                If File.ToLower.StartsWith(p) Then part = p
            Next
            If File.ToLower.EndsWith(".dds") Or File.ToLower.EndsWith(".tga") Then
                Try
                    Dim img As Image = FreeImageAPI.FreeImage.GetBitmap(FreeImageAPI.FreeImage.LoadEx(File))
                    AddItem(New ImageData(CutOffPath(File, part), img))
                Catch
                End Try
            ElseIf File.ToLower.EndsWith(".texlib") Then
                LoadPaths.Add(File)
            End If
            i += 1
        Next
        If LoadPaths.Count <> 0 Then
            LoadData()
        End If
        ProgressbarAdapter(100, "Loading finished.")
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If System.IO.Directory.Exists(Application.StartupPath & "\Textures") Then OpenFileDialog2.InitialDirectory = Application.StartupPath & "\Textures"
        If OpenFileDialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
            LoadPaths.Clear()
            LoadPaths.Add(OpenFileDialog2.FileName)
            LoadThread = New Threading.Thread(AddressOf LoadData)
            LoadThread.IsBackground = True
            LoadThread.Start()
        End If
    End Sub

    Dim LoadPaths As New List(Of String)
    Private Sub LoadData()
        For Each LoadPath As String In LoadPaths
            ProgressbarAdapter(-1, "Indexing...")
            Dim data() As ImagePointer = LoadImageData(LoadPath)
            Dim existingitems As New List(Of String)
            For Each im As ImageData In images
                existingitems.Add(im.Path.ToLower.Trim)
                If AbortProcess = True Then Exit For
            Next
            Dim i As Integer = 0
            Dim aexisting As New List(Of ImageData)
            For Each part As ImagePointer In data
                Try
                    ProgressbarAdapter(i * 50 / data.Count, "Loading: " & part.Path & "...")
                    If existingitems.Contains(part.Path.ToLower.Trim) Then
                        aexisting.Add(New ImageData(part.Path, part.GetImage))
                        i += 1
                    Else
                        AddItem(New ImageData(part.Path, part.GetImage))
                        i += 2
                    End If
                Catch
                End Try
                If AbortProcess = True Then Exit For
            Next
            'Adding existing stuff
            Dim addmode As Integer = 0
            Dim forall As Boolean = False
            '0 = dont add; use old
            '1 = replace old
            '2 = rename path
            Dim ri As Integer = aexisting.Count - 1
            For Each img As ImageData In aexisting
                ProgressbarAdapter(i * 50 / data.Count, "Processing: " & img.Path)
                For Each oimg As ImageData In images
                    If img.Path.ToLower.Trim = oimg.Path.ToLower.Trim Then
                        If forall = False Then addmode = AskReplace(oimg, img, ri)
                        If addmode > 2 Then
                            forall = True
                            addmode -= 3
                        End If
                        If addmode = 1 Then
                            'replace old
                            oimg = img
                        ElseIf addmode = 2 Then
                            'rename path
                            Dim ind As Integer = 1
                            Dim p1 As String = img.Path
                            Dim p2 As String = "." & img.Path.Split(".").Last
                            p1 = p1.Substring(0, p1.Length - p2.Length)
                            If Not img.Path.Contains(".") Then p1 = img.Path
                            Do While existingitems.Contains(img.Path.ToLower.Trim)
                                img.Path = p1 & "(" & ind & ")" & p2
                                ind += 1
                            Loop
                            AddItem(img)
                        End If
                        Exit For
                    End If
                Next
                ri -= 1
                i += 1
                If AbortProcess = True Then Exit For
            Next
            If AbortProcess = True Then Exit For
        Next
        ProgressbarAdapter(100, "Loading finished.")
        AbortProcess = False
    End Sub
    Private Sub SaveData()
        ProgressbarAdapter(-1, "Writing header...")
        If System.IO.File.Exists(LoadPaths(0)) Then
            SetAttr(LoadPaths(0), FileAttribute.Normal)
            System.IO.File.Delete(LoadPaths(0))
        End If
        Dim s As New System.IO.FileStream(LoadPaths(0), System.IO.FileMode.OpenOrCreate)
        Dim headerstring As String = ""
        For Each img As ImageData In images
            headerstring &= img.GetHeader() & vbCrLf
            If AbortProcess = True Then Exit For
        Next
        headerstring &= "==================================================================="
        If AbortProcess = False Then
            Dim header() As Byte = New System.Text.ASCIIEncoding().GetBytes(headerstring)
            s.Write(header, 0, header.Length)
            Dim i As Integer = 0
            For Each img As ImageData In images
                ProgressbarAdapter(i * 100 / images.Count, "Writing: " & img.Path & "...")
                s.Write(img.imageBytes, 0, img.imageBytes.Length)
                i += 1
                If AbortProcess = True Then Exit For
            Next
        End If
        s.Close()
        If AbortProcess = True And System.IO.File.Exists(LoadPaths(0)) Then
            SetAttr(LoadPaths(0), FileAttribute.Normal)
            System.IO.File.Delete(LoadPaths(0))
        End If
        ProgressbarAdapter(100, "Saved.")
        AbortProcess = False
    End Sub
    Private Sub LoadAtlas()
        Try
            ProgressbarAdapter(-1, "Indexing...")
            Dim existingitems As New List(Of String)
            For Each im As ImageData In images
                existingitems.Add(im.Path.ToLower.Trim)
                If AbortProcess = True Then Exit For
            Next
            Dim r As New System.IO.StreamReader(LoadPaths(0))
            Dim data As String = RemoveDoubleSpaces(r.ReadToEnd)
            r.Close()
            Dim AT As New List(Of AtlasTexture)
            Dim AtlasPaths As New List(Of String)
            For Each line As String In data.Split(vbCrLf)
                line = line.Trim
                If Not line.StartsWith("#") And line.Split(" ").Count = 7 Then
                    Dim a As New AtlasTexture(line.Split(" ")(0).Trim.Replace("/", "\"))
                    If a.Path.ToLower.StartsWith("menu\hud\texture\") Then a.Path = a.Path.Remove(0, 17).Trim("\").Trim
                    a.AtlasPath = line.Split(" ")(1).TrimEnd(",").Trim
                    a.X = Val(line.Split(" ")(3).TrimEnd(",").Trim)
                    a.Y = Val(line.Split(" ")(4).TrimEnd(",").Trim)
                    a.W = Val(line.Split(" ")(5).TrimEnd(",").Trim)
                    a.H = Val(line.Split(" ")(6).TrimEnd(",").Trim)
                    If a.AtlasPath <> "" And a.W <> 0 And a.H <> 0 Then
                        AT.Add(a)
                        If Not AtlasPaths.Contains(a.AtlasPath.ToLower) Then
                            AtlasPaths.Add(a.AtlasPath.ToLower)
                        End If
                    End If
                End If
            Next
            data = Nothing
            'Loading atlasses
            Dim afolder As String = StrReverse(StrReverse(LoadPaths(0)).Remove(0, IO.Path.GetFileName(LoadPaths(0)).Length)).Trim("\")
            Dim Atlasses As New List(Of ImageData)
            Dim hasasked As Boolean = False
            Dim i As Integer = 0
            For Each p As String In AtlasPaths
                ProgressbarAdapter(i * 40 / AtlasPaths.Count, "Loading atlas textures...")
                If System.IO.File.Exists(afolder & "\" & p) Then
                    Atlasses.Add(New ImageData(p, LoadImage(afolder & "\" & p)))
                ElseIf System.IO.File.Exists(p) Then
                    Atlasses.Add(New ImageData(p, LoadImage(p)))
                Else
                    Dim newp As String = p
                    If p.ToLower.StartsWith("menu\atlas\") Then newp = p.Remove(0, 11).Trim("\").Trim
                    If System.IO.File.Exists(afolder & "\" & newp) Then
                        Atlasses.Add(New ImageData(p, LoadImage(afolder & "\" & newp)))
                    Else
                        If hasasked = False Then MsgBox("Failed to find one or more atlasses. Please browse for the correct atlas texture. If canceled, all textures using this atlas texture will not be loaded.", MsgBoxStyle.Information)
                        hasasked = True
                        Dim img As Image = AskTexture(p)
                        If Not IsNothing(img) Then Atlasses.Add(New ImageData(p, img))
                    End If
                End If
                i += 1
            Next
            i = 0
            'Loading actual images
            Dim aexisting As New List(Of ImageData)
            For Each A As AtlasTexture In AT
                ProgressbarAdapter(40 + i * 30 / AT.Count, "Loading textures...")
                For Each atlas As ImageData In Atlasses
                    If atlas.Path.ToLower = A.AtlasPath.ToLower Then
                        If existingitems.Contains(A.Path.ToLower) Then
                            aexisting.Add(New ImageData(A.Path, A.GetFrom(atlas.Image)))
                        Else
                            AddItem(New ImageData(A.Path, A.GetFrom(atlas.Image)))
                            i += 1
                        End If
                        Exit For
                    End If
                Next
                i += 1
                If AbortProcess = True Then Exit For
            Next
            If AbortProcess = False Then
                'Adding existing stuff
                Dim addmode As Integer = 0
                Dim forall As Boolean = False
                '0 = dont add; use old
                '1 = replace old
                '2 = rename path
                Dim ri As Integer = aexisting.Count - 1
                For Each img As ImageData In aexisting
                    ProgressbarAdapter(40 + i * 30 / AT.Count, "Loading textures...")
                    For Each oimg As ImageData In images
                        If img.Path.ToLower.Trim = oimg.Path.ToLower.Trim Then
                            If forall = False Then addmode = AskReplace(oimg, img, ri)
                            If addmode > 2 Then
                                forall = True
                                addmode -= 3
                            End If
                            If addmode = 1 Then
                                'replace old
                                oimg = img
                            ElseIf addmode = 2 Then
                                'rename path
                                Dim ind As Integer = 1
                                Dim p1 As String = img.Path
                                Dim p2 As String = "." & img.Path.Split(".").Last
                                p1 = p1.Substring(0, p1.Length - p2.Length)
                                If Not img.Path.Contains(".") Then p1 = img.Path
                                Do While existingitems.Contains(img.Path.ToLower.Trim)
                                    img.Path = p1 & "(" & ind & ")" & p2
                                    ind += 1
                                Loop
                                AddItem(img)
                            End If
                            Exit For
                        End If
                        If AbortProcess = True Then Exit For
                    Next
                    ri -= 1
                    i += 1
                    If AbortProcess = True Then Exit For
                Next
            End If
        Catch ex As Exception
            MsgBox("Loading failed." & vbCrLf & ex.Message)
        End Try
        AbortProcess = False
        ProgressbarAdapter(100, "Loading finished.")
    End Sub
    Private Class AtlasTexture
        Sub New(ByVal Path As String)
            Me.Path = Path
        End Sub
        Public Path As String = ""
        Public AtlasPath As String
        Public X As Double = 0
        Public Y As Double = 0
        Public W As Double = 0
        Public H As Double = 0
        Public Function GetFrom(ByVal Atlas As Bitmap) As Image
            Return CropImage(Atlas, Atlas.Width * X, Atlas.Height * Y, Atlas.Width * W, Atlas.Height * H)
        End Function
    End Class


    Delegate Sub myAddItem(ByVal item As ImageData)
    Private Sub AddItem(ByVal item As ImageData)
        If Me.InvokeRequired Then
            Dim d As New myAddItem(AddressOf AddItem)
            Me.Invoke(d, item)
        Else
            Dim existingitems As New List(Of String)
            For Each oimg As ImageData In images
                existingitems.Add(oimg.Path.ToLower.Trim)
            Next
            Dim add As Boolean = True
            For Each oimg As ImageData In images
                If item.Path.ToLower.Trim = oimg.Path.ToLower.Trim Then
                    Dim addmode As Integer = AskReplace(oimg, item, 0)
                    If addmode = 1 Then
                        'replace old
                        oimg = item
                        add = False
                    ElseIf addmode = 2 Then
                        'rename path
                        Dim ind As Integer = 1
                        Dim p1 As String = item.Path
                        Dim p2 As String = "." & item.Path.Split(".").Last
                        p1 = p1.Substring(0, p1.Length - p2.Length)
                        If Not item.Path.Contains(".") Then p1 = item.Path
                        Do While existingitems.Contains(item.Path.ToLower.Trim)
                            item.Path = p1 & "(" & ind & ")" & p2
                            ind += 1
                        Loop
                    End If
                    Exit For
                End If
            Next
            If add = True Then
                ReDim Preserve images(images.Count)
                images(images.Count - 1) = item
                ListView1.Items.Add(item.Path)
            End If
        End If
    End Sub
    Delegate Sub myProgressbarAdapter(ByVal percent As Integer, ByVal status As String)
    Private Sub ProgressbarAdapter(ByVal percent As Integer, ByVal status As String)
        If Me.InvokeRequired Then
            Dim d As New myProgressbarAdapter(AddressOf ProgressbarAdapter)
            Me.Invoke(d, percent, status)
        Else
            Progress.Label1.Text = status
            Me.Enabled = percent = 100
            If percent = -1 Then
                Progress.Show()
                Progress.ProgressBar1.Style = ProgressBarStyle.Marquee
            Else
                Progress.ProgressBar1.Style = ProgressBarStyle.Blocks
                If percent > 100 Then percent = 100
                If percent < 0 Then percent = 0
                If percent = 100 Then
                    IgnoreProgressClose = True
                    Progress.Close()
                    IgnoreProgressClose = False
                Else
                    Progress.Show()
                End If
                Progress.ProgressBar1.Value = percent
            End If
        End If
    End Sub
    Delegate Function myAskReplace(ByVal OldImage As ImageData, ByVal NewImage As ImageData, ByVal RemainingCount As Integer) As Integer
    Private Function AskReplace(ByVal OldImage As ImageData, ByVal NewImage As ImageData, ByVal RemainingCount As Integer) As Integer
        If OldImage.Path.ToLower.Trim = NewImage.Path.ToLower.Trim Then
            If Me.InvokeRequired Then
                Dim d As New myAskReplace(AddressOf AskReplace)
                Return Me.Invoke(d, OldImage, NewImage, RemainingCount)
            Else
                ReplaceDiag.PictureBox1.Image = OldImage.Image
                ReplaceDiag.PictureBox2.Image = NewImage.Image
                ReplaceDiag.PictureBox3.Image = OldImage.Image
                ReplaceDiag.CheckBox1.Text = "For all (" & RemainingCount & ")"
                ReplaceDiag.TextBox1.Text = OldImage.Path
                Return ReplaceDiag.ShowDialog()
            End If
        Else
            Return 0
        End If
    End Function
    Delegate Sub myEndAskReplace()

    Delegate Function myAskTexture(ByVal Texture As String) As Image
    Private Function AskTexture(ByVal Texture As String) As Image
        If Me.InvokeRequired Then
            Dim d As New myAskTexture(AddressOf AskTexture)
            Return Me.Invoke(d, Texture)
        Else
            OpenFileDialog1.Title = "Browse for texture: " & Texture
            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                Return LoadImage(OpenFileDialog1.FileName)
            Else
                Return Nothing
            End If
        End If
    End Function


    Private Sub AddNewImageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNewImageToolStripMenuItem.Click
        Button2.PerformClick()
    End Sub
    Private Sub RemoveImageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveImageToolStripMenuItem.Click
        Cursor.Current = Cursors.WaitCursor
        Dim delpaths As New List(Of String)
        For Each item As ListViewItem In ListView1.SelectedItems
            delpaths.Add(item.Text.ToLower.Trim)
            ListView1.Items.Remove(item)
        Next
        Dim i As Integer = 0
        Do While i < images.Count And images.Count <> 0
            If delpaths.Contains(images(i).Path.ToLower.Trim) Then
                For d As Integer = i To images.Count - 2
                    images(d) = images(d + 1)
                Next
                ReDim Preserve images(images.Count - 2)
            Else
                i += 1
            End If
        Loop
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub EditPathToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditPathToolStripMenuItem.Click
        ListView1.SelectedItems(0).BeginEdit()
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        RemoveImageToolStripMenuItem.PerformClick()
    End Sub
    Private Sub ListView1_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles ListView1.PreviewKeyDown
        If e.Control = True And e.KeyCode = Keys.A Then
            For Each item As ListViewItem In ListView1.Items
                item.Selected = True
            Next
        End If
    End Sub

    Private Sub CheckboardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckboardToolStripMenuItem.Click
        Try
            PictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath & "\bin\checkboard.png")
        Catch
        End Try
        PictureBox1.BackColor = Color.FromKnownColor(KnownColor.Control)
    End Sub
    Private Sub BlackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlackToolStripMenuItem.Click
        PictureBox1.BackgroundImage = Nothing
        PictureBox1.BackColor = Color.Black
    End Sub
    Private Sub GreyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GreyToolStripMenuItem.Click
        PictureBox1.BackgroundImage = Nothing
        PictureBox1.BackColor = Color.DarkGray
    End Sub
    Private Sub WhiteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WhiteToolStripMenuItem.Click
        PictureBox1.BackgroundImage = Nothing
        PictureBox1.BackColor = Color.White
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If OpenFileDialog3.ShowDialog = Windows.Forms.DialogResult.OK Then
            LoadPaths.Clear()
            LoadPaths.Add(OpenFileDialog3.FileName)
            LoadThread = New Threading.Thread(AddressOf LoadAtlas)
            LoadThread.IsBackground = True
            LoadThread.Start()
        End If
    End Sub
End Class
