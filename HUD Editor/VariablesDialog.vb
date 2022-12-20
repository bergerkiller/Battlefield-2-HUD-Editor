Imports System.Threading

Public Class VariablesDialog
    Dim IsLoading As Boolean = True


    Private Sub VariablesDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        IsLoading = True
    End Sub
    Private Sub VariablesDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For Each item As String In ComboBox1.Items
            If NodeInformation.Items(CurrentIndex).SubItems(16).Text.Trim.ToLower = item.ToLower.Trim Then
                ComboBox1.SelectedItem = item
            End If
        Next
        For Each item As String In ComboBox2.Items
            If NodeInformation.Items(CurrentIndex).SubItems(14).Text.Trim.ToLower = item.ToLower.Trim Then
                ComboBox2.SelectedItem = item
            End If
        Next
        For Each item As String In ComboBox3.Items
            If NodeInformation.Items(CurrentIndex).SubItems(15).Text.Trim.ToLower = item.ToLower.Trim Then
                ComboBox3.SelectedItem = item
            End If
        Next
        NumericUpDown1.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(12).Text)
        NumericUpDown2.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(13).Text)
        ComboBox4.Text = NodeInformation.Items(CurrentIndex).SubItems(17).Text
        IsLoading = False
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedItem = "None (clear)" Then ComboBox2.SelectedIndex = -1
        'hor offset var
        If IsLoading = False Then
            NodeInformation.Items(CurrentIndex).SubItems(14).Text = ComboBox2.SelectedItem
        End If
    End Sub
    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.SelectedItem = "None (clear)" Then ComboBox3.SelectedIndex = -1
        'vert offset var
        If IsLoading = False Then
            NodeInformation.Items(CurrentIndex).SubItems(15).Text = ComboBox3.SelectedItem
        End If
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "None (clear)" Then ComboBox1.SelectedIndex = -1
        'rot var
        If IsLoading = False Then
            NodeInformation.Items(CurrentIndex).SubItems(16).Text = ComboBox1.SelectedItem
        End If
    End Sub
    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        If IsLoading = False Then
            NodeInformation.Items(CurrentIndex).SubItems(12).Text = NumericUpDown1.Value
        End If
    End Sub
    Private Sub NumericUpDown2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        If IsLoading = False Then
            NodeInformation.Items(CurrentIndex).SubItems(13).Text = NumericUpDown2.Value
        End If
    End Sub
    Private Sub ComboBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.TextChanged
        If IsLoading = False Then
            NodeInformation.Items(CurrentIndex).SubItems(17).Text = ComboBox4.Text
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        NumericUpDown1.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(9).Text) + Val(NodeInformation.Items(CurrentIndex).SubItems(7).Text) * 0.5
        NumericUpDown2.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(10).Text) + Val(NodeInformation.Items(CurrentIndex).SubItems(8).Text) * 0.5
    End Sub
End Class
