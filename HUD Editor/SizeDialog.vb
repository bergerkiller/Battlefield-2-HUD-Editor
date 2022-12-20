

Public Class SizeDialog
    Dim IsLoading As Boolean = True

    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged, NumericUpDown2.ValueChanged
        If IsLoading = False Then
            NodeInformation.Items(CurrentIndex).SubItems(7).Text = NumericUpDown1.Value
            NodeInformation.Items(CurrentIndex).SubItems(8).Text = NumericUpDown2.Value
            ProcessPictureNodeImage(False, True)
        End If
    End Sub
    Private Sub SizeDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        NumericUpDown1.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(7).Text)
        NumericUpDown2.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(8).Text)
        IsLoading = False
    End Sub
    Private Sub SizeDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        IsLoading = True
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        NumericUpDown1.Value = OriginalImage(CurrentIndex).Size.Width
        NumericUpDown2.Value = OriginalImage(CurrentIndex).Size.Height
    End Sub
    Private Sub NumericUpDown3_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown3.ValueChanged
        If IsLoading = False Then
            IsLoading = True
            NumericUpDown1.Value = SetValueBounds(OriginalImage(CurrentIndex).Size.Width * NumericUpDown3.Value * 0.01, 1, 2048)
            IsLoading = False
            NumericUpDown2.Value = SetValueBounds(OriginalImage(CurrentIndex).Size.Height * NumericUpDown3.Value * 0.01, 1, 2048)
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        IsLoading = True
        NumericUpDown1.Value = 800
        IsLoading = False
        NumericUpDown2.Value = 600
    End Sub
End Class
