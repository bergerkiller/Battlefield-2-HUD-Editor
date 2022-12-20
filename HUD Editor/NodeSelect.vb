Imports System.Windows.Forms

Public Class NodeSelect




    Private Sub NodeSelector_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NodeSelector.SelectedIndexChanged
        With Form1
            .TextureButton.Visible = False
            .ColorButton.Visible = False
            .SizeButton.Visible = False
            .PositionButton.Visible = False
            .VariablesButton.Visible = False
            .RotationButton.Visible = False
            If NodeSelector.SelectedItems.Count <> 0 Then
                UpButton.Enabled = NodeSelector.SelectedIndices(0) <> 0
                DownButton.Enabled = NodeSelector.SelectedIndices(0) <> NodeSelector.Items.Count - 1


                Button2.Enabled = True
                TextBox1.Text = NodeSelector.SelectedItems(0).Text
                TextBox1.Enabled = True
                CurrentIndex = NodeSelector.SelectedIndices(0)
                NodeType = NodeSelector.SelectedItems(0).SubItems(1).Text
                .Text = "HUD Editor - " & NodeSelector.SelectedItems(0).Text
                If NodeType = "Picture Node" Then
                    .TextureButton.Visible = True
                    .ColorButton.Visible = True
                    .SizeButton.Visible = True
                    .PositionButton.Visible = True
                    .VariablesButton.Visible = True
                    .RotationButton.Visible = True
                    'Loading up variables: not needed
                End If
            Else
                UpButton.Enabled = False
                DownButton.Enabled = False
                Button2.Enabled = False
                TextBox1.Text = ""
                TextBox1.Enabled = False
                .Text = "HUD Editor - No Node Selected"
            End If
        End With


    End Sub
    Private Sub NodeSelector_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles NodeSelector.ItemChecked
        NodesToRender.Items.Clear()
        For Each item As ListViewItem In NodeSelector.CheckedItems
            NodesToRender.Items.Add(item.Text.ToLower)
        Next
        Edited = True
    End Sub
    Private Sub NodeSelect_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True
        Me.Visible = False
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If MessageBox.Show("Are you sure you want to delete node: " & NodeSelector.Items(CurrentIndex).Text & "?", "Delete Node", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            If NodeSelector.SelectedItems.Count <> 0 Then
                If CurrentIndex <> -1 And NodeSelector.Items.Count > CurrentIndex Then
                    For i As Integer = CurrentIndex To MaxNodeCount - 1
                        OriginalImage(i) = OriginalImage(i + 1)
                        ColoredImage(i) = ColoredImage(i + 1)
                        SizedImage(i) = SizedImage(i + 1)
                    Next
                    NodeInformation.Items.RemoveAt(CurrentIndex)
                    NodeSelector.Items.RemoveAt(CurrentIndex)
                End If
            End If
            Edited = True
        End If
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If NodeSelector.SelectedItems.Count <> 0 Then
            NodeSelector.SelectedItems(0).Text = TextBox1.Text
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If AddNode.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim NodeName As String = AddNode.TextBox2.Text
            Dim NodeType As String = AddNode.ComboBox1.SelectedItem
            Dim InfoItem As New ListViewItem
            Dim SelectorItem As New ListViewItem
            'Generate default information node
            Try
                If NodeType = "Picture Node" Then
                    OriginalImage(NodeInformation.Items.Count) = New Bitmap(32, 32)
                    ColoredImage(NodeInformation.Items.Count) = New Bitmap(32, 32)
                    SizedImage(NodeInformation.Items.Count) = New Bitmap(32, 32)
                    InfoItem.Text = NodeName '0
                    InfoItem.SubItems.Add(NodeType) '1
                    InfoItem.SubItems.Add("") '2
                    InfoItem.SubItems.Add(1000) '3
                    InfoItem.SubItems.Add(1000) '4
                    InfoItem.SubItems.Add(1000) '5
                    InfoItem.SubItems.Add(1000) '6
                    InfoItem.SubItems.Add(32) '7
                    InfoItem.SubItems.Add(32) '8
                    InfoItem.SubItems.Add(0) '9
                    InfoItem.SubItems.Add(0) '10
                    InfoItem.SubItems.Add(0) '11
                    InfoItem.SubItems.Add(0) '12
                    InfoItem.SubItems.Add(0) '13
                    InfoItem.SubItems.Add("") '14
                    InfoItem.SubItems.Add("") '15
                    InfoItem.SubItems.Add("") '16
                    InfoItem.SubItems.Add("") '17

                    NodeInformation.Items.Add(InfoItem)
                End If
                'Generate visible node
                SelectorItem.Text = NodeName
                SelectorItem.SubItems.Add(NodeType)
                SelectorItem.Checked = True
                NodeSelector.Items.Add(SelectorItem)
            Catch ex As Exception
                Try
                    NodeInformation.Items.Remove(InfoItem)
                    NodeSelector.Items.Remove(SelectorItem)
                Catch
                End Try
                MsgBox("An unknown error occured while generating your new node.", MsgBoxStyle.Critical)
            End Try
            Edited = True
        End If
    End Sub

    Private Sub UpButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpButton.Click
        Cursor.Current = Cursors.WaitCursor
        Dim i As Integer = CurrentIndex

        SwapArrayItems(OriginalImage, i, i - 1)
        SwapArrayItems(ColoredImage, i, i - 1)
        SwapArrayItems(SizedImage, i, i - 1)

        Dim item1 As ListViewItem = NodeInformation.Items(i)
        NodeInformation.Items.RemoveAt(i)
        NodeInformation.Items.Insert(i - 1, item1)
        Dim item2 As ListViewItem = NodeSelector.Items(i)
        NodeSelector.Items.RemoveAt(i)
        NodeSelector.Items.Insert(i - 1, item2)
        Cursor.Current = Cursors.Default
        Edited = True
    End Sub

    Sub SwapArrayItems(ByVal arr As Array, ByVal index1 As Integer, ByVal index2 As Integer)
        Dim temp As Object = arr(index1)
        arr(index1) = arr(index2)
        arr(index2) = temp
    End Sub






    Private Sub DownButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownButton.Click
        Cursor.Current = Cursors.WaitCursor
        Dim i As Integer = CurrentIndex

        SwapArrayItems(OriginalImage, i, i + 1)
        SwapArrayItems(ColoredImage, i, i + 1)
        SwapArrayItems(SizedImage, i, i + 1)

        Dim item1 As ListViewItem = NodeInformation.Items(i)
        NodeInformation.Items.RemoveAt(i)
        NodeInformation.Items.Insert(i + 1, item1)
        Dim item2 As ListViewItem = NodeSelector.Items(i)
        NodeSelector.Items.RemoveAt(i)
        NodeSelector.Items.Insert(i + 1, item2)
        Cursor.Current = Cursors.Default
        Edited = True
    End Sub
End Class
