Imports System.Windows.Forms
Imports System.Threading

Public Class TextureBrowser
    Delegate Sub myListViewImageSelector(ByVal ItemIndex As Integer, ByVal ImageKey As String, ByVal ItemImage As Image)
    Dim ThreadLoadListviewImages As Thread
    Dim resettingpath As Boolean = False
    Dim tmpchecklistview As New ListView
    Dim threadisrunning As Boolean = False
    Dim comboboxitem As String = ""
    Dim threadabort As Boolean = False

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub ImageSelector_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If System.IO.Directory.Exists(ImageSelectorImagePath) Then
            ComboBox1.Items.Clear()
            ComboBox1.Items.Add(IO.Path.GetFileName(ImageSelectorImagePath))
            For Each folder As String In ListFolderSubFolders(ImageSelectorImagePath)
                ComboBox1.Items.Add(folder)
            Next
            For Each folder As String In System.IO.Directory.GetDirectories(ImageSelectorImagePath)
                If PathIsFile(folder) = False Then
                    ListView1.Items.Add(IO.Path.GetFileName(folder), 0)
                End If
            Next
            For Each file As String In System.IO.Directory.GetFiles(ImageSelectorImagePath)
                If PathIsFile(file) = True Then
                    ListView1.Items.Add(IO.Path.GetFileName(file), 1)
                End If
            Next
            ComboBox1.SelectedIndex = 0
            If ComboBox2.SelectedIndex = -1 Then ComboBox2.SelectedIndex = 0

            'Set preset file
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                SelectedPath.Text = Nodes(CurrentIndex).PictureNodeData.TexturePath
            End If
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                SelectedPath.Text = Nodes(CurrentIndex).CompassNodeData.TexturePath
            End If
            If System.IO.File.Exists(SelectedPath.Text) Then
                Dim filename As String = SelectedPath.Text
                Dim filefolder As String = StrReverse(StrReverse(SelectedPath.Text).Remove(0, IO.Path.GetFileName(SelectedPath.Text).Length + 1))
                SetCBSelectedItem(ComboBox1, filefolder)
                For i As Integer = 0 To ListView1.Items.Count - 1
                    Dim name As String = ListView1.Items.Item(i).Text.ToLower.Trim
                    If IO.Path.GetFileName(SelectedPath.Text).ToLower.Trim = name Then ListView1.Items.Item(i).Selected = True
                Next
            End If
            Form1.TextureButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
            ViewedDialog = 1
        Else
            MsgBox("Error: " & ImageSelectorImagePath & " does not exist", MsgBoxStyle.Critical)
            Me.Close()
        End If
    End Sub
    Private Sub SelectedPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectedPath.TextChanged
        OK_Button.Enabled = System.IO.File.Exists(SelectedPath.Text)
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        comboboxitem = ComboBox1.SelectedItem
        If System.IO.Directory.Exists(ComboBox1.SelectedItem) Then
            If resettingpath = False Then
                forwardpaths.Items.Clear()
                backwardpaths.Items.Add(ComboBox1.SelectedItem)
                backwardpaths.SelectedItem = ComboBox1.SelectedItem
                Button1.Enabled = backwardpaths.SelectedIndex <> 0
                Button2.Enabled = forwardpaths.SelectedIndex <> -1
            End If
            ListView1.Items.Clear()
            tmpchecklistview.Items.Clear()
            For Each folder As String In System.IO.Directory.GetDirectories(ComboBox1.SelectedItem)
                If PathIsFile(folder) = False Then
                    ListView1.Items.Add(IO.Path.GetFileName(folder), 0)
                    tmpchecklistview.Items.Add(IO.Path.GetFileName(folder), 0)
                End If
            Next
            For Each file As String In System.IO.Directory.GetFiles(ComboBox1.SelectedItem)
                If PathIsFile(file) = True Then
                    ListView1.Items.Add(IO.Path.GetFileName(file), 1)
                    tmpchecklistview.Items.Add(IO.Path.GetFileName(file), 1)
                End If
            Next
            'Load thread here
            threadabort = True
            Try
                If threadisrunning = True Then ThreadLoadListviewImages.Abort()
            Catch
            End Try
            Dim fldrim As Image = ImageList1.Images(0)
            Dim fileim As Image = ImageList1.Images(1)
            ImageList1.Images.Clear()
            ImageList1.Images.Add("folder", fldrim)
            ImageList1.Images.Add("file", fileim)
            ThreadLoadListviewImages = New Thread(AddressOf LoadListviewImages)
            ThreadLoadListviewImages.IsBackground = True
            ThreadLoadListviewImages.Start()
        ElseIf ComboBox1.SelectedIndex <> -1 Then
            ComboBox1.Items.Remove(ComboBox1.SelectedItem)
            MsgBox("Error: " & ComboBox1.SelectedItem & " does not exist", MsgBoxStyle.Critical)
        End If
    End Sub
    Private Sub ListView1_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        If ListView1.SelectedItems.Count <> 0 Then
            If ListView1.SelectedItems(0).ImageIndex = 0 Then
                ComboBox1.SelectedItem = ComboBox1.SelectedItem & "\" & ListView1.SelectedItems(0).Text
            Else
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
    Private Sub backwardpaths_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles backwardpaths.SelectedIndexChanged
        Button1.Enabled = backwardpaths.SelectedIndex <> 0
        resettingpath = True
        ComboBox1.SelectedItem = backwardpaths.SelectedItem
        resettingpath = False
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Back
        forwardpaths.Items.Add(ComboBox1.SelectedItem)
        forwardpaths.SelectedIndex = forwardpaths.Items.Count - 1
        Button2.Enabled = forwardpaths.SelectedIndex <> -1
        backwardpaths.SelectedIndex -= 1
        For i As Integer = backwardpaths.SelectedIndex + 1 To backwardpaths.Items.Count - 1
            If i < backwardpaths.Items.Count Then backwardpaths.Items.RemoveAt(i)
        Next
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'Next
        resettingpath = True
        ComboBox1.SelectedItem = forwardpaths.SelectedItem
        resettingpath = False
        backwardpaths.Items.Add(ComboBox1.SelectedItem)
        backwardpaths.SelectedItem = ComboBox1.SelectedItem
        Button1.Enabled = backwardpaths.SelectedIndex <> 0
        If forwardpaths.SelectedIndex <> -1 Then forwardpaths.SelectedIndex -= 1
        Button2.Enabled = forwardpaths.SelectedIndex <> -1
        For i As Integer = forwardpaths.SelectedIndex + 1 To forwardpaths.Items.Count - 1
            forwardpaths.Items.RemoveAt(i)
        Next
    End Sub
    Private Sub ImageSelector_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        forwardpaths.Items.Clear()
        backwardpaths.Items.Clear()
        ComboBox1.Items.Clear()
        Dim fldrim As Image = ImageList1.Images(0)
        Dim fileim As Image = ImageList1.Images(1)
        Try
            If threadisrunning = True Then ThreadLoadListviewImages.Abort()
        Catch
        End Try
        ImageList1.Images.Clear()
        ImageList1.Images.Add("folder", fldrim)
        ImageList1.Images.Add("file", fileim)
        ListView1.Items.Clear()
        Form1.TextureButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        'Save it
        If Me.DialogResult = Windows.Forms.DialogResult.OK Then
            Try
                Dim simage As Image = LoadImage(SelectedPath.Text)
                If Nodes(CurrentIndex).Type = "Picture Node" Then
                    Nodes(CurrentIndex).PictureNodeData.TexturePath = SelectedPath.Text
                    Nodes(CurrentIndex).PictureNodeData.TextureImage = simage
                    Nodes(CurrentIndex).PictureNodeData.ColorChanged = True
                End If
                If Nodes(CurrentIndex).Type = "Compass Node" Then
                    Nodes(CurrentIndex).CompassNodeData.TexturePath = SelectedPath.Text
                    Nodes(CurrentIndex).CompassNodeData.TextureImage = simage
                    Nodes(CurrentIndex).CompassNodeData.ColorChanged = True
                End If
                UpdateScreen = True
            Catch
            End Try
        End If
    End Sub
    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If ListView1.SelectedItems.Count <> 0 Then
            If PathIsFile(ListView1.SelectedItems(0).Text) = False Then e.Cancel = True
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub LoadListviewImages()
        threadisrunning = True
        Dim index As Integer = 0
        For Each item As ListViewItem In tmpchecklistview.Items
            If item.ImageIndex = 1 Then
                Dim filepath As String = comboboxitem & "\" & item.Text
                If System.IO.File.Exists(filepath) Then
                    ListViewImageSelector(index, filepath.ToLower, ResizeImage(LoadImage(filepath), 128, 128, False))
                End If
            End If
            index += 1
        Next
        threadisrunning = False
    End Sub
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

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedIndices.Count <> 0 Then
            SelectedPath.Text = ComboBox1.SelectedItem & "\" & ListView1.SelectedItems(0).Text
        End If
    End Sub

    Private Sub ListView1_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.MouseHover
        ListView1.Focus()
    End Sub

End Class
