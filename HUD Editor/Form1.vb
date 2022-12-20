Public Class Form1
    Dim tupdater As System.Threading.Thread


#Region "Main, settings and file"
    Dim oldbounds As Rectangle
    Dim oldstate As FormWindowState
    Private Sub ReOpenToolstrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UseFixedResolutionToolStripMenuItem.Click, FullScreenToolStripMenuItem.Click, DrawSelectionSquareToolStripMenuItem.Click, DrawReferenceCrossToolStripMenuItem.Click
        ToolStripDropDownButton1.ShowDropDown()
    End Sub
    Private Sub TextureLibraryCreatorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextureLibraryCreatorToolStripMenuItem.Click
        Me.Hide()
        LCForm.ShowDialog()
        LoadLibraries()
        Me.Show()
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
                If IO.Path.GetFileNameWithoutExtension(bfile).ToLower = "Black" Then add = False
                If add = True Then ToolStripComboBox2.Items.Add(IO.Path.GetFileNameWithoutExtension(bfile))
            Next
        End If
        ToolStripComboBox2.Items.Add("Black")
        ToolStripComboBox2.SelectedItem = selitem
    End Sub
    Private Sub OverlayToolStripMenuItem_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OverlayToolStripMenuItem.DropDownOpened
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
        ToolStripComboBox3.SelectedItem = selitem
    End Sub
    Private Sub ToolStripComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripComboBox2.SelectedIndexChanged
        If ToolStripComboBox2.SelectedIndex <> -1 Then
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
            If IO.Path.GetFileNameWithoutExtension(finalpath).ToLower = "black" Then finalpath = ""
            If System.IO.File.Exists(finalpath) Then
                BGname = IO.Path.GetFileName(finalpath)
                BackgroundToolStripMenuItem.Checked = True
                MainScreen.BackgroundImage = LoadImage(finalpath)
            Else
                BGname = "black"
                BackgroundToolStripMenuItem.Checked = False
                MainScreen.BackgroundImage = Nothing
            End If
            MainScreen.Invalidate()
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
            If IO.Path.GetFileNameWithoutExtension(finalpath).ToLower = "no overlay" Then finalpath = ""
            If System.IO.File.Exists(finalpath) Then
                OVname = IO.Path.GetFileName(finalpath)
                OverlayToolStripMenuItem.Checked = True
                MainScreen.OverlayImage = LoadImage(finalpath)
            Else
                OVname = "no overlay"
                OverlayToolStripMenuItem.Checked = False
                MainScreen.OverlayImage = Nothing
            End If
            MainScreen.Invalidate()
        Catch
        End Try
    End Sub
    Private Sub DrawReferenceCrossToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DrawReferenceCrossToolStripMenuItem.CheckedChanged
        MainScreen.ShowReferenceCross = DrawReferenceCrossToolStripMenuItem.Checked
    End Sub
    Private Sub DrawSelectionSquareToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DrawSelectionSquareToolStripMenuItem.CheckedChanged
        MainScreen.ShowSelectionSquare = DrawSelectionSquareToolStripMenuItem.Checked
    End Sub
    Private Sub FullScreenToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FullScreenToolStripMenuItem.CheckedChanged
        Cursor.Current = Cursors.WaitCursor
        If FullScreenToolStripMenuItem.Checked = True Then
            oldbounds = New Rectangle(MyBase.Location, MyBase.Size)
            oldstate = MyBase.WindowState
            Me.WindowState = FormWindowState.Normal
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            Me.Location = New Point(0, 0)
            Me.Size = My.Computer.Screen.Bounds.Size
            Me.BringToFront()
            MainScreen.Location = New Point(0, 0)
            MainScreen.Size = MyBase.Size
        Else
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
            Me.Location = oldbounds.Location
            Me.Size = oldbounds.Size
            Me.WindowState = oldstate
            ToolStrip1.Visible = True
            Panel1.Visible = True
            MainScreen.Location = New Point(0, 26)
            MainScreen.Size = New Size(Me.Size.Width - 17, Me.Size.Height - 65)
        End If
        Me.BringToFront()
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub UseFixedResolutionToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UseFixedResolutionToolStripMenuItem.CheckedChanged
        MainScreen.UseFixedResolution = UseFixedResolutionToolStripMenuItem.Checked
    End Sub
    Private Sub ViewLogToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewLogToolStripMenuItem.Click
        Try
            If System.IO.File.Exists(Application.StartupPath & "\log.txt") Then Process.Start(Application.StartupPath & "\log.txt")
            If Me.FullScreenToolStripMenuItem.Checked = True Then MyBase.WindowState = FormWindowState.Minimized
        Catch
        End Try
    End Sub
    Private Sub ResetScreenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetScreenToolStripMenuItem.Click
        FullScreenToolStripMenuItem.Checked = False
        Me.Size = New Size(817, 665)
        Me.WindowState = FormWindowState.Normal
    End Sub
    Public Sub SaveSettings()
        Try
            Dim setfile As String = Application.StartupPath & "\HEsettings.ini"
            If System.IO.File.Exists(setfile) Then System.IO.File.Delete(setfile)
            Dim w As New System.IO.StreamWriter(setfile, True)
            w.WriteLine("tpath=" & TexturesPath)
            w.WriteLine("Background=" & BGname)
            w.WriteLine("Overlay=" & OVname)
            w.Close()
        Catch ex As Exception
            WriteLog("Failed to save settings: " & ex.Message)
        End Try
    End Sub
    Public Sub LoadSettings()
        Try
            Dim setfile As String = Application.StartupPath & "\HEsettings.ini"
            If System.IO.File.Exists(setfile) Then
                Dim r As New System.IO.StreamReader(setfile)
                Do While r.Peek <> -1
                    Dim textline As String = r.ReadLine.Trim
                    If textline.ToLower.StartsWith("tpath=") Then TexturesPath = textline.Remove(0, 6).Trim("\").Trim
                    If textline.ToLower.StartsWith("background=") Then
                        BGname = textline.Remove(0, 11).Trim
                    ElseIf textline.ToLower.StartsWith("overlay=") Then
                        OVname = textline.Remove(0, 8).Trim
                    End If
                Loop
                r.Close()
            Else
                If System.IO.Directory.Exists(Application.StartupPath & "\Textures\Battlefield 2142") Then TexturesPath = "Textures\Battlefield 2142"
                If System.IO.Directory.Exists(Application.StartupPath & "\Textures\Battlefield 2") Then TexturesPath = "Textures\Battlefield 2"
            End If
        Catch ex As Exception
            WriteLog("Failed to load settings: " & ex.Message)
        End Try
    End Sub
    Private Sub ToolStrip1_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStrip1.VisibleChanged
        If ToolStrip1.Visible = True Then ToolStrip1.BringToFront() Else ToolStrip1.SendToBack()
        If ToolStrip1.Visible = False Then
            Panel1.Location = New Point(0, 0)
            Panel1.Size = New Size(Panel1.Size.Width, MyBase.Size.Height)
        Else
            Panel1.Location = New Point(0, 26)
            Panel1.Size = New Size(Panel1.Size.Width, MyBase.Size.Height - 26)
        End If
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub ToolStripDropDownButton1_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripDropDownButton1.DropDownOpening
        TextureFilterToolStripMenuItem.DropDownItems.Clear()
        If System.IO.Directory.Exists(Application.StartupPath & "\textures") Then
            With TextureFilterToolStripMenuItem
                For Each subfolder As String In System.IO.Directory.GetDirectories(Application.StartupPath & "\textures")
                    Dim item As New ToolStripMenuItem(IO.Path.GetFileName(subfolder))
                    item.Checked = TexturesPath.ToLower = "textures\" & item.Text.ToLower
                    .DropDownItems.Add(item)
                Next
                Dim nitem As New ToolStripMenuItem("none")
                nitem.Checked = TexturesPath.ToLower = "textures"
                .DropDownItems.Add(nitem)
            End With
        End If
        TextureFilterToolStripMenuItem.Visible = TextureFilterToolStripMenuItem.DropDownItems.Count <> 0
    End Sub
    Private Sub TextureFilterToolStripMenuItem_DropDownItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles TextureFilterToolStripMenuItem.DropDownItemClicked
        TexturesPath = "Textures\" & e.ClickedItem.Text
        If e.ClickedItem.Text = "none" Then TexturesPath = "Textures"
        LoadLibraries()
        MainScreen.Root.RefreshAllTextures()
        SaveSettings()
    End Sub
    Private Sub ForceUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ForceUpdateToolStripMenuItem.Click
        For Each n As Node In MainScreen.Root.All
            n.Changed = True
            n.UpdateOnScreen(True)
        Next
    End Sub
    Private Sub MainScreen_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainScreen.MouseMove
        If e.Button = Windows.Forms.MouseButtons.None Then
            If FullScreenToolStripMenuItem.Checked = True Then ToolStrip1.Visible = MainScreen.CursorPosition(False).Y < 26
            Panel1.Visible = MainScreen.CursorPosition(False).X < 16 Or Panel1.Width > 20
        End If
    End Sub
    Private Sub MainScreen_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MainScreen.KeyPress
        If e.KeyChar = " " Then
            If DrawSelectionSquareToolStripMenuItem.Checked = True Then
                DrawSelectionSquareToolStripMenuItem.Checked = False
            Else
                DrawSelectionSquareToolStripMenuItem.Checked = True
            End If
        End If
    End Sub
    Private Sub LoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Cursor.Current = Cursors.WaitCursor
            LoadFile(OpenFileDialog1.FileName)
            Cursor.Current = Cursors.Default
        End If
    End Sub
    Private Sub LoadFile(ByVal FileName As String)
        If System.IO.File.Exists(FileName) Then
            WriteLog("Loading file: " & FileName)
            Dim reader As New System.IO.StreamReader(FileName)
            Dim NewNodes() As Node = LoadNodeData(reader.ReadToEnd)
            reader.Close()
            WriteLog(NewNodes.Count & " nodes found")
            Dim result As MsgBoxResult = MsgBoxResult.No
            If NewNodes.Count = 0 Then
                MsgBox("The file: " & Chr(34) & IO.Path.GetFileName(FileName) & Chr(34) & " does not contain node data.", MsgBoxStyle.Information)
            Else
                Dim isempty As Boolean = True
                For Each n As Node In MainScreen.Root.All
                    If n.CanBeSaved = True Then isempty = False : Exit For
                Next
                If isempty = False Then
                    result = MessageBox.Show("The current scene is not empty. Replace the current scene with the loaded file?", "Load", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)
                    If result = MsgBoxResult.Yes Then
                        ResetNodes()
                    End If
                End If
                If result <> MsgBoxResult.Cancel Then
                    If AddNodes(NewNodes, MainScreen.Root) = False Then MsgBox("Failed to load data.", MsgBoxStyle.Critical)
                End If
            End If
            SaveFileDialog1.FileName = IO.Path.GetFileNameWithoutExtension(FileName)
        End If
    End Sub
    Private Sub SaveSnapshotToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveSnapshotToolStripMenuItem.Click
        If SaveFileDialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Dim img As New Bitmap(800, 600)
                Dim g As Graphics = Graphics.FromImage(img)
                MainScreen.Root.Draw(g, Color.FromArgb(255, 255, 255, 255))
                g.Dispose()
                FreeImageAPI.FreeImage.SaveBitmap(img, SaveFileDialog2.FileName)
            Catch
                MsgBox("Failed to create a snapshot.", MsgBoxStyle.Critical)
            End Try
        End If
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Cursor.Current = Cursors.WaitCursor
        ResetNodes()
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                If System.IO.File.Exists(SaveFileDialog1.FileName) Then System.IO.File.Delete(SaveFileDialog1.FileName)
                Dim w As New System.IO.StreamWriter(SaveFileDialog1.FileName, True)
                For Each n As Node In MainScreen.Nodes
                    If n.CanBeSaved = True Then w.WriteLine(n.SaveData & vbCrLf)
                Next
                w.Close()
            Catch
            End Try
        End If
    End Sub
#End Region

#Region "Context menu"
    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        If IsInSimulationMode = False Then
            Dim selcount As Integer = MainScreen.SelectedNodes.Count
            DeselectToolStripMenuItem.Visible = selcount >= 1
            DeleteToolStripMenuItem.Visible = selcount >= 1
            CutToolStripMenuItem.Visible = selcount >= 1
            CopyToolStripMenuItem.Visible = selcount >= 1
            PasteToolStripMenuItem.Visible = Clipboard.ContainsData(DataFormats.StringFormat)
            HideToolStripMenuItem.Visible = selcount >= 1
            SendToBackToolStripMenuItem.Visible = selcount >= 1
            BringToFrontToolStripMenuItem.Visible = selcount >= 1
            Dim nodehidden As Boolean = False
            SelectNodeToolStripMenuItem.DropDownItems.Clear()
            For Each n As Node In MainScreen.Nodes
                If n.Render = False Then nodehidden = True
                If n.Render = True And n.CanBeSaved = True And n.Selected = False Then
                    SelectNodeToolStripMenuItem.DropDownItems.Add(n.Name)
                End If
            Next
            ShowAllNodesToolStripMenuItem.Visible = nodehidden
            SelectNodeToolStripMenuItem.Visible = SelectNodeToolStripMenuItem.DropDownItems.Count <> 0
        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub SelectNodeToolStripMenuItem_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectNodeToolStripMenuItem.DropDownOpening
        For Each subnode As ToolStripItem In SelectNodeToolStripMenuItem.DropDownItems
            AddHandler subnode.MouseMove, AddressOf HightlightHoveredItem
        Next
    End Sub
    Private Sub SelectNodeToolStripMenuItem_DropDownClosed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectNodeToolStripMenuItem.DropDownClosed
        For Each subnode As ToolStripItem In SelectNodeToolStripMenuItem.DropDownItems
            RemoveHandler subnode.MouseMove, AddressOf HightlightHoveredItem
        Next
        MainScreen.Root.Highlighted = False
    End Sub
    Private Sub HightlightHoveredItem(ByVal sender As System.Object, ByVal e As MouseEventArgs)
        MainScreen.HightlightItem(CType(sender, ToolStripItem).Text)
    End Sub
    Private Sub DeselectToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeselectToolStripMenuItem.Click
        MainScreen.DeselectAll()
    End Sub
    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        For Each n As Node In MainScreen.Root.All(Node.SelectType.Deselected)
            If n.CanBeSaved = True Then n.Selected = True
        Next
        MainScreen.InvokeSelectionChanged()
    End Sub
    Private Sub BringToFrontToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BringToFrontToolStripMenuItem.Click
        For Each Node As Node In MainScreen.SelectedNodes
            Node.BringToFront()
        Next
    End Sub
    Private Sub SendToBackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendToBackToolStripMenuItem.Click
        For Each Node As Node In MainScreen.SelectedNodes
            Node.SendToBack()
        Next
    End Sub
    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        If MainScreen.SelectedNodes.Count <> 0 Then
            Cursor.Current = Cursors.WaitCursor
            Dim data As String = ""
            For Each n As Node In MainScreen.SelectedNodes
                data &= vbCrLf & vbCrLf & n.SaveData
            Next
            data = data.Trim(vbCrLf).Trim
            Clipboard.SetData(DataFormats.Text, data)
        End If
    End Sub
    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        Cursor.Current = Cursors.WaitCursor
        If Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) Then
            Dim NewNodes() As Node = LoadNodeData(Clipboard.GetDataObject().GetData(DataFormats.Text))
            Dim checknames As New ListBox
            For Each Node As Node In MainScreen.Root.All
                checknames.Items.Add(Node.Name.ToLower.Trim)
            Next
            Dim parent As Node = MainScreen.Root
            For Each n As Node In MainScreen.SelectedNodes
                If Not IsNothing(n.Parent) AndAlso (n.Parent.Type = "Split Node") Then parent = n : Exit For
            Next
            MainScreen.Root.DeselectAll()
            For Each Node As Node In NewNodes
                Node.Selected = True
            Next
            If AddNodes(NewNodes, parent) = False Then MsgBox("Failed to load data from clipboard.", MsgBoxStyle.Critical)
            UpdateScreen()
        End If
        MainScreen.InvokeSelectionChanged()
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
        If MainScreen.SelectedNodes.Count <> 0 Then
            Cursor.Current = Cursors.WaitCursor
            Dim data As String = ""
            For Each n As Node In MainScreen.SelectedNodes
                data &= vbCrLf & vbCrLf & n.SaveData
                n.Delete()
            Next
            data = data.Trim(vbCrLf).Trim
            Clipboard.SetData(DataFormats.Text, data)
        End If
        MainScreen.SelectionSquare.UpdateOnScreen()
        MainScreen.UpdateSelectionSquare()
        MainScreen.InvokeSelectionChanged()
    End Sub
    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        Dim snodes() As Node = MainScreen.SelectedNodes.ToArray
        If snodes.Count = 1 Then
            If MessageBox.Show("Are you sure you want to delete node: " & Chr(34) & snodes(0).Name & Chr(34) & " and all possible children?", "Delete Node", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                snodes(0).Delete()
                MainScreen.SelectionSquare.UpdateOnScreen()
                MainScreen.UpdateSelectionSquare()
            End If
        ElseIf snodes.Count > 1 Then
            If MessageBox.Show("Are you sure you want to delete all " & snodes.Count & " selected nodes and all possible children?", "Delete Node", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                For Each n As Node In snodes
                    n.Delete()
                Next
                MainScreen.SelectionSquare.UpdateOnScreen()
                MainScreen.UpdateSelectionSquare()
            End If
        End If
        MainScreen.InvokeSelectionChanged()
    End Sub
    Private Sub HideToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideToolStripMenuItem.Click
        For Each n As Node In MainScreen.SelectedNodes
            n.Render = False
            n.Selected = False
        Next
        UpdateTreeview()
        UpdateScreen()
    End Sub
    Private Sub ShowAllNodesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowAllNodesToolStripMenuItem.Click
        For Each n As Node In MainScreen.Nodes
            n.Render = True
        Next
    End Sub
    Private Sub SelectNodeToolStripMenuItem_DropDownItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles SelectNodeToolStripMenuItem.DropDownItemClicked
        MainScreen.Root.GetNodeByName(e.ClickedItem.Text).Selected = True
        MainScreen.InvokeSelectionChanged()
    End Sub
    Private Sub NewNodeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewNodeToolStripMenuItem.Click
        AddNode.ComboBox2.Items.Clear()
        For Each n As Node In MainScreen.Root.All
            If n.Type = "Split Node" Then AddNode.ComboBox2.Items.Add(n.Name)
        Next
        Dim p As Node = MainScreen.Root
        If Not IsNothing(TreeView1.SelectedNode) Then
            p = MainScreen.Root.GetNodeByName(TreeView1.SelectedNode.Name)
        ElseIf MainScreen.SelectedNodes.Count <> 0 Then
            p = MainScreen.SelectedNodes(0)
        End If
        If IsNothing(p) Then
            p = MainScreen.Root
        ElseIf p.Type <> "Split Node" Then
            p = p.Parent
        End If
        AddNode.ComboBox2.SelectedItem = p.Name
        If AddNode.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If AddNode.ComboBox2.SelectedIndex <> -1 Then p = MainScreen.Root.GetNodeByName(AddNode.ComboBox2.SelectedItem)
            Dim n As New Node(AddNode.TextBox1.Text, AddNode.ComboBox1.SelectedItem)
            n.Selected = True
            MainScreen.DeselectAll()
            p.AddChild(n)
            UpdateScreen()
            MainScreen.InvokeSelectionChanged()
        End If
    End Sub
#End Region

    Public Sub ResetNodes()
        MainScreen.Root.Delete()
        MainScreen.Root = New Node("Global", "Split Node", False)
        MainScreen.Root.AddChild(New Node("VehicleHuds", "Split Node", False))
        MainScreen.Root.AddChild(New Node("IngameHud", "Split Node", False))
        MainScreen.Root.AddChild(New Node("Chat", "Split Node", False))
        MainScreen.Root.AddChild(New Node("WeaponHuds", "Split Node", False))
        MainScreen.Root.AddChild(New Node("SpecialVehicleHuds", "Split Node", False))
        MainScreen.Root.AddChild(New Node("SpecialWeaponHuds", "Split Node", False))
        MainScreen.Root.AddChild(New Node("CommunicationHuds", "Split Node", False))
        MainScreen.Root.AddChild(New Node("SpecialCommunicationHuds", "Split Node", False))
        TreeView1.Nodes.Clear()
        UpdateTreeview()
        Me.Text = "HUD Editor - No Nodes Selected"
        UpdateScreen()
    End Sub
    Public Function AddNodes(ByVal Nodes As Node(), ByVal FixedParent As Node) As Boolean
        AddNodes = False
        Dim checknames As New ListBox
        For Each Node As Node In MainScreen.Nodes
            checknames.Items.Add(Node.Name.ToLower.Trim)
        Next
        For Each NewNode As Node In Nodes
            On Error Resume Next
            If NewNode.Name <> "" And NewNode.Type <> "" Then
                Dim oldname As String = NewNode.Name.Trim
                Dim cindex As Integer = 1
                Do While checknames.Items.Contains(NewNode.Name.ToLower.Trim)
                    NewNode.Name = oldname & "_copy" & cindex
                    cindex += 1
                Loop
                checknames.Items.Add((NewNode.Name.ToLower.Trim))
                If checknames.Items.Contains(NewNode.Parent.Name.ToLower.Trim) Then
                    Dim p As Node = MainScreen.Root.GetNodeByName(NewNode.Parent.Name)
                    If IsNothing(p) Then FixedParent.AddChild(NewNode) Else p.AddChild(NewNode)
                Else
                    FixedParent.AddChild(NewNode)
                End If
                AddNodes = True
            End If
        Next
        UpdateTreeview()
    End Function
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
            Me.Close()
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub LoadLibraries()
        ReDim LibraryTextures(-1)
        If System.IO.Directory.Exists(Application.StartupPath & "\" & TexturesPath) Then
            'Loading library image paths
            For Each file As String In ListFolderFiles(Application.StartupPath & "\" & TexturesPath, "*.texlib")
                WriteLog("Loading library: " & file.Remove(0, Application.StartupPath.Length).Trim("\").Trim)
                Dim newimages() As ImagePointer = LoadImageData(file)
                Dim oldcount As Integer = LibraryTextures.Length
                ReDim Preserve LibraryTextures(oldcount + newimages.Count - 1)
                newimages.CopyTo(LibraryTextures, oldcount)
                newimages = Nothing
            Next
        End If
        WriteLog("Total of " & LibraryTextures.Count & " textures found in libraries")
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UpdateVariables()
        ContentCheck()
        Panel1.Width = 16
        LoadSettings()
        If System.IO.File.Exists(Application.StartupPath & "\log.txt") Then
            Try
                System.IO.File.Delete(Application.StartupPath & "\log.txt")
            Catch
            End Try
        End If
        If BGname.ToLower = "black" Then
            MainScreen.BackgroundImage = Nothing
        Else
            MainScreen.BackgroundImage = LoadImage("bin\Background\" & BGname)
        End If
        If OVname.ToLower = "no overlay" Then
            MainScreen.OverlayImage = Nothing
        Else
            MainScreen.OverlayImage = LoadImage("bin\Overlay\" & OVname)
        End If
        MainScreen.ReferenceCrossImage = LoadImage("bin\referencecross.gif")
        LoadLibraries()
        ResetNodes()
        UpdateScreen()
        LoadFile(Command.Trim(Chr(34)))
        ApplicationLoaded = True
    End Sub

#Region "Slide menu"
    Dim ispanelresizing As Boolean = False
    Dim oldwidth As Integer = 200
    Dim draggednode As Node = Nothing
    Dim ctrldown As Boolean = False
    Private Sub Button1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button1.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim cpos As Point = Button1.PointToClient(Cursor.Position)
            Cursor.Current = Cursors.SizeWE
            Panel1.Width = SetValueBounds(cpos.X + Button1.Location.X + 8, 16, 600)
            ispanelresizing = True
        Else
            ispanelresizing = False
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If ispanelresizing = False Then
            If Panel1.Size.Width < 21 Then Panel1.Width = oldwidth Else oldwidth = Panel1.Width : Panel1.Width = 16
        End If
    End Sub
    Private Sub Button1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button1.MouseUp
        ispanelresizing = False
    End Sub
    Private Sub TreeViewAdapter(ByVal FilePath As String, ByVal Checked As Boolean, ByVal Selected As Boolean)
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
        If Selected = True Then ActiveNode.ForeColor = Drawing.Color.Blue Else ActiveNode.ForeColor = Drawing.Color.Red
        ActiveNode.Checked = Checked
        If hasadded = True Then
            'Node has been added

        End If
    End Sub
    Private Sub Panel1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel1.SizeChanged
        Button1.Height = Panel1.Size.Height
        If Panel1.Size.Width < 21 Then Button1.Text = ">>>>>>" Else Button1.Text = "<<<<<<"
    End Sub
    Private Sub TreeView1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragDrop
        If e.Data.GetDataPresent(DataFormats.Text) Then
            Me.Focus()
            TreeView1.Focus()
            e.Effect = DragDropEffects.Move
            Dim tnode As TreeNode = TreeView1.GetNodeAt(TreeView1.PointToClient(Cursor.Position))
            If Not IsNothing(tnode) Then
                Dim onode As Node = MainScreen.Root.GetNodeByName(tnode.Text)
                If IsNothing(onode) Then onode = MainScreen.Root
                If onode.Type <> "Split Node" Then onode = onode.Parent
                If IsNothing(onode) Then onode = MainScreen.Root
                If IsNothing(draggednode) Then
                    'Text was dragged
                    Dim NewNodes() As Node = LoadNodeData(e.Data.GetData(DataFormats.Text))
                    If NewNodes.Count <> 0 Then
                        For Each n As Node In NewNodes
                            n.Parent = onode
                        Next
                        AddNodes(NewNodes, onode)
                    End If
                Else
                    draggednode.Parent = onode
                    draggednode = Nothing
                End If
                UpdateTreeview()
                UpdateScreen()
            End If
        End If
    End Sub
    Private Sub TreeView1_ItemDrag(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles TreeView1.ItemDrag
        draggednode = MainScreen.Root.GetNodeByName(CType(e.Item, TreeNode).Text)
        If Not IsNothing(draggednode) AndAlso draggednode.CanBeSaved = True Then TreeView1.DoDragDrop(draggednode.SaveData, DragDropEffects.Move)
    End Sub
    Private Sub TreeView1_DragOver(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragOver, MainScreen.DragOver
        If e.Data.GetDataPresent(DataFormats.Text) Then
            Me.Focus()
            TreeView1.Focus()
            e.Effect = DragDropEffects.Move
            Dim tnode As TreeNode = TreeView1.GetNodeAt(TreeView1.PointToClient(Cursor.Position))
            If Not IsNothing(tnode) Then
                Dim onode As Node = MainScreen.Root.GetNodeByName(tnode.Text)
                If IsNothing(onode) Then onode = MainScreen.Root
                If onode.Type <> "Split Node" Then onode = onode.Parent
                If IsNothing(onode) Then onode = MainScreen.Root
                TreeView1.SelectedNode = GetTreeViewNode(TreeView1, onode.Name)
            End If
        End If
    End Sub
    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        If Not IsNothing(TreeView1.SelectedNode) Then
            Dim n As Node = MainScreen.Root.GetNodeByName(TreeView1.SelectedNode.Text)
            If ctrldown = False Then MainScreen.Root.DeselectAll()
            If n.CanBeSaved = True Then
                n.Selected = True
                UpdateScreen()
                MainScreen.InvokeSelectionChanged()
            End If
        End If
    End Sub
    Private Sub TreeView1_BeforeLabelEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles TreeView1.BeforeLabelEdit
        If MainScreen.Root.GetNodeByName(e.Node.Text).CanBeSaved = False Then e.CancelEdit = True
    End Sub
    Private Sub TreeView1_AfterLabelEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles TreeView1.AfterLabelEdit
        e.CancelEdit = True
        If Not IsNothing(e.Label) Then
            Dim newname As String = e.Label.Replace(" ", "_").Replace("\", "").Replace("/", "")
            If e.Node.Text <> newname Then
                Dim aexist As Boolean = False
                For Each n As Node In MainScreen.Root.All
                    If n.Name.ToLower.Trim = newname.ToLower.Trim Then
                        If n.Name.ToLower.Trim <> e.Node.Text.ToLower.Trim Then aexist = True
                    End If
                Next
                If aexist = True Then
                    MsgBox("A node with the name: " & Chr(34) & newname & Chr(34) & " already exists.", MsgBoxStyle.Information)
                Else
                    MainScreen.Root.GetNodeByName(e.Node.Text).Name = newname
                    MainScreen.InvokeSelectionChanged()
                End If
            End If
        End If
    End Sub
    Private Sub TreeView1_AfterCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterCheck
        If ApplicationLoaded = True And UpdatingTreeview = False Then
            Dim n As Node = MainScreen.Root.GetNodeByName(e.Node.Text)
            n.Render = e.Node.Checked
            For Each subnode As TreeNode In e.Node.Nodes
                subnode.Checked = e.Node.Checked
            Next
            UpdateTreeview()
        End If
    End Sub
    Private Sub TreeView1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView1.MouseMove
        Dim n As TreeNode = TreeView1.GetNodeAt(TreeView1.PointToClient(Cursor.Position))
        If Not IsNothing(n) Then MainScreen.HightlightItem(n.Text)
    End Sub
    Private Sub TreeView1_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TreeView1.MouseLeave
        MainScreen.HightlightItem("")
    End Sub
    Dim UpdatingTreeview As Boolean = False
    Public Sub UpdateTreeview()
        UpdatingTreeview = True
        Dim existingpaths As New List(Of String)
        For Each Node As Node In MainScreen.Nodes
            Dim path As String = Node.Tree
            existingpaths.Add(path.ToLower.Trim)
            TreeViewAdapter(path, Node.Render, Node.Selected)
        Next
        Dim delnodes As New List(Of TreeNode)
        For Each Node As TreeNode In GetTreeViewNodes(TreeView1)
            Try
                If Not existingpaths.Contains(Node.FullPath.ToLower.Trim) Then Node.Remove()
            Catch
            End Try
        Next
        UpdatingTreeview = False
    End Sub
    Private Sub TreeView1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TreeView1.KeyDown, TreeView1.KeyUp
        ctrldown = e.Control
    End Sub
    Private Sub ComboBox1_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.DropDown
        ComboBox1.Items.Clear()
        For Each n As Node In Me.MainScreen.Nodes
            For Each line As String In n.LogicShowVariables.Split(vbCrLf)
                line = line.ToLower.Trim
                If line.StartsWith("equal guiindex ") Or line.StartsWith("or guiindex ") Then
                    Dim gui As Integer = Val(line.Split(" ").Last)
                    If Not ComboBox1.Items.Contains(gui) Then ComboBox1.Items.Add(gui)
                End If
            Next
        Next
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim gui As Integer = Val(Val(ComboBox1.SelectedItem))
        For Each n As Node In MainScreen.Nodes
            n.Render = True
            For Each line As String In n.LogicShowVariables.Split(vbCrLf)
                line = line.ToLower.Trim
                If line.StartsWith("equal guiindex ") Or line.StartsWith("or guiindex ") Then
                    If Val(line.Split(" ").Last) = gui Then
                        n.Render = True
                        Exit For
                    Else
                        n.Render = False
                    End If
                End If
            Next
        Next
        UpdateTreeview()
        UpdateScreen()
    End Sub
#End Region

#Region "Node tool dialog buttons"
    Private Sub MainButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainButton.Click
        MainDialog.Show()
    End Sub
    Private Sub TextureButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextureButton1.Click, TextureButton2.Click
        If CType(sender, ToolStripItem).Text = "Empty Texture" Then TextureBrowser.Text = "empty"
        If CType(sender, ToolStripItem).Text = "Full Texture" Then TextureBrowser.Text = "full"
        If CType(sender, ToolStripItem).Text = "Inactive Texture" Then TextureBrowser.Text = "empty"
        If CType(sender, ToolStripItem).Text = "Active Texture" Then TextureBrowser.Text = "full"
        TextureBrowser.ShowDialog()
    End Sub
    Private Sub ColorButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorButton.Click
        ColorDialog.Show()
        ColorDialog.BringToFront()
    End Sub
    Private Sub StyleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StyleButton.Click
        Dim snodes As List(Of Node) = MainScreen.SelectedNodes
        If snodes.Count = 1 Then
            If snodes(0).Type = "Bar Node" Then BarStyle.Show()
            If snodes(0).Type = "Compass Node" Then CompassStyle.Show()
            If snodes(0).Type = "Text Node" Then TextStyle.Show()
            If snodes(0).Type = "Button Node" Then ButtonStyle.Show()
        End If
    End Sub
    Private Sub StyleButton_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StyleButton.VisibleChanged
        If StyleButton.Visible = False Then
            BarStyle.Close()
            CompassStyle.Close()
            TextStyle.Close()
            ButtonStyle.Close()
        End If
    End Sub
    Private Sub RotationButton_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RotationButton.VisibleChanged
        If RotationButton.Visible = False And ApplicationLoaded = True Then RotationDialog.Close()
    End Sub
    Private Sub RotationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RotationButton.Click
        RotationDialog.Show()
    End Sub
    Private Sub MainButton_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainButton.VisibleChanged
        If MainButton.Visible = False Then MainDialog.Close()
    End Sub
    Private Sub ColorButton_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorButton.VisibleChanged
        If ColorButton.Visible = False Then ColorDialog.Close()
    End Sub
    Private Sub SimulateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimulateButton.Click
        If SimulateButton.BackColor = Color.Control Then
            SimulateButton.BackColor = Color.Active
            IsInSimulationMode = True
            Timer1.Start()
        Else
            SimulateButton.BackColor = Color.Control
            IsInSimulationMode = False
            Timer1.Stop()
            For Each v As VariableHandler In Variables
                v.ResetValue()
            Next
            UpdateScreen()
        End If
        MainScreen.InvokeSelectionChanged()
        MainScreen.Invalidate()
    End Sub
    Private Sub ShowButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowButton.Click
        NodeShowDialog.Show()
    End Sub
    Private Sub ShowButton_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowButton.VisibleChanged
        If ShowButton.Visible = False Then NodeShowDialog.Close()
    End Sub
#End Region

#Region "Simulation"
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal Key As Windows.Forms.Keys) As Boolean

    Dim simpitch As Single = 0
    Dim simbanking As Single = 0
    Dim simangle As Single = 0
    Dim simspeed As Single = 100
    Dim simtorque As Single = 0
    Dim simalt As Single = 100
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If GetAsyncKeyState(Keys.Escape) Then
            SimulateButton.BackColor = Color.Control
            IsInSimulationMode = False
            Timer1.Stop()
        End If
        If GetAsyncKeyState(Keys.A) Then
            simangle -= 1.5
            If simangle < 1 Then simangle = 360
        End If
        If GetAsyncKeyState(Keys.D) Then
            simangle += 1.5
            If simangle > 360 Then simangle = 1
        End If
        If GetAsyncKeyState(Keys.Right) Then
            simbanking -= 1.5
            If simbanking < -60 Then simbanking = -60
        End If
        If GetAsyncKeyState(Keys.Left) Then
            simbanking += 1.5
            If simbanking > 60 Then simbanking = 60
        End If
        If GetAsyncKeyState(Keys.Up) Then
            simpitch -= 1.5
            If simpitch < -90 Then simpitch = -90
        End If
        If GetAsyncKeyState(Keys.Down) Then
            simpitch += 1.5
            If simpitch > 90 Then simpitch = 90
        End If
        If GetAsyncKeyState(Keys.S) Then
            simtorque -= 0.05
        ElseIf GetAsyncKeyState(Keys.W) Then
            simtorque += 0.03
        Else
            simtorque -= 0.03
        End If
        If simtorque > 1 Then simtorque = 1
        If simtorque < 0 Then simtorque = 0
        simspeed += (simtorque - 0.5) * (20 - simspeed / 100)
        If simspeed < 0 Then simspeed = 0
        If simspeed > 2000 Then simspeed = 2000
        'altitude
        Dim altchange As Single = (simpitch * simspeed / 1500) * ((4000 - simalt) / 4000) - 10
        simalt += altchange
        Dim VehicleElevationSpeedAngle As Single = SetValueBounds(altchange * 0.01, 0, 1)
        If altchange < 0 Then VehicleElevationSpeedAngle = SetValueBounds(1 - (altchange * -0.01), 0, 1)
        If VehicleElevationSpeedAngle > 0.3 And altchange > 0 Then VehicleElevationSpeedAngle = 0.3
        If VehicleElevationSpeedAngle < 0.7 And altchange < 0 Then VehicleElevationSpeedAngle = 0.7
        If simalt < 0 Then simalt = 0
        If simalt > 4000 Then simalt = 4000
        If simalt = 0 Or simspeed = 0 Then VehicleElevationSpeedAngle = 0
        'setting variables
        For Each v As VariableHandler In Variables
            If v.Name.ToLower = "angleofattack" Then v.Value = CInt(simpitch)
            If v.Name.ToLower = "vehiclebanking" Then v.Value = CInt(simbanking)
            If v.Name.ToLower = "vehicleangle" Then v.Value = simangle
            If v.Name.ToLower = "speedstring" Then v.Value = CInt(simspeed)
            If v.Name.ToLower = "altitudestring" Then v.Value = CInt(simalt)
            If v.Name.ToLower = "torquestring" Then v.Value = CInt(simtorque * 100)
            If v.Name.ToLower = "torqueangle" Then v.Value = simtorque * 360
            If v.Name.ToLower = "torque" Then v.Value = simtorque
            If v.Name.ToLower = "vehicleelevationspeedangle" Then v.Value = VehicleElevationSpeedAngle * 360
            If v.Name.ToLower = "radiointerfaceshow" Then v.Value = GetAsyncKeyState(Keys.Q)
            If v.Name.ToLower = "hitindicatoriconshow" Then v.Value = GetAsyncKeyState(Keys.LButton)
        Next
        UpdateScreen()
    End Sub
#End Region

    Private Sub MainScreen_SelectionChanged() Handles MainScreen.SelectionChanged
        UpdateTreeview()
        Dim nodes() As Node = MainScreen.SelectedNodes.ToArray
        If nodes.Count = 0 Then Me.Text = "HUD Editor - No Nodes Selected"
        If nodes.Count = 1 Then Me.Text = "HUD Editor - " & nodes(0).Name & " (" & nodes(0).Type & ")"
        If nodes.Count > 1 Then Me.Text = "HUD Editor - " & nodes.Count & " nodes selected"
        Dim colore As Boolean = IsInSimulationMode = False
        Dim maine As Boolean = IsInSimulationMode = False
        Dim tex1e As Boolean = False
        Dim tex2e As Boolean = False
        Dim style As Boolean = False
        Dim rote As Boolean = False
        Dim showe As Boolean = False
        If nodes.Count = 1 And IsInSimulationMode = False Then
            showe = True
            If nodes(0).Type = "Picture Node" Then
                TextureButton1.Text = "Texture"
                tex1e = True
                rote = True
            ElseIf nodes(0).Type = "Compass Node" Then
                TextureButton1.Text = "Texture"
                tex1e = True
                style = True
            ElseIf nodes(0).Type = "Bar Node" Then
                TextureButton1.Text = "Empty Texture"
                TextureButton2.Text = "Full Texture"
                tex1e = True
                tex2e = True
                style = True
            ElseIf nodes(0).Type = "Text Node" Then
                style = True
            ElseIf nodes(0).Type = "Button Node" Then
                TextureButton1.Text = "Inactive Texture"
                TextureButton2.Text = "Active Texture"
                tex1e = True
                tex2e = True
                style = True
            End If
        End If
        ShowButton.Visible = showe
        ColorButton.Visible = colore
        MainButton.Visible = maine
        TextureButton1.Visible = tex1e
        TextureButton2.Visible = tex2e
        StyleButton.Visible = style
        RotationButton.Visible = rote
    End Sub
    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        SaveSettings()
    End Sub

    Private Sub Form1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        MsgBox(e.KeyCode)
    End Sub

End Class
