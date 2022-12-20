Public Class Form1
    Dim tupdater As System.Threading.Thread

#Region "Mouse selection"
    Public ResizeType As SelectionType = 0
    Public IsResizing As Boolean = False
    Public ResizeStartPosition As New Point(0, 0)
    Public ResizeStartPosVal As New Point(0, 0)
    Public ResizeStartSizeVal As New Size(32, 32)
    Public RefreshTime As Integer
    Private Sub PictureBox1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        PBCursorPosition = PictureBox1.PointToClient(Cursor.Position)
        PBCursorPosition.X *= ScaleX
        PBCursorPosition.Y *= ScaleY
        ResizeType = GetSelectionTypeAtPoint(PBCursorPosition)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If ResizeType = SelectionType.Outside Then
                Dim clickedindex As Integer = GetNodeIndexAtPoint(PBCursorPosition)
                If CurrentIndex <> clickedindex Or CurrentIndex = -1 Then
                    IsResizing = False
                    'Load not-loaded node
                    LoadNode(clickedindex)
                    Cursor.Current = Cursors.Default
                End If
            ElseIf CurrentIndex <> -1 Then
                'Resizing
                SetCursor(ResizeType, Nodes(CurrentIndex).GetValue(Node.ValueType.Rotation))
                ResizeStartPosition = Cursor.Position
                ResizeStartPosVal = Nodes(CurrentIndex).GetValue(Node.ValueType.Position)
                If ResizeType = SelectionType.SpecialMP Then ResizeStartPosVal = Nodes(CurrentIndex).PictureNodeData.DRotationMid
                ResizeStartSizeVal = Nodes(CurrentIndex).GetValue(Node.ValueType.Size)
                IsResizing = True
            End If
        End If
    End Sub
    Private Sub PictureBox1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp
        ResizeType = SelectionType.Outside
        IsResizing = False
    End Sub
    Public Function GetSelectionTypeAtPoint(ByVal Point As Point) As SelectionType
        If CurrentIndex <> -1 Then
            'Special type check!
            If ViewedDialog = 6 And Nodes(CurrentIndex).Type = "Picture Node" Then
                Dim mp As Point = Nodes(CurrentIndex).PictureNodeData.DRotationMid
                mp.X += 400
                mp.Y += 300
                If Math.Sqrt((mp.X - Point.X) ^ 2 + (mp.Y - Point.Y) ^ 2) <= 5 Then
                    Return 10 'SpecialMP
                End If
            End If
            Dim Position As Point = Nodes(CurrentIndex).GetValue(Node.ValueType.Position)
            Dim Size As Size = Nodes(CurrentIndex).GetValue(Node.ValueType.Size)
            Dim Rotation As Integer = Nodes(CurrentIndex).GetValue(Node.ValueType.Rotation)
            Return GetSquareSelectionType(Point, New Rectangle(Position, Size), Rotation)
        Else
            Return 0
        End If
    End Function
    Public Enum SelectionType
        Outside = 0
        TopLeft = 1
        TopMiddle = 2
        TopRight = 3
        MiddleLeft = 4
        MiddleRight = 5
        BottomLeft = 6
        BottomMiddle = 7
        BottomRight = 8
        Middle = 9
        SpecialMP = 10
    End Enum
    Private Sub SetCursor(ByVal ResizeType As SelectionType, ByVal RotationFactor As Integer)
        If ResizeType = SelectionType.Outside Then
            PictureBox1.Cursor = Cursors.Default
        ElseIf ResizeType = SelectionType.Middle Then
            PictureBox1.Cursor = Cursors.SizeAll
        ElseIf ResizeType = SelectionType.SpecialMP Then
            PictureBox1.Cursor = Cursors.NoMove2D
        Else
            Dim cmode As Integer = 0
            If ResizeType = SelectionType.TopRight Or ResizeType = SelectionType.BottomLeft Then cmode = 1
            If ResizeType = SelectionType.MiddleLeft Or ResizeType = SelectionType.MiddleRight Then cmode = 2
            If ResizeType = SelectionType.TopLeft Or ResizeType = SelectionType.BottomRight Then cmode = 3
            If RotationFactor >= 23 And RotationFactor < 68 Then cmode += 1
            If RotationFactor >= 68 And RotationFactor < 113 Then cmode += 2
            If RotationFactor >= 113 And RotationFactor < 158 Then cmode += 3
            If RotationFactor >= 203 And RotationFactor < 248 Then cmode += 1
            If RotationFactor >= 248 And RotationFactor < 293 Then cmode += 2
            If RotationFactor >= 293 And RotationFactor < 338 Then cmode += 3
            If cmode > 3 Then cmode -= 4
            If cmode = 0 Then PictureBox1.Cursor = Cursors.SizeNS
            If cmode = 1 Then PictureBox1.Cursor = Cursors.SizeNESW
            If cmode = 2 Then PictureBox1.Cursor = Cursors.SizeWE
            If cmode = 3 Then PictureBox1.Cursor = Cursors.SizeNWSE
        End If
    End Sub
    Private Sub PictureBox1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        PBCursorPosition = PictureBox1.PointToClient(Cursor.Position)
        PBCursorPosition.X *= ScaleX
        PBCursorPosition.Y *= ScaleY
        If CurrentIndex <> -1 Then
            If Nodes(CurrentIndex).Type = "Button Node" Then
                Dim oldm As Integer = Nodes(CurrentIndex).ButtonNodeData.DisplayMode
                Dim newm As Integer = 0
                If GetPointIsInNode(PBCursorPosition, CurrentIndex) = True Then newm = 1
                If oldm <> newm Then
                    Nodes(CurrentIndex).ButtonNodeData.DisplayMode = newm
                    Nodes(CurrentIndex).ButtonNodeData.PosTypeChanged = True
                    UpdateScreen = True
                End If
            End If
            If IsResizing = False Then
                ResizeType = GetSelectionTypeAtPoint(PBCursorPosition)
                If CurrentIndex <> -1 Then
                    'Me.Text = ResizeType
                    SetCursor(ResizeType, Nodes(CurrentIndex).GetValue(Node.ValueType.Rotation))
                Else
                    PictureBox1.Cursor = Cursors.Default
                End If
            ElseIf CurrentIndex <> -1 Then
                Dim rotation As Integer = Nodes(CurrentIndex).GetValue(Node.ValueType.Rotation)
                'Actual value changing
                'Set new variables
                Dim NewPosX As Integer = ResizeStartPosVal.X
                Dim NewPosY As Integer = ResizeStartPosVal.Y
                Dim NewSizeW As Integer = ResizeStartSizeVal.Width
                Dim NewSizeH As Integer = ResizeStartSizeVal.Height
                Dim offsetX As Integer = (Cursor.Position.X - ResizeStartPosition.X) * ScaleX
                Dim offsetY As Integer = (Cursor.Position.Y - ResizeStartPosition.Y) * ScaleY
                If ResizeType = SelectionType.Middle Or ResizeType = SelectionType.SpecialMP Then
                    'Move
                    NewPosX += offsetX
                    NewPosY += offsetY
                Else
                    'Convert point to square-related
                    Dim point As Point = PictureBox1.PointToClient(Cursor.Position)
                    point.X *= ScaleX
                    point.Y *= ScaleY
                    Dim Square As New Rectangle(ResizeStartPosVal, ResizeStartSizeVal)
                    point.X -= Square.X + Square.Width * 0.5
                    point.Y -= Square.Y + Square.Height * 0.5
                    Dim rotradian As Double = rotation * Math.PI / -180
                    If point.X > 0 Then rotradian += 0.5 * Math.PI
                    If point.X < 0 Then rotradian += 1.5 * Math.PI
                    If point.Y > 0 And point.X = 0 Then rotradian += Math.PI
                    If point.Y <> 0 And point.X <> 0 Then rotradian += Math.Atan(point.Y / point.X)
                    Dim radius As Double = Math.Sqrt(point.X ^ 2 + point.Y ^ 2)
                    offsetX = Math.Sin(rotradian) * radius + Square.Width * 0.5
                    offsetY = Square.Height * 0.5 - Math.Cos(rotradian) * radius
                    'Disabling offset if not used
                    If ResizeType = SelectionType.TopMiddle Or ResizeType = SelectionType.BottomMiddle Then offsetX = 0
                    If ResizeType = SelectionType.MiddleLeft Or ResizeType = SelectionType.MiddleRight Then offsetY = 0
                    'Using correct movement; 
                    If ResizeType = SelectionType.TopMiddle Then
                        If offsetY >= NewSizeH Then offsetY = NewSizeH
                        NewPosY += offsetY
                        NewSizeH -= offsetY
                    ElseIf ResizeType = SelectionType.BottomMiddle Then
                        If offsetY < 0 Then offsetY = 0
                        offsetY -= Square.Height
                        NewSizeH += offsetY
                    ElseIf ResizeType = SelectionType.MiddleRight Then
                        If offsetX < 0 Then offsetX = 0
                        offsetX -= Square.Width
                        NewSizeW += offsetX
                    ElseIf ResizeType = SelectionType.MiddleLeft Then
                        If offsetX >= NewSizeW Then offsetX = NewSizeW
                        NewPosX += offsetX
                        NewSizeW -= offsetX
                    ElseIf ResizeType = SelectionType.TopLeft Then
                        If offsetX >= NewSizeW Then offsetX = NewSizeW
                        If offsetY >= NewSizeH Then offsetY = NewSizeH
                        NewPosX += offsetX
                        NewPosY += offsetY
                        NewSizeW -= offsetX
                        NewSizeH -= offsetY
                    ElseIf ResizeType = SelectionType.TopRight Then
                        If offsetX < 0 Then offsetX = 0
                        If offsetY >= NewSizeH Then offsetY = NewSizeH
                        NewPosY += offsetY
                        NewSizeH -= offsetY
                        offsetX -= Square.Width
                        NewSizeW += offsetX
                    ElseIf ResizeType = SelectionType.BottomRight Then
                        If offsetX < 0 Then offsetX = 0
                        If offsetY < 0 Then offsetY = 0
                        offsetX -= Square.Width
                        NewSizeW += offsetX
                        offsetY -= Square.Height
                        NewSizeH += offsetY
                    ElseIf ResizeType = SelectionType.BottomLeft Then
                        If offsetX >= NewSizeW Then offsetX = NewSizeW
                        If offsetY < 0 Then offsetY = 0
                        NewPosX += offsetX
                        NewSizeW -= offsetX
                        offsetY -= Square.Height
                        NewSizeH += offsetY
                    End If
                    NewPosX -= offsetX * 0.5 * (1 - Math.Cos(rotation / 180 * Math.PI))
                    NewPosY -= offsetY * 0.5 * (1 - Math.Cos(rotation / 180 * Math.PI))
                    NewPosX -= offsetY * 0.5 * Math.Sin(rotation / 180 * Math.PI)
                    NewPosY -= offsetX * -0.5 * Math.Sin(rotation / 180 * Math.PI)
                End If
                Dim NewPos As New Point(NewPosX, NewPosY)
                Dim NewSize As New Size(SetValueBounds(NewSizeW, 1, 2048), SetValueBounds(NewSizeH, 1, 2048))
                If ResizeType = SelectionType.SpecialMP Then
                    Nodes(CurrentIndex).PictureNodeData.DRotationMid = NewPos
                    Nodes(CurrentIndex).PictureNodeData.PosRotChanged = True
                Else
                    Nodes(CurrentIndex).SetValue(Node.ValueType.Size, NewSize)
                    Nodes(CurrentIndex).SetValue(Node.ValueType.Position, NewPos)
                End If
                UpdateScreen = True
            End If
        End If
    End Sub
#End Region

    Private Sub Updater()
        Dim s As New Stopwatch
        s.Start()
        WriteLog("Updater thread running")
        Do
            s.Reset()
            s.Start()
            Try
                If UpdateScreen = True Then
                    UpdateScreen = False
                    Dim screenimage As New Bitmap(800, 600)
                    Dim g As Graphics = Graphics.FromImage(screenimage)
                    Dim CurSelPos As New Point(0, 0)
                    Dim CurSelSize As New Size(1, 1)
                    Dim CurSelRot As Integer = 0
                    If CurrentIndex <> -1 And DrawSelectionSquareToolStripMenuItem.Checked = True Then
                        If Nodes(CurrentIndex).Render = True Then
                            CurSelPos = Nodes(CurrentIndex).GetValue(Node.ValueType.Position)
                            CurSelSize = Nodes(CurrentIndex).GetValue(Node.ValueType.Size)
                            CurSelRot = Nodes(CurrentIndex).GetValue(Node.ValueType.Rotation)
                        End If
                    End If
                    RenderNodes(g)

                    '=================================================
                    'Render node selection boxes
                    Try
                        If CurrentIndex <> -1 Then
                            If Nodes(CurrentIndex).Render = True And Nodes(CurrentIndex).Type <> "Split Node" Then
                                If DrawSelectionSquareToolStripMenuItem.Checked = True Then g.DrawImage(RenderSelectionBox(CurSelPos, CurSelSize, CurSelRot), New Point(0, 0))
                                If ViewedDialog = 6 And Nodes(CurrentIndex).Type = "Picture Node" Then
                                    g.DrawPie(Pens.Red, New Rectangle(Nodes(CurrentIndex).PictureNodeData.DRotationMid.X + 395, Nodes(CurrentIndex).PictureNodeData.DRotationMid.Y + 295, 10, 10), 90, 360)
                                End If
                            End If
                        End If
                    Catch
                    End Try
                    '==================================================
                    PictureBoxAdapter(screenimage, Me)
                Else
                    System.Threading.Thread.Sleep(50)
                End If
            RefreshTime = s.ElapsedMilliseconds
            Catch ex As Exception
                MsgBox("Render error: " & vbCrLf & vbCrLf & ex.Message)
                WriteLog("Render error: " & ex.Message)
            End Try
        Loop
    End Sub
    Private Sub LoadLibraries()
        ReDim LibraryTextures(-1)
        If System.IO.Directory.Exists(Application.StartupPath & "\Textures") Then
            'Loading library image paths
            For Each file As String In System.IO.Directory.GetFiles(Application.StartupPath & "\Textures")
                If file.EndsWith(".texlib") Then
                    WriteLog("Loading library: " & file)
                    Dim newimages() As ImagePointer = LoadImageData(file)
                    Dim oldcount As Integer = LibraryTextures.Length
                    ReDim Preserve LibraryTextures(oldcount + newimages.Count - 1)
                    newimages.CopyTo(LibraryTextures, oldcount)
                    newimages = Nothing
                End If
            Next
        End If
        WriteLog("Total of " & LibraryTextures.Count & " textures found in libraries")
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If ContentCheck() Then
            Try
                If System.IO.File.Exists(Application.StartupPath & "\log.txt") Then System.IO.File.Delete(Application.StartupPath & "\log.txt")
            Catch
            End Try
            ChDir(Application.StartupPath)
            BGimage = ResizeImage(Image.FromFile(Application.StartupPath & "\bin\Background\Generic background.jpg"), 800, 600, True)
            PictureBox1.BackgroundImage = BGimage
            LoadLibraries()
            'Loading dragged and dropped files
            For Each file As String In Command.Split(Chr(34) & " " & Chr(34))
                If System.IO.File.Exists(file) Then
                    LoadFile(file)
                End If
            Next
            'Test-mode
            Dim testmodeactive As Boolean = False
            If testmodeactive = True Then
                Timer1.Start()
                Label2.Visible = True
                Dim n As New Node("VehicleHuds", "TestNode1", "Picture Node")
                n.PictureNodeData.Position = New Point(369, 269)
                n.PictureNodeData.Size = New Size(64, 64)
                n.LoadLine("hudBuilder.setPictureNodeTexture Ingame\Vehicles\Icons\Hud\Air\Attack\Ah1z\ah1z_gunnercross3.dds")
                n.LoadLine("hudBuilder.setNodeColor 0 0.8 0 1")
                n.LoadLine("hudBuilder.setPictureNodeRotation 315")
                n.PictureNodeData.ColorChanged = True
                n.PictureNodeData.SizeChanged = True
                n.PictureNodeData.PosRotChanged = True
                InsertNode(n, 1)
                VariableTester.Show()
                WriteLog("Test mode initialized")
            End If
            WriteLog("Application loaded")
            'Updater
            UpdateScreen = True
            tupdater = New System.Threading.Thread(AddressOf Updater)
            tupdater.IsBackground = True
            tupdater.Start()
        End If
    End Sub
    Private Sub Form1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        If Me.WindowState = FormWindowState.Minimized Then
            SetViewedDialog(0)
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Panel1.Visible = True Then Panel1.Visible = False Else Panel1.Visible = True
    End Sub

    Private Sub SimulateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimulateButton.Click
        SetViewedDialog(0)
        Panel1.Visible = False
        Simulator.ShowDialog()
    End Sub
    Private Sub TextureButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextureButton.Click
        SetViewedDialog(1)
    End Sub
    Private Sub ColorButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorButton.Click
        SetViewedDialog(2)
    End Sub
    Private Sub MainButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainButton.Click
        SetViewedDialog(3)
    End Sub
    Private Sub TSizeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TSizeButton.Click
        SetViewedDialog(4)
    End Sub
    Private Sub RotationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RotationButton.Click
        SetViewedDialog(5)
    End Sub
    Private Sub VariablesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VariablesButton.Click
        SetViewedDialog(6)
    End Sub
    Private Sub StyleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StyleButton.Click
        SetViewedDialog(7)
    End Sub
    Private Sub FTextureButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FTextureButton.Click
        TextureBrowser.Text = "full"
        SetViewedDialog(1)
    End Sub
    Private Sub ETextureButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ETextureButton.Click
        TextureBrowser.Text = "empty"
        SetViewedDialog(1)
    End Sub
    Private Sub ShowButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowButton.Click
        SetViewedDialog(8)
    End Sub

    Private Sub BackgroundToolStripMenuItem_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackgroundToolStripMenuItem.DropDownOpening
        Dim selitem As String = ToolStripComboBox2.SelectedItem
        ToolStripComboBox2.Items.Clear()
        If System.IO.Directory.Exists(Application.StartupPath & "\bin\Background") Then
            For Each bfile As String In System.IO.Directory.GetFiles(Application.StartupPath & "\bin\Background")
                Dim add As Boolean = False
                If bfile.ToLower.EndsWith(".png") Then add = True
                If bfile.ToLower.EndsWith(".gif") Then add = True
                If bfile.ToLower.EndsWith(".bmp") Then add = True
                If bfile.ToLower.EndsWith(".tga") Then add = True
                If bfile.ToLower.EndsWith(".dds") Then add = True
                If bfile.ToLower.EndsWith(".jpg") Then add = True
                If bfile.ToLower.EndsWith(".jpeg") Then add = True
                If add = True Then ToolStripComboBox2.Items.Add(IO.Path.GetFileNameWithoutExtension(bfile))
            Next
        End If
        ToolStripComboBox2.SelectedItem = selitem
    End Sub
    Private Sub OverlayToolStripMenuItem_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OverlayToolStripMenuItem.DropDownOpening
        Dim selitem As String = ToolStripComboBox3.SelectedItem
        ToolStripComboBox3.Items.Clear()
        If System.IO.Directory.Exists(Application.StartupPath & "\bin\Overlay") Then
            For Each bfile As String In System.IO.Directory.GetFiles(Application.StartupPath & "\bin\Overlay")
                Dim add As Boolean = False
                If bfile.ToLower.EndsWith(".png") Then add = True
                If bfile.ToLower.EndsWith(".gif") Then add = True
                If bfile.ToLower.EndsWith(".bmp") Then add = True
                If bfile.ToLower.EndsWith(".tga") Then add = True
                If bfile.ToLower.EndsWith(".dds") Then add = True
                If bfile.ToLower.EndsWith(".jpg") Then add = True
                If bfile.ToLower.EndsWith(".jpeg") Then add = True
                If IO.Path.GetFileNameWithoutExtension(bfile).ToLower = "no overlay" Then add = False
                If add = True Then ToolStripComboBox3.Items.Add(IO.Path.GetFileNameWithoutExtension(bfile))
            Next
        End If
        ToolStripComboBox3.Items.Add("no overlay")
        If selitem = "" Then selitem = "no overlay"
        ToolStripComboBox3.SelectedItem = selitem
    End Sub
    Private Sub ToolStripComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripComboBox2.SelectedIndexChanged
        If ToolStripComboBox2.SelectedIndex <> -1 Then
            Try
                'getting the file path
                Dim filepath As String = Application.StartupPath & "\bin\Background\"
                Dim finalpath As String = ""
                If System.IO.File.Exists(filepath & ToolStripComboBox2.SelectedItem & ".jpg") Then finalpath = filepath & ToolStripComboBox2.SelectedItem & ".jpg"
                If System.IO.File.Exists(filepath & ToolStripComboBox2.SelectedItem & ".jpeg") Then finalpath = filepath & ToolStripComboBox2.SelectedItem & ".jpeg"
                If System.IO.File.Exists(filepath & ToolStripComboBox2.SelectedItem & ".bmp") Then finalpath = filepath & ToolStripComboBox2.SelectedItem & ".bmp"
                If System.IO.File.Exists(filepath & ToolStripComboBox2.SelectedItem & ".gif") Then finalpath = filepath & ToolStripComboBox2.SelectedItem & ".gif"
                If System.IO.File.Exists(filepath & ToolStripComboBox2.SelectedItem & ".png") Then finalpath = filepath & ToolStripComboBox2.SelectedItem & ".png"
                If System.IO.File.Exists(filepath & ToolStripComboBox2.SelectedItem & ".dds") Then finalpath = filepath & ToolStripComboBox2.SelectedItem & ".dds"
                If System.IO.File.Exists(filepath & ToolStripComboBox2.SelectedItem & ".tga") Then finalpath = filepath & ToolStripComboBox2.SelectedItem & ".tga"
                If System.IO.File.Exists(finalpath) Then
                    BGimage = ResizeImage(LoadImage(finalpath), 800, 600, True)
                Else
                    MsgBox(finalpath)
                End If
            Catch
                BGimage = ResizeImage(LoadImage(Application.StartupPath & "\Bin\Background\Generic background.jpg"), 800, 600, True)
            End Try
            Dim finalpbimage As New Bitmap(800, 600)
            Dim g As Graphics = Graphics.FromImage(finalpbimage)
            g.DrawImage(BGimage, New Point(0, 0))
            If DrawReferenceCrossToolStripMenuItem.Checked = True Then
                g.DrawImage(ResizeImage(LoadImage(Application.StartupPath & "\bin\referencecross.gif"), 800, 600, True), New Point(0, 0))
            End If
            g.Dispose()
            PictureBox1.BackgroundImage = finalpbimage
        End If
    End Sub
    Private Sub DrawReferenceCrossToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DrawReferenceCrossToolStripMenuItem.CheckedChanged
        If DrawReferenceCrossToolStripMenuItem.Checked = True Then
            Dim finalpbimage As New Bitmap(800, 600)
            Dim g As Graphics = Graphics.FromImage(finalpbimage)
            g.DrawImage(BGimage, New Point(0, 0))
            If DrawReferenceCrossToolStripMenuItem.Checked = True Then Graphics.FromImage(finalpbimage).DrawImage(ResizeImage(LoadImage(Application.StartupPath & "\bin\referencecross.gif"), 800, 600, True), New Point(0, 0))
            PictureBox1.BackgroundImage = finalpbimage
        Else
            PictureBox1.BackgroundImage = BGimage
        End If
    End Sub
    Private Sub ToolStripComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripComboBox3.SelectedIndexChanged
        Try
            'getting the file path
            Dim filepath As String = Application.StartupPath & "\bin\Overlay\"
            Dim finalpath As String = ""
            If System.IO.File.Exists(filepath & ToolStripComboBox3.SelectedItem & ".jpg") Then finalpath = filepath & ToolStripComboBox3.SelectedItem & ".jpg"
            If System.IO.File.Exists(filepath & ToolStripComboBox3.SelectedItem & ".jpeg") Then finalpath = filepath & ToolStripComboBox3.SelectedItem & ".jpeg"
            If System.IO.File.Exists(filepath & ToolStripComboBox3.SelectedItem & ".bmp") Then finalpath = filepath & ToolStripComboBox3.SelectedItem & ".bmp"
            If System.IO.File.Exists(filepath & ToolStripComboBox3.SelectedItem & ".gif") Then finalpath = filepath & ToolStripComboBox3.SelectedItem & ".gif"
            If System.IO.File.Exists(filepath & ToolStripComboBox3.SelectedItem & ".png") Then finalpath = filepath & ToolStripComboBox3.SelectedItem & ".png"
            If System.IO.File.Exists(filepath & ToolStripComboBox3.SelectedItem & ".dds") Then finalpath = filepath & ToolStripComboBox3.SelectedItem & ".dds"
            If System.IO.File.Exists(filepath & ToolStripComboBox3.SelectedItem & ".tga") Then finalpath = filepath & ToolStripComboBox3.SelectedItem & ".tga"
            If System.IO.File.Exists(finalpath) Then
                OVimage = ResizeImage(LoadImage(finalpath), 800, 600, True)
            Else
                OVimage = New Bitmap(16, 16)
            End If
        Catch
        End Try
        UpdateScreen = True
    End Sub
    Private Sub DrawSelectionSquareToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DrawSelectionSquareToolStripMenuItem.CheckedChanged
        UpdateScreen = True
    End Sub
    Private Sub ForceUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ForceUpdateToolStripMenuItem.Click
        RefreshNodes()
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        SaveFileDialog1.FileName = IO.Path.GetFileNameWithoutExtension(CurrentFileName)
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            WriteLog("Saving to: " & SaveFileDialog1.FileName)
            Try
                'pre-loading
                Dim nodestoprocess As New List(Of Node)
                For i As Integer = 1 To Nodes.Count - 1
                    nodestoprocess.Add(Nodes(i))
                Next
                WriteLog("Nodes to save from scene: " & nodestoprocess.Count)
                'Start saving to nodedata
                Dim NodeData As String = ""
                Dim cpnindex As Integer = 0
                Do While nodestoprocess.Count <> 0
                    Dim cpnparent As String = nodestoprocess(cpnindex).Parent
                    Dim cpnname As String = nodestoprocess(cpnindex).Name
                    'Determine the parent node in the nodes list
                    Dim parentexists As Boolean = False
                    For i As Integer = 0 To nodestoprocess.Count - 1
                        If i <> cpnindex Then
                            If nodestoprocess(i).Name.ToLower.Trim = cpnparent.ToLower.Trim Then
                                'A parent exists, continue with parent
                                cpnindex = i
                                parentexists = True
                                Exit For
                            End If
                        End If
                    Next
                    If parentexists = False And nodestoprocess.Count <> 0 Then
                        'No parent exists, this is a new root. Add it and remove it from the list
                        NodeData &= nodestoprocess(cpnindex).SaveData & vbCrLf & vbCrLf
                        nodestoprocess.RemoveAt(cpnindex)
                        cpnindex = -1

                        'Determine if nodes were added to this node
                        For i As Integer = 0 To nodestoprocess.Count - 1
                            If nodestoprocess(i).Parent.ToLower.Trim = cpnname.ToLower.Trim Then
                                cpnindex = i
                                If nodestoprocess(i).Type <> "Split Node" Then Exit For
                            End If
                        Next
                        If cpnindex = -1 Then
                            'This node was no parent of other nodes, search for nodes with the same parent
                            For i As Integer = 0 To nodestoprocess.Count - 1
                                If nodestoprocess(i).Parent.ToLower.Trim = cpnparent.ToLower.Trim Then
                                    cpnindex = i
                                    If nodestoprocess(i).Type <> "Split Node" Then Exit For
                                End If
                            Next
                        End If
                        If cpnindex = -1 Then cpnindex = 0
                    End If
                Loop
                nodestoprocess.Clear()
                nodestoprocess = Nothing
                Dim writer As New System.IO.StreamWriter(SaveFileDialog1.FileName, False)
                writer.WriteLine(NodeData)
                writer.Close()
            Catch ex As Exception
                MsgBox("Failed to save: " & vbCrLf & ex.Message, MsgBoxStyle.Critical)
                WriteLog("Error while saving: " & ex.Message)
            End Try
        End If
    End Sub
    Private Sub ResetScreenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetScreenToolStripMenuItem.Click
        Me.WindowState = FormWindowState.Normal
        Me.Size = New Size(832, 665)
    End Sub

    Private Sub UpdateGUIComboBox()
        Dim OldIndex As String = ComboBox1.SelectedItem
        ComboBox1.Items.Clear()
        For i As Integer = 1 To Nodes.Count - 1
            For Each Var As String In Nodes(i).LogicShowVariables.Items
                Var = Var.ToLower.Trim
                If Var.StartsWith("equal guiindex ") Then
                    Dim Index As String = Var.Remove(0, 15).Trim
                    If Not ComboBox1.Items.Contains(Index) Then ComboBox1.Items.Add(Index)
                ElseIf Var.StartsWith("or guiindex ") Then
                    Dim Index As String = Var.Remove(0, 12).Trim
                    If Not ComboBox1.Items.Contains(Index) Then ComboBox1.Items.Add(Index)
                End If
            Next
        Next
        ComboBox1.Items.Add("All")
        ComboBox1.SelectedItem = OldIndex
    End Sub
    Dim UpdatingTreeView As Boolean = False
    Private Sub UpdateTreeView()
        UpdatingTreeView = True
        If Panel1.Visible = True Then
            'Get a list of node trees
            Dim nodepaths As New List(Of String)
            For i As Integer = 1 To Nodes.Count - 1
                Dim nodetree As String = GetNodeTree(Nodes(i))
                TreeViewAdapter(nodetree)
                GetTreeViewNode(TreeView1, Nodes(i).Name).Checked = Nodes(i).Render
                nodepaths.Add(nodetree.ToLower.Trim)
            Next
            'Get a list of nodes that should possibly be deleted
            Dim deletepaths As New List(Of String)
            For Each item As TreeNode In GetTreeViewNodes(TreeView1)
                If Not nodepaths.Contains(item.FullPath.ToLower.Trim) Then deletepaths.Add(item.FullPath.ToLower.Trim)
            Next
            If Not deletepaths.Contains("vehiclehuds") Then TreeViewAdapter("VehicleHuds")
            If Not deletepaths.Contains("weaponhuds") Then TreeViewAdapter("WeaponHuds")
            If Not deletepaths.Contains("ingamehud") Then TreeViewAdapter("IngameHud")
            For Each deletepath In deletepaths
                'Check if this node should really be deleted
                Dim deletethis As Boolean = deletepath <> "vehiclehuds" And deletepath <> "weaponhuds" And deletepath <> "ingamehud"
                For Each nodetree As String In nodepaths
                    If deletepath = nodetree.Split("\").First Then deletethis = False
                Next
                If deletethis = True Then
                    For Each TNode As TreeNode In GetTreeViewNodes(TreeView1)
                        If TNode.FullPath.ToLower.Trim = deletepath Then
                            TNode.Remove()
                            Exit For
                        End If
                    Next
                End If
            Next
        End If
        UpdatingTreeView = False
    End Sub

    Private Sub TreeViewAdapter(ByVal FilePath As String)
        Dim InputPath As String = FilePath.Replace("/", "\").Replace("\\", "\")
        Dim ActiveNode As New TreeNode
        Dim counter As Integer = 0
        Dim hasadded As Boolean = False
        For Each word As String In InputPath.Split("\")
            Dim add As Boolean = True
            If counter = 0 Then
                For Each node As TreeNode In TreeView1.Nodes
                    If node.Text.ToLower = word.ToLower Then add = False
                Next
                If add = True Then TreeView1.Nodes.Add(word, word)
                For Each node As TreeNode In TreeView1.Nodes
                    If node.Text.ToLower = word.ToLower Then ActiveNode = node
                Next
            Else
                For Each node As TreeNode In ActiveNode.Nodes
                    If node.Text.ToLower = word.ToLower Then add = False
                Next
                If add = True Then ActiveNode.Nodes.Add(word, word)
                For Each node As TreeNode In ActiveNode.Nodes
                    If node.Text.ToLower = word.ToLower Then ActiveNode = node
                Next
            End If
            If add = True Then hasadded = True
            counter += 1
        Next
        If hasadded = True Then
            'Node has been added

        End If
    End Sub
    Private Sub AddNewNode()
        Dim SelNodeName As String = "VehicleHuds"
        If CurrentIndex = -1 Then
            Try
                SelNodeName = TreeView1.SelectedNode.Text
            Catch
            End Try
        Else
            SelNodeName = Nodes(CurrentIndex).Name
        End If
        Dim Type As Integer = 0 '0=splitnode; 1=other
        Dim Index As Integer = -1
        For i As Integer = 1 To Nodes.Count - 1
            If Nodes(i).Name = SelNodeName And Nodes(i).Type <> "Split Node" Then
                Type = 1
                Index = i
                Exit For
            End If
        Next
        Dim ParentName As String = SelNodeName
        If Type = 1 Then ParentName = Nodes(Index).Parent
        AddNode.Text = "Add Node to: " & ParentName
        If AddNode.ShowDialog = Windows.Forms.DialogResult.OK Then
            InsertNode(New Node(ParentName, AddNode.TextBox1.Text, AddNode.ComboBox1.SelectedItem), Nodes.Count)
            LoadNode(Nodes.Count - 1)
            UpdateTreeView()
        End If
    End Sub
    Private Sub DeleteNode()
        If CurrentIndex > 0 Then
            If Nodes(CurrentIndex).Type = "Split Node" Then
                If MessageBox.Show("Are you sure you want to delete split node: " & Chr(34) & Nodes(CurrentIndex).Name & Chr(34) & " and all underlying child nodes?", "Delete Node", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    Dim selnodetree As String = GetNodeTree(Nodes(CurrentIndex)).ToLower.Trim
                    Dim delindices As New List(Of Integer)
                    Dim ui As Integer = 1
                    For i As Integer = 1 To Nodes.Count - 1
                        If GetNodeTree(Nodes(i)).ToLower.Trim.StartsWith(selnodetree) Then
                            delindices.Add(ui)
                        Else
                            ui += 1
                        End If
                    Next
                    For Each index As Integer In delindices
                        RemoveNode(index)
                    Next
                    delindices = Nothing
                    LoadNode(-1)
                    UpdateScreen = True
                    UpdateTreeView()
                    SetViewedDialog(0)
                End If
            Else
                If MessageBox.Show("Are you sure you want to delete node: " & Chr(34) & Nodes(CurrentIndex).Name & Chr(34) & "?", "Delete Node", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    RemoveNode(CurrentIndex)
                    LoadNode(-1)
                    UpdateScreen = True
                    UpdateTreeView()
                    SetViewedDialog(0)
                End If
            End If
        End If
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        PictureBox1.Focus()
    End Sub

    Dim ControlPressed As Boolean = False
    Private Sub PictureBox1_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles PictureBox1.PreviewKeyDown, Button1.PreviewKeyDown
        ControlPressed = False
        PictureBox1.Focus()
        If e.KeyCode = Keys.Space Then
            If DrawSelectionSquareToolStripMenuItem.Checked = True Then
                DrawSelectionSquareToolStripMenuItem.Checked = False
            Else
                DrawSelectionSquareToolStripMenuItem.Checked = True
            End If
        ElseIf e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Then
            Dim offset As New Point(0, 0)
            If e.KeyCode = Keys.Up Then offset.Y = -1
            If e.KeyCode = Keys.Down Then offset.Y = 1
            If e.KeyCode = Keys.Left Then offset.X = -1
            If e.KeyCode = Keys.Right Then offset.X = 1
            Dim posval As Point = Nodes(CurrentIndex).GetValue(Node.ValueType.Position)
            Dim sizeval As Size = Nodes(CurrentIndex).GetValue(Node.ValueType.Size)
            posval.X += offset.X
            posval.Y += offset.Y
            Nodes(CurrentIndex).SetValue(Node.ValueType.Position, posval)
            Nodes(CurrentIndex).SetValue(Node.ValueType.Size, sizeval)
            UpdateScreen = True
        End If
    End Sub

    Private Sub PictureBox1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.SizeChanged
        ScaleX = 800 / PictureBox1.Size.Width
        ScaleY = 600 / PictureBox1.Size.Height
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        SelectNodeToolStripMenuItem.DropDownItems.Clear()
        For i As Integer = 1 To Nodes.Count - 1
            If i <> CurrentIndex Then SelectNodeToolStripMenuItem.DropDownItems.Add(Nodes(i).Name)
        Next
        SelectNodeToolStripMenuItem.Visible = SelectNodeToolStripMenuItem.DropDownItems.Count <> 0
        DeselectToolStripMenuItem.Visible = CurrentIndex <> -1
        HideToolStripMenuItem.Visible = CurrentIndex <> -1
        BringToFrontToolStripMenuItem.Visible = CurrentIndex <> -1
        SendToBackToolStripMenuItem.Visible = CurrentIndex <> -1
        CopyToolStripMenuItem.Enabled = CurrentIndex <> -1
        CutToolStripMenuItem.Enabled = CurrentIndex <> -1
        PasteToolStripMenuItem.Enabled = Clipboard.GetDataObject().GetDataPresent(DataFormats.Text)
        DeleteToolStripMenuItem.Enabled = CurrentIndex <> -1
        If IsResizing = True Then
            'Set defaults back
            Nodes(CurrentIndex).SetValue(Node.ValueType.Position, ResizeStartPosVal)
            Nodes(CurrentIndex).SetValue(Node.ValueType.Size, ResizeStartSizeVal)
            SetCursor(SelectionType.Outside, 0)
            IsResizing = False
            UpdateScreen = True
        End If
    End Sub
    Private Sub SelectNodeToolStripMenuItem_DropDownItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles SelectNodeToolStripMenuItem.DropDownItemClicked
        For i As Integer = 0 To Nodes.Count - 1
            If Nodes(i).Name = e.ClickedItem.Text Then LoadNode(i)
        Next
    End Sub
    Private Sub SendToBackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendToBackToolStripMenuItem.Click
        Dim node As Node = Nodes(CurrentIndex)
        RemoveNode(CurrentIndex)
        InsertNode(node, 1)
        LoadNode(1)
        UpdateScreen = True
    End Sub
    Private Sub BringToFrontToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BringToFrontToolStripMenuItem.Click
        Dim node As Node = Nodes(CurrentIndex)
        RemoveNode(CurrentIndex)
        InsertNode(node, Nodes.Count)
        LoadNode(Nodes.Count - 1)
        UpdateScreen = True
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        Cursor.Current = Cursors.WaitCursor
        If CurrentIndex <> -1 Then Clipboard.SetData(DataFormats.Text, Nodes(CurrentIndex).SaveData)
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        Cursor.Current = Cursors.WaitCursor
        If Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) Then
            Dim NewNodes() As Node = LoadNodeData(Clipboard.GetDataObject().GetData(DataFormats.Text))
            Dim success As Boolean = False
            Dim checknames As New ListBox
            For i As Integer = 1 To Nodes.Count - 1
                checknames.Items.Add(Nodes(i).Name.ToLower.Trim)
            Next
            For Each NewNode As Node In NewNodes
                If NewNode.Name <> "" And NewNode.Parent <> "" And NewNode.Type <> "" Then
                    Dim oldname As String = NewNode.Name.Trim
                    Dim cindex As Integer = 1
                    Do While checknames.Items.Contains(NewNode.Name.ToLower.Trim)
                        NewNode.Name = oldname & "_copy" & cindex
                        cindex += 1
                    Loop
                    checknames.Items.Add((oldname & "_copy" & cindex).Trim.ToLower)
                    InsertNode(NewNode, Nodes.Count)
                    LoadNode(Nodes.Count - 1)
                    success = True
                End If
            Next
            If success = False Then MsgBox("Failed to load data from clipboard.", MsgBoxStyle.Critical)
            If success = True Then RefreshNodes()
            UpdateTreeView()
        End If
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
        Cursor.Current = Cursors.WaitCursor
        If CurrentIndex <> -1 Then
            Clipboard.SetData(DataFormats.Text, Nodes(CurrentIndex).SaveData)
            RemoveNode(CurrentIndex)
            LoadNode(-1)
            UpdateTreeView()
        End If
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click, DeleteButton.Click
        DeleteNode()
    End Sub
    Private Sub NewNodeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewNodeToolStripMenuItem.Click, AddButton.Click
        AddNewNode()
    End Sub
    Private Sub ToolStripButtons_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VariablesButton.VisibleChanged, TSizeButton.VisibleChanged, TextureButton.VisibleChanged, StyleButton.VisibleChanged, SimulateButton.VisibleChanged, RotationButton.VisibleChanged, MainButton.VisibleChanged, ColorButton.VisibleChanged
        If MainButton.Visible = False And ViewedDialog = 3 Then MainDialog.Close()
        If StyleButton.Visible = False And ViewedDialog = 7 Then TextStyle.Close()
        If StyleButton.Visible = False And ViewedDialog = 7 Then CompassStyle.Close()
        If VariablesButton.Visible = False And ViewedDialog = 6 Then cnvariables.Close()
        If VariablesButton.Visible = False And ViewedDialog = 6 Then pnvariables.Close()
        If VariablesButton.Visible = False And ViewedDialog = 6 Then tnvariables.Close()
        If ColorButton.Visible = False And ViewedDialog = 2 Then ColorDialog.Close()
        If RotationButton.Visible = False And ViewedDialog = 5 Then RotationDialog.Close()
        If TextureButton.Visible = False And ViewedDialog = 1 Then TextureBrowser.Close()
        If TSizeButton.Visible = False And ViewedDialog = 4 Then TSizeDialog.Close()
    End Sub
    Private Sub ContextMenuStrip1_Closing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles ContextMenuStrip1.Closing
        CopyToolStripMenuItem.Enabled = True
        CutToolStripMenuItem.Enabled = True
        DeleteToolStripMenuItem.Enabled = True
        PasteToolStripMenuItem.Enabled = True
    End Sub

    Private Sub Panel1_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel1.VisibleChanged
        If Panel1.Visible = True Then
            UpdateTreeView()
            UpdateGUIComboBox()
        End If
    End Sub

    Private Sub LoadFile(ByVal FileName As String)
        Try
            WriteLog("Loading file: " & FileName)
            Dim reader As New System.IO.StreamReader(FileName)
            Dim NewNodes() As Node = LoadNodeData(reader.ReadToEnd)
            reader.Close()
            WriteLog(NewNodes.Count & " nodes found")
            Dim result As MsgBoxResult = MsgBoxResult.No
            If Nodes.Count > 1 Then
                result = MessageBox.Show("The current scene is not empty. Replace the current scene with the loaded file?", "Load", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)
                If result = MsgBoxResult.Yes Then
                    WriteLog("Erasing previous scene: " & Nodes.Count - 1 & " nodes.")
                    Do While Nodes.Count > 1
                        RemoveNode(1)
                    Loop
                    WriteLog(Nodes.Count - 1 & " nodes remain after erasing")
                    Modified = False
                End If
            End If
            If result <> MsgBoxResult.Cancel Then
                Dim success As Boolean = False
                Dim checknames As New ListBox
                For i As Integer = 1 To Nodes.Count - 1
                    checknames.Items.Add(Nodes(i).Name.ToLower.Trim)
                Next
                For Each NewNode As Node In NewNodes
                    If NewNode.Name <> "" And NewNode.Parent <> "" And NewNode.Type <> "" Then
                        Dim oldname As String = NewNode.Name.Trim
                        Dim cindex As Integer = 2
                        Do While checknames.Items.Contains(NewNode.Name.ToLower.Trim)
                            NewNode.Name = oldname & "(" & cindex & ")"
                            cindex += 1
                        Loop
                        checknames.Items.Add((oldname & "_copy" & cindex).ToLower.Trim)
                        InsertNode(NewNode, Nodes.Count)
                        LoadNode(Nodes.Count - 1)
                        success = True
                    End If
                Next
                If success = False Then MsgBox("Failed to load data.", MsgBoxStyle.Critical)
            End If
            RefreshNodes()
            UpdateTreeView()
            CurrentFileName = FileName
        Catch ex As Exception
            MsgBox("Failed to load file: " & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical)
            WriteLog("Failed to load: " & OpenFileDialog1.FileName)
            WriteLog("Error: " & ex.Message)
        End Try
    End Sub

    Dim draggedindex As Integer = -1
    Private Sub TreeView1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragDrop
        Dim newparent As String = TreeView1.GetNodeAt(TreeView1.PointToClient(Cursor.Position)).Text
        If newparent <> "VehicleHuds" And newparent <> "IngameHud" And newparent <> "WeaponHuds" Then
            Dim nodeindex As Integer = GetNodeNameIndex(newparent)
            If Nodes(nodeindex).Type <> "Split Node" Then
                newparent = Nodes(nodeindex).Parent
            End If
        End If
        If draggedindex = -1 Then
            'Text was dragged
            Dim NewNodes() As Node = LoadNodeData(e.Data.GetData(DataFormats.Text))
            Dim success As Boolean = False
            Dim checknames As New ListBox
            For i As Integer = 1 To Nodes.Count - 1
                checknames.Items.Add(Nodes(i).Name.ToLower.Trim)
            Next
            For Each NewNode As Node In NewNodes
                If NewNode.Name <> "" And NewNode.Parent <> "" And NewNode.Type <> "" Then
                    Dim oldname As String = NewNode.Name.Trim
                    Dim cindex As Integer = 1
                    Do While checknames.Items.Contains(NewNode.Name.ToLower.Trim)
                        NewNode.Name = oldname & "_copy" & cindex
                        cindex += 1
                    Loop
                    checknames.Items.Add((oldname & "_copy" & cindex).Trim.ToLower)
                    NewNode.Parent = newparent
                    InsertNode(NewNode, Nodes.Count)
                    success = True
                End If
            Next
            LoadNode(Nodes.Count - 1)
            If success = False Then MsgBox("Failed to load dragged data.", MsgBoxStyle.Critical)
            If success = True Then RefreshNodes()
        Else
            'Existing item was dragged
            Nodes(draggedindex).Parent = newparent
            WriteLog("Parent of node " & Nodes(draggedindex).Name & " (" & draggedindex & ") changed to " & newparent)
        End If
        UpdateTreeView()
        draggedindex = -1
    End Sub
    Private Sub TreeView1_ItemDrag(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles TreeView1.ItemDrag
        draggedindex = GetNodeNameIndex(CType(e.Item, TreeNode).Text)
        If draggedindex <> -1 Then TreeView1.DoDragDrop(Nodes(draggedindex).SaveData, DragDropEffects.Move)
    End Sub
    Private Sub TreeView1_DragOver(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragOver, PictureBox1.DragOver
        If e.Data.GetDataPresent(DataFormats.Text) Then
            Me.Focus()
            TreeView1.Focus()
            e.Effect = DragDropEffects.Move
            Dim otreenode As TreeNode = TreeView1.GetNodeAt(TreeView1.PointToClient(Cursor.Position))
            If otreenode.Text = "VehicleHuds" Or otreenode.Text = "IngameHud" Or otreenode.Text = "WeaponHuds" Then
                TreeView1.SelectedNode = otreenode
            Else
                Dim nodeindex As Integer = GetNodeNameIndex(otreenode.Text)
                If Nodes(nodeindex).Type = "Split Node" And Nodes(nodeindex).Name <> Nodes(draggedindex).Name Then
                    TreeView1.SelectedNode = otreenode
                Else
                    TreeView1.SelectedNode = GetTreeViewNode(TreeView1, Nodes(draggedindex).Parent)
                End If
            End If
        End If
    End Sub
    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        Try
            Dim currindex As Integer = GetNodeNameIndex(TreeView1.SelectedNode.Text)
            LoadNode(currindex, False)
            If TreeView1.SelectedNode.Text <> "VehicleHuds" And TreeView1.SelectedNode.Text <> "IngameHud" And TreeView1.SelectedNode.Text <> "WeaponHuds" Then
                DeleteButton.Enabled = True
                If Nodes(currindex).Render = False Then
                    For i As Integer = 0 To ComboBox1.Items.Count - 1
                        ComboBox1.SelectedIndex = i
                        If Nodes(currindex).Render = True Then Exit For
                    Next
                End If
            Else
                DeleteButton.Enabled = False
            End If
        Catch
            DeleteButton.Enabled = False
        End Try
    End Sub
    Private Sub TreeView1_AfterCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterCheck
        If UpdatingTreeView = False Then
            Dim nodeindex As Integer = GetNodeNameIndex(e.Node.Text)
            If nodeindex <> -1 Then Nodes(nodeindex).Render = e.Node.Checked
            For Each subnode As TreeNode In e.Node.Nodes
                subnode.Checked = e.Node.Checked
            Next
            UpdateScreen = True
        End If
    End Sub

    Private Sub ComboBox1_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.DropDown
        UpdateGUIComboBox()
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Cursor.Current = Cursors.WaitCursor
        If ComboBox1.SelectedItem <> "All" Then
            Dim allowedpaths As New List(Of String)
            For i As Integer = 1 To Nodes.Count - 1
                Dim render As Boolean = True
                Dim nodepath As String = GetNodeTree(Nodes(i))
                For Each Node As String In nodepath.Split("\")
                    If render = True Then
                        Dim nodeindex As Integer = GetNodeNameIndex(Node)
                        If nodeindex <> -1 Then
                            Dim allow As Boolean = False
                            Dim haschecked As Boolean = False
                            For Each var As String In Nodes(nodeindex).LogicShowVariables.Items
                                haschecked = True
                                var = var.ToLower.Trim
                                If var.StartsWith("equal guiindex ") Then
                                    If var.Remove(0, 15).Trim = ComboBox1.SelectedItem Then allow = True
                                ElseIf var.StartsWith("or guiindex ") Then
                                    If var.Remove(0, 12).Trim = ComboBox1.SelectedItem Then allow = True
                                End If
                                If allow = True Then Exit For
                            Next
                            If allow = False And haschecked = True Then render = False
                        End If
                    End If
                Next
                Nodes(i).Render = render
            Next
        Else
            For i As Integer = 1 To Nodes.Count - 1
                Nodes(i).Render = True
            Next
        End If
        Cursor.Current = Cursors.Default
        UpdateTreeView()
        UpdateScreen = True
    End Sub
    Private Sub ViewLogToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewLogToolStripMenuItem.Click
        Try
            If System.IO.File.Exists(Application.StartupPath & "\log.txt") Then Process.Start(Application.StartupPath & "\log.txt")
        Catch
        End Try
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub SaveSnapshotToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveSnapshotToolStripMenuItem.Click
        SaveFileDialog2.FileName = IO.Path.GetFileNameWithoutExtension(CurrentFileName)
        Try
            If SaveFileDialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim DestPath As String = SaveFileDialog2.FileName
                If System.IO.File.Exists(DestPath) Then System.IO.File.Delete(DestPath)
                Dim Image As New Bitmap(800, 600)
                Dim g As Graphics = Graphics.FromImage(Image)
                RenderNodes(g)
                SaveImage(Image, DestPath)
                If Not System.IO.File.Exists(DestPath) Then
                    MsgBox("An unknown error occured while saving the snapshot.", MsgBoxStyle.Critical)
                End If
            End If
        Catch ex As Exception
            MsgBox("An error occured while saving the snapshot:" & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub LoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            LoadFile(OpenFileDialog1.FileName)
        End If
    End Sub
    Private Sub DeselectToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeselectToolStripMenuItem.Click
        LoadNode(-1)
    End Sub

    Private Sub TreeView1_BeforeLabelEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles TreeView1.BeforeLabelEdit
        e.CancelEdit = True
        For i As Integer = 1 To Nodes.Count - 1
            If Nodes(i).Name.ToLower.Trim = e.Node.Text.ToLower.Trim Then e.CancelEdit = False
        Next
    End Sub
    Private Sub TreeView1_AfterLabelEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles TreeView1.AfterLabelEdit
        If Not IsNothing(e.Label) Then
            Try
                Dim newname As String = e.Label.Trim.Replace(" ", "_")
                Dim oldname As String = Nodes(CurrentIndex).Name
                e.CancelEdit = True
                If Val(newname) = 0 And newname.Trim <> "" And newname.Trim <> "0" And newname.ToLower <> oldname.ToLower Then
                    Dim aexist As Boolean = False
                    For i As Integer = 1 To Nodes.Count - 1
                        If Nodes(i).Name.Trim.Replace(" ", "_").ToLower = newname.ToLower Then aexist = True : Exit For
                    Next
                    If aexist = True Then
                        MsgBox("Can't rename " & Chr(34) & oldname & Chr(34) & " to " & Chr(34) & newname & Chr(34) & " because a node already exists with this name.", MsgBoxStyle.Information)
                    Else
                        Nodes(CurrentIndex).Name = newname
                        WriteLog("Name of node " & oldname & " (" & CurrentIndex & ") changed to " & newname)
                        Me.Text = "HUD Editor - " & newname & " (" & Nodes(CurrentIndex).Type & ")"
                        For i As Integer = 1 To Nodes.Count - 1
                            If Nodes(i).Parent = oldname Then Nodes(i).Parent = newname
                        Next
                        If ViewedDialog = 3 Then MainDialog.TextBox1.Text = newname
                        TreeView1.SelectedNode.Text = newname
                    End If
                End If
            Catch
            End Try
        End If
    End Sub

    Dim ispanelresizing As Boolean = False
    Private Sub Panel1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseMove
        Dim cpos As Point = Panel1.PointToClient(Cursor.Position)
        If ispanelresizing = False Then
            If cpos.X > Panel1.Width - 10 And cpos.X < Panel1.Width + 3 Then Cursor.Current = Cursors.SizeWE
        Else
            Cursor.Current = Cursors.SizeWE
            Panel1.Width = SetValueBounds(cpos.X, 200, 400)
        End If
    End Sub

    Private Sub Panel1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseDown
        Dim cpos As Point = Panel1.PointToClient(Cursor.Position)
        If cpos.X > Panel1.Width - 10 And cpos.X < Panel1.Width + 2 Then
            ispanelresizing = True
        End If
    End Sub
    Private Sub Panel1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseUp
        ispanelresizing = False
    End Sub

    Private Sub HideToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideToolStripMenuItem.Click
        If Not IsNothing(TreeView1.SelectedNode) Then TreeView1.SelectedNode.Checked = False
        Nodes(CurrentIndex).Render = False
        LoadNode(-1)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        If MessageBox.Show("This will erase everything in the current scene. Continue with a new HUD?", "New HUD warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
            SetViewedDialog(0)
            ReDim Nodes(0)
            LoadNode(-1)
            PerformGlobalUpdate = True
            UpdateScreen = True
            UpdateTreeView()
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Label2.Text = Math.Round(1000 / RefreshTime, 0) & " fps"
    End Sub
    Private Sub TextureLibraryCreatorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextureLibraryCreatorToolStripMenuItem.Click
        Me.Hide()
        LCForm.ShowDialog()
        LoadLibraries()
        Me.Show()
    End Sub

    Private Sub Form1_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            For Each file As String In e.Data.GetData(DataFormats.FileDrop)
                If file.ToLower.EndsWith(".con") Then e.Effect = DragDropEffects.Move
            Next
        End If
    End Sub

    Private Sub Form1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            For Each file As String In e.Data.GetData(DataFormats.FileDrop)
                If file.ToLower.EndsWith(".con") Then LoadFile(file)
            Next
        End If
    End Sub
End Class
