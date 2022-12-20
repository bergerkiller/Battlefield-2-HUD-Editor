Imports System.Windows.Forms
Imports System.Threading

Public Class TextureBrowser
    Dim dmode As Integer = 0 '0=default ; 1=full (on) texture ; 2=empty (off) texture
    Dim BrowseTimeLine As New List(Of String)
    Dim TimeLineIndex As Integer = 0
    Dim Textures As New List(Of String)
    Dim CurrentFolder As String = ""
    Dim IsLoadingImages As Boolean = False
    Dim ThreadLoadListviewImages As Thread
    Dim tmpchecklistview As New ListView
    Dim IsUpdatingPath As Boolean = False
    Private Sub UndoRedoModified(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If IsUpdatingPath = False Then
            'Update browsing time line
            Do While BrowseTimeLine.Count > TimeLineIndex + 1
                BrowseTimeLine.RemoveAt(BrowseTimeLine.Count - 1)
            Loop
            BrowseTimeLine.Add(ComboBox1.SelectedItem)
            TimeLineIndex = BrowseTimeLine.Count - 1
            Button1.Enabled = TimeLineIndex > 0
            Button2.Enabled = TimeLineIndex < BrowseTimeLine.Count - 1
        End If
    End Sub
    Private Sub UndoRedoBack(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Back timeline
        If TimeLineIndex > 0 Then TimeLineIndex -= 1
        IsUpdatingPath = True
        ComboBox1.SelectedItem = BrowseTimeLine(TimeLineIndex)
        IsUpdatingPath = False
        Button1.Enabled = TimeLineIndex > 0
        Button2.Enabled = TimeLineIndex < BrowseTimeLine.Count - 1
    End Sub
    Private Sub UndoRedoForward(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'Forward timeline
        If TimeLineIndex < BrowseTimeLine.Count - 1 Then TimeLineIndex += 1
        IsUpdatingPath = True
        ComboBox1.SelectedItem = BrowseTimeLine(TimeLineIndex)
        IsUpdatingPath = False
        Button1.Enabled = TimeLineIndex > 0
        Button2.Enabled = TimeLineIndex < BrowseTimeLine.Count - 1
    End Sub
    Private Sub AbortLoadingTextures()
        Try
            If IsLoadingImages = True Then ThreadLoadListviewImages.Abort()
        Catch
        End Try
    End Sub
    Private Sub LoadListviewImages()
        IsLoadingImages = True
        Dim index As Integer = 0
        For Each item As ListViewItem In tmpchecklistview.Items
            If item.ImageIndex = 1 Then
                Dim libpath As String = CurrentFolder & "\" & item.Text
                Dim filepath As String = Application.StartupPath & "\" & TexturesPath & "\" & CurrentFolder & "\" & item.Text
                If System.IO.File.Exists(filepath) Then
                    ListViewImageSelector(index, filepath.ToLower, ResizeImage(LoadImage(filepath), 128, 128, False))
                Else
                    Try
                        ListViewImageSelector(index, filepath.ToLower, ResizeImage(LoadImage(libpath), 128, 128, False))
                    Catch
                        WriteLog("Failed to load library texture: " & libpath)
                    End Try
                End If
            End If
            index += 1
        Next
        IsLoadingImages = False
    End Sub
    Delegate Sub myListViewImageSelector(ByVal ItemIndex As Integer, ByVal ImageKey As String, ByVal ItemImage As Image)
    Private Sub ListViewImageSelector(ByVal ItemIndex As Integer, ByVal ImageKey As String, ByVal ItemImage As Image)
        If Me.InvokeRequired Then
            Dim d As New myListViewImageSelector(AddressOf ListViewImageSelector)
            Me.Invoke(d, ItemIndex, ImageKey, ItemImage)
        Else
            ImageList1.Images.Add(ImageKey, ItemImage)
            If ItemIndex < ListView1.Items.Count Then
                ListView1.Items.Item(ItemIndex).ImageKey = ImageKey
            End If
        End If
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedIndex = 0 Then
            ListView1.BackColor = Color.LightGray
            ListView1.ForeColor = Color.Black
        End If
        If ComboBox2.SelectedIndex = 1 Then
            ListView1.BackColor = Color.Black
            ListView1.ForeColor = Color.White
        End If
        If ComboBox2.SelectedIndex = 2 Then
            ListView1.BackColor = Color.White
            ListView1.ForeColor = Color.Black
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If IsLoadingImages = True Then
            Try
                ThreadLoadListviewImages.Abort()
            Catch
            End Try
        End If
        ListView1.Items.Clear()
        tmpchecklistview.Items.Clear()
        If ComboBox1.SelectedIndex <> -1 Then
            'Loading folders into listview
            Dim addedfolders As New List(Of String)
            For Each folder As String In ComboBox1.Items
                If folder.ToLower.StartsWith(ComboBox1.SelectedItem.ToString.ToLower) Then
                    folder = folder.Remove(0, ComboBox1.SelectedItem.ToString.Length)
                    If folder.StartsWith("\") Then
                        folder = folder.Trim("\").Split("\").First
                        If Not addedfolders.Contains(folder.ToLower) Then
                            addedfolders.Add(folder.ToLower)
                            ListView1.Items.Add(System.IO.Path.GetFileName(folder), 0)
                            tmpchecklistview.Items.Add(System.IO.Path.GetFileName(folder), 0)
                        End If
                    End If
                End If
            Next
            'Load filenames into listview
            Dim addeditems As New List(Of String)
            For Each file As String In Textures
                Dim folder As String = StrReverse(StrReverse(file).Remove(0, IO.Path.GetFileName(file).Length)).Trim("\").Trim
                file = System.IO.Path.GetFileName(file)
                If folder.ToLower = ComboBox1.SelectedItem.ToString.ToLower And Not addeditems.Contains(file.ToLower) Then
                    ListView1.Items.Add(System.IO.Path.GetFileName(file), 1)
                    tmpchecklistview.Items.Add(System.IO.Path.GetFileName(file), 1)
                    addeditems.Add(file.ToLower)
                End If
            Next
        End If
        'Start loading textures
        CurrentFolder = ComboBox1.SelectedItem
        Dim fldrim As Image = ImageList1.Images(0)
        Dim fileim As Image = ImageList1.Images(1)
        ImageList1.Images.Clear()
        ImageList1.Images.Add("folder", fldrim)
        ImageList1.Images.Add("file", fileim)
        ThreadLoadListviewImages = New Thread(AddressOf LoadListviewImages)
        ThreadLoadListviewImages.IsBackground = True
        ThreadLoadListviewImages.Start()
    End Sub

    Private Sub TextureBrowser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dmode = 0
        If Me.Text = "full" Then dmode = 1
        If Me.Text = "empty" Then dmode = 2
        Me.Text = "Texture Browser"
        If ComboBox2.SelectedIndex = -1 Then ComboBox2.SelectedIndex = 0
        Textures.Clear()
        Dim checktextures As New List(Of String)
        'Loading files from "textures" folder
        If System.IO.Directory.Exists(TexturesPath) Then
            For Each file As String In ListFolderFiles(TexturesPath, "*.dds|*.tga")
                file = file.Replace("/", "\")
                If file.ToLower.StartsWith(TexturesPath.ToLower) Then file = file.Remove(0, TexturesPath.Length).Trim("\")
                Textures.Add(file)
                checktextures.Add(file.ToLower)
            Next
        End If
        'Loading files from library
        For Each imgp As ImagePointer In LibraryTextures
            If Not checktextures.Contains(imgp.Path.ToLower) Then Textures.Add(imgp.Path)
        Next
        'Sort the names
        Textures.Sort()
        Dim checkfolders As New List(Of String)
        ComboBox1.Items.Clear()
        'Generate the texture folders
        For Each file As String In Textures
            Dim folder As String = StrReverse(StrReverse(file).Remove(0, IO.Path.GetFileName(file).Length)).Trim("\").Trim
            Dim tmppart As String = ""
            For Each subfolder As String In folder.Split("\")
                tmppart &= subfolder & "\"
                If Not checkfolders.Contains(tmppart.Trim("\").ToLower) Then
                    checkfolders.Add(tmppart.Trim("\").ToLower)
                    ComboBox1.Items.Add(tmppart.Trim("\"))
                End If
            Next
        Next
        If ComboBox1.Items.Count <> 0 Then
            'Setting current file
            Dim currentpath As String = ""
            With Nodes(CurrentIndex)
                If .Type = "Picture Node" Then currentpath = .PictureNodeData.TexturePath.Trim
                If .Type = "Compass Node" Then currentpath = .CompassNodeData.TexturePath.Trim
                If .Type = "Bar Node" And dmode = 1 Then currentpath = .BarNodeData.FullTexturePath.Trim
                If .Type = "Bar Node" And dmode = 2 Then currentpath = .BarNodeData.EmptyTexturePath.Trim
                If .Type = "Button Node" And dmode = 1 Then currentpath = .ButtonNodeData.OnTexturePath
                If .Type = "Button Node" And dmode = 2 Then currentpath = .ButtonNodeData.OffTexturePath
            End With
            If currentpath <> "" Then
                Dim filefolder As String = StrReverse(StrReverse(currentpath).Remove(0, IO.Path.GetFileName(currentpath).Length).Trim("\"))
                SetCBSelectedItem(ComboBox1, filefolder)
                For i As Integer = 0 To ListView1.Items.Count - 1
                    Dim name As String = ListView1.Items.Item(i).Text.ToLower.Trim
                    If IO.Path.GetFileName(currentpath).ToLower.Trim = name Then
                        ListView1.Items(i).EnsureVisible()
                        ListView1.Items(i).Selected = True
                    End If
                Next
            Else
                ComboBox1.SelectedIndex = 0
            End If
            'Setting the loaded state
            If dmode = 0 Then Form1.TextureButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
            If dmode = 1 Then Form1.FTextureButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
            If dmode = 2 Then Form1.ETextureButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
            ViewedDialog = 1
        Else
            'Close because no textures were found
            MsgBox("Failed to find any textures to display.", MsgBoxStyle.Critical)
            Me.Close()
        End If
    End Sub
    Private Sub ListView1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDoubleClick
        If ListView1.SelectedItems.Count <> 0 Then
            If ListView1.SelectedItems(0).ImageIndex = 0 Then
                'Open folder
                ComboBox1.SelectedItem = ComboBox1.SelectedItem & "\" & ListView1.SelectedItems(0).Text
            Else
                'Click OK button to use file
                OK_Button.PerformClick()
            End If
        End If
    End Sub
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        'Check if file can be used
        If ListView1.SelectedItems.Count <> 0 Then
            OK_Button.Enabled = ListView1.SelectedItems(0).ImageIndex <> 0
        Else
            OK_Button.Enabled = False
        End If
    End Sub
    Private Sub TextureBrowser_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        BrowseTimeLine.Clear()
        TimeLineIndex = 0
        ListView1.Items.Clear()
        tmpchecklistview.Items.Clear()
        Textures.Clear()
        ComboBox1.Items.Clear()
        Dim fldrim As Image = ImageList1.Images(0)
        Dim fileim As Image = ImageList1.Images(1)
        ImageList1.Images.Clear()
        ImageList1.Images.Add("folder", fldrim)
        ImageList1.Images.Add("file", fileim)
        If dmode = 0 Then Form1.TextureButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        If dmode = 1 Then Form1.FTextureButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        If dmode = 2 Then Form1.ETextureButton.BackColor = Color.FromKnownColor(KnownColor.Control)
    End Sub
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim filepath As String = ComboBox1.SelectedItem & "\" & ListView1.SelectedItems(0).Text
        Try
            Dim simage As Image
            If System.IO.File.Exists(Application.StartupPath & "\" & TexturesPath & "\" & filepath) Then
                simage = LoadImage(Application.StartupPath & "\" & TexturesPath & "\" & filepath)
            Else
                simage = LoadImage(filepath)
            End If
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                Nodes(CurrentIndex).PictureNodeData.TexturePath = filepath
                Nodes(CurrentIndex).PictureNodeData.TextureImage = simage
                Nodes(CurrentIndex).PictureNodeData.ColorChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                Nodes(CurrentIndex).CompassNodeData.TexturePath = filepath
                Nodes(CurrentIndex).CompassNodeData.TextureImage = simage
                Nodes(CurrentIndex).CompassNodeData.ColorChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Bar Node" Then
                If dmode = 1 Then
                    Nodes(CurrentIndex).BarNodeData.FullTexturePath = filepath
                    Nodes(CurrentIndex).BarNodeData.FullTextureImage = simage
                ElseIf dmode = 2 Then
                    Nodes(CurrentIndex).BarNodeData.EmptyTexturePath = filepath
                    Nodes(CurrentIndex).BarNodeData.EmptyTextureImage = simage
                End If
                Nodes(CurrentIndex).BarNodeData.ColorChanged = True
            ElseIf Nodes(CurrentIndex).Type = "Button Node" Then
                If dmode = 1 Then
                    Nodes(CurrentIndex).ButtonNodeData.OnTexturePath = filepath
                    Nodes(CurrentIndex).ButtonNodeData.OnTextureImage = simage
                ElseIf dmode = 2 Then
                    Nodes(CurrentIndex).ButtonNodeData.OffTexturePath = filepath
                    Nodes(CurrentIndex).ButtonNodeData.OffTextureImage = simage
                End If
                Nodes(CurrentIndex).ButtonNodeData.ColorChanged = True
            End If
            UpdateScreen = True
        Catch
        End Try
        Me.Close()
    End Sub
End Class
