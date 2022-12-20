Public Class Form1
    Dim tupdater As System.Threading.Thread

    Private Sub Updater()
        Do
            If UpdateScreen = True Then
                UpdateScreen = False
                UpdateSelectionField = True
                UpdateDotField = True
                Dim screenimage As New Bitmap(800, 600)
                Dim g As Graphics = Graphics.FromImage(screenimage)
                RenderNodes(g)
                '=================================================
                'Render node selection boxes
                Try
                    If CurrentIndex <> -1 And DrawSelectionSquareToolStripMenuItem.Checked = True Then
                        If Nodes(CurrentIndex).Render = True Then
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
                    End If
                Catch
                End Try
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




        'Dim snode As New Node("VehicleHuds", "Ah1zPilotHud", "Split Node")
        'ReDim snode.SplitNodeData.Guiindices(0)
        'snode.SplitNodeData.Guiindices(0) = 24

        'Generating font

        'Adding test nodes

        'Dim tnode1 As New Node("parent_name", "PictureNode1", "Picture Node")
        'tnode1.PictureNodeData = New PictureNode("Textures\Ingame\Vehicles\Icons\Hud\Air\Attack\Ah1z\Ah1z_compasarrow.dds")
        'tnode1.PictureNodeData.Position = New Point(393, 131)
        'tnode1.PictureNodeData.Size = New Point(16, 16)
        'tnode1.PictureNodeData.Color = Color.FromArgb(255, 0, 204, 0)

        'Dim tnode2 As New Node("parent_name", "CompassNode1", "Compass Node")
        'tnode2.CompassNodeData = New CompassNode("Textures\Ingame\Vehicles\Icons\Hud\Air\Attack\Ah1z\Ah1z_compas.dds")
        'tnode2.CompassNodeData.Position = New Point(340, 100)
        'tnode2.CompassNodeData.Size = New Size(128, 32)
        'tnode2.CompassNodeData.TextureSize = New Size(256, 32)
        'tnode2.CompassNodeData.Color = Color.FromArgb(255, 0, 204, 0)
        'tnode2.CompassNodeData.Type = 3
        'tnode2.CompassNodeData.Border = 76
        'tnode2.CompassNodeData.Offset = 19
        'tnode2.CompassNodeData.ValueVariable = "VehicleAngle"
        'ReDim Nodes(1)
        'Nodes(0) = tnode1
        'Nodes(1) = tnode2
        'LoadNode(1)
        PictureBox1.Location = New Point(15, 26)
        PictureBox1.Size = New Size(Me.Size.Width - 32, Me.Size.Height - 65)
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
        RefreshNodes()
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

    Private Sub UpdateTreeView()
        If Panel1.Visible = True Then
            Dim TNodeNames As New ListBox
            Dim NodeNames As New ListBox
            Dim DeleteNames As New ListBox
            For Each Tnode As TreeNode In GetTreeViewNodes(TreeView1)
                TNodeNames.Items.Add(Tnode.Name.ToLower.Trim)
            Next
            For i As Integer = 1 To Nodes.Count - 1
                If Not TNodeNames.Items.Contains(Nodes(i).Name.ToLower.Trim) Then
                    TreeViewAdapter(GetNodeTree(Nodes(i)))
                End If
                NodeNames.Items.Add(Nodes(i).Name.ToLower.Trim)
            Next
            If Not TNodeNames.Items.Contains("vehiclehuds") Then TreeViewAdapter("VehicleHuds")
            If Not TNodeNames.Items.Contains("ingamehud") Then TreeViewAdapter("IngameHud")
            For Each TNName As String In TNodeNames.Items
                If TNName <> "vehiclehuds" And TNName <> "ingamehud" Then
                    If Not NodeNames.Items.Contains(TNName) Then
                        GetTreeViewNode(TreeView1, TNName).Remove()
                        'TreeView1.Nodes.RemoveByKey(TNName)
                    End If
                End If
            Next
        End If
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
        If AddNode.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim SelNodeName As String = "VehicleHuds"
            Try
                SelNodeName = TreeView1.SelectedNode.Text
            Catch
            End Try
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
            InsertNode(New Node(ParentName, AddNode.TextBox1.Text, AddNode.ComboBox1.SelectedItem), Nodes.Count)
            LoadNode(Nodes.Count - 1)
            UpdateTreeView()
        End If
    End Sub
    Private Sub DeleteNode()
        If CurrentIndex > 0 Then
            If MessageBox.Show("Are you sure you want to delete node: " & Nodes(CurrentIndex).Name & "?", "Delete Node", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                RemoveNode(CurrentIndex)
                LoadNode(-1)
                UpdateScreen = True
                UpdateTreeView()
            End If
        End If
    End Sub

    Public IsResizing As Boolean = False
    Public ResizeMode As Integer = -1
    Public ResizeStartPosition As New Point(0, 0)
    Public ResizeStartPosVal As New Point(0, 0)
    Public ResizeStartSizeVal As New Size(32, 32)
    Private Sub SetCursor(ByVal MoveType As Integer)
        If MoveType = 0 Then Cursor.Current = Cursors.SizeNWSE
        If MoveType = 1 Then Cursor.Current = Cursors.SizeNESW
        If MoveType = 2 Then Cursor.Current = Cursors.SizeNWSE
        If MoveType = 3 Then Cursor.Current = Cursors.SizeNESW
        If MoveType = 4 Then Cursor.Current = Cursors.SizeNS
        If MoveType = 5 Then Cursor.Current = Cursors.SizeNS
        If MoveType = 6 Then Cursor.Current = Cursors.SizeWE
        If MoveType = 7 Then Cursor.Current = Cursors.SizeWE
    End Sub
    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        PictureBox1.Focus()
    End Sub
    Private Sub PictureBox1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        PBCursorPosition = PictureBox1.PointToClient(Cursor.Position)
        PBCursorPosition.X *= ScaleX
        PBCursorPosition.Y *= ScaleY
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If DotMoveType = -1 Then
                Cursor.Current = Cursors.WaitCursor
                Dim clickedindex As Integer = GetNodeIndexAtPoint(PBCursorPosition)
                Cursor.Current = Cursors.Default
                If CurrentIndex <> clickedindex Or CurrentIndex = -1 Then
                    IsResizing = False
                    'Load not-loaded node
                    LoadNode(clickedindex)
                    If clickedindex <> -1 Then
                        TreeView1.SelectedNode = GetTreeViewNode(TreeView1, Nodes(clickedindex).Name)
                    Else
                        TreeView1.SelectedNode = Nothing
                    End If
                    Cursor.Current = Cursors.Default
                Else
                    'Free move
                    Cursor.Current = Cursors.NoMove2D
                    IsResizing = True
                    ResizeMode = 1
                End If
            Else
                'Resizing
                SetCursor(DotMoveType)
                IsResizing = True
                ResizeMode = DotMoveType + 2
            End If
            If IsResizing = True And CurrentIndex <> -1 Then
                'Load Start Values
                ResizeStartPosition = Cursor.Position
                If Nodes(CurrentIndex).Type = "Picture Node" Then
                    ResizeStartPosVal = Nodes(CurrentIndex).PictureNodeData.Position
                    ResizeStartSizeVal = Nodes(CurrentIndex).PictureNodeData.Size
                ElseIf Nodes(CurrentIndex).Type = "Compass Node" Then
                    ResizeStartPosVal = Nodes(CurrentIndex).CompassNodeData.Position
                    ResizeStartSizeVal = Nodes(CurrentIndex).CompassNodeData.Size
                ElseIf Nodes(CurrentIndex).Type = "Text Node" Then
                    ResizeStartPosVal = Nodes(CurrentIndex).TextNodeData.Position
                    ResizeStartSizeVal = Nodes(CurrentIndex).TextNodeData.Size
                End If
            End If
        End If
    End Sub
    Private Sub PictureBox1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp
        ResizeMode = 0
        IsResizing = False
    End Sub
    Private Sub PictureBox1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        PBCursorPosition = PictureBox1.PointToClient(Cursor.Position)
        PBCursorPosition.X *= ScaleX
        PBCursorPosition.Y *= ScaleY
        If IsResizing = False Then
            SetCursor(DotMoveType)
            If CurrentIndex <> -1 Then
                DotMoveType = GetDotIndexAtPoint(PBCursorPosition)
            Else
                Cursor.Current = Cursors.Default
            End If
        Else
            'Actual value changing
            Dim offsetX As Integer = (Cursor.Position.X - ResizeStartPosition.X) * ScaleX
            Dim offsetY As Integer = (Cursor.Position.Y - ResizeStartPosition.Y) * ScaleY
            'Set new variables
            Dim NewPosX As Integer = ResizeStartPosVal.X
            Dim NewPosY As Integer = ResizeStartPosVal.Y
            Dim NewSizeW As Integer = ResizeStartSizeVal.Width
            Dim NewSizeH As Integer = ResizeStartSizeVal.Height
            If ResizeMode = 1 Then
                NewPosX += offsetX
                NewPosY += offsetY
            ElseIf ResizeMode = 2 Then
                NewPosX += offsetX
                NewPosY += offsetY
                NewSizeW -= offsetX
                NewSizeH -= offsetY
            ElseIf ResizeMode = 3 Then
                NewPosY += offsetY
                NewSizeW += offsetX
                NewSizeH -= offsetY
            ElseIf ResizeMode = 4 Then
                NewSizeW += offsetX
                NewSizeH += offsetY
            ElseIf ResizeMode = 5 Then
                NewPosX += offsetX
                NewSizeW -= offsetX
                NewSizeH += offsetY
            ElseIf ResizeMode = 6 Then
                NewPosY += offsetY
                NewSizeH -= offsetY
            ElseIf ResizeMode = 7 Then
                NewSizeH += offsetY
            ElseIf ResizeMode = 8 Then
                NewPosX += offsetX
                NewSizeW -= offsetX
            ElseIf ResizeMode = 9 Then
                NewSizeW += offsetX
            End If
            'Set max and min values
            Dim NewPos As New Point(NewPosX, NewPosY)
            Dim NewSize As New Size(SetValueBounds(NewSizeW, 1, 2048), SetValueBounds(NewSizeH, 1, 2048))
            If ViewedDialog = 3 Then
                'Set the main dialog
                MainDialog.NumericUpDown1.Value = NewPos.X
                MainDialog.NumericUpDown2.Value = NewPos.Y
                MainDialog.NumericUpDown3.Value = NewSize.Width
                MainDialog.NumericUpDown4.Value = NewSize.Height
            Else
                'Set the current node
                If Nodes(CurrentIndex).Type = "Picture Node" Then
                    Nodes(CurrentIndex).PictureNodeData.Position = NewPos
                    Nodes(CurrentIndex).PictureNodeData.Size = NewSize
                    Nodes(CurrentIndex).PictureNodeData.PosRotChanged = True
                    If ResizeMode <> 1 Then Nodes(CurrentIndex).PictureNodeData.SizeChanged = True
                ElseIf Nodes(CurrentIndex).Type = "Compass Node" Then
                    Nodes(CurrentIndex).CompassNodeData.Position = NewPos
                    Nodes(CurrentIndex).CompassNodeData.Size = NewSize
                    Nodes(CurrentIndex).CompassNodeData.ValueChanged = True
                ElseIf Nodes(CurrentIndex).Type = "Text Node" Then
                    Nodes(CurrentIndex).TextNodeData.Position = NewPos
                    Nodes(CurrentIndex).TextNodeData.Size = NewSize
                    Nodes(CurrentIndex).TextNodeData.Modified = True
                End If
                UpdateScreen = True
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
        BringToFrontToolStripMenuItem.Visible = CurrentIndex <> -1
        SendToBackToolStripMenuItem.Visible = CurrentIndex <> -1
        CopyToolStripMenuItem.Enabled = CurrentIndex <> -1
        CutToolStripMenuItem.Enabled = CurrentIndex <> -1
        PasteToolStripMenuItem.Enabled = Clipboard.GetDataObject().GetDataPresent(DataFormats.Text)
        DeleteToolStripMenuItem.Enabled = CurrentIndex <> -1
        If IsResizing = True Then
            'Set defaults back
            If ViewedDialog = 3 Then
                'Set the main dialog
                MainDialog.NumericUpDown1.Value = ResizeStartPosVal.X
                MainDialog.NumericUpDown2.Value = ResizeStartPosVal.Y
                MainDialog.NumericUpDown3.Value = ResizeStartSizeVal.Width
                MainDialog.NumericUpDown4.Value = ResizeStartSizeVal.Height
            Else
                'Set the current node
                If Nodes(CurrentIndex).Type = "Picture Node" Then
                    Nodes(CurrentIndex).PictureNodeData.Position = ResizeStartPosVal
                    Nodes(CurrentIndex).PictureNodeData.Size = ResizeStartSizeVal
                    Nodes(CurrentIndex).PictureNodeData.PosRotChanged = True
                    If ResizeMode <> 1 Then Nodes(CurrentIndex).PictureNodeData.SizeChanged = True
                ElseIf Nodes(CurrentIndex).Type = "Compass Node" Then
                    Nodes(CurrentIndex).CompassNodeData.Position = ResizeStartPosVal
                    Nodes(CurrentIndex).CompassNodeData.Size = ResizeStartSizeVal
                    Nodes(CurrentIndex).CompassNodeData.ValueChanged = True
                ElseIf Nodes(CurrentIndex).Type = "Text Node" Then
                    Nodes(CurrentIndex).TextNodeData.Position = ResizeStartPosVal
                    Nodes(CurrentIndex).TextNodeData.Size = ResizeStartSizeVal
                    Nodes(CurrentIndex).TextNodeData.Modified = True
                End If
                UpdateScreen = True
            End If
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
        If CurrentIndex <> -1 Then Clipboard.SetData(DataFormats.Text, SaveNodeData(Nodes(CurrentIndex)))
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
            Clipboard.SetData(DataFormats.Text, SaveNodeData(Nodes(CurrentIndex)))
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
    Private Sub LoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim reader As New System.IO.StreamReader(OpenFileDialog1.FileName)
            Dim NewNodes() As Node = LoadNodeData(reader.ReadToEnd)
            reader.Close()
            Dim result As MsgBoxResult = MsgBoxResult.No
            If Nodes.Count > 1 Then
                result = MessageBox.Show("The current scene is not empty. Replace the current scene with the loaded file?", "Load", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)
                If result = MsgBoxResult.Yes Then
                    Do While Nodes.Count > 0
                        RemoveNode(0)
                    Loop
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
                        Dim cindex As Integer = 1
                        Do While checknames.Items.Contains(NewNode.Name.ToLower.Trim)
                            NewNode.Name = oldname & "_copy" & cindex
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
        End If
        RefreshNodes()
    End Sub

    Private Sub Panel1_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel1.VisibleChanged
        If Panel1.Visible = True Then
            UpdateTreeView()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Panel1.Visible = True Then Panel1.Visible = False Else Panel1.Visible = True
    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        Try
            Dim currindex As Integer = -1
            For i As Integer = 1 To Nodes.Count - 1
                If Nodes(i).Name = TreeView1.SelectedNode.Text Then
                    currindex = i
                    Exit For
                End If
            Next
            LoadNode(currindex)
            If TreeView1.SelectedNode.Text <> "VehicleHuds" And TreeView1.SelectedNode.Text <> "IngameHud" Then
                DeleteButton.Enabled = True
            Else
                DeleteButton.Enabled = False
            End If
        Catch
            DeleteButton.Enabled = False
        End Try
    End Sub

    Private Sub ComboBox1_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.DropDown
        Dim OldIndex As String = ComboBox1.SelectedItem
        ComboBox1.Items.Clear()
        For i As Integer = 1 To Nodes.Count - 1
            If Nodes(i).Type = "Split Node" Then
                For Each Var As String In Nodes(i).ShowVariables.Items
                    Var = Var.ToLower.Trim
                    If Var.StartsWith("equal guiindex ") Then
                        Dim Index As String = Var.Remove(0, 15).Trim
                        If Not ComboBox1.Items.Contains(Index) Then ComboBox1.Items.Add(Index)
                    End If
                Next
            End If
        Next
        ComboBox1.SelectedItem = OldIndex
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        For i As Integer = 1 To Nodes.Count - 1
            Dim Render As Boolean = False
            Dim NodePath As String = GetNodeTree(Nodes(i))
            For Each Part As String In NodePath.Split("\")
                Dim Index As Integer = GetNodeNameIndex(Part)
                If Nodes(Index).Type = "Split Node" Then
                    For Each var As String In Nodes(i).ShowVariables.Items
                        var = var.ToLower.Trim

                    Next


                End If
            Next
            Nodes(i).Render = Render
        Next
    End Sub
End Class
