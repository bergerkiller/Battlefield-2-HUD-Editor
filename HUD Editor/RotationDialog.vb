
Public Class RotationDialog
    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        If NumericUpDown1.Value = -1 Then
            NumericUpDown1.Value = 359
        ElseIf NumericUpDown1.Value = 360 Then
            NumericUpDown1.Value = 0
        ElseIf Nodes(CurrentIndex).Type = "Picture Node" Then
            Nodes(CurrentIndex).PictureNodeData.StaticRotation = NumericUpDown1.Value
            Nodes(CurrentIndex).PictureNodeData.PosRotChanged = True
            UpdateScreen = True
        End If
    End Sub
    Private Sub RotationDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Nodes(CurrentIndex).Type = "Picture Node" Then
            NumericUpDown1.Value = Nodes(CurrentIndex).PictureNodeData.StaticRotation
        End If
        Form1.RotationButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        ViewedDialog = 5
        UpdateScreen = True
    End Sub
    Private Sub RotationDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.RotationButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        ViewedDialog = 0
        UpdateScreen = True
    End Sub
End Class
