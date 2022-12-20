Imports System.Drawing.Imaging

Module Module1
    Public ApplicationLoaded As Boolean = False
    Public LibraryTextures(-1) As ImagePointer

    Public TexturesPath As String = "Textures"

    Public Variables As New List(Of VariableHandler)
    Public RootVariables(-1) As VariableHandler
    Public IsInSimulationMode As Boolean = False
    Public BGname As String = "Generic background.jpg"
    Public OVname As String = "No Overlay.gif"

#Region "NON-node related Program functions"
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
        TexturePath = TexturePath.Replace("/", "\").Replace(Chr(34), "")
        TexturePath = TexturePath.Replace(Chr(34), "").Replace("|", "").Replace("*", "").Replace("?", "").Replace("<", "").Replace(">", "")
        If TexturePath.ToLower.StartsWith(Application.StartupPath.ToLower) Then TexturePath = TexturePath.Remove(0, Application.StartupPath.Length).Trim("\")
        If System.IO.File.Exists(TexturesPath & "\" & TexturePath) Then Return TexturesPath & "\" & TexturePath
        Try
            If Not System.IO.File.Exists(TexturePath) And Not System.IO.File.Exists(Application.StartupPath & "\" & TexturePath) Then
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
            End If
        Catch
            MsgBox(TexturePath)
        End Try
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
    Public Sub SetCBSelectedItem(ByRef cb As ComboBox, ByVal Item As String, Optional ByVal SetNothing As Boolean = True)
        Item = Item.ToLower.Trim
        If SetNothing = True Then cb.SelectedIndex = -1
        For i As Integer = 0 To cb.Items.Count - 1
            If cb.Items(i).tolower.trim = Item Then cb.SelectedIndex = i
        Next
    End Sub
    Public Function CombineBounds(ByVal Bound1 As Rectangle, ByVal Bound2 As Rectangle) As Rectangle
        If IsNothing(Bound1) And Not IsNothing(Bound2) Then Return Bound2
        If Not IsNothing(Bound1) And IsNothing(Bound2) Then Return Bound1
        If IsNothing(Bound1) And IsNothing(Bound2) Then Return New Rectangle(0, 0, 0, 0)
        CombineBounds = New Rectangle(Bound1.Location, New Size(0, 0))
        If Bound2.X < CombineBounds.X Then CombineBounds.X = Bound2.X
        If Bound2.Y < CombineBounds.Y Then CombineBounds.Y = Bound2.Y
        Dim bottomright As New Point(Bound1.X + Bound1.Width, Bound1.Y + Bound1.Height)
        If bottomright.X < Bound2.X + Bound2.Width Then bottomright.X = Bound2.X + Bound2.Width
        If bottomright.Y < Bound2.Y + Bound2.Height Then bottomright.Y = Bound2.Y + Bound2.Height
        CombineBounds.Width = bottomright.X - CombineBounds.Location.X
        CombineBounds.Height = bottomright.Y - CombineBounds.Location.Y
    End Function
    Public Function RotatePoint(ByVal Point As Point, ByVal BasePoint As Point, ByVal Rotation As Single) As Point
        Dim width As Integer = Math.Abs(BasePoint.X - Point.X)
        Dim height As Integer = Math.Abs(BasePoint.Y - Point.Y)
        Dim r As Double = Math.Sqrt(width ^ 2 + height ^ 2)
        If r = 0 Then
            Return Point
        Else
            Rotation *= Math.PI / 180
            If Point.X >= BasePoint.X And Point.Y >= BasePoint.Y Then
                Rotation += Math.Atan(height / width)
            ElseIf Point.X <= BasePoint.X And Point.Y >= BasePoint.Y Then
                Rotation += Math.Atan(width / height) + 0.5 * Math.PI
            ElseIf Point.X <= BasePoint.X And Point.Y <= BasePoint.Y Then
                Rotation += Math.Atan(height / width) + Math.PI
            ElseIf Point.X >= BasePoint.X And Point.Y <= BasePoint.Y Then
                Rotation += Math.Atan(width / height) + 1.5 * Math.PI
            End If
            Dim x As Single = Math.Cos(Rotation) * r + BasePoint.X
            Dim y As Single = Math.Sin(Rotation) * r + BasePoint.Y
            Return New Point(x, y)
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
    Public Function CombineMatrices(ByVal Matrix1 As System.Drawing.Imaging.ColorMatrix, ByVal Matrix2 As System.Drawing.Imaging.ColorMatrix) As System.Drawing.Imaging.ColorMatrix
        Matrix1.Matrix00 *= Matrix2.Matrix00
        Matrix1.Matrix01 *= Matrix2.Matrix01
        Matrix1.Matrix02 *= Matrix2.Matrix02
        Matrix1.Matrix03 *= Matrix2.Matrix03
        Matrix1.Matrix04 *= Matrix2.Matrix04
        Matrix1.Matrix10 *= Matrix2.Matrix10
        Matrix1.Matrix11 *= Matrix2.Matrix11
        Matrix1.Matrix12 *= Matrix2.Matrix12
        Matrix1.Matrix13 *= Matrix2.Matrix13
        Matrix1.Matrix14 *= Matrix2.Matrix14
        Matrix1.Matrix20 *= Matrix2.Matrix20
        Matrix1.Matrix21 *= Matrix2.Matrix21
        Matrix1.Matrix22 *= Matrix2.Matrix22
        Matrix1.Matrix23 *= Matrix2.Matrix23
        Matrix1.Matrix24 *= Matrix2.Matrix24
        Matrix1.Matrix30 *= Matrix2.Matrix30
        Matrix1.Matrix31 *= Matrix2.Matrix31
        Matrix1.Matrix32 *= Matrix2.Matrix32
        Matrix1.Matrix33 *= Matrix2.Matrix33
        Matrix1.Matrix34 *= Matrix2.Matrix34
        Matrix1.Matrix40 *= Matrix2.Matrix40
        Matrix1.Matrix41 *= Matrix2.Matrix41
        Matrix1.Matrix42 *= Matrix2.Matrix42
        Matrix1.Matrix43 *= Matrix2.Matrix43
        Matrix1.Matrix44 *= Matrix2.Matrix44
        Return Matrix1
    End Function
    Public Function Equal(ByVal Matrix1 As System.Drawing.Imaging.ColorMatrix, ByVal Matrix2 As System.Drawing.Imaging.ColorMatrix) As Boolean
        If IsNothing(Matrix1) Then Return False
        If IsNothing(Matrix2) Then Return False
        If Matrix1.Matrix00 <> Matrix2.Matrix00 Then Return False
        If Matrix1.Matrix01 <> Matrix2.Matrix01 Then Return False
        If Matrix1.Matrix02 <> Matrix2.Matrix02 Then Return False
        If Matrix1.Matrix03 <> Matrix2.Matrix03 Then Return False
        If Matrix1.Matrix04 <> Matrix2.Matrix04 Then Return False
        If Matrix1.Matrix10 <> Matrix2.Matrix10 Then Return False
        If Matrix1.Matrix11 <> Matrix2.Matrix11 Then Return False
        If Matrix1.Matrix12 <> Matrix2.Matrix12 Then Return False
        If Matrix1.Matrix13 <> Matrix2.Matrix13 Then Return False
        If Matrix1.Matrix14 <> Matrix2.Matrix14 Then Return False
        If Matrix1.Matrix20 <> Matrix2.Matrix20 Then Return False
        If Matrix1.Matrix21 <> Matrix2.Matrix21 Then Return False
        If Matrix1.Matrix22 <> Matrix2.Matrix22 Then Return False
        If Matrix1.Matrix23 <> Matrix2.Matrix23 Then Return False
        If Matrix1.Matrix24 <> Matrix2.Matrix24 Then Return False
        If Matrix1.Matrix30 <> Matrix2.Matrix30 Then Return False
        If Matrix1.Matrix31 <> Matrix2.Matrix31 Then Return False
        If Matrix1.Matrix32 <> Matrix2.Matrix32 Then Return False
        If Matrix1.Matrix33 <> Matrix2.Matrix33 Then Return False
        If Matrix1.Matrix34 <> Matrix2.Matrix34 Then Return False
        If Matrix1.Matrix40 <> Matrix2.Matrix40 Then Return False
        If Matrix1.Matrix41 <> Matrix2.Matrix41 Then Return False
        If Matrix1.Matrix42 <> Matrix2.Matrix42 Then Return False
        If Matrix1.Matrix43 <> Matrix2.Matrix43 Then Return False
        If Matrix1.Matrix44 <> Matrix2.Matrix44 Then Return False
        Return True
    End Function
    Public Function GetImgAttr(ByVal Matrix As System.Drawing.Imaging.ColorMatrix)
        Dim img As New System.Drawing.Imaging.ImageAttributes
        img.SetColorMatrix(Matrix)
        Return img
    End Function
#End Region

#Region "Image editing"
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
        TexturePath = TexturePath.Replace(Chr(34), "").Replace("|", "").Replace("*", "").Replace("?", "").Replace("<", "").Replace(">", "")
        If TexturePath.Trim <> "" Then
            On Error Resume Next
            TexturePath = FixTexturePath(TexturePath)
            If System.IO.File.Exists(Application.StartupPath & "\" & TexturesPath & "\" & TexturePath) Then
                TexturePath = Application.StartupPath & "\" & TexturesPath & "\" & TexturePath
            ElseIf System.IO.File.Exists(Application.StartupPath & "\" & TexturePath) Then
                TexturePath = Application.StartupPath & "\" & TexturePath
            End If
            Dim searchinlibrary As Boolean = False
            If System.IO.File.Exists(TexturePath) Then
                LoadImage = Nothing
                If Not IsNothing(LoadImage) Then Return LoadImage
                LoadImage = FreeImageAPI.FreeImage.GetBitmap(FreeImageAPI.FreeImage.LoadEx(TexturePath))
                If Not IsNothing(LoadImage) Then Return LoadImage
                LoadImage = Image.FromFile(TexturePath)
                If Not IsNothing(LoadImage) Then Return LoadImage
            End If
            TexturePath = TexturePath.ToLower.Trim
            If TexturePath.ToLower.StartsWith(Application.StartupPath.ToLower) Then TexturePath = TexturePath.Remove(0, Application.StartupPath.Length).Trim("\").Trim
            TexturePath = StrReverse(StrReverse(TexturePath).Remove(0, IO.Path.GetExtension(TexturePath).Length)).Trim("\").Trim
            For Each img As ImagePointer In LibraryTextures
                Dim imgpath As String = StrReverse(StrReverse(img.Path).Remove(0, IO.Path.GetExtension(img.Path).Length)).Trim("\").Trim.ToLower
                If imgpath = TexturePath Then
                    LoadImage = img.GetImage
                    If Not IsNothing(LoadImage) Then Return LoadImage
                End If
            Next
            WriteLog("Failed to load texture: " & TexturePath)
        End If
        Return New Bitmap(32, 32)
    End Function
    Public Function ResizeImage(ByVal source As Bitmap, ByVal NewSizeX As Integer, ByVal NewSizeY As Integer, ByVal Stretch As Boolean) As Bitmap
        If IsNothing(source) Then Return New Bitmap(32, 32)
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
#End Region

    Public Sub UpdateScreen()
        Form1.MainScreen.UpdateSelectionSquare()
        If Form1.MainScreen.SelectionSquare.Changed = True Then
            Form1.MainScreen.SelectionSquare.UpdateOnScreen()
        End If
        For Each Node As Node In Form1.MainScreen.Root.All
            If Node.Changed = True Then
                Node.UpdateOnScreen()
            End If
        Next
    End Sub
    Public Sub ResetScreenGraphics(ByRef g As Graphics)
        g.ResetTransform()
        g.TranslateTransform(Form1.MainScreen.DrawOffset.X, Form1.MainScreen.DrawOffset.Y)
        g.ScaleTransform(Form1.MainScreen.Scale.X, Form1.MainScreen.Scale.Y)
    End Sub
    Public Sub ResetScreenGraphics(ByRef m As System.Drawing.Drawing2D.Matrix)
        m.Reset()
        m.Translate(Form1.MainScreen.DrawOffset.X, Form1.MainScreen.DrawOffset.Y)
        m.Scale(Form1.MainScreen.Scale.X, Form1.MainScreen.Scale.Y)
    End Sub

    Public ReadOnly Property Inversionmatrix() As Imaging.ColorMatrix
        Get
            Dim m As New System.Drawing.Imaging.ColorMatrix
            m.Matrix00 = -1
            m.Matrix11 = -1
            m.Matrix22 = -1
            m.Matrix33 = 1
            m.Matrix44 = 1
            Return New Imaging.ColorMatrix(New Single()() {New Single() {-1, 0, 0, 0, 0} _
                                                           , New Single() {0, -1, 0, 0, 0} _
                                                           , New Single() {0, 0, -1, 0, 0} _
                                                           , New Single() {0, 0, 0, 1, 0} _
                                                           , New Single() {0, 0, 0, 0, 1}})
        End Get
    End Property


    Public Sub ApplyBlendEffect(ByRef m As System.Drawing.Imaging.ColorMatrix, ByVal EffA As Integer, ByVal EffB As Integer)
        If EffA = 0 And EffB = 2 Then EffB = 0
        If EffA = 0 And EffB = 0 Then
            m.Matrix03 = m.Matrix00 / 3
            m.Matrix13 = m.Matrix11 / 3
            m.Matrix23 = m.Matrix22 / 3
            m.Matrix33 = 0
        ElseIf EffA = 0 And EffB = 1 Then
            m.Matrix33 = 0
            m.Matrix43 = 1
        ElseIf EffA = 0 And EffB = 3 Then
            Dim val1 As Single = Math.Abs(m.Matrix00 - 0.5) + 0.5
            Dim val2 As Single = Math.Abs(m.Matrix11 - 0.5) + 0.5
            Dim val3 As Single = Math.Abs(m.Matrix22 - 0.5) + 0.5
            m.Matrix03 = val1 / 3
            m.Matrix13 = val2 / 3
            m.Matrix23 = val3 / 3
            m.Matrix33 = 0
        End If
    End Sub


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
                    RNodes(RNodes.Count - 1) = New Node(Name, "Compass Node")
                    RNodes(RNodes.Count - 1).Parent = New Node(Parent, "Split Node")
                    RNodes(RNodes.Count - 1).CompassNode.Location.Value = Position
                    RNodes(RNodes.Count - 1).CompassNode.Size.Value = Size
                    RNodes(RNodes.Count - 1).CompassNode.Style = Type
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createtextnode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Position As New Point(Val(GetValueAt(line, 3)), Val(GetValueAt(line, 4)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 5)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048))
                    If Parent = "" Then Parent = "no_parent"
                    If Name = "" Then Name = "no_name"
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Name, "Text Node")
                    RNodes(RNodes.Count - 1).Parent = New Node(Parent, "Split Node")
                    RNodes(RNodes.Count - 1).TextNode.Location.Value = Position
                    RNodes(RNodes.Count - 1).TextNode.Size.Value = Size
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createpicturenode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Position As New Point(Val(GetValueAt(line, 3)), Val(GetValueAt(line, 4)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 5)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048))
                    If Parent = "" Then Parent = "no_parent"
                    If Name = "" Then Name = "no_name"
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Name, "Picture Node")
                    RNodes(RNodes.Count - 1).Parent = New Node(Parent, "Split Node")
                    RNodes(RNodes.Count - 1).PictureNode.Location.Value = Position
                    RNodes(RNodes.Count - 1).PictureNode.Size.Value = Size
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createsplitnode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Name, "Split Node")
                    RNodes(RNodes.Count - 1).Parent = New Node(Parent, "Split Node")
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createbarnode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Style As Integer = SetValueBounds(Val(GetValueAt(line, 3)), 0, 3)
                    Dim Position As New Point(Val(GetValueAt(line, 4)), Val(GetValueAt(line, 5)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 7)), 1, 2048))
                    If Parent = "" Then Parent = "no_parent"
                    If Name = "" Then Name = "no_name"
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Name, "Bar Node")
                    RNodes(RNodes.Count - 1).Parent = New Node(Parent, "Split Node")
                    RNodes(RNodes.Count - 1).BarNode.Style = Style
                    RNodes(RNodes.Count - 1).BarNode.Location.Value = Position
                    RNodes(RNodes.Count - 1).BarNode.Size.Value = Size
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createbuttonnode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Position As New Point(Val(GetValueAt(line, 3)), Val(GetValueAt(line, 4)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 5)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048))
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Name, "Button Node")
                    RNodes(RNodes.Count - 1).Parent = New Node(Parent, "Split Node")
                    RNodes(RNodes.Count - 1).ButtonNode.Location.Value = Position
                    RNodes(RNodes.Count - 1).ButtonNode.Size.Value = Size
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createhovernode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Position As New Point(Val(GetValueAt(line, 3)), Val(GetValueAt(line, 4)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 5)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048))
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Name, "Hover Node")
                    RNodes(RNodes.Count - 1).Parent = New Node(Parent, "Split Node")
                    RNodes(RNodes.Count - 1).HoverNode.Location.Value = Position
                    RNodes(RNodes.Count - 1).HoverNode.Size.Value = Size
                ElseIf line.ToLower.Trim.StartsWith("hudbuilder.createobjectmarkernode") Then
                    Dim Parent As String = GetValueAt(line, 1)
                    Dim Name As String = GetValueAt(line, 2)
                    Dim Position As New Point(Val(GetValueAt(line, 3)), Val(GetValueAt(line, 4)))
                    Dim Size As New Size(SetValueBounds(Val(GetValueAt(line, 5)), 1, 2048), SetValueBounds(Val(GetValueAt(line, 6)), 1, 2048))
                    ReDim Preserve RNodes(RNodes.Count)
                    RNodes(RNodes.Count - 1) = New Node(Name, "Object Marker Node")
                    RNodes(RNodes.Count - 1).Parent = New Node(Parent, "Split Node")
                    RNodes(RNodes.Count - 1).ObjectMarkerNode.Location.Value = Position
                    RNodes(RNodes.Count - 1).ObjectMarkerNode.Size.Value = Size
                ElseIf RNodes.Count = 0 Then
                    FailedLines &= vbCrLf & line
                ElseIf RNodes(RNodes.Count - 1).LoadLine(line) = False Then
                    FailedLines &= vbCrLf & line
                    RNodes(RNodes.Count - 1).FailedLines &= vbCrLf & line
                End If
            End If
        Next
        If FailedLines <> "" Then WriteLog("Failed to process line(s): " & FailedLines)
        Return RNodes
    End Function
    Public Sub UpdateVariables(Optional ByVal type As GameMode = 2)
        Variables.Clear()
        If type = GameMode.Battlefield2 Or type = GameMode.Both Then
            Variables.Add(New VariableHandler("AngleOfAttack", VariableType.VT_Angle, 0))
            Variables.Add(New VariableHandler("VehicleBanking", VariableType.VT_Angle, 0))
            Variables.Add(New VariableHandler("VehicleAngle", VariableType.VT_Angle, 0))
            Variables.Add(New VariableHandler("SpeedString", VariableType.VT_String, "100"))
            Variables.Add(New VariableHandler("AltitudeString", VariableType.VT_String, "100"))
            Variables.Add(New VariableHandler("TorqueString", VariableType.VT_String, "100"))
            Variables.Add(New VariableHandler("TorqueAngle", VariableType.VT_Angle, 0))
            Variables.Add(New VariableHandler("Torque", VariableType.VT_Value, 0.5))
            Variables.Add(New VariableHandler("VehicleElevationSpeedAngle", VariableType.VT_Angle, 0))
            Variables.Add(New VariableHandler("HitIndicatorIconShow", VariableType.VT_Show, False))
            Variables.Add(New VariableHandler("RadioInterfaceShow", VariableType.VT_Show, False))
            Variables.Add(New VariableHandler("CommanderShow", VariableType.VT_Show, False))
            Variables.Add(New VariableHandler("radio1string", VariableType.VT_String, "SPOTTED"))
            Variables.Add(New VariableHandler("radio2string", VariableType.VT_String, "GO, GO, GO"))
            Variables.Add(New VariableHandler("radio3string", VariableType.VT_String, "NEED PICKUP"))
            Variables.Add(New VariableHandler("radio4string", VariableType.VT_String, "NEGATIVE"))
            Variables.Add(New VariableHandler("radio5string", VariableType.VT_String, "NEED AMMO"))
            Variables.Add(New VariableHandler("radio6string", VariableType.VT_String, "FOLLOW ME"))
            Variables.Add(New VariableHandler("radio7string", VariableType.VT_String, "NEED MEDIC"))
            Variables.Add(New VariableHandler("radio8string", VariableType.VT_String, "ROGER THAT"))
            Variables.Add(New VariableHandler("radio9string", VariableType.VT_String, "NEED BACKUP"))
            Variables.Add(New VariableHandler("radio10string", VariableType.VT_String, "THANK YOU"))
            Variables.Add(New VariableHandler("radio11string", VariableType.VT_String, "SORRY"))
        End If
        If type = GameMode.Battlefield2142 Or type = GameMode.Both Then

        End If
    End Sub
End Module
