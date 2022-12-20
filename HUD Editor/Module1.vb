Imports System.Drawing.Imaging

Module Module1
    Public LibraryTextures(-1) As ImagePointer
    Public CurrentFileName As String = ""
    Public PBCursorPosition As New Point(0, 0)
    Public BGimage As Bitmap
    Public OVimage As Bitmap = New Bitmap(16, 16)

    Public Modified As Boolean = False
    Public UpdateScreen As Boolean = False
    Public PerformGlobalUpdate As Boolean = False
    Public CurrentIndex As Integer = -1
    Public TexturesPath As String = "Textures"
    Public ViewedDialog As Integer = 0
    Public ScaleX As Single = 1
    Public ScaleY As Single = 1
    Public Nodes(0) As Node

#Region "Program Functions"
    Public Sub VarDispForm1(ByVal ParamArray Variables() As Object)
        Dim t As String = ""
        For Each v As Object In Variables
            t &= " | " & v
        Next
        If t.Trim.StartsWith("|") Then t = t.Remove(0, 2).Trim
        Form1.Text = t
    End Sub
    Public Sub WriteLog(ByVal Text As String)
        Try
            Dim w As New System.IO.StreamWriter(Application.StartupPath & "\log.txt", True)
            w.WriteLine(Date.Now.ToString & " - " & Text)
            w.Close()
        Catch
        End Try
    End Sub
    Public Function GetTreeViewNodes(ByVal TreeView As TreeView) As TreeNode()
        Dim rval(-1) As TreeNode
        For Each Node As TreeNode In TreeView.Nodes
            GetNodeSubNodes(Node, rval)
        Next
        Return rval
    End Function
    Private Sub GetNodeSubNodes(ByVal TNode As TreeNode, ByRef PrevNodes() As TreeNode)
        ReDim Preserve PrevNodes(PrevNodes.Count)
        PrevNodes(PrevNodes.Count - 1) = TNode
        For Each subnode As TreeNode In TNode.Nodes
            GetNodeSubNodes(subnode, PrevNodes)
        Next
    End Sub
    Public Function GetTreeViewNode(ByVal TreeView As TreeView, ByVal Node As String) As TreeNode
        Dim rnode As New TreeNode
        For Each Tnode As TreeNode In GetTreeViewNodes(TreeView)
            If Tnode.Name.ToLower.Trim = Node.ToLower.Trim Then rnode = Tnode
        Next
        Return rnode
    End Function
    Public Function GetNodeNameIndex(ByVal Name As String) As Integer
        Dim rval As Integer = -1
        For i As Integer = 1 To Nodes.Count - 1
            If Nodes(i).Name.ToLower.Trim = Name.ToLower.Trim Then rval = i
        Next
        Return rval
    End Function
    Public Function PointInSquare(ByVal Point As Point, ByVal Square As Rectangle, Optional ByVal SquareRotation As Integer = 0) As Boolean
        'Convert point to square-related
        Point.X -= Square.X + Square.Width * 0.5
        Point.Y -= Square.Y + Square.Height * 0.5
        Dim rotradian As Double = SquareRotation * Math.PI / -180
        If Point.X > 0 Then rotradian += 0.5 * Math.PI Else If Point.X < 0 Then rotradian += 1.5 * Math.PI Else rotradian += Math.PI
        If Point.Y <> 0 And Point.X <> 0 Then rotradian += Math.Atan(Point.Y / Point.X)
        Dim radius As Single = Math.Sqrt(Point.X ^ 2 + Point.Y ^ 2)
        Point.X = Math.Sin(rotradian) * radius + Square.Width * 0.5
        Point.Y = Math.Cos(rotradian) * radius + Square.Height * 0.5
        'Evaluate
        Return Point.X >= 0 And Point.Y >= 0 And Point.X <= Square.Width And Point.Y <= Square.Height
    End Function
    Public Function GetSquareSelectionType(ByVal Point As Point, ByVal Square As Rectangle, Optional ByVal SquareRotation As Integer = 0) As Integer
        'Convert point to square-related
        Point.X -= Square.X + Square.Width * 0.5
        Point.Y -= Square.Y + Square.Height * 0.5
        Dim rotradian As Double = SquareRotation * Math.PI / -180
        If Point.X > 0 Then rotradian += 0.5 * Math.PI
        If Point.X < 0 Then rotradian += 1.5 * Math.PI
        If Point.Y <> 0 And Point.X <> 0 Then rotradian += Math.Atan(Point.Y / Point.X)
        Dim radius As Double = Math.Sqrt(Point.X ^ 2 + Point.Y ^ 2)
        If Point.Y > 0 And Point.X = 0 Then rotradian += Math.PI
        Point.X = Math.Sin(rotradian) * radius + Square.Width * 0.5
        Point.Y = Square.Height * 0.5 - Math.Cos(rotradian) * radius
        'Evaluate
        Dim PRadius As Single = 5
        Dim TL As New Point(0, 0)
        Dim TM As Point = New Point(Square.Width * 0.5, 0)
        Dim TR As Point = New Point(Square.Width, 0)
        Dim ML As Point = New Point(0, Square.Height * 0.5)
        Dim MR As Point = New Point(Square.Width, Square.Height * 0.5)
        Dim BL As Point = New Point(0, Square.Height)
        Dim BM As Point = New Point(Square.Width * 0.5, Square.Height)
        Dim BR As Point = New Point(Square.Width, Square.Height)
        If Math.Sqrt((TL.X - Point.X) ^ 2 + (TL.Y - Point.Y) ^ 2) <= PRadius Then Return 1 'Top left
        If Math.Sqrt((TM.X - Point.X) ^ 2 + (TM.Y - Point.Y) ^ 2) <= PRadius Then Return 2 'Top Middle
        If Math.Sqrt((TR.X - Point.X) ^ 2 + (TR.Y - Point.Y) ^ 2) <= PRadius Then Return 3 'Top Right
        If Math.Sqrt((ML.X - Point.X) ^ 2 + (ML.Y - Point.Y) ^ 2) <= PRadius Then Return 4 'Middle Left
        If Math.Sqrt((MR.X - Point.X) ^ 2 + (MR.Y - Point.Y) ^ 2) <= PRadius Then Return 5 'Middle Right
        If Math.Sqrt((BL.X - Point.X) ^ 2 + (BL.Y - Point.Y) ^ 2) <= PRadius Then Return 6 'Bottom Left
        If Math.Sqrt((BM.X - Point.X) ^ 2 + (BM.Y - Point.Y) ^ 2) <= PRadius Then Return 7 'Botton Middle
        If Math.Sqrt((BR.X - Point.X) ^ 2 + (BR.Y - Point.Y) ^ 2) <= PRadius Then Return 8 'Bottom Right
        If Point.X >= 0 And Point.Y >= 0 And Point.X <= Square.Width And Point.Y <= Square.Height Then Return 9 'Middle
        Return 0 'Outside
    End Function

    Public Function SetValueBounds(ByVal Value As Single, ByVal Min As Single, ByVal Max As Single) As Single
        If Value > Max Then Value = Max
        If Value < Min Then Value = Min
        Return Value
    End Function
    Public Function GetValueAt(ByVal Text As String, ByVal Index As Integer) As String
        Dim i As Integer = 0
        Dim rval As String = ""
        For Each part As String In Text.Split(" ")
            If part.Trim <> "" Then
                If i = Index Then rval = part
                i += 1
            End If
        Next
        Return rval
    End Function
    Public Function FixTexturePath(ByVal TexturePath As String) As String
        TexturePath = TexturePath.Replace("/", "\")
        If TexturePath.Contains(":") Then
            'path is fixed, make relative
            If TexturePath.ToLower.StartsWith(Application.StartupPath.ToLower) Then TexturePath = TexturePath.Remove(0, Application.StartupPath.Length).Trim("\")
        End If
        Dim ddspath As String = StrReverse(StrReverse(TexturePath).Remove(0, System.IO.Path.GetExtension(TexturePath).Length)) & ".dds"
        Dim tgapath As String = StrReverse(StrReverse(TexturePath).Remove(0, System.IO.Path.GetExtension(TexturePath).Length)) & ".tga"
        Dim relpath1 As String = Application.StartupPath & "\" & ddspath
        Dim relpath2 As String = Application.StartupPath & "\" & tgapath
        Dim relpath3 As String = Application.StartupPath & "\" & TexturesPath & "\" & ddspath
        Dim relpath4 As String = Application.StartupPath & "\" & TexturesPath & "\" & tgapath
        If System.IO.File.Exists(relpath1) Then TexturePath = relpath1
        If System.IO.File.Exists(relpath2) Then TexturePath = relpath2
        If System.IO.File.Exists(relpath3) Then TexturePath = relpath1
        If System.IO.File.Exists(relpath4) Then TexturePath = relpath2
        If TexturePath.ToLower.StartsWith(Application.StartupPath.ToLower) Then TexturePath = TexturePath.Remove(0, Application.StartupPath.Length).Trim("\")
        Return TexturePath
    End Function
    Public Function RemoveDoubleSpaces(ByVal Text As String) As String
        Text = Text.Replace(vbTab, " ")
        Do While Text.Contains("  ")
            Text = Text.Replace("  ", " ")
        Loop
        Return Text
    End Function
    Public Function ConvertColorToText(ByVal Color As Color)
        Dim A As String = Color.A / 255
        Dim R As String = Color.R / 255
        Dim G As String = Color.G / 255
        Dim B As String = Color.B / 255
        If A.Length > 5 Then A = A.Substring(0, 5)
        If R.Length > 5 Then R = R.Substring(0, 5)
        If G.Length > 5 Then G = G.Substring(0, 5)
        If B.Length > 5 Then B = B.Substring(0, 5)
        Return (R & " " & G & " " & B & " " & A).Replace(",", ".")
    End Function

    Public Sub SetCBSelectedItem(ByRef cb As ComboBox, ByVal Item As String, Optional ByVal SetNothing As Boolean = True)
        Item = Item.ToLower.Trim
        If SetNothing = True Then cb.SelectedIndex = -1
        For i As Integer = 0 To cb.Items.Count - 1
            If cb.Items(i).tolower.trim = Item Then cb.SelectedIndex = i
        Next
    End Sub
    Public Delegate Sub myPictureBoxAdapter(ByVal PBImage As Image, ByVal Invokefrom As Control)
    Public Sub PictureBoxAdapter(ByVal PBImage As Image, ByVal Invokefrom As Control)
        Try
            If Invokefrom.InvokeRequired Then
                Dim d As New myPictureBoxAdapter(AddressOf PictureBoxAdapter)
                Invokefrom.Invoke(d, PBImage, Invokefrom)
            Else
                Form1.PictureBox1.Image = PBImage
            End If
        Catch
        End Try
    End Sub
    Public Sub SetViewedDialog(ByVal DialogIndex As Integer)
        If CurrentIndex <> -1 Then
            ViewedDialog = DialogIndex
            On Error Resume Next
            If DialogIndex = 2 Then ColorDialog.Show() Else ColorDialog.Close()
            If DialogIndex = 3 Then MainDialog.Show() Else MainDialog.Close()
            If DialogIndex = 4 Then TSizeDialog.Show() Else TSizeDialog.Close()
            If DialogIndex = 5 Then RotationDialog.Show() Else RotationDialog.Close()
            If DialogIndex = 6 Then
                If Nodes(CurrentIndex).Type = "Picture Node" Then pnvariables.Show()
                If Nodes(CurrentIndex).Type = "Text Node" Then tnvariables.Show()
                If Nodes(CurrentIndex).Type = "Compass Node" Then cnvariables.Show()
                If Nodes(CurrentIndex).Type = "Bar Node" Then bnvariables.Show()
            Else
                pnvariables.Close()
                tnvariables.Close()
                cnvariables.Close()
                bnvariables.Close()
            End If
            If DialogIndex = 7 Then
                If Nodes(CurrentIndex).Type = "Text Node" Then TextStyle.Show()
                If Nodes(CurrentIndex).Type = "Compass Node" Then CompassStyle.Show()
                If Nodes(CurrentIndex).Type = "Bar Node" Then BarStyle.Show()
            Else
                TextStyle.Close()
                CompassStyle.Close()
                BarStyle.Close()
            End If
            If DialogIndex = 8 Then ShowDialog.ShowDialog() Else ShowDialog.Close()
            If DialogIndex = 1 Then TextureBrowser.ShowDialog() Else TextureBrowser.Close()
        Else
            ViewedDialog = 0
        End If
    End Sub

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

    Public Function LoadImage(ByVal TexturePath As String) As Image
        If TexturePath.Trim <> "" Then
            TexturePath = FixTexturePath(TexturePath)
            If System.IO.File.Exists(Application.StartupPath & "\" & TexturesPath & "\" & TexturePath) Then
                TexturePath = Application.StartupPath & "\" & TexturesPath & "\" & TexturePath
            ElseIf System.IO.File.Exists(Application.StartupPath & "\" & TexturePath) Then
                TexturePath = Application.StartupPath & "\" & TexturePath
            End If
            Dim searchinlibrary As Boolean = False
            If System.IO.File.Exists(TexturePath) Then
                Try
                    Return FreeImageAPI.FreeImage.GetBitmap(FreeImageAPI.FreeImage.LoadEx(TexturePath))
                Catch
                    Try
                        Return Image.FromFile(TexturePath)
                    Catch
                        searchinlibrary = True
                    End Try
                End Try
            Else
                searchinlibrary = True
            End If
            If searchinlibrary = True Then
                TexturePath = TexturePath.ToLower.Trim
                If TexturePath.ToLower.StartsWith(Application.StartupPath.ToLower) Then TexturePath = TexturePath.Remove(0, Application.StartupPath.Length).Trim
                TexturePath = StrReverse(StrReverse(TexturePath).Remove(0, IO.Path.GetExtension(TexturePath).Length)).Trim
                For Each img As ImagePointer In LibraryTextures
                    Dim imgpath As String = StrReverse(StrReverse(img.Path).Remove(0, IO.Path.GetExtension(img.Path).Length)).Trim.ToLower
                    If imgpath = TexturePath Then Return img.GetImage
                Next
            End If
            WriteLog("Failed to load texture: " & TexturePath)
            Return New Bitmap(32, 32)
        End If
        Return New Bitmap(32, 32)
    End Function

    Public Sub SaveImage(ByVal Image As Image, ByVal DestPath As String)
        FreeImageAPI.FreeImage.SaveBitmap(Image, DestPath)
    End Sub
    Public Function ColorImage(ByVal source As Bitmap, ByVal Amult As Single, ByVal Rmult As Single, ByVal Gmult As Single, ByVal Bmult As Single) As Image
        Dim bm As New Bitmap(source.Width, source.Height)
        Dim cmxPic As ColorMatrix = New ColorMatrix
        cmxPic.Matrix33 = Amult
        cmxPic.Matrix22 = Bmult
        cmxPic.Matrix11 = Gmult
        cmxPic.Matrix00 = Rmult
        Dim imga As ImageAttributes = New ImageAttributes
        imga.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap)
        Dim g As Graphics = Graphics.FromImage(bm)
        g.DrawImage(source, New Rectangle(0, 0, bm.Width, bm.Height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, imga)
        g.Dispose()
        Return bm
    End Function
    Public Function ResizeImage(ByVal source As Bitmap, ByVal NewSizeX As Integer, ByVal NewSizeY As Integer, ByVal Stretch As Boolean) As Bitmap
        If NewSizeX < 1 Then NewSizeX = 1
        If NewSizeY < 1 Then NewSizeY = 1
        Dim returnbitmap As New Bitmap(NewSizeX, NewSizeY)
        Dim g As Graphics = Graphics.FromImage(returnbitmap)
        If Stretch = True Then
            g.DrawImage(source, 0, 0, NewSizeX + 1, NewSizeY + 1)
        Else
            Dim scale1 As Integer = NewSizeX * 100000 \ source.Size.Width
            Dim scale2 As Integer = NewSizeY * 100000 \ source.Size.Height
            Dim finalscale As Integer = 1
            If scale1 < scale2 Then finalscale = scale1
            If scale1 >= scale2 Then finalscale = scale2
            g.DrawImage(source, 0, 0, (source.Size.Width * finalscale \ 100000) + 1, (source.Size.Height * finalscale \ 100000) + 1)
        End If
        g.Dispose()
        Return returnbitmap
    End Function
    Public Function CropImage(ByRef bmp As Bitmap, ByVal cropX As Integer, ByVal cropY As Integer, ByVal cropWidth As Integer, ByVal cropHeight As Integer) As Bitmap
        Dim validoperation As Boolean = True
        If cropX + cropWidth > bmp.Width Then validoperation = False
        If cropY + cropHeight > bmp.Height Then validoperation = False
        If cropWidth = 0 Then validoperation = False
        If cropHeight = 0 Then validoperation = False
        If validoperation = True Then
            Dim rect As New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim cropped As Bitmap = bmp.Clone(rect, bmp.PixelFormat)
            Return cropped
        Else
            Return New Bitmap(32, 32)
        End If
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
    Public Function ListFolderSubFolders(ByVal Path As String) As String()
        Dim rval As New List(Of String)
        For Each directory As String In IO.Directory.GetDirectories(Path)
            rval.Add(directory)
            For Each folder As String In ListFolderSubFolders(directory)
                rval.Add(folder)
            Next
        Next
        Dim frval(rval.Count - 1) As String
        For i As Integer = 0 To rval.Count - 1
            frval(i) = rval(i)
        Next
        Return frval
    End Function
    Public Function PathIsFile(ByVal Path As String) As Boolean
        Return IO.Path.GetFileName(Path) <> IO.Path.GetFileNameWithoutExtension(Path)
    End Function

#End Region

#Region "Render functions"
    Public Function RenderSelectionBox(ByVal Position As Point, ByVal Size As Size, ByVal Rotation As Integer) As Image
        Dim returnbm As New Bitmap(800, 600)
        Dim g As Graphics = Graphics.FromImage(returnbm)
        g.TranslateTransform(Position.X + Size.Width * 0.5, Position.Y + Size.Height * 0.5)
        g.RotateTransform(Rotation)
        g.TranslateTransform(Size.Width * -0.5, Size.Height * -0.5)
        Dim p As New Pen(Color.Black)
        p.DashStyle = Drawing2D.DashStyle.Dash
        g.DrawRectangle(p, New Rectangle(0, 0, Size.Width, Size.Height))
        g.FillRectangle(New SolidBrush(Color.White), New Rectangle(-2, -2, 4, 4))
        g.FillRectangle(New SolidBrush(Color.White), New Rectangle(Size.Width - 2, -2, 4, 4))
        g.FillRectangle(New SolidBrush(Color.White), New Rectangle(Size.Width - 2, Size.Height - 2, 4, 4))
        g.FillRectangle(New SolidBrush(Color.White), New Rectangle(-2, Size.Height - 2, 4, 4))
        g.DrawRectangle(Pens.Black, New Rectangle(-3, -3, 6, 6))
        g.DrawRectangle(Pens.Black, New Rectangle(Size.Width - 3, -3, 6, 6))
        g.DrawRectangle(Pens.Black, New Rectangle(Size.Width - 3, Size.Height - 3, 6, 6))
        g.DrawRectangle(Pens.Black, New Rectangle(-3, Size.Height - 3, 6, 6))
        If Size.Width > 31 Then
            g.FillRectangle(New SolidBrush(Color.White), New Rectangle(Size.Width * 0.5 - 2, -2, 4, 4))
            g.DrawRectangle(Pens.Black, New Rectangle(Size.Width * 0.5 - 3, -3, 6, 6))
            g.FillRectangle(New SolidBrush(Color.White), New Rectangle(Size.Width * 0.5 - 2, Size.Height - 2, 4, 4))
            g.DrawRectangle(Pens.Black, New Rectangle(Size.Width * 0.5 - 3, Size.Height - 3, 6, 6))
        End If
        If Size.Height > 31 Then
            g.FillRectangle(New SolidBrush(Color.White), New Rectangle(-2, Size.Height * 0.5 - 2, 4, 4))
            g.DrawRectangle(Pens.Black, New Rectangle(-3, Size.Height * 0.5 - 3, 6, 6))
            g.FillRectangle(New SolidBrush(Color.White), New Rectangle(Size.Width - 2, Size.Height * 0.5 - 2, 4, 4))
            g.DrawRectangle(Pens.Black, New Rectangle(Size.Width - 3, Size.Height * 0.5 - 3, 6, 6))
        End If
        g.Dispose()
        Return returnbm
    End Function
    Public Function RenderPictureNode(ByRef pnode As PictureNode) As Image
        Dim cchanged As Boolean = pnode.ColorChanged
        Dim schanged As Boolean = pnode.SizeChanged
        Dim pchanged As Boolean = pnode.PosRotChanged
        If cchanged = True Then
            pnode.ColorChanged = False
            pnode.ColoredImage = ColorImage(pnode.TextureImage, pnode.Color.A / 255, pnode.Color.R / 255, pnode.Color.G / 255, pnode.Color.B / 255)
        End If
        If schanged = True Or cchanged = True Then
            pnode.SizeChanged = False
            pnode.SizedImage = ResizeImage(pnode.ColoredImage, pnode.Size.Width, pnode.Size.Height, True)
        End If
        If schanged = True Or cchanged = True Or pchanged = True Then
            pnode.PosRotChanged = False
            pnode.FinalImage = New Bitmap(800, 600)
            Dim g As Graphics = Graphics.FromImage(pnode.FinalImage)
            g.TranslateTransform(pnode.DRotationMid.X + 400, pnode.DRotationMid.Y + 300)
            g.RotateTransform(pnode.DRotation)
            g.TranslateTransform((pnode.Position.X + pnode.DOffsetX) - pnode.DRotationMid.X - 400 + pnode.Size.Width * 0.5, (pnode.Position.Y + pnode.DOffsetY) - pnode.DRotationMid.Y - 300 + pnode.Size.Height * 0.5)
            g.RotateTransform(pnode.StaticRotation)
            g.TranslateTransform(pnode.Size.Width * -0.5, pnode.Size.Height * -0.5)
            g.DrawImage(pnode.SizedImage, New Point(0, 0))
            g.Dispose()
        End If
        If IsNothing(pnode.FinalImage) Then pnode.FinalImage = New Bitmap(800, 600)
        Return pnode.FinalImage
    End Function
    Public Function RenderTextNode(ByRef tnode As TextNode) As Image
        If tnode.Modified = True Then
            If tnode.StringVariable = "" Then tnode.Text = tnode.StringText
            tnode.Modified = False
            'Generate Text Bitmap (tbmp)
            Dim tbox As New TextBox
            tbox.Font = New Font("Arial", 8, FontStyle.Regular)
            tbox.Text = tnode.Text & " "
            Dim tbmp As New Bitmap(tbox.GetPositionFromCharIndex(tbox.Text.Length - 1).X, 20)
            Dim myBrush As New Drawing2D.LinearGradientBrush(New Rectangle(0, 0, 1, 1), tnode.Color, tnode.Color, Drawing2D.LinearGradientMode.Horizontal)
            Dim tg As Graphics = Graphics.FromImage(tbmp)
            tg.DrawString(tnode.Text, tbox.Font, myBrush, -1, -1)
            tg.Dispose()

            'Draw this Text Bitmap inside the bounds
            tnode.FinalImage = New Bitmap(800, 600)
            Dim g As Graphics = Graphics.FromImage(tnode.FinalImage)
            If tnode.Style = 0 Then
                'center
                g.DrawImage(tbmp, New Point(tnode.Position.X + tnode.Size.Width * 0.5 - tbmp.Width * 0.5, tnode.Position.Y))
            ElseIf tnode.Style = 1 Then
                'right
                g.DrawImage(tbmp, New Point(tnode.Position.X + tnode.Size.Width - tbmp.Width, tnode.Position.Y))
            ElseIf tnode.Style = 2 Then
                'left
                g.DrawImage(tbmp, New Point(tnode.Position.X, tnode.Position.Y))
            End If
        End If
        If IsNothing(tnode.FinalImage) Then tnode.FinalImage = New Bitmap(800, 600)
        Return tnode.FinalImage
    End Function
    Public Function RenderCompassNode(ByRef cnode As CompassNode) As Image
        Dim cchanged As Boolean = cnode.ColorChanged
        Dim schanged As Boolean = cnode.SizeChanged
        Dim vchanged As Boolean = cnode.ValueChanged
        If cchanged = True Then
            cnode.ColorChanged = False
            cnode.ColoredImage = ColorImage(cnode.TextureImage, cnode.Color.A / 255, cnode.Color.R / 255, cnode.Color.G / 255, cnode.Color.B / 255)
        End If
        If cchanged = True Or schanged = True Then
            cnode.SizeChanged = False
            cnode.SizedImage = ResizeImage(cnode.ColoredImage, cnode.TextureSize.Width, cnode.TextureSize.Height, True)
        End If
        If cchanged = True Or schanged = True Or vchanged = True Then
            cnode.ValueChanged = False
            'Tough rendering part
            cnode.FinalImage = New Bitmap(800, 600)
            Dim g As Graphics = Graphics.FromImage(cnode.FinalImage)
            Dim bmp As New Bitmap(cnode.Size.Width, cnode.Size.Height)
            If cnode.Type = 3 Then
                'Step1: Convert value into offset
                Dim offset As Integer = -1 * (cnode.TextureSize.Width - cnode.Border) * cnode.Value - cnode.Offset

                'Step2: Write part 1 (main part)
                Dim cg As Graphics = Graphics.FromImage(bmp)
                cg.DrawImage(cnode.SizedImage, New Point(offset, 0))
                cg.DrawImage(cnode.SizedImage, New Point(offset + cnode.TextureSize.Width - cnode.Border, 0))
            ElseIf cnode.Type = 0 Then
                'Step1: Convert value into offset
                Dim offset As Integer = -1 * (cnode.TextureSize.Height - cnode.Border) * cnode.Value - cnode.Offset
                'Step2: Write part 1 (main part)
                Dim cg As Graphics = Graphics.FromImage(bmp)
                cg.DrawImage(cnode.SizedImage, New Point(0, offset))
                cg.DrawImage(cnode.SizedImage, New Point(0, offset + cnode.TextureSize.Height - cnode.Border))
            End If
            g.DrawImage(bmp, cnode.Position)
        End If
        If IsNothing(cnode.FinalImage) Then cnode.FinalImage = New Bitmap(800, 600)
        Return cnode.FinalImage
    End Function
    Public Function RenderBarNode(ByRef bnode As BarNode) As Image
        Dim cchanged As Boolean = bnode.ColorChanged
        Dim schanged As Boolean = bnode.SizeChanged
        Dim vchanged As Boolean = bnode.ValueChanged
        If cchanged = True Then
            bnode.ColorChanged = False
            bnode.ColoredFullImage = ColorImage(bnode.FullTextureImage, bnode.Color.A / 255, bnode.Color.R / 255, bnode.Color.G / 255, bnode.Color.B / 255)
            bnode.ColoredEmptyImage = ColorImage(bnode.EmptyTextureImage, bnode.Color.A / 255, bnode.Color.R / 255, bnode.Color.G / 255, bnode.Color.B / 255)
        End If
        If schanged = True Or cchanged = True Then
            bnode.SizeChanged = False
            bnode.SizedFullImage = ResizeImage(bnode.ColoredFullImage, bnode.Size.Width, bnode.Size.Height, True)
            bnode.SizedEmptyImage = ResizeImage(bnode.ColoredEmptyImage, bnode.Size.Width, bnode.Size.Height, True)
        End If
        If schanged = True Or cchanged = True Or vchanged = True Then
            bnode.ValueChanged = False
            bnode.FinalImage = New Bitmap(800, 600)
            Dim g As Graphics = Graphics.FromImage(bnode.FinalImage)
            Dim bmp As New Bitmap(bnode.Size.Width, bnode.Size.Height)
            Dim bg As Graphics = Graphics.FromImage(bmp)
            If bnode.Style = 0 Then
                'Vertical increasing from below
                Dim h As Integer = bnode.SizedFullImage.Size.Height - bnode.SizedFullImage.Size.Height * bnode.Value
                If h < bnode.SizedFullImage.Size.Height Then
                    Dim fullpart As Bitmap = CropImage(bnode.SizedFullImage, 0, h, bnode.SizedFullImage.Size.Width, bnode.SizedFullImage.Size.Height - h)
                    bg.DrawImage(fullpart, New Point(0, h))
                End If
                If h > 0 Then
                    Dim emptypart As Bitmap = CropImage(bnode.SizedEmptyImage, 0, 0, bnode.SizedEmptyImage.Size.Width, h)
                    bg.DrawImage(emptypart, New Point(0, 0))
                End If
            ElseIf bnode.Style = 1 Then
                'Vertical increasing from above
                Dim h As Integer = bnode.SizedFullImage.Size.Height * bnode.Value
                If h > 0 Then
                    Dim fullpart As Bitmap = CropImage(bnode.SizedFullImage, 0, 0, bnode.SizedFullImage.Size.Width, h)
                    bg.DrawImage(fullpart, New Point(0, 0))
                End If
                If h < bnode.SizedFullImage.Size.Height Then
                    Dim emptypart As Bitmap = CropImage(bnode.SizedEmptyImage, 0, h, bnode.SizedEmptyImage.Size.Width, bnode.SizedEmptyImage.Size.Height - h)
                    bg.DrawImage(emptypart, New Point(0, h))
                End If
            ElseIf bnode.Style = 2 Then
                'Horizontal increasing from right
                Dim w As Integer = bnode.SizedFullImage.Size.Width - bnode.SizedFullImage.Size.Width * bnode.Value
                If w < bnode.SizedFullImage.Size.Width Then
                    Dim fullpart As Bitmap = CropImage(bnode.SizedFullImage, w, 0, bnode.SizedFullImage.Size.Width - w, bnode.SizedFullImage.Size.Height)
                    bg.DrawImage(fullpart, New Point(w, 0))
                End If
                If w > 0 Then
                    Dim emptypart As Bitmap = CropImage(bnode.SizedEmptyImage, 0, 0, w, bnode.SizedEmptyImage.Height)
                    bg.DrawImage(emptypart, New Point(0, 0))
                End If
            ElseIf bnode.Style = 3 Then
                'Horizontal increasing from left
                Dim w As Integer = bnode.SizedFullImage.Size.Width * bnode.Value
                If w > 0 Then
                    Dim fullpart As Bitmap = CropImage(bnode.SizedFullImage, 0, 0, w, bnode.SizedFullImage.Size.Height)
                    bg.DrawImage(fullpart, New Point(0, 0))
                End If
                If w < bnode.SizedFullImage.Size.Width Then
                    Dim emptypart As Bitmap = CropImage(bnode.SizedEmptyImage, w, 0, bnode.SizedEmptyImage.Size.Width - w, bnode.SizedEmptyImage.Size.Height)
                    bg.DrawImage(emptypart, New Point(w, 0))
                End If
            End If
            g.DrawImage(bmp, bnode.Position)
        End If
        Return bnode.FinalImage
    End Function
#End Region

#Region "Nodes array manipulation functions"

    Public Function GetNodeIndexAtPoint(ByVal Point As Point) As Integer
        Dim selindex As Integer = -1
        For i As Integer = 1 To Nodes.Count - 1
            If Nodes(i).Render = True Then
                Dim Position As Point = Nodes(i).Position
                Dim Size As Size = Nodes(i).Size
                Dim Rotation As Integer = Nodes(i).Rotation
                If PointInSquare(Point, New Rectangle(Position, Size), Rotation) Then
                    selindex = i
                End If
            End If
        Next
        Return selindex
    End Function
    Public Sub RemoveNode(ByVal Index As Integer)
        WriteLog("Removing node: " & Nodes(Index).Name)
        For i As Integer = Index To Nodes.Count - 2
            Nodes(i) = Nodes(i + 1)
        Next
        ReDim Preserve Nodes(Nodes.Count - 2)
    End Sub
    Public Sub InsertNode(ByVal Node As Node, ByVal Index As Integer)
        WriteLog("Inserting node: " & Node.Name & " at " & Index)
        ReDim Preserve Nodes(Nodes.Count)
        Dim buNodes(Nodes.Count - 1) As Node
        Nodes.CopyTo(buNodes, 0)
        For i As Integer = Index To Nodes.Count - 2
            Nodes(i + 1) = buNodes(i)
        Next
        buNodes = Nothing
        Nodes(Index) = Node
    End Sub
    Public Sub LoadNode(ByVal NodeIndex As Integer, Optional ByVal SetTreeView As Boolean = True)
        SetViewedDialog(0)
        Form1.TextureButton.Visible = False
        Form1.ColorButton.Visible = False
        Form1.MainButton.Visible = False
        Form1.VariablesButton.Visible = False
        Form1.RotationButton.Visible = False
        Form1.StyleButton.Visible = False
        Form1.TSizeButton.Visible = False
        Form1.ETextureButton.Visible = False
        Form1.FTextureButton.Visible = False
        Form1.ShowButton.Visible = False
        CurrentIndex = NodeIndex
        If NodeIndex < Nodes.Count And NodeIndex > -1 Then
            If SetTreeView = True Then Form1.TreeView1.SelectedNode = GetTreeViewNode(Form1.TreeView1, Nodes(CurrentIndex).Name)
            Form1.Text = "HUD Editor - " & Nodes(CurrentIndex).Name & " (" & Nodes(NodeIndex).Type & ")"
            'Set button visibility
            If Nodes(NodeIndex).Type = "Picture Node" Then
                Form1.TextureButton.Visible = True
                Form1.ColorButton.Visible = True
                Form1.MainButton.Visible = True
                Form1.VariablesButton.Visible = True
                Form1.RotationButton.Visible = True
            ElseIf Nodes(NodeIndex).Type = "Text Node" Then
                Form1.ColorButton.Visible = True
                Form1.MainButton.Visible = True
                Form1.StyleButton.Visible = True
                Form1.VariablesButton.Visible = True
            ElseIf Nodes(CurrentIndex).Type = "Compass Node" Then
                Form1.TSizeButton.Visible = True
                Form1.MainButton.Visible = True
                Form1.TextureButton.Visible = True
                Form1.ColorButton.Visible = True
                Form1.VariablesButton.Visible = True
                Form1.StyleButton.Visible = True
            ElseIf Nodes(CurrentIndex).Type = "Split Node" Then
                Form1.VariablesButton.Visible = True
            ElseIf Nodes(CurrentIndex).Type = "Bar Node" Then
                Form1.ColorButton.Visible = True
                Form1.MainButton.Visible = True
                Form1.VariablesButton.Visible = True
                Form1.ETextureButton.Visible = True
                Form1.FTextureButton.Visible = True
                Form1.StyleButton.Visible = True
            ElseIf Nodes(CurrentIndex).Type = "Hover Node" Then
                Form1.MainButton.Visible = True
            ElseIf Nodes(CurrentIndex).Type = "Object Marker Node" Then
                Form1.MainButton.Visible = True
            End If
            Form1.ShowButton.Visible = True
        Else
            Form1.Text = "HUD Editor - No Node Selected"
            If SetTreeView = True Then Form1.TreeView1.SelectedNode = Nothing
        End If
        PerformGlobalUpdate = True
        UpdateScreen = True
    End Sub
    Public Sub RefreshNodes()
        For i As Integer = 1 To Nodes.Count - 1
            If Nodes(i).Render = True Then
                If Nodes(i).Type = "Picture Node" Then
                    Nodes(i).PictureNodeData.SizeChanged = True
                    Nodes(i).PictureNodeData.ColorChanged = True
                    Nodes(i).PictureNodeData.PosRotChanged = True
                End If
                If Nodes(i).Type = "Compass Node" Then
                    Nodes(i).CompassNodeData.SizeChanged = True
                    Nodes(i).CompassNodeData.ColorChanged = True
                    Nodes(i).CompassNodeData.ValueChanged = True
                End If
                If Nodes(i).Type = "Text Node" Then
                    Nodes(i).TextNodeData.Modified = True
                End If
            End If
        Next
        UpdateScreen = True
    End Sub
    Public Sub ResetNodeSimvars()
        For i As Integer = 1 To Nodes.Count - 1
            If Nodes(i).Type = "Picture Node" Then
                Nodes(i).PictureNodeData.DOffsetX = 0
                Nodes(i).PictureNodeData.DOffsetY = 0
                Nodes(i).PictureNodeData.DRotation = 0
                Nodes(i).PictureNodeData.PosRotChanged = True
            ElseIf Nodes(i).Type = "Text Node" Then
                Nodes(i).TextNodeData.Text = "100"
                Nodes(i).TextNodeData.Modified = True
            ElseIf Nodes(i).Type = "Compass Node" Then
                Nodes(i).CompassNodeData.Value = 0
                Nodes(i).CompassNodeData.ValueChanged = True
            End If
        Next
        UpdateScreen = True
    End Sub

    Public Function ContentCheck() As Boolean
        Dim errormsg As String = ""
        If Not System.IO.File.Exists(Application.StartupPath & "\bin\checkboard.png") Then
            errormsg = "File not found: " & Chr(34) & "bin\checkboard.png" & Chr(34)
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\bin\no_texture.gif") Then
            errormsg = "File not found: " & Chr(34) & "bin\no_texture.gif" & Chr(34)
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\bin\referencecross.gif") Then
            errormsg = "File not found: " & Chr(34) & "bin\referencecross.gif" & Chr(34)
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\bin\Background\Generic background.jpg") Then
            errormsg = "File not found: " & Chr(34) & "bin\Background\Generic background.jpg" & Chr(34)
        End If
        If errormsg <> "" Then
            MsgBox("Fatal error: " & vbCrLf & errormsg, MsgBoxStyle.Critical)
            Form1.Close()
            Return False
        Else
            Return True
        End If
    End Function

    Dim OLDINDEX As Integer = -1
    Dim BackLayer As New Bitmap(800, 600)
    Dim ActiveLayer As New Bitmap(800, 600)
    Dim FrontLayer As New Bitmap(800, 600)
    Public Sub RenderNodes(ByRef g As Graphics)
        'Get the layer modified types
        Dim UpdateActiveLayer As Boolean = False
        Dim UpdateBackLayer As Boolean = False
        Dim UpdateFrontLayer As Boolean = False
        'Dim Nodes() As Node = Module1.Nodes
        If OLDINDEX = CurrentIndex And PerformGlobalUpdate = False Then
            For i As Integer = 1 To Nodes.Count - 1
                Dim NodeModified As Boolean = Nodes(i).PerformRefresh
                If NodeModified = True Then Nodes(i).PerformRefresh = False
                If Nodes(i).Render = True Then
                    If Nodes(i).Type = "Picture Node" Then
                        If Nodes(i).PictureNodeData.ColorChanged = True Then NodeModified = True
                        If Nodes(i).PictureNodeData.PosRotChanged = True Then NodeModified = True
                        If Nodes(i).PictureNodeData.SizeChanged = True Then NodeModified = True
                    ElseIf Nodes(i).Type = "Compass Node" Then
                        If Nodes(i).CompassNodeData.ColorChanged = True Then NodeModified = True
                        If Nodes(i).CompassNodeData.ValueChanged = True Then NodeModified = True
                        If Nodes(i).CompassNodeData.SizeChanged = True Then NodeModified = True
                    ElseIf Nodes(i).Type = "Bar Node" Then
                        If Nodes(i).BarNodeData.ColorChanged = True Then NodeModified = True
                        If Nodes(i).BarNodeData.ValueChanged = True Then NodeModified = True
                        If Nodes(i).BarNodeData.SizeChanged = True Then NodeModified = True
                    ElseIf Nodes(i).Type = "Text Node" Then
                        If Nodes(i).TextNodeData.Modified = True Then NodeModified = True
                    End If
                End If
                If NodeModified = True And i < CurrentIndex Then UpdateBackLayer = True
                If NodeModified = True And i > CurrentIndex Then UpdateFrontLayer = True
                If NodeModified = True And i = CurrentIndex Then UpdateActiveLayer = True
            Next
        Else
            PerformGlobalUpdate = False
            UpdateBackLayer = True
            UpdateActiveLayer = True
            UpdateFrontLayer = True
        End If
        If UpdateActiveLayer = True Then ActiveLayer = New Bitmap(800, 600)
        If UpdateFrontLayer = True Then FrontLayer = New Bitmap(800, 600)
        If UpdateBackLayer = True Then BackLayer = New Bitmap(800, 600)
        If UpdateBackLayer = True Or UpdateFrontLayer = True Or UpdateActiveLayer = True Then
            Dim e As Integer = Nodes.Count - 1
            Dim s As Integer = 1
            If UpdateBackLayer = False Then s = CurrentIndex
            If UpdateFrontLayer = False Then e = CurrentIndex
            For i As Integer = s To e
                If i < Nodes.Count And i > 0 And ((UpdateBackLayer = True And i < CurrentIndex) Or (UpdateFrontLayer = True And i > CurrentIndex) Or (UpdateActiveLayer = True And i = CurrentIndex)) Then
                    If Nodes(i).Render = True Then
                        Dim lg As Graphics = Graphics.FromImage(ActiveLayer)
                        If i < CurrentIndex Then lg = Graphics.FromImage(BackLayer)
                        If i > CurrentIndex Then lg = Graphics.FromImage(FrontLayer)
                        If Nodes(i).Type = "Picture Node" Then
                            lg.DrawImage(RenderPictureNode(Nodes(i).PictureNodeData), New Point(0, 0))
                        End If
                        If Nodes(i).Type = "Text Node" Then
                            lg.DrawImage(RenderTextNode(Nodes(i).TextNodeData), New Point(0, 0))
                        End If
                        If Nodes(i).Type = "Compass Node" Then
                            lg.DrawImage(RenderCompassNode(Nodes(i).CompassNodeData), New Point(0, 0))
                        End If
                        If Nodes(i).Type = "Bar Node" Then
                            lg.DrawImage(RenderBarNode(Nodes(i).BarNodeData), New Point(0, 0))
                        End If
                    End If
                End If
            Next
        End If
        'Render all the nodes 
        If CurrentIndex <> 1 Then g.DrawImage(BackLayer, New Point(0, 0))
        If CurrentIndex <> -1 Then g.DrawImage(ActiveLayer, New Point(0, 0))
        If CurrentIndex <> Nodes.Count - 1 Then g.DrawImage(FrontLayer, New Point(0, 0))
        g.DrawImage(OVimage, New Point(0, 0))
        OLDINDEX = CurrentIndex
    End Sub
    Public Function GetNodeTree(ByVal Node As Node) As String
        Dim path As String = Node.Parent & "\" & Node.Name
        Dim oldparent As String = ""
        Dim currentparent As String = Node.Parent.ToLower.Trim
        Do While oldparent <> currentparent
            oldparent = currentparent
            For i As Integer = 1 To Nodes.Count - 1
                If Nodes(i).Name.ToLower.Trim = currentparent Then
                    path = Nodes(i).Parent & "\" & path
                    currentparent = Nodes(i).Parent.ToLower.Trim
                End If
            Next
        Loop
        Return path
    End Function
    Public Function GetTreeNodeTree(ByVal TNodes As TreeNode(), ByVal Node As TreeNode) As String
        Dim path As String = Node.Parent.Name & "\" & Node.Name
        Dim oldparent As String = ""
        Dim currentparent As String = Node.Parent.Name.ToLower.Trim
        Do While oldparent <> currentparent
            oldparent = currentparent
            For i As Integer = 0 To TNodes.Count - 1
                If TNodes(i).Name.ToLower.Trim = currentparent Then
                    path = TNodes(i).Parent.Name & "\" & path
                    currentparent = TNodes(i).Parent.Name.ToLower.Trim
                End If
            Next
        Loop
        Return path
    End Function
#End Region

#Region "Structures and structure loading related"
    Public Function LoadNodeData(ByVal Data As String) As Node()
        Data = RemoveDoubleSpaces(Data).Replace(ControlChars.Lf, vbCrLf)
        Dim RNodes(-1) As Node
        Dim FailedLines As String = ""
        Dim isinrem As Boolean = False
        For Each line As String In Data.Split(vbCrLf)
            If line.ToLower.Trim.StartsWith("beginrem") Then isinrem = True
            If line.ToLower.Trim.StartsWith("endrem") Then isinrem = False
            If isinrem = False And Not line.ToLower.Trim.StartsWith("rem ") And Not line.ToLower.Trim.StartsWith("beginrem") And Not line.ToLower.Trim.StartsWith("endrem") And Not line.ToLower.Trim.StartsWith("# ") And Not line.Trim = "" Then
                'New node generation
                If line.ToLower.Trim.StartsWith("hudbuilder.createcompassnode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Type As Integer = Val(GetValueAt(line, 3))
                    Dim Position As New Point(Val(GetValueAt(line, 4)), Val(GetValueAt(line, 5)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 7)), 1, 2048))
                    If Parent = "" Then Parent = "no_parent"
                    If Name = "" Then Name = "no_name"
                    If Size.Width = 0 Then Size.Width = 32
                    If Size.Height = 0 Then Size.Height = 32
                    If Type <> 3 And Type <> 0 Then Type = 3
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Parent, Name, "Compass Node")
                    RNodes(RNodes.Count - 1).CompassNodeData.Position = Position
                    RNodes(RNodes.Count - 1).CompassNodeData.Size = Size
                    RNodes(RNodes.Count - 1).CompassNodeData.Type = Type
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createtextnode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Position As New Point(Val(GetValueAt(line, 3)), Val(GetValueAt(line, 4)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 5)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048))
                    If Parent = "" Then Parent = "no_parent"
                    If Name = "" Then Name = "no_name"
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Parent, Name, "Text Node")
                    RNodes(RNodes.Count - 1).TextNodeData.Position = Position
                    RNodes(RNodes.Count - 1).TextNodeData.Size = Size
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createpicturenode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Position As New Point(Val(GetValueAt(line, 3)), Val(GetValueAt(line, 4)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 5)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048))
                    If Parent = "" Then Parent = "no_parent"
                    If Name = "" Then Name = "no_name"
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Parent, Name, "Picture Node")
                    RNodes(RNodes.Count - 1).PictureNodeData.Position = Position
                    RNodes(RNodes.Count - 1).PictureNodeData.Size = Size
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createsplitnode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Parent, Name, "Split Node")
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createbarnode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Style As Integer = SetValueBounds(Val(GetValueAt(line, 3)), 0, 3)
                    Dim Position As New Point(Val(GetValueAt(line, 4)), Val(GetValueAt(line, 5)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 7)), 1, 2048))
                    If Parent = "" Then Parent = "no_parent"
                    If Name = "" Then Name = "no_name"
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Parent, Name, "Bar Node")
                    RNodes(RNodes.Count - 1).BarNodeData.Style = Style
                    RNodes(RNodes.Count - 1).BarNodeData.Position = Position
                    RNodes(RNodes.Count - 1).BarNodeData.Size = Size
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createhovernode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Position As New Point(Val(GetValueAt(line, 3)), Val(GetValueAt(line, 4)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 5)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048))
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Parent, Name, "Hover Node")
                    RNodes(RNodes.Count - 1).HoverNodeData.Position = Position
                    RNodes(RNodes.Count - 1).HoverNodeData.Size = Size
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createobjectmarkernode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Position As New Point(Val(GetValueAt(line, 3)), Val(GetValueAt(line, 4)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 5)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048))
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Parent, Name, "Object Marker Node")
                    RNodes(RNodes.Count - 1).ObjectMarkerNodeData.Position = Position
                    RNodes(RNodes.Count - 1).ObjectMarkerNodeData.Size = Size
                ElseIf RNodes.Count = 0 Then
                    FailedLines &= line
                ElseIf RNodes(RNodes.Count - 1).LoadLine(line) = False Then
                    FailedLines &= line
                End If
            End If
        Next
        If FailedLines <> "" Then WriteLog("Failed to process line(s): " & vbCrLf & FailedLines)
        Return RNodes
    End Function

    Public Structure Node
        Sub New(ByVal Parent As String, ByVal Name As String, ByVal Type As String)
            Me.Parent = Parent
            Me.Name = Name
            Me.Type = Type
            Me.Render = True
            Me.ShowVariables = New ListBox
            Me.LogicShowVariables = New ListBox
            Me.InTime = 0
            Me.OutTime = 0
            Me.AlphaVariable = ""
            If Type = "Picture Node" Then Me.PictureNodeData = New PictureNode("")
            If Type = "Text Node" Then Me.TextNodeData = New TextNode("100")
            If Type = "Compass Node" Then Me.CompassNodeData = New CompassNode("")
            If Type = "Split Node" Then Me.SplitNodeData = New SplitNode("no_parent", "new_splitnode")
            If Type = "Bar Node" Then Me.BarNodeData = New BarNode("Ingame\GeneralIcons\full.dds", "Ingame\GeneralIcons\empty.dds")
            If Type = "Hover Node" Then Me.HoverNodeData = New HoverNode()
            If Type = "Object Marker Node" Then Me.ObjectMarkerNodeData = New ObjectMarkerNode("", "", "", "")
            PerformRefresh = True
        End Sub
        Function LoadLine(ByVal Line As String) As Boolean
            Dim rval As Boolean = True
            If Line.ToLower.Trim.StartsWith("hudbuilder.setnodelogicshowvariable") Then
                Me.LogicShowVariables.Items.Add(Line.Remove(0, 36).Trim)
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setnodeshowvariable") Then
                Me.ShowVariables.Items.Add(Line.Remove(0, 31).Trim)
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setnodealphavariable") Then
                Me.AlphaVariable = GetValueAt(Line, 1)
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.addnodeblendeffect") Then
                Me.BlendEffectA = Val(GetValueAt(Line, 1))
                Me.BlendEffectB = Val(GetValueAt(Line, 2))
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setnodecolor") Then
                Dim R As Integer = SetValueBounds(GetValueAt(Line, 1).Replace(".", ",").Trim(",") * 255, 0, 255)
                Dim G As Integer = SetValueBounds(GetValueAt(Line, 2).Replace(".", ",").Trim(",") * 255, 0, 255)
                Dim B As Integer = SetValueBounds(GetValueAt(Line, 3).Replace(".", ",").Trim(",") * 255, 0, 255)
                Dim A As Integer = SetValueBounds(GetValueAt(Line, 4).Replace(".", ",").Trim(",") * 255, 0, 255)
                Dim c As Color = Color.FromArgb(A, R, G, B)
                If Me.Type = "Picture Node" Then Me.PictureNodeData.Color = c
                If Me.Type = "Compass Node" Then Me.CompassNodeData.Color = c
                If Me.Type = "Text Node" Then Me.TextNodeData.Color = c
                If Me.Type = "Bar Node" Then Me.BarNodeData.Color = c
                If Me.Type = "Object Marker Node" Then Me.ObjectMarkerNodeData.Color = c
            Else
                If Me.Type = "Picture Node" Then rval = Me.PictureNodeData.LoadLine(Line)
                If Me.Type = "Compass Node" Then rval = Me.CompassNodeData.LoadLine(Line)
                If Me.Type = "Text Node" Then rval = Me.TextNodeData.LoadLine(Line)
                If Me.Type = "Bar Node" Then rval = Me.BarNodeData.LoadLine(Line)
                If Me.Type = "Split Node" Then rval = Me.SplitNodeData.LoadLine(Line)
                If Me.Type = "Hover Node" Then rval = Me.HoverNodeData.LoadLine(Line)
                If Me.Type = "Object Marker Node" Then rval = Me.ObjectMarkerNodeData.LoadLine(Line)
            End If
            Return rval
        End Function
        Function SaveData()
            Dim rdata As String = ""
            If Me.Type = "Picture Node" Then
                With Me.PictureNodeData
                    rdata = "hudBuilder.createPictureNode		" & Me.Parent & " " & Me.Name & " "
                    rdata &= .Position.X & " " & .Position.Y & " " & .Size.Width & " " & .Size.Height
                    If .TexturePath <> "" Then rdata &= vbCrLf & "hudBuilder.setPictureNodeTexture 	" & .TexturePath
                    rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & ConvertColorToText(.Color)
                    If .StaticRotation <> 0 And .StaticRotation <> 360 Then rdata &= vbCrLf & "hudBuilder.setPictureNodeRotation 	" & 360 - .StaticRotation
                    If .DOffsetXVar <> "" Then rdata &= vbCrLf & "hudBuilder.setNodePosVariable		0 " & .DOffsetXVar
                    If .DOffsetYVar <> "" Then rdata &= vbCrLf & "hudBuilder.setNodePosVariable		1 " & .DOffsetYVar
                    If .DRotationVar <> "" Then rdata &= vbCrLf & "hudBuilder.setPictureNodeRotateVariable " & .DRotationVar
                    If .DRotationVar <> "" Then rdata &= vbCrLf & "hudBuilder.setPictureNodeCenterPoint 	" & .DRotationMid.X & " " & .DRotationMid.Y
                End With
            ElseIf Me.Type = "Text Node" Then
                With Me.TextNodeData
                    rdata = "hudBuilder.createTextNode		" & Me.Parent & " " & Me.Name & " "
                    rdata &= .Position.X & " " & .Position.Y & " " & .Size.Width & " " & .Size.Height
                    rdata &= vbCrLf & "hudBuilder.setTextNodeStyle		Fonts/vehicleHudFont_6.dif " & .Style
                    If .StringVariable.Trim = "" Then
                        rdata &= vbCrLf & "hudBuilder.setTextNodeString	"
                        If .StringText.Contains(" ") Then rdata &= Chr(34) & .StringText & Chr(34) Else rdata &= .StringText
                    Else
                        rdata &= vbCrLf & "hudBuilder.setTextNodeStringVariable	" & .StringVariable
                    End If
                    rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & ConvertColorToText(.Color)
                End With
            ElseIf Me.Type = "Compass Node" Then
                With Me.CompassNodeData
                    rdata = "hudBuilder.createCompassNode 		" & Me.Parent & " " & Me.Name & " "
                    rdata &= .Type & " " & .Position.X & " " & .Position.Y & " " & .Size.Width & " " & .Size.Height
                    If .Type = 3 Then rdata &= " 1 0"
                    If .Type = 0 Then rdata &= " 0 1"
                    rdata &= vbCrLf & "hudBuilder.setCompassNodeTexture 	1 " & .TexturePath
                    rdata &= vbCrLf & "hudBuilder.setCompassNodeTextureSize	" & .TextureSize.Width & " " & .TextureSize.Height
                    If .Type = 3 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeBorder		0 0 0 " & .Border
                    If .Type = 0 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeBorder		0 " & .Border & " 0 0"
                    rdata &= vbCrLf & "hudBuilder.setCompassNodeValueVariable	" & .ValueVariable
                    rdata &= vbCrLf & "hudBuilder.setCompassNodeOffset		" & .Offset
                    If .Type = 3 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeSnapOffset	0 0 " & .OffsetSnapMin & " " & .OffsetSnapMax
                    If .Type = 0 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeSnapOffset	" & .OffsetSnapMin & " " & .OffsetSnapMax & " 0 0"
                    rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & ConvertColorToText(.Color)
                End With
            ElseIf Me.Type = "Split Node" Then
                With Me.SplitNodeData
                    rdata = "hudBuilder.createSplitNode			" & Me.Parent & " " & Me.Name
                    If Me.BlendEffectA <> 0 And Me.BlendEffectB <> 0 Then rdata &= vbCrLf & "hudBuilder.addNodeBlendEffect		" & Me.BlendEffectA & " " & Me.BlendEffectB
                End With
            ElseIf Me.Type = "Bar Node" Then
                With Me.BarNodeData
                    rdata = "hudBuilder.createBarNode 		" & Me.Parent & " " & Me.Name & " " & .Style & " "
                    rdata &= .Position.X & " " & .Position.Y & " " & .Size.Width & " " & .Size.Height
                    If .FullTexturePath <> "" Then rdata &= vbCrLf & "hudbuilder.setBarNodeTexture		1 " & .FullTexturePath
                    If .EmptyTexturePath <> "" Then rdata &= vbCrLf & "hudbuilder.setBarNodeTexture		2 " & .EmptyTexturePath
                    rdata &= vbCrLf & "hudBuilder.setBarNodeValueVariable 	" & .ValueVariable
                    rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & ConvertColorToText(.Color)
                End With
            ElseIf Me.Type = "Hover Node" Then
                With Me.HoverNodeData
                    rdata = "hudBuilder.createHoverNode		" & Me.Parent & " " & Me.Name & " "
                    rdata &= .Position.X & " " & .Position.Y & " " & .Size.Width & " " & .Size.Height
                    rdata &= vbCrLf & "hudBuilder.setHoverInMiddlePos		" & .MiddlePos.X & " " & .MiddlePos.Y
                    rdata &= vbCrLf & "hudBuilder.setHoverMaxValue		" & .MaxValue
                    rdata &= vbCrLf & "hudBuilder.setHoverWidthLength		" & .WidthLength.Width & " " & .WidthLength.Height
                End With
            ElseIf Me.Type = "Object Marker Node" Then
                With Me.ObjectMarkerNodeData
                    rdata = "hudBuilder.createObjectMarkerNode 		" & Me.Parent & " " & Me.Name & " "
                    rdata &= .Position.X & " " & .Position.Y & " " & .Size.Width & " " & .Size.Height
                    If .FriendlyTexturePath <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTexture 		0 " & .FriendlyTexturePath
                    If .EnemyTexturePath <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTexture 		1 " & .EnemyTexturePath
                    If .LockedTexturePath <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTexture 		2 " & .LockedTexturePath
                    If .RangeLineTexturePath <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTexture 		3 " & .RangeLineTexturePath
                    If .FriendlyTexturePath <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTextureSize	0 " & .FriendlyTextureSize.Width & " " & .FriendlyTextureSize.Height
                    If .EnemyTexturePath <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTextureSize	1 " & .EnemyTextureSize.Width & " " & .EnemyTextureSize.Height
                    If .LockedTexturePath <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTextureSize	2 " & .LockedTextureSize.Width & " " & .LockedTextureSize.Height
                    If .RangeLineTexturePath <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTextureSize	3 " & .RangeLineTextureSize.Width & " " & .RangeLineTextureSize.Height
                    If .LockOnType <> 0 Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeLockOnType	" & .LockOnType
                    rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeObjects 		" & .omnobjects
                    If .LockTextString <> "" Then
                        rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeLockText 		" & .LockTextStyle & " " & .LockTextString
                        rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeLockTextOffset	" & .LockTextOffSet.X & " " & .LockTextOffSet.Y
                    End If
                    For Each tnode As String In .LockTextNodes.Items
                        rdata &= vbCrLf & "hudBuilder.addObjectMarkerNodeLockTextNode	" & tnode
                    Next
                    If .omnweapon <> 0 Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeWeapon 		" & .omnweapon
                    rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & ConvertColorToText(.Color)
                End With
            End If


            'All node specific data
            For Each showvar As String In Me.LogicShowVariables.Items
                rdata &= vbCrLf & "hudBuilder.setNodeLogicShowVariable 		" & showvar
            Next
            For Each showvar As String In Me.ShowVariables.Items
                rdata &= vbCrLf & "hudBuilder.setNodeShowVariable 		" & showvar
            Next
            If Me.AlphaVariable <> "" Then rdata &= vbCrLf & "hudBuilder.setNodeAlphaVariable     	" & Me.AlphaVariable
            Return rdata
        End Function

        Public Name As String
        Public Parent As String
        Public Type As String
        Public ShowVariables As ListBox
        Public LogicShowVariables As ListBox
        Public AlphaVariable As String
        Public BlendEffectA As Integer
        Public BlendEffectB As Integer
        Public InTime As Single
        Public OutTime As Single
        Private _Render As Boolean
        Public Property Render() As Boolean
            Get
                Return _Render
            End Get
            Set(ByVal value As Boolean)
                _Render = value
                Me.PerformRefresh = True
            End Set
        End Property
        Public PerformRefresh As Boolean
        Public PictureNodeData As PictureNode
        Public TextNodeData As TextNode
        Public CompassNodeData As CompassNode
        Public SplitNodeData As SplitNode
        Public BarNodeData As BarNode
        Public HoverNodeData As HoverNode
        Public ObjectMarkerNodeData As ObjectMarkerNode

        Public Property Position() As Point
            Get
                If Me.Type = "Picture Node" Then
                    Return Me.PictureNodeData.Position
                ElseIf Me.Type = "Compass Node" Then
                    Return Me.CompassNodeData.Position
                ElseIf Me.Type = "Text Node" Then
                    Return Me.TextNodeData.Position
                ElseIf Me.Type = "Bar Node" Then
                    Return Me.BarNodeData.Position
                ElseIf Me.Type = "Hover Node" Then
                    Return Me.HoverNodeData.Position
                ElseIf Me.Type = "Object Marker Node" Then
                    Return Me.ObjectMarkerNodeData.Position
                End If
                Return New Point(0, 0)
            End Get
            Set(ByVal value As Point)
                Dim vchange As Boolean = False
                If ViewedDialog = 3 Then
                    Try
                        MainDialog.NumericUpDown1.Value = value.X
                        MainDialog.NumericUpDown2.Value = value.Y
                    Catch
                        vchange = True
                    End Try
                Else
                    vchange = True
                End If
                If vchange = True Then
                    If Me.Type = "Picture Node" Then
                        Me.PictureNodeData.Position = value
                        Me.PictureNodeData.PosRotChanged = True
                    ElseIf Me.Type = "Compass Node" Then
                        Me.CompassNodeData.Position = value
                        Me.CompassNodeData.ValueChanged = True
                    ElseIf Me.Type = "Text Node" Then
                        Me.TextNodeData.Position = value
                        Me.TextNodeData.Modified = True
                    ElseIf Me.Type = "Bar Node" Then
                        Me.BarNodeData.Position = value
                        Me.BarNodeData.ValueChanged = True
                    ElseIf Me.Type = "Hover Node" Then
                        Me.HoverNodeData.Position = value
                    ElseIf Me.Type = "Object Marker Node" Then
                        Me.ObjectMarkerNodeData.Position = value
                    End If
                End If
            End Set
        End Property
        Public Property Size() As Size
            Get
                If Me.Type = "Picture Node" Then
                    Return Me.PictureNodeData.Size
                ElseIf Me.Type = "Compass Node" Then
                    Return Me.CompassNodeData.Size
                ElseIf Me.Type = "Text Node" Then
                    Return Me.TextNodeData.Size
                ElseIf Me.Type = "Bar Node" Then
                    Return Me.BarNodeData.Size
                ElseIf Me.Type = "Hover Node" Then
                    Return Me.HoverNodeData.Size
                ElseIf Me.Type = "Object Marker Node" Then
                    Return Me.ObjectMarkerNodeData.Size
                End If
                Return New Size(1, 1)
            End Get
            Set(ByVal value As Size)
                Dim vchange As Boolean = False
                If ViewedDialog = 3 Then
                    Try
                        MainDialog.NumericUpDown3.Value = value.Width
                        MainDialog.NumericUpDown4.Value = value.Height
                    Catch
                        vchange = True
                    End Try
                Else
                    vchange = True
                End If
                If vchange = True Then
                    If Me.Type = "Picture Node" Then
                        Me.PictureNodeData.Size = value
                        Me.PictureNodeData.SizeChanged = True
                    ElseIf Me.Type = "Compass Node" Then
                        Me.CompassNodeData.Size = value
                        Me.CompassNodeData.ValueChanged = True
                    ElseIf Me.Type = "Text Node" Then
                        Me.TextNodeData.Size = value
                        Me.TextNodeData.Modified = True
                    ElseIf Me.Type = "Bar Node" Then
                        Me.BarNodeData.Size = value
                        Me.BarNodeData.ValueChanged = True
                    ElseIf Me.Type = "Hover Node" Then
                        Me.HoverNodeData.Size = value
                    ElseIf Me.Type = "Object Marker Node" Then
                        Me.ObjectMarkerNodeData.Size = value
                    End If
                End If
            End Set
        End Property
        Public Property Rotation() As Integer
            Get
                If Me.Type = "Picture Node" Then
                    Return Me.PictureNodeData.StaticRotation
                End If
                Return 0
            End Get
            Set(ByVal value As Integer)
                Dim vchange As Boolean = False
                If ViewedDialog = 5 Then
                    Try
                        RotationDialog.NumericUpDown1.Value = value
                    Catch
                        vchange = True
                    End Try
                Else
                    vchange = True
                End If
                If vchange = True Then
                    If Me.Type = "Picture Node" Then
                        Me.PictureNodeData.StaticRotation = value
                        Me.PictureNodeData.PosRotChanged = True
                    End If
                End If
            End Set
        End Property

    End Structure
    Public Structure SplitNode
        Sub New(ByVal Parent As String, ByVal Name As String)
            Me.Name = Name
            Me.Parent = Parent
        End Sub
        Function LoadLine(ByVal Line As String) As Boolean
            Dim rval As Boolean = True
            'Line failed to load
            rval = False
            Return rval
        End Function

        Public Name As String
        Public Parent As String
    End Structure
    Public Structure PictureNode
        'Render cycle:
        '1. Color it
        '2. Size it
        '3. Draw it on the surface
        Sub New(ByVal TexturePath As String)
            Me.TexturePath = TexturePath
            Me.TextureImage = LoadImage(TexturePath)
            Me.SizedImage = New Bitmap(32, 32)
            Me.ColoredImage = New Bitmap(32, 32)
            Me.FinalImage = New Bitmap(32, 32)
            Me.Color = Color.FromArgb(255, 255, 255, 255)
            Me.Position = New Point(0, 0)
            Me.Size = Me.TextureImage.Size
            Me.StaticRotation = 0
            Me.DRotation = 0
            Me.DRotationMid = New Point(0, 0)
            Me.DRotationVar = ""
            Me.DOffsetX = 0
            Me.DOffsetY = 0
            Me.DOffsetXVar = ""
            Me.DOffsetYVar = ""
            Me.ColorChanged = True
            Me.SizeChanged = True
            Me.PosRotChanged = True
        End Sub
        Function LoadLine(ByVal Line As String) As Boolean
            Dim rval As Boolean = True
            If Line.ToLower.Trim.StartsWith("hudbuilder.setpicturenodetexture") Then
                Me.TexturePath = FixTexturePath(GetValueAt(Line, 1))
                Me.TextureImage = LoadImage(Me.TexturePath)
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setnodeposvariable") Then
                If Val(GetValueAt(Line, 1)) = 0 Then Me.DOffsetXVar = GetValueAt(Line, 2)
                If Val(GetValueAt(Line, 1)) = 1 Then Me.DOffsetYVar = GetValueAt(Line, 2)
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setpicturenoderotatevariable") Then
                Me.DRotationVar = GetValueAt(Line, 1)
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setpicturenodecenterpoint") Then
                Me.DRotationMid = New Point(Val(GetValueAt(Line, 1)), Val(GetValueAt(Line, 2)))
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setpicturenoderotation") Then
                Dim srotation As Integer = Val(GetValueAt(Line, 1))
                Do While srotation < 0
                    srotation += 360
                Loop
                If srotation = 0 Then srotation = 360
                Me.StaticRotation = 360 - srotation
            Else
                'Line failed to load
                rval = False
            End If
            Return rval
        End Function

        Public TexturePath As String
        Public TextureImage As Image
        Public Color As Color
        Public Position As Point
        Public Size As Size
        Public StaticRotation As Integer

        'Dynamic variables (simulator)
        Public DRotation As Integer
        Public DRotationMid As Point
        Public DRotationVar As String
        Public DOffsetX As Integer
        Public DOffsetY As Integer
        Public DOffsetXVar As String
        Public DOffsetYVar As String

        'Node render information
        Public ColoredImage As Image
        Public SizedImage As Image
        Public FinalImage As Image
        Public ColorChanged As Boolean
        Public SizeChanged As Boolean
        Public PosRotChanged As Boolean
    End Structure
    Public Structure TextNode
        Sub New(ByVal Text As String)
            Me.Text = Text
            Me.FinalImage = New Bitmap(32, 32)
            Me.Position = New Point(0, 0)
            Me.Size = New Size(40, 10)
            Me.Style = 0
            Me.StringVariable = ""
            Me.StringText = ""
            Me.Color = Color.FromArgb(255, 255, 255, 255)
            Me.Modified = True
        End Sub
        Function LoadLine(ByVal Line As String) As Boolean
            Dim rval As Boolean = True
            If Line.ToLower.Trim.StartsWith("hudbuilder.settextnodestyle") Then
                Me.Style = SetValueBounds(Val(GetValueAt(Line, 2)), 0, 2)
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.settextnodestringvariable") Then
                Me.StringVariable = GetValueAt(Line, 1)
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.settextnodestring") Then
                Me.StringText = GetValueAt(Line, 1).Trim(Chr(34))
            Else
                'Line failed to load
                rval = False
            End If
            Return rval
        End Function

        Public Color As Color
        Public Text As String
        Public StringVariable As String
        Public StringText As String
        Public Position As Point
        Public Size As Size
        Public Style As Integer
        Public Modified As Boolean
        Public FinalImage As Image
    End Structure
    Public Structure CompassNode
        Sub New(ByVal TexturePath As String)
            Me.TexturePath = TexturePath
            Me.TextureImage = LoadImage(TexturePath)
            Me.SizedImage = New Bitmap(32, 32)
            Me.ColoredImage = New Bitmap(32, 32)
            Me.FinalImage = New Bitmap(32, 32)
            Me.Color = Color.FromArgb(255, 255, 255, 255)
            Me.Position = New Point(0, 0)
            Me.TextureSize = Me.TextureImage.Size
            Me.Size = New Size(32, 32)
            Me.ValueVariable = ""
            Me.Type = 3
            Me.Value = 0
            Me.Border = 0
            Me.OffsetSnapMin = 0
            Me.OffsetSnapMax = 0
            Me.Offset = 0
            Me.ColorChanged = True
            Me.SizeChanged = True
            Me.ValueChanged = True
        End Sub
        Function LoadLine(ByVal Line As String) As Boolean
            Dim rval As Boolean = True
            If Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodetexturesize") Then
                Me.TextureSize.Width = Val(GetValueAt(Line, 1))
                Me.TextureSize.Height = Val(GetValueAt(Line, 2))
                If Me.TextureSize.Width = 0 Then Me.TextureSize.Width = 32
                If Me.TextureSize.Height = 0 Then Me.TextureSize.Height = 32
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodetexture") Then
                Me.TexturePath = FixTexturePath(GetValueAt(Line, 2))
                Me.TextureImage = LoadImage(Me.TexturePath)
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodeborder") Then
                If Me.Type = 3 Then Me.Border = Val(GetValueAt(Line, 4))
                If Me.Type = 0 Then Me.Border = Val(GetValueAt(Line, 2))
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodevaluevariable") Then
                Me.ValueVariable = GetValueAt(Line, 1)
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodesnapoffset") Then
                If Me.Type = 0 Then Me.OffsetSnapMin = Val(GetValueAt(Line, 1))
                If Me.Type = 0 Then Me.OffsetSnapMax = Val(GetValueAt(Line, 2))
                If Me.Type = 3 Then Me.OffsetSnapMin = Val(GetValueAt(Line, 3))
                If Me.Type = 3 Then Me.OffsetSnapMax = Val(GetValueAt(Line, 4))
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodeoffset") Then
                Me.Offset = Val(GetValueAt(Line, 1))
            Else
                'Line failed to load
                rval = False
            End If
            Return rval
        End Function

        Public TexturePath As String
        Public TextureImage As Image
        Public Color As Color
        Public Size As Size
        Public TextureSize As Size
        Public Position As Point
        Public ValueVariable As String
        Public Value As Single
        Public Border As Integer
        Public Offset As Integer
        Public OffsetSnapMax As Integer
        Public OffsetSnapMin As Integer
        Public Type As Integer '0=vertical; 3=horizontal

        'Node render information
        Public ColoredImage As Image
        Public SizedImage As Image
        Public FinalImage As Image
        Public ColorChanged As Boolean
        Public SizeChanged As Boolean
        Public ValueChanged As Boolean
    End Structure
    Public Structure BarNode
        Sub New(ByVal FullTexturePath As String, ByVal EmptyTexturePath As String)
            Me.FullTexturePath = FullTexturePath
            Me.FullTextureImage = LoadImage(FullTexturePath)
            Me.EmptyTexturePath = EmptyTexturePath
            Me.EmptyTextureImage = LoadImage(EmptyTexturePath)
            Me.Style = 0
            Me.Value = 0.5
            Me.ValueVariable = ""
            Me.Position = New Point(0, 0)
            Me.Size = Me.FullTextureImage.Size
            Me.Color = Color.FromArgb(255, 255, 255, 255)
            Me.ColoredFullImage = New Bitmap(32, 32)
            Me.SizedFullImage = New Bitmap(32, 32)
            Me.ColoredEmptyImage = New Bitmap(32, 32)
            Me.SizedEmptyImage = New Bitmap(32, 32)
            Me.FinalImage = New Bitmap(32, 32)
            Me.ColorChanged = True
            Me.SizeChanged = True
            Me.ValueChanged = True
        End Sub
        Function LoadLine(ByVal Line As String) As Boolean
            Dim rval As Boolean = True
            If Line.ToLower.Trim.StartsWith("hudbuilder.setbarnodetexture") Then
                If Val(GetValueAt(Line, 1)) = 1 Then
                    Me.FullTexturePath = FixTexturePath(GetValueAt(Line, 2))
                    Me.FullTextureImage = LoadImage(Me.FullTexturePath)
                ElseIf Val(GetValueAt(Line, 1)) = 2 Then
                    Me.EmptyTexturePath = FixTexturePath(GetValueAt(Line, 2))
                    Me.EmptyTextureImage = LoadImage(Me.EmptyTexturePath)
                End If
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setbarnodevaluevariable") Then
                Me.ValueVariable = GetValueAt(Line, 1)
            Else
                'Line failed to load
                rval = False
            End If
            Return rval
        End Function

        Public FullTexturePath As String
        Public FullTextureImage As Image
        Public EmptyTexturePath As String
        Public EmptyTextureImage As Image
        Public Style As Integer
        'types:
        '0 = vertical increasing from below
        '1 = vertical incresing from above
        '2 = horizontal increasing from right
        '3 = horizontal increasing from left
        Public ValueVariable As String
        Public Position As Point
        Public Size As Size
        Public Color As Color

        'Simulation variables
        Public Value As Single

        'Node render information
        Public ColoredFullImage As Image
        Public SizedFullImage As Image
        Public ColoredEmptyImage As Image
        Public SizedEmptyImage As Image

        Public FinalImage As Image
        Public ColorChanged As Boolean
        Public SizeChanged As Boolean
        Public ValueChanged As Boolean
    End Structure
    Public Structure HoverNode
        Sub New(ByVal PlaceHolder As Boolean)
            Me.Position = New Point(0, 0)
            Me.Size = New Size(32, 32)
            Me.MiddlePos = New Point(0, 0)
            Me.MaxValue = 1
            Me.WidthLength = New Size(32, 32)
        End Sub
        Function LoadLine(ByVal Line As String) As Boolean
            Dim rval As Boolean = True
            If Line.ToLower.Trim.StartsWith("hudbuilder.sethoverinmiddlepos") Then
                Me.MiddlePos = New Point(Val(GetValueAt(Line, 1)), Val(GetValueAt(Line, 2)))
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.sethovermaxvalue") Then
                Me.MaxValue = Val(GetValueAt(Line, 1))
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.sethoverwidthlength") Then
                Me.WidthLength = New Size(Val(GetValueAt(Line, 1)), Val(GetValueAt(Line, 2)))
            Else
                'Line failed to load
                rval = False
            End If
            Return rval
        End Function
        Public Position As Point
        Public Size As Size
        Public MiddlePos As Point
        Public MaxValue As Single
        Public WidthLength As Size
    End Structure
    Public Structure ObjectMarkerNode
        Sub New(ByVal FriendlyTpath As String, ByVal EnemyTpath As String, ByVal RangeLineTpath As String, ByVal LockedTpath As String)
            Me.Position = New Point(0, 0)
            Me.Size = New Size(32, 32)
            Me.FriendlyTexturePath = FriendlyTpath
            Me.EnemyTexturePath = EnemyTpath
            Me.RangeLineTexturePath = RangeLineTpath
            Me.LockedTexturePath = LockedTpath
            Me.FriendlyTextureImage = LoadImage(FriendlyTpath)
            Me.EnemyTextureImage = LoadImage(EnemyTpath)
            Me.LockedTextureImage = LoadImage(LockedTpath)
            Me.RangeLineTextureImage = LoadImage(RangeLineTpath)
            Me.FriendlyTextureSize = Me.FriendlyTextureImage.Size
            Me.EnemyTextureSize = Me.EnemyTextureImage.Size
            Me.LockedTextureSize = Me.LockedTextureImage.Size
            Me.RangeLineTextureSize = Me.RangeLineTextureImage.Size
            Me.omnobjects = 0
            If Me.FriendlyTexturePath <> "" Then Me.omnobjects += 1
            If Me.EnemyTexturePath <> "" Then Me.omnobjects += 1
            If Me.LockedTexturePath <> "" Then Me.omnobjects += 1
            If Me.RangeLineTexturePath <> "" Then Me.omnobjects += 1
            Me.omnweapon = 0
            Me.LockOnType = 0
            Me.LockTextOffSet = New Point(0, 0)
            Me.LockTextString = ""
            Me.LockTextStyle = 0
            Me.LockTextNodes = New ListBox
            Me.Color = Color.FromArgb(255, 255, 255, 255)
        End Sub
        Function LoadLine(ByVal Line As String) As Boolean
            Dim rval As Boolean = True
            If Line.ToLower.Trim.StartsWith("hudbuilder.setobjectmarkernodetexture") Then
                Dim tpath As String = FixTexturePath(GetValueAt(Line, 2))
                If Val(GetValueAt(Line, 1)) = 0 Then
                    Me.FriendlyTexturePath = tpath
                ElseIf Val(GetValueAt(Line, 1)) = 1 Then
                    Me.EnemyTexturePath = tpath
                ElseIf Val(GetValueAt(Line, 1)) = 2 Then
                    Me.LockedTexturePath = tpath
                ElseIf Val(GetValueAt(Line, 1)) = 3 Then
                    Me.RangeLineTexturePath = tpath
                End If
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setobjectmarkernodetexturesize") Then
                Dim tsize As Size = New Size(SetValueBounds(Val(GetValueAt(Line, 2)), 1, 2048), SetValueBounds(Val(GetValueAt(Line, 2)), 1, 2048))
                If Val(GetValueAt(Line, 1)) = 0 Then
                    Me.FriendlyTextureSize = tsize
                ElseIf Val(GetValueAt(Line, 1)) = 1 Then
                    Me.EnemyTextureSize = tsize
                ElseIf Val(GetValueAt(Line, 1)) = 2 Then
                    Me.LockedTextureSize = tsize
                ElseIf Val(GetValueAt(Line, 1)) = 3 Then
                    Me.RangeLineTextureSize = tsize
                End If
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setobjectmarkernodeobjects") Then
                Me.omnobjects = Val(GetValueAt(Line, 1))
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setobjectmarkernodeweapon") Then
                Me.omnweapon = Val(GetValueAt(Line, 1))
            ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setobjectmarkernodelockontype") Then
                Me.LockOnType = Val(GetValueAt(Line, 1))
            ElseIf Line.ToLower.Trim.StartsWith("hudBuilder.setobjectmarkernodelocktextoffset") Then
                Me.LockTextOffSet = New Point(Val(GetValueAt(Line, 1)), Val(GetValueAt(Line, 2)))
            ElseIf Line.ToLower.Trim.StartsWith("hudBuilder.setobjectmarkernodelocktext") Then
                Me.LockTextStyle = Val(GetValueAt(Line, 1))
                Me.LockTextString = GetValueAt(Line, 2)
            ElseIf Line.ToLower.StartsWith("hudbuilder.addobjectmarkernodelocktextnode") Then
                Me.LockTextNodes.Items.Add(GetValueAt(Line, 1))
            Else
                'Line failed to load
                rval = False
            End If
            Return rval
        End Function
        Public Color As Color
        Public Position As Point
        Public Size As Size

        Public FriendlyTexturePath As String
        Public EnemyTexturePath As String
        Public RangeLineTexturePath As String
        Public LockedTexturePath As String
        Public FriendlyTextureImage As Image
        Public EnemyTextureImage As Image
        Public RangeLineTextureImage As Image
        Public LockedTextureImage As Image
        Public FriendlyTextureSize As Size
        Public EnemyTextureSize As Size
        Public RangeLineTextureSize As Size
        Public LockedTextureSize As Size
        Public omnobjects As Integer
        Public omnweapon As Integer
        Public LockOnType As Integer
        Public LockTextString As String
        Public LockTextStyle As Integer
        Public LockTextOffSet As Point
        Public LockTextNodes As ListBox
    End Structure
#End Region

End Module
