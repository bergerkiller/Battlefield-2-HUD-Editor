Public Class NodeSelect

    Private Sub NodeSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListView1.Items.Clear()
        For Each Node As Node In Nodes
            Dim item As New ListViewItem
            item.Text = Node.Name
            item.SubItems.Add(Node.Type)
            item.Checked = Node.Render
            ListView1.Items.Add(item)
        Next
        If CurrentIndex >= 0 Then ListView1.Items(CurrentIndex).Selected = True
        SetViewedDialog(0)
    End Sub
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedIndices.Count <> 0 Then
            UpButton.Enabled = ListView1.SelectedIndices(0) <> 0
            DownButton.Enabled = ListView1.SelectedIndices(0) <> ListView1.Items.Count - 1
            DeleteButton.Enabled = True
            LoadNode(ListView1.SelectedIndices(0))
            TextBox1.Enabled = True
            CloneButton.Enabled = True
            TextBox1.Text = ListView1.Items(CurrentIndex).Text
        Else
            TextBox1.Enabled = False
            TextBox1.Text = ""
            UpButton.Enabled = False
            CloneButton.Enabled = False
            DownButton.Enabled = False
            DeleteButton.Enabled = False
            LoadNode(-1)
            CurrentIndex = -1
        End If
        UpdateScreen = True
    End Sub
    Private Sub ListView1_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles ListView1.ItemChecked
        Nodes(e.Item.Index).Render = e.Item.Checked
        UpdateScreen = True
    End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        If ListView1.SelectedItems.Count <> 0 Then
            If MessageBox.Show("Are you sure you want to delete node: " & ListView1.SelectedItems(0).Text & "?", "Delete Node", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                RemoveNode(CurrentIndex)
                ListView1.Items.Remove(ListView1.SelectedItems(0))
                UpdateScreen = True
            End If
        End If
    End Sub
    Private Sub UpButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpButton.Click
        Dim i As Integer = ListView1.SelectedIndices(0)
        Dim node As Node = Nodes(i)
        RemoveNode(i)
        InsertNode(node, i - 1)
        Dim item As ListViewItem = ListView1.Items(i)
        ListView1.Items.RemoveAt(i)
        ListView1.Items.Insert(i - 1, item)
        ListView1.Refresh()
        ListView1.Focus()
        UpdateScreen = True
    End Sub
    Private Sub DownButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownButton.Click
        Dim i As Integer = ListView1.SelectedIndices(0)
        Dim node As Node = Nodes(i)
        RemoveNode(i)
        InsertNode(node, i + 1)
        Dim item As ListViewItem = ListView1.Items(i)
        ListView1.Items.RemoveAt(i)
        ListView1.Items.Insert(i + 1, item)
        ListView1.Focus()
        UpdateScreen = True
    End Sub
    Private Sub ListView1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListView1.KeyDown
        If e.KeyCode = Keys.Delete And DeleteButton.Enabled = True Then DeleteButton.PerformClick()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Enabled = True Then
            Nodes(CurrentIndex).Name = TextBox1.Text
            ListView1.Items(CurrentIndex).Text = TextBox1.Text
            Form1.Text = "HUD Editor - " & TextBox1.Text
        End If
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click
        If AddNode.ShowDialog = Windows.Forms.DialogResult.OK Then
            InsertNode(New Node("parent_name", AddNode.TextBox1.Text, AddNode.ComboBox1.SelectedItem), Nodes.Count)
            Dim item As New ListViewItem
            item.Text = AddNode.TextBox1.Text
            item.SubItems.Add(AddNode.ComboBox1.SelectedItem)
            item.Selected = True
            item.Checked = True
            ListView1.Items.Add(item)
            LoadNode(Nodes.Count - 1)
        End If
    End Sub

    Private Sub CloneButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloneButton.Click
        Dim existingnames As New ListBox
        For Each item As ListViewItem In ListView1.Items
            existingnames.Items.Add(item.Text.ToLower.Trim)
        Next
        Dim cloneindex As Integer = 1
        Do While existingnames.Items.Contains(ListView1.SelectedItems(0).Text.ToLower.Trim & "_clone" & cloneindex)
            cloneindex += 1
        Loop
        Dim finalname As String = ListView1.SelectedItems(0).Text.Trim & "_clone" & cloneindex
        Dim nnode As Node = Nodes(ListView1.SelectedIndices(0))
        nnode.Name = finalname
        InsertNode(nnode, Nodes.Count)
        Dim nitem As New ListViewItem
        nitem.Text = finalname
        nitem.SubItems.Add(ListView1.SelectedItems(0).SubItems(1).Text)
        nitem.Checked = True
        nitem.Selected = False
        ListView1.Items.Add(nitem)
    End Sub
End Class
