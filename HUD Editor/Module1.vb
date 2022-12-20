Imports System.Drawing.Imaging

Module Module1
    Public BGimage As Bitmap
    Public OVimage As Bitmap = New Bitmap(16, 16)
    Public UpdateScreen As Boolean = False
    Public CurrentIndex As Integer = -1
    Public ImageSelectorImagePath As String = "Textures"
    Public ViewedDialog As Integer = 0

    Public Nodes(0) As Node

    Public Function SetValueBounds(ByVal Value As Single, ByVal Min As Single, ByVal Max As Single) As Single
        If Value > Max Then Value = Max
        If Value < Min Then Value = Min
        Return Value
    End Function
    Public Function GetDistance(ByVal Point1 As Point, ByVal Point2 As Point) As Single
        Return Math.Sqrt((Point1.X - Point2.X) ^ 2 + (Point1.Y - Point2.Y) ^ 2)
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


    Public Sub SetCBSelectedItem(ByRef cb As ComboBox, ByVal Item As String)
        Item = Item.ToLower.Trim
        cb.SelectedIndex = -1
        For i As Integer = 0 To cb.Items.Count - 1
            If cb.Items(i).tolower.trim = Item Then cb.SelectedIndex = i
        Next
    End Sub
    Public Function RotatePointInCircle(ByVal point As Point, ByVal CircleRadius As Single, ByVal Angle As Integer) As Point
        Return New Point(Math.Cos(Angle / 180 * Math.PI) * CircleRadius, Math.Sin(Angle / 180 * Math.PI) * CircleRadius)
    End Function
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
            If DialogIndex = 3 Then SizeDialog.Show() Else SizeDialog.Close()
            If DialogIndex = 4 Then PositionDialog.Show() Else PositionDialog.Close()
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
            If DialogIndex = 8 Then TSizeDialog.Show() Else TSizeDialog.Close()
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
        'For y As Integer = 0 To bm.Height - 1
        '    For x As Integer = 0 To bm.Width - 1
        '        Dim c As Color = source.GetPixel(x, y)
        '        Dim Alpha As Integer = SetValueBounds(c.A * Amult, 0, 255)
        '        Dim Red As Integer = SetValueBounds(c.R * Rmult, 0, 255)
        '        Dim Green As Integer = SetValueBounds(c.G * Gmult, 0, 255)
        '        Dim Blue As Integer = SetValueBounds(c.B * Bmult, 0, 255)
        '        bm.SetPixel(x, y, Color.FromArgb(Alpha, Red, Green, Blue))
        '    Next
        'Next
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
        Nodes.CopyTo(bunodes, 0)
        For i As Integer = Index To Nodes.Count - 2
            Nodes(i + 1) = buNodes(i)
        Next
        Nodes(Index) = Node
    End Sub
    Public Sub LoadNode(ByVal NodeIndex As Integer)
        Form1.TextureButton.Visible = False
        Form1.ColorButton.Visible = False
        Form1.SizeButton.Visible = False
        Form1.PositionButton.Visible = False
        Form1.VariablesButton.Visible = False
        Form1.RotationButton.Visible = False
        Form1.StyleButton.Visible = False
        Form1.TSizeButton.Visible = False
        CurrentIndex = NodeIndex
        If NodeIndex < Nodes.Count And NodeIndex > -1 Then
            Form1.Text = "HUD Editor - " & Nodes(NodeIndex).Name
            'Set button visibility
            If Nodes(NodeIndex).Type = "Picture Node" Then
                Form1.TextureButton.Visible = True
                Form1.ColorButton.Visible = True
                Form1.SizeButton.Visible = True
                Form1.PositionButton.Visible = True
                Form1.VariablesButton.Visible = True
                Form1.RotationButton.Visible = True
            ElseIf Nodes(NodeIndex).Type = "Text Node" Then
                Form1.ColorButton.Visible = True
                Form1.PositionButton.Visible = True
                Form1.SizeButton.Visible = True
                Form1.StyleButton.Visible = True
                Form1.VariablesButton.Visible = True
            ElseIf Nodes(CurrentIndex).Type = "Compass Node" Then
                Form1.SizeButton.Visible = True
                Form1.TSizeButton.Visible = True
                Form1.PositionButton.Visible = True
                Form1.TextureButton.Visible = True
                Form1.ColorButton.Visible = True
                Form1.VariablesButton.Visible = True
                Form1.StyleButton.Visible = True
            End If
            Else
                Form1.Text = "HUD Editor - No Node Selected"
            End If
    End Sub
    Public Function SaveNodeData(ByVal Node As Node) As String
        Dim rdata As String = ""
        If Node.Type = "Picture Node" Then
            With Node.PictureNodeData
                rdata = "hudBuilder.createPictureNode		" & Node.Parent & " " & Node.Name & " "
                rdata &= .Position.X & " " & .Position.Y & " " & .Size.Width & " " & .Size.Height
                rdata &= vbCrLf & "hudBuilder.setPictureNodeTexture 	" & .TexturePath.Remove(0, ImageSelectorImagePath.Length + 1)
                rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & .Color.R \ 255 & " " & .Color.G \ 255 & " " & .Color.B \ 255 & " " & .Color.A \ 255
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
                rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & .Color.R \ 255 & " " & .Color.G \ 255 & " " & .Color.B \ 255 & " " & .Color.A \ 255
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
                rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & .Color.R \ 255 & " " & .Color.G \ 255 & " " & .Color.B \ 255 & " " & .Color.A \ 255
            End With
        End If
        Return rdata
    End Function
    Public Function LoadNodeData(ByVal Data As String) As Node
        Data = RemoveDoubleSpaces(Data)
        Dim RNode As New Node("", "", "")
        For Each line As String In Data.Split(vbCrLf)
            If line.ToLower.Trim.StartsWith("hudbuilder.createcompassnode") Then
                Dim Parent As String = GetValueAt(line, 1)
                Dim Name As String = GetValueAt(line, 2)
                Dim Type As Integer = Val(GetValueAt(line, 3))
                Dim Position As New Point(Val(GetValueAt(line, 4)), Val(GetValueAt(line, 5)))
                Dim Size As New Size(Val(GetValueAt(line, 6)), Val(GetValueAt(line, 7)))
                If Parent = "" Then Parent = "no_parent"
                If Name = "" Then Name = "no_name"
                If Size.Width = 0 Then Size.Width = 32
                If Size.Height = 0 Then Size.Height = 32
                If Type <> 3 And Type <> 0 Then Type = 3
                RNode = New Node(Parent, Name, "Compass Node")
                RNode.CompassNodeData.Position = Position
                RNode.CompassNodeData.Size = Size
                RNode.CompassNodeData.Type = Type
            ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodetexturesize") Then
                RNode.CompassNodeData.TextureSize.Width = Val(GetValueAt(line, 1))
                RNode.CompassNodeData.TextureSize.Height = Val(GetValueAt(line, 2))
                If RNode.CompassNodeData.TextureSize.Width = 0 Then RNode.CompassNodeData.TextureSize.Width = 32
                If RNode.CompassNodeData.TextureSize.Height = 0 Then RNode.CompassNodeData.TextureSize.Height = 32
            ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodetexture") Then
                RNode.CompassNodeData.TexturePath = FixTexturePath(ImageSelectorImagePath & "\" & GetValueAt(line, 2))
                RNode.CompassNodeData.TextureImage = LoadImage(RNode.CompassNodeData.TexturePath)
            ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodeborder") Then
                RNode.CompassNodeData.Border = Val(GetValueAt(line, 4))
            ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodevaluevariable") Then
                RNode.CompassNodeData.ValueVariable = GetValueAt(line, 1)
            ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodeoffset") Then
                RNode.CompassNodeData.Offset = Val(GetValueAt(line, 1))
            ElseIf line.ToLower.Trim.StartsWith("hudbuilder.setnodecolor") Then
                Dim R As Integer = SetValueBounds(GetValueAt(line, 1).Replace(".", ",") * 255, 0, 255)
                Dim G As Integer = SetValueBounds(GetValueAt(line, 2).Replace(".", ",") * 255, 0, 255)
                Dim B As Integer = SetValueBounds(GetValueAt(line, 3).Replace(".", ",") * 255, 0, 255)
                Dim A As Integer = SetValueBounds(GetValueAt(line, 4).Replace(".", ",") * 255, 0, 255)
                Dim c As Color = Color.FromArgb(A, R, G, B)
                If RNode.Type = "Compass Node" Then RNode.CompassNodeData.Color = c
                If RNode.Type = "Picture Node" Then RNode.PictureNodeData.Color = c
                If RNode.Type = "Text Node" Then RNode.TextNodeData.Color = c
            Else
                MsgBox("Failed to process line: " & vbCrLf & line, MsgBoxStyle.Critical)
            End If
        Next
        Return RNode
    End Function

    Public Sub ResetNodeSimvars()
        For i As Integer = 0 To Nodes.Count - 1
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
    Public Sub RenderNodes(ByRef g As Graphics)
        'Render all the nodes
        Try
            For i As Integer = 0 To Nodes.Count - 1
                If Nodes(i).Render = True Then
                    If Nodes(i).Type = "Picture Node" Then
                        g.DrawImage(RenderPictureNode(Nodes(i).PictureNodeData), New Point(0, 0))
                    End If
                    If Nodes(i).Type = "Text Node" Then
                        g.DrawImage(RenderTextNode(Nodes(i).TextNodeData), New Point(0, 0))
                    End If
                    If Nodes(i).Type = "Compass Node" Then
                        g.DrawImage(RenderCompassNode(Nodes(i).CompassNodeData), New Point(0, 0))
                    End If
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        'Render overlay
        g.DrawImage(OVimage, New Point(0, 0))
    End Sub

    Public Structure Node
        Sub New(ByVal Parent As String, ByVal Name As String, ByVal Type As String)
            Me.Parent = Parent
            Me.Name = Name
            Me.Type = Type
            Me.Render = True
            If Type = "Picture Node" Then Me.PictureNodeData = New PictureNode("")
            If Type = "Text Node" Then Me.TextNodeData = New TextNode("0000")
            If Type = "Compass Node" Then Me.CompassNodeData = New CompassNode("")
        End Sub
        Public Name As String
        Public Parent As String
        Public Type As String
        Public Render As Boolean
        Public PictureNodeData As PictureNode
        Public TextNodeData As TextNode
        Public CompassNodeData As CompassNode
    End Structure
    Public Structure PictureNode
        'Render cycle:
        '1. Color it
        '2. Size it
        '3. Draw it on the surface
        Sub New(ByVal TexturePath As String)
            Me.TexturePath = TexturePath
            Me.TextureImage = LoadImage(TexturePath)
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
            Me.Color = Color.FromArgb(255, 255, 255, 255)
            Me.Position = New Point(0, 0)
            Me.TextureSize = Me.TextureImage.Size
            Me.Size = New Size(32, 32)
            Me.ValueVariable = ""
            Me.Type = 3
            Me.Value = 0
            Me.Border = 0
            Me.Offset = 0
            Me.tmpval1 = 0
            Me.tmpval2 = 0
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
        Public Type As Integer '0=vertical; 3=horizontal


        Public tmpval1 As Single
        Public tmpval2 As Single

        'Node render information
        Public ColoredImage As Image
        Public SizedImage As Image
        Public FinalImage As Image
        Public ColorChanged As Boolean
        Public SizeChanged As Boolean
        Public ValueChanged As Boolean
    End Structure
End Module
