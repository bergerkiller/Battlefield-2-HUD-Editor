
Public Class CompassStyle
    Dim isloading As Boolean = True

    Private Sub CompassStyle_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Nodes(CurrentIndex).CompassNodeData.Type = 3 Then RadioButton1.Checked = True
        If Nodes(CurrentIndex).CompassNodeData.Type = 0 Then RadioButton2.Checked = True
        NumericUpDown1.Value = Nodes(CurrentIndex).CompassNodeData.Border
        NumericUpDown2.Value = Nodes(CurrentIndex).CompassNodeData.Offset
        Form1.StyleButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        ViewedDialog = 7
        isloading = False
    End Sub
    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged, RadioButton1.CheckedChanged
        If isloading = False Then
            If RadioButton1.Checked = True Then Nodes(CurrentIndex).CompassNodeData.Type = 3
            If RadioButton2.Checked = True Then Nodes(CurrentIndex).CompassNodeData.Type = 0
            Nodes(CurrentIndex).CompassNodeData.ValueChanged = True
            UpdateScreen = True
        End If
    End Sub
    Private Sub CompassStyle_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.StyleButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        ViewedDialog = 0
        isloading = True
    End Sub
    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        If isloading = False Then
            Nodes(CurrentIndex).CompassNodeData.Border = NumericUpDown1.Value
            Nodes(CurrentIndex).CompassNodeData.ValueChanged = True
            UpdateScreen = True
        End If
    End Sub
    Private Sub NumericUpDown2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        If isloading = False Then
            Nodes(CurrentIndex).CompassNodeData.Offset = NumericUpDown2.Value
            Nodes(CurrentIndex).CompassNodeData.ValueChanged = True
            UpdateScreen = True
        End If
    End Sub
End Class
