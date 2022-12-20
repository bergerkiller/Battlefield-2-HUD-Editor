Imports System.Windows.Forms

Public Class RotationDialog
    Dim IsLoading As Boolean = True


    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        If NumericUpDown1.Value = 360 Then NumericUpDown1.Value = 0
        If IsLoading = False Then
            NodeInformation.Items(CurrentIndex).SubItems(11).Text = NumericUpDown1.Value
            Edited = True
        End If
    End Sub

    Private Sub RotationDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If NodeType = "Picture Node" Then
            NumericUpDown1.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(11).Text)
        End If
        IsLoading = False
    End Sub

    Private Sub RotationDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        IsLoading = True
    End Sub
End Class
