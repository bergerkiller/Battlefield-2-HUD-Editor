
Public Class PositionDialog
    Dim IsLoading As Boolean = True

    Private Sub NumericUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown4.ValueChanged, NumericUpDown3.ValueChanged
        Form1.TrackBarXPos.Value = SetValueBounds(NumericUpDown3.Value, 0, 800)
        Form1.TrackBarYpos.Value = SetValueBounds(NumericUpDown4.Value * -1, -600, 0)
        If IsLoading = False Then
            NodeInformation.Items(CurrentIndex).SubItems(9).Text = NumericUpDown3.Value
            NodeInformation.Items(CurrentIndex).SubItems(10).Text = NumericUpDown4.Value
            Edited = True
        End If
    End Sub
    Private Sub PositionDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If NodeType = "Picture Node" Then
            NumericUpDown3.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(9).Text)
            NumericUpDown4.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(10).Text)
        End If
        Form1.TrackBarXPos.Value = SetValueBounds(NumericUpDown3.Value, 0, 800)
        Form1.TrackBarYpos.Value = SetValueBounds(NumericUpDown4.Value * -1, -600, 0)
        IsLoading = False
    End Sub
    Private Sub PositionDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        SetBarsEnabled(False)
        IsLoading = True
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        NumericUpDown3.Value = 400 - (SizedImage(CurrentIndex).Width * 0.5)
        NumericUpDown4.Value = 300 - (SizedImage(CurrentIndex).Height * 0.5)
    End Sub
End Class
