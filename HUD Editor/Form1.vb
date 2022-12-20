Imports System.Threading

Public Class Form1
    Dim MouseIsDown As Boolean = False
    Dim MouseDownStartPos As New Point(0, 0)
    Dim MouseDownStartValueX, MouseDownStartValueY As Integer

    Dim threadupdateimage As Thread

    Private Sub UpdateImage()
        Do
            Try
                If Edited = True Then
                    Edited = False
                    Dim finimage As New Bitmap(800, 600)
                    Dim g As Graphics = Graphics.FromImage(finimage)
                    For i As Integer = 0 To NodeInformation.Items.Count - 1
                        Dim item As ListViewItem = NodeInformation.Items(i)
                        If NodesToRender.Items.Contains(item.Text.ToLower) Then
                            'Process this node and then node types
                            If item.SubItems(1).Text = "Picture Node" Then
                                Dim posX As Integer = Val(NodeInformation.Items(i).SubItems(9).Text)
                                Dim posY As Integer = Val(NodeInformation.Items(i).SubItems(10).Text)
                                Dim staticrot As Integer = Val(NodeInformation.Items(i).SubItems(11).Text)
                                Dim rotvarmidX As Integer = Val(NodeInformation.Items(i).SubItems(12).Text)
                                Dim rotvarmidY As Integer = Val(NodeInformation.Items(i).SubItems(13).Text)
                                Dim rotvarangle As Integer = Val(NodeInformation.Items(i).SubItems(14).Text)
                                g.DrawImage(RenderPictureNode(SizedImage(i), staticrot, posX, posY, rotvarmidX, rotvarmidY, rotvarangle), New Point(0, 0))
                            ElseIf item.SubItems(1).Text = "Text Node" Then

                            End If
                        End If
                    Next
                    g.DrawImage(OVImage, New Point(0, 0))
                    PictureBoxAdapter(finimage, Me)
                    g.Dispose()
                End If
                Thread.Sleep(50)
            Catch
            End Try
        Loop
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        PictureBox1.Focus()
        'ViewDialog(0)
    End Sub

    Private Sub TrackBarYpos_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarYpos.ValueChanged
        PositionDialog.NumericUpDown4.Value = TrackBarYpos.Value * -1
    End Sub
    Private Sub TrackBarXpos_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarXPos.ValueChanged
        PositionDialog.NumericUpDown3.Value = TrackBarXPos.Value
    End Sub

    Private Sub TextureButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextureButton.Click
        If NodeType = "Picture Node" Then
            TextureBrowser.SelectedPath.Text = NodeInformation.Items(CurrentIndex).SubItems(2).Text
            ImageSelectorImagePath = "Textures"
            If TextureBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Cursor.Current = Cursors.WaitCursor
                Dim path = TextureBrowser.SelectedPath.Text
                If System.IO.File.Exists(path) Then
                    Try
                        NodeInformation.Items(CurrentIndex).SubItems(2).Text = path
                        OriginalImage(CurrentIndex) = LoadImage(path)
                        NodeInformation.Items(CurrentIndex).SubItems(7).Text = OriginalImage(CurrentIndex).Size.Width
                        NodeInformation.Items(CurrentIndex).SubItems(8).Text = OriginalImage(CurrentIndex).Size.Height
                        ProcessPictureNodeImage(True, True)
                    Catch ex As Exception
                        MsgBox(ex.Message, MsgBoxStyle.Critical)
                    End Try
                End If
            End If
        End If
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub ColorButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorButton.Click
        ViewDialog(1)
    End Sub
    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        ViewDialog(0)
        NodeSelect.Visible = True
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        NodeSelect.Visible = False
        NodeSelect.Show()
        NodeSelect.Visible = False
        'Generate default node
        'OriginalImage(0) = New Bitmap(32, 32)
        'ColoredImage(0) = New Bitmap(32, 32)
        'SizedImage(0) = New Bitmap(32, 32)
        'Dim item As New ListViewItem
        'item.Text = "Testnode1"
        'item.SubItems.Add("Picture Node")
        'item.SubItems.Add("")
        'item.SubItems.Add(1000)
        'item.SubItems.Add(1000)
        'item.SubItems.Add(1000)
        'item.SubItems.Add(1000)
        'item.SubItems.Add(32)
        'item.SubItems.Add(32)
        'item.SubItems.Add(0)
        'item.SubItems.Add(0)
        'NodeInformation.Items.Add(item)
        'Dim item2 As New ListViewItem
        'item2.Text = "Testnode1"
        'item2.SubItems.Add("Picture Node")
        'item2.Checked = True
        'NodeSelect.NodeSelector.Items.Add(item2)

        threadupdateimage = New Thread(AddressOf UpdateImage)
        threadupdateimage.IsBackground = True
        threadupdateimage.Start()
        Loading = False
    End Sub

    Private Sub SizeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SizeButton.Click
        ViewDialog(2)
    End Sub
    Private Sub PositionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PositionButton.Click
        ViewDialog(3)
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
                BGImage = ResizeImage(LoadImage(finalpath), 800, 600, True)
            End If
        Catch
        End Try
        LoadBackground()
    End Sub
    Private Sub LoadBackground()
        If System.IO.File.Exists("bin\referencecross.gif") Then
            If DrawReferenceCrossToolStripMenuItem.Checked = True Then
                Dim rcross As Bitmap = ResizeImage(Image.FromFile("bin\referencecross.gif"), 800, 600, True)
                Dim nimage As New Bitmap(800, 600)
                Dim g As Graphics = Graphics.FromImage(nimage)
                g.DrawImage(BGImage, New Point(0, 0))
                g.DrawImage(rcross, New Point(0, 0))
                PictureBox1.BackgroundImage = nimage
                'g.Dispose()
            Else
                PictureBox1.BackgroundImage = BGImage
            End If
        Else
            DrawReferenceCrossToolStripMenuItem.Enabled = False
        End If
    End Sub
    Private Sub DrawReferenceCrossToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DrawReferenceCrossToolStripMenuItem.CheckedChanged
        LoadBackground()
    End Sub

    Private Sub PictureBox1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        MouseIsDown = True
        Cursor.Current = Cursors.NoMove2D
        MouseDownStartPos = PictureBox1.PointToClient(Cursor.Position)
        If ViewedDialog = 2 Then
            'Size
            MouseDownStartValueX = SizeDialog.NumericUpDown1.Value
            MouseDownStartValueY = SizeDialog.NumericUpDown2.Value
        End If
        If ViewedDialog = 3 Then
            'Position
            MouseDownStartValueX = PositionDialog.NumericUpDown3.Value
            MouseDownStartValueY = PositionDialog.NumericUpDown4.Value
        End If
    End Sub
    Private Sub PictureBox1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp
        Cursor.Current = Cursors.Default
        MouseIsDown = False
    End Sub



    Private Sub PictureBox1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        Try
            If MouseIsDown = True Then
                Dim MousePos As Point = PictureBox1.PointToClient(Cursor.Position)
                If ViewedDialog = 2 Then
                    'Size
                    SizeDialog.NumericUpDown1.Value = SetValueBounds(MouseDownStartValueX + MousePos.X - MouseDownStartPos.X, 1, 2048)
                    SizeDialog.NumericUpDown2.Value = SetValueBounds(MouseDownStartValueY + MousePos.Y - MouseDownStartPos.Y, 1, 2048)
                End If
                If ViewedDialog = 3 Then
                    'Position
                    PositionDialog.NumericUpDown3.Value = SetValueBounds(MouseDownStartValueX + MousePos.X - MouseDownStartPos.X, -2048, 800)
                    PositionDialog.NumericUpDown4.Value = SetValueBounds(MouseDownStartValueY + MousePos.Y - MouseDownStartPos.Y, -2048, 600)
                End If
                If ViewedDialog = 4 Then
                    'Rotation
                    If NodeType = "Picture Node" Then
                        Dim offsetX As Integer = MousePos.X - (Val(NodeInformation.Items(CurrentIndex).SubItems(9).Text) + SizedImage(CurrentIndex).Width * 0.5)
                        Dim offsetY As Integer = MousePos.Y - (Val(NodeInformation.Items(CurrentIndex).SubItems(10).Text) + SizedImage(CurrentIndex).Height * 0.5)
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
            End If
        Catch
        End Try
    End Sub

    Private Sub LoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        Dim goload As Boolean = True
        If NodeSelect.NodeSelector.Items.Count <> 0 Then
            Dim result As MsgBoxResult = MessageBox.Show("Press 'YES' to add the new HUD to the current scene, press 'NO' to delete the current scene and load the new HUD or press 'CANCEL' to cancel loading.", "Load", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            If result = MsgBoxResult.Cancel Then goload = False
            If result = MsgBoxResult.No Then
                For i As Integer = 0 To MaxNodeCount
                    OriginalImage(i) = New Bitmap(32, 32)
                    ColoredImage(i) = New Bitmap(32, 32)
                    SizedImage(i) = New Bitmap(32, 32)
                Next
                NodeSelect.NodeSelector.Items.Clear()
                NodeInformation.Items.Clear()
            End If
        End If

        If goload = True Then
            Cursor.Current = Cursors.WaitCursor
            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                If System.IO.File.Exists(OpenFileDialog1.FileName) Then

                    Dim currenttype As String = ""
                    Dim reader As New System.IO.StreamReader(OpenFileDialog1.FileName)
                    Do While reader.Peek <> -1
                        Try
                            Dim textline As String = reader.ReadLine
                            Do While textline.Contains("	")
                                textline = textline.Replace("	", " ")
                            Loop
                            Do While textline.Contains("  ")
                                textline = textline.Replace("  ", " ")
                            Loop
                            'Process Textline

                            If textline.ToLower.StartsWith("hudbuilder.setpicturenodetexture") Then
                                If currenttype = "Picture Node" Then
                                    Dim texpath As String = "Textures\" & textline.Split(" ").Last.Trim
                                    If Not System.IO.File.Exists(texpath) And System.IO.File.Exists(StrReverse(StrReverse(texpath).Remove(0, 4)) & ".dds") Then
                                        texpath = StrReverse(StrReverse(texpath).Remove(0, 4)) & ".dds"
                                    End If
                                    If Not System.IO.File.Exists(texpath) And System.IO.File.Exists(StrReverse(StrReverse(texpath).Remove(0, 4)) & ".tga") Then
                                        texpath = StrReverse(StrReverse(texpath).Remove(0, 4)) & ".tga"
                                    End If
                                    texpath = texpath.Replace("/", "\")
                                    If System.IO.File.Exists(texpath) Then
                                        NodeInformation.Items(CurrentIndex).SubItems(2).Text = texpath
                                        OriginalImage(CurrentIndex) = LoadImage(texpath)
                                        ProcessPictureNodeImage(True, True)
                                    End If
                                End If
                            End If


                            If textline.ToLower.StartsWith("hudbuilder.setpicturenoderotation") Then
                                If currenttype = "Picture Node" Then
                                    NodeInformation.Items(CurrentIndex).SubItems(11).Text = 360 - Val(textline.Split(" ").Last.Trim)
                                End If
                            End If
                            If textline.ToLower.StartsWith("hudbuilder.setnodeposvariable 0") Then
                                If currenttype = "Picture Node" Then
                                    NodeInformation.Items(CurrentIndex).SubItems(14).Text = textline.Split(" ").Last.Trim
                                End If
                            End If
                            If textline.ToLower.StartsWith("hudbuilder.setnodeposvariable 1") Then
                                If currenttype = "Picture Node" Then
                                    NodeInformation.Items(CurrentIndex).SubItems(15).Text = textline.Split(" ").Last.Trim
                                End If
                            End If
                            If textline.ToLower.StartsWith("hudbuilder.setpicturenoderotatevariable") Then
                                If currenttype = "Picture Node" Then
                                    NodeInformation.Items(CurrentIndex).SubItems(16).Text = textline.Split(" ").Last.Trim
                                End If
                            End If
                            If textline.ToLower.StartsWith("hudbuilder.setpicturenodecenterpoint") Then
                                If currenttype = "Picture Node" Then
                                    NodeInformation.Items(CurrentIndex).SubItems(12).Text = Val(textline.Split(" ").GetValue(1).Trim)
                                    NodeInformation.Items(CurrentIndex).SubItems(13).Text = Val(textline.Split(" ").GetValue(2).Last.Trim)
                                End If
                            End If

                            If textline.ToLower.StartsWith("hudbuilder.setnodecolor") Then
                                Dim ColorA As Integer = 1000
                                Dim ColorR As Integer = Val(textline.Split(" ").GetValue(1).trim) * 1000
                                Dim ColorG As Integer = Val(textline.Split(" ").GetValue(2).trim) * 1000
                                Dim ColorB As Integer = Val(textline.Split(" ").GetValue(3).trim) * 1000
                                Try
                                    ColorA = Val(textline.Split(" ").GetValue(4).trim) * 1000
                                Catch
                                End Try
                                If currenttype = "Picture Node" Then
                                    NodeInformation.Items(CurrentIndex).SubItems(3).Text = ColorA
                                    NodeInformation.Items(CurrentIndex).SubItems(4).Text = ColorR
                                    NodeInformation.Items(CurrentIndex).SubItems(5).Text = ColorG
                                    NodeInformation.Items(CurrentIndex).SubItems(6).Text = ColorB
                                    ProcessPictureNodeImage(True, True)
                                End If
                            End If
                            If textline.ToLower.StartsWith("hudbuilder.create") Then
                                currenttype = ""
                                If textline.ToLower.StartsWith("hudbuilder.createpicturenode") Then

                                    currenttype = "Picture Node"
                                    Dim NodeName As String = ""
                                    Dim NodePosX As Integer = 0
                                    Dim NodePosY As Integer = 0
                                    Dim NodeSizeX As Integer = 32
                                    Dim NodeSizeY As Integer = 32
                                    Try
                                        NodeName = textline.Split(" ").GetValue(2)
                                        NodePosX = Val(textline.Split(" ").GetValue(3).trim)
                                        NodePosY = Val(textline.Split(" ").GetValue(4).trim)
                                        NodeSizeX = Val(textline.Split(" ").GetValue(5).trim)
                                        NodeSizeY = Val(textline.Split(" ").GetValue(6).trim)
                                    Catch
                                    End Try

                                    Dim InfoItem As New ListViewItem
                                    Dim SelectorItem As New ListViewItem
                                    'Generate default information node
                                    OriginalImage(NodeInformation.Items.Count) = New Bitmap(32, 32)
                                    ColoredImage(NodeInformation.Items.Count) = New Bitmap(32, 32)
                                    SizedImage(NodeInformation.Items.Count) = New Bitmap(32, 32)
                                    InfoItem.Text = NodeName
                                    InfoItem.SubItems.Add(currenttype)
                                    InfoItem.SubItems.Add("")
                                    InfoItem.SubItems.Add(1000)
                                    InfoItem.SubItems.Add(1000)
                                    InfoItem.SubItems.Add(1000)
                                    InfoItem.SubItems.Add(1000)
                                    InfoItem.SubItems.Add(NodeSizeX)
                                    InfoItem.SubItems.Add(NodeSizeY)
                                    InfoItem.SubItems.Add(NodePosX)
                                    InfoItem.SubItems.Add(NodePosY)
                                    InfoItem.SubItems.Add(0) '10
                                    InfoItem.SubItems.Add(0) '11
                                    InfoItem.SubItems.Add(0) '12
                                    InfoItem.SubItems.Add(0) '13
                                    InfoItem.SubItems.Add("") '14
                                    InfoItem.SubItems.Add("") '15
                                    InfoItem.SubItems.Add("") '16
                                    InfoItem.SubItems.Add("") '17



                                    NodeInformation.Items.Add(InfoItem)
                                    'Generate visible node
                                    SelectorItem.Text = NodeName
                                    SelectorItem.SubItems.Add(currenttype)
                                    SelectorItem.Checked = True
                                    SelectorItem.Selected = True
                                    NodeSelect.NodeSelector.Items.Add(SelectorItem)
                                End If
                            End If
                        Catch
                        End Try
                    Loop
                    reader.Close()
                End If
            End If
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RotationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RotationButton.Click
        ViewDialog(4)
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim writer As New System.IO.StreamWriter(SaveFileDialog1.FileName)
            For i As Integer = 0 To NodeSelect.NodeSelector.Items.Count - 1
                Dim nodename As String = NodeInformation.Items(i).Text.Replace(" ", "_")
                Dim texture As String = NodeInformation.Items(i).SubItems(2).Text.Remove(0, 9)
                Dim colorA As Single = Val(NodeInformation.Items(i).SubItems(3).Text) * 0.001
                Dim colorR As Single = Val(NodeInformation.Items(i).SubItems(4).Text) * 0.001
                Dim colorG As Single = Val(NodeInformation.Items(i).SubItems(5).Text) * 0.001
                Dim colorB As Single = Val(NodeInformation.Items(i).SubItems(6).Text) * 0.001
                Dim sizeX As Integer = Val(NodeInformation.Items(i).SubItems(7).Text)
                Dim sizeY As Integer = Val(NodeInformation.Items(i).SubItems(8).Text)
                Dim posX As Integer = Val(NodeInformation.Items(i).SubItems(9).Text)
                Dim posY As Integer = Val(NodeInformation.Items(i).SubItems(10).Text)
                Dim midX As Integer = Val(NodeInformation.Items(i).SubItems(12).Text)
                Dim midY As Integer = Val(NodeInformation.Items(i).SubItems(13).Text)
                Dim rotvar As String = NodeInformation.Items(i).SubItems(16).Text
                Dim offXvar As String = NodeInformation.Items(i).SubItems(14).Text
                Dim offYvar As String = NodeInformation.Items(i).SubItems(15).Text
                Dim rotstaticangle As Integer = 360 - Val(NodeInformation.Items(i).SubItems(11).Text)
                writer.WriteLine("hudBuilder.createPictureNode Parent_name " & nodename & " " & posX & " " & posY & " " & sizeX & " " & sizeY)
                writer.WriteLine("hudBuilder.setPictureNodeTexture " & texture)
                writer.WriteLine("hudBuilder.setNodeColor " & (Val(NodeInformation.Items(i).SubItems(4).Text) * 0.001).ToString.Replace(",", ".") & " " & (Val(NodeInformation.Items(i).SubItems(5).Text) * 0.001).ToString.Replace(",", ".") & " " & (Val(NodeInformation.Items(i).SubItems(6).Text) * 0.001).ToString.Replace(",", ".") & " " & (Val(NodeInformation.Items(i).SubItems(3).Text) * 0.001).ToString.Replace(",", "."))
                If rotstaticangle <> 0 Then writer.WriteLine("hudBuilder.setPictureNodeRotation " & rotstaticangle)


                If offXvar <> "" Then writer.WriteLine("hudBuilder.setNodePosVariable 0 " & offXvar)
                If offYvar <> "" Then writer.WriteLine("hudBuilder.setNodePosVariable 1 " & offYvar)
                If rotvar <> "" Then
                    writer.WriteLine("hudBuilder.setPictureNodeRotateVariable " & rotvar)
                    writer.WriteLine("hudBuilder.setPictureNodeCenterPoint " & midX & " " & midY)
                End If
                writer.WriteLine("")
            Next
            writer.Close()
        End If
    End Sub

    Private Sub VariablesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VariablesButton.Click
        ViewDialog(5)
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
                OVImage = ResizeImage(LoadImage(finalpath), 800, 600, True)
            Else
                OVImage = New Bitmap(16, 16)
            End If
        Catch
        End Try
        Edited = True
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        ViewDialog(0)
        SimulateDialog.ShowDialog()
    End Sub
End Class
