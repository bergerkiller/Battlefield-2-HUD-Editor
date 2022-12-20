Public Class Form1
    Dim tupdater As System.Threading.Thread
    Dim MouseDownStartPos As New Point(0, 0)
    Dim MouseDownStartValueX, MouseDownStartValueY As Integer

    Private Sub Updater()
        Do
            If UpdateScreen = True Then
                UpdateScreen = False
                Dim screenimage As New Bitmap(800, 600)
                Dim g As Graphics = Graphics.FromImage(screenimage)
                RenderNodes(g)
                '=================================================
                'Render node selection boxes
                If CurrentIndex <> -1 And DrawSelectionSquareToolStripMenuItem.Checked = True Then
                    If Nodes(CurrentIndex).Type = "Picture Node" Then
                        g.DrawImage(RenderSelectionBox(Nodes(CurrentIndex).PictureNodeData.Position, Nodes(CurrentIndex).PictureNodeData.Size, Nodes(CurrentIndex).PictureNodeData.StaticRotation), New Point(0, 0))
                    End If
                    If Nodes(CurrentIndex).Type = "Text Node" Then
                        g.DrawImage(RenderSelectionBox(Nodes(CurrentIndex).TextNodeData.Position, Nodes(CurrentIndex).TextNodeData.Size, 0), New Point(0, 0))
                    End If
                    If Nodes(CurrentIndex).Type = "Compass Node" Then
                        g.DrawImage(RenderSelectionBox(Nodes(CurrentIndex).CompassNodeData.Position, Nodes(CurrentIndex).CompassNodeData.Size, 0), New Point(0, 0))
                    End If
                    'Render special dialogs
                    If ViewedDialog = 6 And Nodes(CurrentIndex).Type = "Picture Node" Then
                        g.DrawPie(Pens.Red, New Rectangle(Nodes(CurrentIndex).PictureNodeData.DRotationMid.X + 395, Nodes(CurrentIndex).PictureNodeData.DRotationMid.Y + 295, 10, 10), 90, 360)
                    End If
                End If
                '==================================================
                PictureBoxAdapter(screenimage, Me)
            Else
                System.Threading.Thread.Sleep(50)
            End If
        Loop
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BGimage = ResizeImage(Image.FromFile("bin\Background\Generic background.jpg"), 800, 600, True)
        PictureBox1.BackgroundImage = BGimage

        tupdater = New System.Threading.Thread(AddressOf Updater)
        tupdater.IsBackground = True
        tupdater.Start()

        'Generating font

        'Adding test nodes

        Dim tnode1 As New Node("parent_name", "PictureNode1", "Picture Node")
        tnode1.PictureNodeData = New PictureNode("Textures\Ingame\Vehicles\Icons\Hud\Air\Attack\Ah1z\Ah1z_compasarrow.dds")
        tnode1.PictureNodeData.Position = New Point(393, 131)
        tnode1.PictureNodeData.Size = New Point(16, 16)
        tnode1.PictureNodeData.Color = Color.FromArgb(255, 0, 204, 0)

        Dim tnode2 As New Node("parent_name", "CompassNode1", "Compass Node")
        tnode2.CompassNodeData = New CompassNode("Textures\Ingame\Vehicles\Icons\Hud\Air\Attack\Ah1z\Ah1z_compas.dds")
        tnode2.CompassNodeData.Position = New Point(340, 100)
        tnode2.CompassNodeData.Size = New Size(128, 32)
        tnode2.CompassNodeData.TextureSize = New Size(256, 32)
        tnode2.CompassNodeData.Color = Color.FromArgb(255, 0, 204, 0)
        tnode2.CompassNodeData.Type = 3
        tnode2.CompassNodeData.Border = 76
        tnode2.CompassNodeData.Offset = 19
        tnode2.CompassNodeData.ValueVariable = "VehicleAngle"
        ReDim Nodes(1)
        Nodes(0) = tnode1
        Nodes(1) = tnode2
        LoadNode(1)
        UpdateScreen = True
    End Sub
    Private Sub Form1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        If Me.WindowState = FormWindowState.Minimized Then
            SetViewedDialog(0)
        End If
    End Sub

    Private Sub SimulateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimulateButton.Click
        SetViewedDialog(0)
        Simulator.ShowDialog()
    End Sub
    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        NodeSelect.ShowDialog()
    End Sub
    Private Sub TSizeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TSizeButton.Click
        SetViewedDialog(8)
    End Sub
    Private Sub TextureButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextureButton.Click
        SetViewedDialog(1)
    End Sub
    Private Sub ColorButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorButton.Click
        SetViewedDialog(2)
    End Sub
    Private Sub SizeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SizeButton.Click
        SetViewedDialog(3)
    End Sub
    Private Sub PositionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PositionButton.Click
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

    Private Sub BackgroundToolStripMenuItem_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackgroundToolStripMenuItem.DropDownOpening
        Dim selitem As String = ToolStripComboBox2.SelectedItem
        ToolStripComboBox2.Items.Clear()
        If System.IO.Directory.Exists("bin\Background") Then
            For Each bfile As String In System.IO.Directory.GetFiles("bin\Background")
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
        If System.IO.Directory.Exists("bin\Overlay") Then
            For Each bfile As String In System.IO.Directory.GetFiles("bin\Overlay")
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
                Dim filepath As String = "bin\Background\"
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
                End If
            Catch
                BGimage = ResizeImage(LoadImage("Bin\Background\Generic background.jpg"), 800, 600, True)
            End Try
            Dim finalpbimage As New Bitmap(800, 600)
            Dim g As Graphics = Graphics.FromImage(finalpbimage)
            g.DrawImage(BGimage, New Point(0, 0))
            If DrawReferenceCrossToolStripMenuItem.Checked = True Then Graphics.FromImage(finalpbimage).DrawImage(ResizeImage(Image.FromFile("bin\referencecross.gif"), 800, 600, True), New Point(0, 0))
            PictureBox1.BackgroundImage = finalpbimage
        End If
    End Sub
    Private Sub DrawReferenceCrossToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DrawReferenceCrossToolStripMenuItem.CheckedChanged
        If DrawReferenceCrossToolStripMenuItem.Checked = True Then
            Dim finalpbimage As New Bitmap(800, 600)
            Dim g As Graphics = Graphics.FromImage(finalpbimage)
            g.DrawImage(BGimage, New Point(0, 0))
            If DrawReferenceCrossToolStripMenuItem.Checked = True Then Graphics.FromImage(finalpbimage).DrawImage(ResizeImage(Image.FromFile("bin\referencecross.gif"), 800, 600, True), New Point(0, 0))
            PictureBox1.BackgroundImage = finalpbimage
        Else
            PictureBox1.BackgroundImage = BGimage
        End If
    End Sub
    Private Sub ToolStripComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripComboBox3.SelectedIndexChanged
        Try
            'getting the file path
            Dim filepath As String = "bin\Overlay\"
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
        For i As Integer = 0 To Nodes.Count - 1
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

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        PictureBox1.Focus()
    End Sub
    Private Sub PictureBox1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        MouseDownStartPos = PictureBox1.PointToClient(Cursor.Position)
        If ViewedDialog = 0 Then
            For Each Frm As Form In My.Application.OpenForms
                If Frm Is SizeDialog Then ViewedDialog = 3
                If Frm Is PositionDialog Then ViewedDialog = 4
                If Frm Is RotationDialog Then ViewedDialog = 5
            Next
        End If
        If ViewedDialog = 3 Or ViewedDialog = 4 Or ViewedDialog = 5 Then Cursor.Current = Cursors.NoMove2D
        If ViewedDialog = 3 Then
            'Size
            If e.Button = Windows.Forms.MouseButtons.Right Then SizeDialog.Button2.PerformClick()
            MouseDownStartValueX = SizeDialog.NumericUpDown1.Value
            MouseDownStartValueY = SizeDialog.NumericUpDown2.Value
        End If
        If ViewedDialog = 4 Then
            'Position
            If e.Button = Windows.Forms.MouseButtons.Right Then PositionDialog.Button4.PerformClick()
            MouseDownStartValueX = PositionDialog.NumericUpDown3.Value
            MouseDownStartValueY = PositionDialog.NumericUpDown4.Value
        End If
        If ViewedDialog = 5 Then
            'Rotation
            If e.Button = Windows.Forms.MouseButtons.Right Then RotationDialog.NumericUpDown1.Value = 0
        End If
        If ViewedDialog = 6 Then
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                Cursor.Current = Cursors.NoMove2D
                If e.Button = Windows.Forms.MouseButtons.Right Then pnvariables.Button1.PerformClick()
                MouseDownStartValueX = pnvariables.NumericUpDown1.Value
                MouseDownStartValueY = pnvariables.NumericUpDown2.Value
            End If
        End If
    End Sub
    Private Sub PictureBox1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim MousePos As Point = PictureBox1.PointToClient(Cursor.Position)
            If ViewedDialog = 3 Then
                'Size
                SizeDialog.NumericUpDown1.Value = SetValueBounds(MouseDownStartValueX + MousePos.X - MouseDownStartPos.X, 1, 2048)
                SizeDialog.NumericUpDown2.Value = SetValueBounds(MouseDownStartValueY + MousePos.Y - MouseDownStartPos.Y, 1, 2048)
            End If
            If ViewedDialog = 4 Then
                'Position
                PositionDialog.NumericUpDown3.Value = SetValueBounds(MouseDownStartValueX + MousePos.X - MouseDownStartPos.X, -2048, 800)
                PositionDialog.NumericUpDown4.Value = SetValueBounds(MouseDownStartValueY + MousePos.Y - MouseDownStartPos.Y, -2048, 600)
            End If
            If ViewedDialog = 5 Then
                'Rotation
                If Nodes(CurrentIndex).Type = "Picture Node" Then
                    Dim offsetX As Integer = MousePos.X - Nodes(CurrentIndex).PictureNodeData.Position.X - (Nodes(CurrentIndex).PictureNodeData.Size.Width * 0.5)
                    Dim offsetY As Integer = MousePos.Y - Nodes(CurrentIndex).PictureNodeData.Position.Y - (Nodes(CurrentIndex).PictureNodeData.Size.Height * 0.5)
                    Dim rotangle As Integer = 0
                    If offsetX = 0 And offsetY > 0 Then rotangle = 180
                    If offsetX > 0 Then
                        If offsetY = 0 Then rotangle = 90
                        If offsetY < 0 Then rotangle = 90 - Math.Atan((offsetY * -1000 \ offsetX) * 0.001) * 57.29577951
                        If offsetY > 0 Then rotangle = 90 + Math.Atan((offsetY * 1000 \ offsetX) * 0.001) * 57.29577951
                    End If
                    If offsetX < 0 Then
                        If offsetY = 0 Then rotangle = 270
                        If offsetY < 0 Then rotangle = 270 - Math.Atan((offsetY * -1000 \ offsetX) * 0.001) * 57.29577951
                        If offsetY > 0 Then rotangle = 270 + Math.Atan((offsetY * 1000 \ offsetX) * 0.001) * 57.29577951
                    End If
                    Do While rotangle > 360
                        rotangle -= 360
                    Loop
                    RotationDialog.NumericUpDown1.Value = SetValueBounds(rotangle, 0, 360)
                End If
            End If
            If ViewedDialog = 6 Then
                'Variables
                If Nodes(CurrentIndex).Type = "Picture Node" Then
                    pnvariables.NumericUpDown1.Value = SetValueBounds(MouseDownStartValueX + MousePos.X - MouseDownStartPos.X, -400, 400)
                    pnvariables.NumericUpDown2.Value = SetValueBounds(MouseDownStartValueY + MousePos.Y - MouseDownStartPos.Y, -300, 300)
                End If
            End If
        End If
    End Sub
    Private Sub PictureBox1_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles PictureBox1.PreviewKeyDown
        If e.KeyCode = Keys.Space Then
            If DrawSelectionSquareToolStripMenuItem.Checked = True Then
                DrawSelectionSquareToolStripMenuItem.Checked = False
            Else
                DrawSelectionSquareToolStripMenuItem.Checked = True
            End If
        End If
    End Sub
    Private Sub TrackBarYpos_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarYpos.Scroll
        PositionDialog.NumericUpDown4.Value = TrackBarYpos.Value * -1
    End Sub
    Private Sub TrackBarXpos_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarXPos.Scroll
        PositionDialog.NumericUpDown3.Value = TrackBarXPos.Value
    End Sub
    Private Sub TrackBarYpos_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles TrackBarYpos.PreviewKeyDown
        If e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Then TrackBarXPos.Focus()
    End Sub
    Private Sub TrackBarXPos_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles TrackBarXPos.PreviewKeyDown
        If e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Then TrackBarYpos.Focus()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Dim NodeData As String = ""
                For Each Node As Node In Nodes
                    NodeData &= SaveNodeData(Node) & vbCrLf & vbCrLf
                Next
                Dim writer As New System.IO.StreamWriter(SaveFileDialog1.FileName, False)
                writer.WriteLine(NodeData)
                writer.Close()
            Catch ex As Exception
                MsgBox("Failed to save: " & vbCrLf & ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Private Sub LoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        If Dialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim n As Node = LoadNodeData(Dialog1.TextBox1.Text)
            ReDim Preserve Nodes(2)
            Nodes(2) = n
            UpdateScreen = True
        End If
    End Sub
End Class
