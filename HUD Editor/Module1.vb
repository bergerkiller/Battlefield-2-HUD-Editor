Imports System.Drawing.Imaging

Module Module1
    Public PBCursorPosition As New Point(0, 0)
    Public BGimage As Bitmap
    Public OVimage As Bitmap = New Bitmap(16, 16)

    Public UpdateScreen As Boolean = False

    Public UpdateSelectionField As Boolean = False
    Public UpdateDotField As Boolean = False
    Public SelectionField As New Bitmap(800, 600)
    Public DotField As New Bitmap(800, 600)
    Public DotMoveType As Integer = -1
    Public CurrentIndex As Integer = -1
    Public ImageSelectorImagePath As String = "Textures"
    Public ViewedDialog As Integer = 0
    Public ScaleX As Single = 1
    Public ScaleY As Single = 1

    Public Nodes(0) As Node

    Public Function GetTreeViewNodes(ByVal TreeView As TreeView) As TreeNode()
        Dim rval(0) As TreeNode
        rval(0) = New TreeNode
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
        If Not System.IO.File.Exists(TexturePath) Then
            Dim ddspath As String = StrReverse(StrReverse(TexturePath).Remove(0, System.IO.Path.GetExtension(TexturePath).Length)) & ".dds"
            Dim tgapath As String = StrReverse(StrReverse(TexturePath).Remove(0, System.IO.Path.GetExtension(TexturePath).Length)) & ".tga"
            If System.IO.File.Exists(tgapath) Then TexturePath = tgapath
            If System.IO.File.Exists(ddspath) Then TexturePath = ddspath
        End If
        Return TexturePath
    End Function
    Public Function RemoveDoubleSpaces(ByVal Text As String) As String
        Return Text.Replace("   ", " ").Replace(vbTab, " ").Replace(ControlChars.Tab, " ").Replace("	", " ").Replace("  ", " ")
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

    Public Sub SetCBSelectedItem(ByRef cb As ComboBox, ByVal Item As String)
        Item = Item.ToLower.Trim
        cb.SelectedIndex = -1
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
            If DialogIndex = 1 Then TextureBrowser.ShowDialog() Else TextureBrowser.Close()
            If DialogIndex = 2 Then ColorDialog.Show() Else ColorDialog.Close()
            If DialogIndex = 3 Then MainDialog.Show() Else MainDialog.Close()
            If DialogIndex = 4 Then TSizeDialog.Show() Else TSizeDialog.Close()
            If DialogIndex = 5 Then RotationDialog.Show() Else RotationDialog.Close()
            If DialogIndex = 6 Then
                If Nodes(CurrentIndex).Type = "Picture Node" Then pnvariables.Show()
                If Nodes(CurrentIndex).Type = "Text Node" Then tnvariables.Show()
                If Nodes(CurrentIndex).Type = "Compass Node" Then cnvariables.Show()
            Else
                pnvariables.Close()
                tnvariables.Close()
                cnvariables.Close()
            End If
            If DialogIndex = 7 Then
                If Nodes(CurrentIndex).Type = "Text Node" Then TextStyle.Show()
                If Nodes(CurrentIndex).Type = "Compass Node" Then CompassStyle.Show()
            Else
                TextStyle.Close()
                CompassStyle.Close()
            End If
        Else
            ViewedDialog = 0
            LoadNode(-1)
        End If
    End Sub

    Public Function LoadImage(ByVal TexturePath As String) As Image
        If System.IO.File.Exists(TexturePath) Then
            Try
                Return FreeImageAPI.FreeImage.GetBitmap(FreeImageAPI.FreeImage.LoadEx(TexturePath))
            Catch
                Return New Bitmap(32, 32)
                Debug.WriteLine("Failed to load image: " & TexturePath)
            End Try
        Else
            Return New Bitmap(32, 32)
            Debug.WriteLine("Failed to load image; not found: " & TexturePath)
        End If
    End Function

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

    Public Sub RemoveNode(ByVal Index As Integer)
        For i As Integer = Index To Nodes.Count - 2
            Nodes(i) = Nodes(i + 1)
        Next
        ReDim Preserve Nodes(0 To Nodes.Count - 2)
    End Sub
    Public Sub InsertNode(ByVal Node As Node, ByVal Index As Integer)
        ReDim Preserve Nodes(Nodes.Count)
        Dim buNodes(Nodes.Count - 1) As Node
        Nodes.CopyTo(buNodes, 0)
        For i As Integer = Index To Nodes.Count - 2
            Nodes(i + 1) = buNodes(i)
        Next
        Nodes(Index) = Node
    End Sub
    Public Sub LoadNode(ByVal NodeIndex As Integer)
        Form1.TextureButton.Visible = False
        Form1.ColorButton.Visible = False
        Form1.MainButton.Visible = False
        Form1.VariablesButton.Visible = False
        Form1.RotationButton.Visible = False
        Form1.StyleButton.Visible = False
        Form1.TSizeButton.Visible = False
        CurrentIndex = NodeIndex
        If NodeIndex < Nodes.Count And NodeIndex > -1 Then
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
            End If
        Else
            Form1.Text = "HUD Editor - No Node Selected"
        End If
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
    Public Function GetNodeIndexAtPoint(ByVal Point As Point) As Integer
        If UpdateSelectionField = True Then
            UpdateSelectionField = False
            SelectionField = New Bitmap(800, 600)
            Dim sfg As Graphics = Graphics.FromImage(SelectionField)
            For i As Integer = 1 To Nodes.Count - 1
                If Nodes(i).Render = True Then
                    Dim Position As New Point(0, 0)
                    Dim Size As New Size(32, 32)
                    Dim Rotation As Integer = 0
                    If Nodes(i).Type = "Picture Node" Then
                        Position = Nodes(i).PictureNodeData.Position
                        Size = Nodes(i).PictureNodeData.Size
                        Rotation = Nodes(i).PictureNodeData.StaticRotation
                    End If
                    If Nodes(i).Type = "Text Node" Then
                        Position = Nodes(i).TextNodeData.Position
                        Size = Nodes(i).TextNodeData.Size
                        Rotation = 0
                    End If
                    If Nodes(i).Type = "Compass Node" Then
                        Position = Nodes(i).CompassNodeData.Position
                        Size = Nodes(i).CompassNodeData.Size
                        Rotation = 0
                    End If
                    Dim bmp As New Bitmap(800, 600)
                    Dim g As Graphics = Graphics.FromImage(bmp)
                    g.TranslateTransform(Position.X + Size.Width * 0.5, Position.Y + Size.Height * 0.5)
                    g.RotateTransform(Rotation)
                    g.TranslateTransform(Size.Width * -0.5, Size.Height * -0.5)
                    g.FillRectangle(New SolidBrush(ColorTranslator.FromOle(i + 1)), New Rectangle(0, 0, Size.Width, Size.Height))
                    g.Dispose()
                    sfg.DrawImage(bmp, New Point(0, 0))
                End If
            Next
            sfg.Dispose()
        End If
        Dim pcolor As Color = SelectionField.GetPixel(Point.X, Point.Y)
        Dim selindex As Integer = -1
        For i As Integer = 0 To Nodes.Count - 1
            If pcolor = System.Drawing.ColorTranslator.FromOle(i + 1) = True Then
                selindex = i
                Exit For
            End If
        Next
        Return selindex
    End Function
    Public Function GetDotIndexAtPoint(ByVal Point As Point) As Integer
        Point.X = SetValueBounds(Point.X, 0, 800)
        Point.Y = SetValueBounds(Point.Y, 0, 600)
        If CurrentIndex <> -1 Then
            If UpdateDotField = True Then
                DotField = New Bitmap(800, 600)
                Dim position As New Point(0, 0)
                Dim size As New Size(32, 32)
                Dim rotation As Integer = 0
                If Nodes(CurrentIndex).Type = "Picture Node" Then
                    position = Nodes(CurrentIndex).PictureNodeData.Position
                    size = Nodes(CurrentIndex).PictureNodeData.Size
                    rotation = Nodes(CurrentIndex).PictureNodeData.StaticRotation
                ElseIf Nodes(CurrentIndex).Type = "Compass Node" Then
                    position = Nodes(CurrentIndex).CompassNodeData.Position
                    size = Nodes(CurrentIndex).CompassNodeData.Size
                ElseIf Nodes(CurrentIndex).Type = "Text Node" Then
                    position = Nodes(CurrentIndex).TextNodeData.Position
                    size = Nodes(CurrentIndex).TextNodeData.Size
                End If
                Dim g As Graphics = Graphics.FromImage(DotField)
                g.TranslateTransform(position.X + size.Width * 0.5, position.Y + size.Height * 0.5)
                g.RotateTransform(rotation)
                g.TranslateTransform(size.Width * -0.5, size.Height * -0.5)
                g.FillRectangle(New SolidBrush(Color.FromArgb(255, 1, 0, 0)), New Rectangle(-4, -4, 8, 8))
                g.FillRectangle(New SolidBrush(Color.FromArgb(255, 2, 0, 0)), New Rectangle(size.Width - 4, -4, 8, 8))
                g.FillRectangle(New SolidBrush(Color.FromArgb(255, 3, 0, 0)), New Rectangle(size.Width - 4, size.Height - 4, 8, 8))
                g.FillRectangle(New SolidBrush(Color.FromArgb(255, 4, 0, 0)), New Rectangle(-4, size.Height - 4, 8, 8))
                If size.Width > 31 Then
                    g.FillRectangle(New SolidBrush(Color.FromArgb(255, 5, 0, 0)), New Rectangle(size.Width * 0.5 - 4, -4, 8, 8))
                    g.FillRectangle(New SolidBrush(Color.FromArgb(255, 6, 0, 0)), New Rectangle(size.Width * 0.5 - 4, size.Height - 4, 8, 8))
                End If
                If size.Height > 31 Then
                    g.FillRectangle(New SolidBrush(Color.FromArgb(255, 7, 0, 0)), New Rectangle(-4, size.Height * 0.5 - 4, 8, 8))
                    g.FillRectangle(New SolidBrush(Color.FromArgb(255, 8, 0, 0)), New Rectangle(size.Width - 4, size.Height * 0.5 - 4, 8, 8))
                End If
                g.Dispose()
            End If
            Return DotField.GetPixel(Point.X, Point.Y).R - 1
        Else
            Return -1
        End If
    End Function

    Public Function SaveNodeData(ByVal Node As Node) As String
        Dim rdata As String = ""
        If Node.Type = "Picture Node" Then
            With Node.PictureNodeData
                rdata = "hudBuilder.createPictureNode		" & Node.Parent & " " & Node.Name & " "
                rdata &= .Position.X & " " & .Position.Y & " " & .Size.Width & " " & .Size.Height
                If .TexturePath <> "" Then rdata &= vbCrLf & "hudBuilder.setPictureNodeTexture 	" & .TexturePath.Remove(0, ImageSelectorImagePath.Length + 1)
                rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & ConvertColorToText(.Color)
                If .StaticRotation <> 0 And .StaticRotation <> 360 Then rdata &= vbCrLf & "hudBuilder.setPictureNodeRotation 	" & 360 - .StaticRotation
                If .DOffsetXVar <> "" Then rdata &= vbCrLf & "hudBuilder.setNodePosVariable		0 " & .DOffsetXVar
                If .DOffsetYVar <> "" Then rdata &= vbCrLf & "hudBuilder.setNodePosVariable		1 " & .DOffsetYVar
                If .DRotationVar <> "" Then rdata &= vbCrLf & "hudBuilder.setPictureNodeRotateVariable " & .DRotationVar
                If .DRotationVar <> "" Then rdata &= vbCrLf & "hudBuilder.setPictureNodeCenterPoint 	" & .DRotationMid.X & " " & .DRotationMid.Y
            End With
        ElseIf Node.Type = "Text Node" Then
            With Node.TextNodeData
                rdata = "hudBuilder.createTextNode		" & Node.Parent & " " & Node.Name & " "
                rdata &= .Position.X & " " & .Position.Y & " " & .Size.Width & " " & .Size.Height
                rdata &= vbCrLf & "hudBuilder.setTextNodeStyle		Fonts/vehicleHudFont_6.dif " & .Style
                rdata &= vbCrLf & "hudBuilder.setTextNodeStringVariable	" & .StringVariable
                rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & ConvertColorToText(.Color)
            End With
        ElseIf Node.Type = "Compass Node" Then
            With Node.CompassNodeData
                rdata = "hudBuilder.createCompassNode 		" & Node.Parent & " " & Node.Name & " "
                rdata &= .Type & " " & .Position.X & " " & .Position.Y & " " & .Size.Width & " " & .Size.Height
                If .Type = 3 Then rdata &= " 1 0"
                If .Type = 0 Then rdata &= " 0 1"
                rdata &= vbCrLf & "hudBuilder.setCompassNodeTexture 	1 " & .TexturePath.Remove(0, ImageSelectorImagePath.Length + 1)
                rdata &= vbCrLf & "hudBuilder.setCompassNodeTextureSize	" & .TextureSize.Width & " " & .TextureSize.Height
                If .Type = 3 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeBorder		0 0 0 " & .Border
                If .Type = 0 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeBorder		0 " & .Border & " 0 0"
                rdata &= vbCrLf & "hudBuilder.setCompassNodeValueVariable	" & .ValueVariable
                rdata &= vbCrLf & "hudBuilder.setCompassNodeOffset		" & .Offset
                If .Type = 3 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeSnapOffset	0 0 " & .OffsetSnapMin & " " & .OffsetSnapMax
                If .Type = 0 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeSnapOffset	" & .OffsetSnapMin & " " & .OffsetSnapMax & " 0 0"
                rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & ConvertColorToText(.Color)
            End With
        End If
        Return rdata
    End Function
    Public Function LoadNodeData(ByVal Data As String) As Node()
        Data = RemoveDoubleSpaces(Data)
        Dim RNodes(0) As Node
        Dim FailedLines As String = ""
        Dim isinrem As Boolean = False
        For Each line As String In Data.Split(vbCrLf)
            If line.ToLower.Trim = "beginrem" Then isinrem = True
            If line.ToLower.Trim = "endrem" Then isinrem = False
            If isinrem = False And Not line.ToLower.Trim.StartsWith("rem ") And Not line.ToLower.Trim.StartsWith("# ") And Not line.Trim = "" Then
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
                ElseIf RNodes(RNodes.Count - 1).Type = "Split Node" Then



                    'Split Node Data
                    If line.ToLower.Trim.StartsWith("hudbuilder.setnodelogicshowvariable") Then
                        RNodes(RNodes.Count - 1).ShowVariables.Items.Add(line.Remove(0, 36).Trim)
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.addnodeblendeffect") Then
                        RNodes(RNodes.Count - 1).SplitNodeData.BlendEffectA = Val(GetValueAt(line, 1))
                        RNodes(RNodes.Count - 1).SplitNodeData.BlendEffectB = Val(GetValueAt(line, 2))
                    Else
                        FailedLines &= line
                    End If
                ElseIf RNodes(RNodes.Count - 1).Type = "Text Node" Then



                    'Text Node Data
                    If line.ToLower.Trim.StartsWith("hudbuilder.settextnodestyle") Then
                        RNodes(RNodes.Count - 1).TextNodeData.Style = SetValueBounds(Val(GetValueAt(line, 2)), 0, 2)
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.settextnodestringvariable") Then
                        RNodes(RNodes.Count - 1).TextNodeData.StringVariable = GetValueAt(line, 1)
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setnodecolor") Then
                        Dim R As Integer = SetValueBounds(GetValueAt(line, 1).Replace(".", ",") * 255, 0, 255)
                        Dim G As Integer = SetValueBounds(GetValueAt(line, 2).Replace(".", ",") * 255, 0, 255)
                        Dim B As Integer = SetValueBounds(GetValueAt(line, 3).Replace(".", ",") * 255, 0, 255)
                        Dim A As Integer = SetValueBounds(GetValueAt(line, 4).Replace(".", ",") * 255, 0, 255)
                        RNodes(RNodes.Count - 1).TextNodeData.Color = Color.FromArgb(A, R, G, B)
                    Else
                        FailedLines &= line
                    End If
                ElseIf RNodes(RNodes.Count - 1).Type = "Picture Node" Then





                    'Picture Node Data
                    If line.ToLower.Trim.StartsWith("hudbuilder.setpicturenodetexture") Then
                        RNodes(RNodes.Count - 1).PictureNodeData.TexturePath = FixTexturePath(ImageSelectorImagePath & "\" & GetValueAt(line, 1))
                        RNodes(RNodes.Count - 1).PictureNodeData.TextureImage = LoadImage(RNodes(RNodes.Count - 1).PictureNodeData.TexturePath)
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setnodeposvariable") Then
                        If Val(GetValueAt(line, 1)) = 0 Then RNodes(RNodes.Count - 1).PictureNodeData.DOffsetXVar = GetValueAt(line, 2)
                        If Val(GetValueAt(line, 1)) = 1 Then RNodes(RNodes.Count - 1).PictureNodeData.DOffsetYVar = GetValueAt(line, 2)
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setpicturenoderotatevariable") Then
                        RNodes(RNodes.Count - 1).PictureNodeData.DRotationVar = GetValueAt(line, 1)
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setpicturenodecenterpoint") Then
                        RNodes(RNodes.Count - 1).PictureNodeData.DRotationMid = New Point(Val(GetValueAt(line, 1)), Val(GetValueAt(line, 2)))
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setpicturenoderotation") Then
                        Dim srotation As Integer = Val(GetValueAt(line, 1))
                        Do While srotation < 0
                            srotation += 360
                        Loop
                        If srotation = 0 Then srotation = 360
                        RNodes(RNodes.Count - 1).PictureNodeData.StaticRotation = 360 - srotation
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setnodecolor") Then
                        Dim R As Integer = SetValueBounds(GetValueAt(line, 1).Replace(".", ",") * 255, 0, 255)
                        Dim G As Integer = SetValueBounds(GetValueAt(line, 2).Replace(".", ",") * 255, 0, 255)
                        Dim B As Integer = SetValueBounds(GetValueAt(line, 3).Replace(".", ",") * 255, 0, 255)
                        Dim A As Integer = SetValueBounds(GetValueAt(line, 4).Replace(".", ",") * 255, 0, 255)
                        RNodes(RNodes.Count - 1).PictureNodeData.Color = Color.FromArgb(A, R, G, B)
                    Else
                        FailedLines &= line
                    End If
                ElseIf RNodes(RNodes.Count - 1).Type = "Compass Node" Then





                    'Compass Node Data
                    If line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodetexturesize") Then
                        RNodes(RNodes.Count - 1).CompassNodeData.TextureSize.Width = Val(GetValueAt(line, 1))
                        RNodes(RNodes.Count - 1).CompassNodeData.TextureSize.Height = Val(GetValueAt(line, 2))
                        If RNodes(RNodes.Count - 1).CompassNodeData.TextureSize.Width = 0 Then RNodes(RNodes.Count - 1).CompassNodeData.TextureSize.Width = 32
                        If RNodes(RNodes.Count - 1).CompassNodeData.TextureSize.Height = 0 Then RNodes(RNodes.Count - 1).CompassNodeData.TextureSize.Height = 32
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodetexture") Then
                        RNodes(RNodes.Count - 1).CompassNodeData.TexturePath = FixTexturePath(ImageSelectorImagePath & "\" & GetValueAt(line, 2))
                        RNodes(RNodes.Count - 1).CompassNodeData.TextureImage = LoadImage(RNodes(RNodes.Count - 1).CompassNodeData.TexturePath)
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodeborder") Then
                        If RNodes(RNodes.Count - 1).CompassNodeData.Type = 3 Then RNodes(RNodes.Count - 1).CompassNodeData.Border = Val(GetValueAt(line, 4))
                        If RNodes(RNodes.Count - 1).CompassNodeData.Type = 0 Then RNodes(RNodes.Count - 1).CompassNodeData.Border = Val(GetValueAt(line, 2))
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodevaluevariable") Then
                        RNodes(RNodes.Count - 1).CompassNodeData.ValueVariable = GetValueAt(line, 1)
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodesnapoffset") Then
                        If RNodes(RNodes.Count - 1).CompassNodeData.Type = 0 Then RNodes(RNodes.Count - 1).CompassNodeData.OffsetSnapMin = Val(GetValueAt(line, 1))
                        If RNodes(RNodes.Count - 1).CompassNodeData.Type = 0 Then RNodes(RNodes.Count - 1).CompassNodeData.OffsetSnapMax = Val(GetValueAt(line, 2))
                        If RNodes(RNodes.Count - 1).CompassNodeData.Type = 3 Then RNodes(RNodes.Count - 1).CompassNodeData.OffsetSnapMin = Val(GetValueAt(line, 3))
                        If RNodes(RNodes.Count - 1).CompassNodeData.Type = 3 Then RNodes(RNodes.Count - 1).CompassNodeData.OffsetSnapMax = Val(GetValueAt(line, 4))
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodeoffset") Then
                        RNodes(RNodes.Count - 1).CompassNodeData.Offset = Val(GetValueAt(line, 1))
                    ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setnodecolor") Then
                        Dim R As Integer = SetValueBounds(GetValueAt(line, 1).Replace(".", ",") * 255, 0, 255)
                        Dim G As Integer = SetValueBounds(GetValueAt(line, 2).Replace(".", ",") * 255, 0, 255)
                        Dim B As Integer = SetValueBounds(GetValueAt(line, 3).Replace(".", ",") * 255, 0, 255)
                        Dim A As Integer = SetValueBounds(GetValueAt(line, 4).Replace(".", ",") * 255, 0, 255)
                        RNodes(RNodes.Count - 1).CompassNodeData.Color = Color.FromArgb(A, R, G, B)
                    Else
                        FailedLines &= line
                    End If
                End If
            End If
        Next
        If FailedLines <> "" Then MsgBox("Failed to process line(s): " & vbCrLf & FailedLines, MsgBoxStyle.Exclamation)
        Return RNodes
    End Function

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

    Dim OLDINDEX As Integer = -1
    Dim BackLayer As New Bitmap(800, 600)
    Dim ActiveLayer As New Bitmap(800, 600)
    Dim FrontLayer As New Bitmap(800, 600)
    Public Sub RenderNodes(ByRef g As Graphics)
        'Get the layer modified types
        Dim UpdateActiveLayer As Boolean = False
        Dim UpdateBackLayer As Boolean = False
        Dim UpdateFrontLayer As Boolean = False
        If OLDINDEX = CurrentIndex Then
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
                    ElseIf Nodes(i).Type = "Text Node" Then
                        If Nodes(i).TextNodeData.Modified = True Then NodeModified = True
                    End If
                End If
                If NodeModified = True And i < CurrentIndex Then UpdateBackLayer = True
                If NodeModified = True And i > CurrentIndex Then UpdateFrontLayer = True
                If NodeModified = True And i = CurrentIndex Then UpdateActiveLayer = True
            Next
        Else
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

    Public Structure Node
        Sub New(ByVal Parent As String, ByVal Name As String, ByVal Type As String)
            Me.Parent = Parent
            Me.Name = Name
            Me.Type = Type
            Me.Render = True
            Me.ShowVariables = New ListBox
            Me.InTime = 0
            Me.OutTime = 0
            If Type = "Picture Node" Then Me.PictureNodeData = New PictureNode("")
            If Type = "Text Node" Then Me.TextNodeData = New TextNode("100")
            If Type = "Compass Node" Then Me.CompassNodeData = New CompassNode("")
            If Type = "Split Node" Then Me.SplitNodeData = New SplitNode("no_parent", "new_splitnode")
            PerformRefresh = True
        End Sub
        Public Name As String
        Public Parent As String
        Public Type As String
        Public ShowVariables As ListBox
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
    End Structure
    Public Structure SplitNode
        Sub New(ByVal Parent As String, ByVal Name As String)
            Me.Name = Name
            Me.Parent = Parent
            Me.BlendEffectA = 0
            Me.BlendEffectB = 0
        End Sub
        Public Name As String
        Public Parent As String
        Public BlendEffectA As Integer
        Public BlendEffectB As Integer
        Public Guiindices() As Integer
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
            Me.Color = Color.FromArgb(255, 255, 255, 255)
            Me.Modified = True
        End Sub
        Public Color As Color
        Public Text As String
        Public StringVariable As String
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


End Module
