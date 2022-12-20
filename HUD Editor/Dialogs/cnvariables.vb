
Public Class cnvariables
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.TextUpdate
        If ComboBox1.SelectedItem = "Clear" Then ComboBox1.SelectedIndex = -1
        Nodes(CurrentIndex).CompassNodeData.ValueVariable = ComboBox1.Text
    End Sub
    Private Sub cnvariables_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetCBSelectedItem(ComboBox1, Nodes(CurrentIndex).CompassNodeData.ValueVariable, False)
        ComboBox1.Text = Nodes(CurrentIndex).CompassNodeData.ValueVariable
        Form1.VariablesButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        ViewedDialog = 6
    End Sub
    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        If TrackBar1.Value > 500 Then
            Nodes(CurrentIndex).CompassNodeData.Value = 1.5 - TrackBar1.Value / 1000
        Else
            Nodes(CurrentIndex).CompassNodeData.Value = 0.5 - TrackBar1.Value / 1000
        End If
        Nodes(CurrentIndex).CompassNodeData.ValueChanged = True
        UpdateScreen = True
    End Sub
    Private Sub tnvariables_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.VariablesButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        Nodes(CurrentIndex).CompassNodeData.Value = 0
        Nodes(CurrentIndex).CompassNodeData.ValueChanged = True
        UpdateScreen = True
        ViewedDialog = 0
    End Sub
End Class
