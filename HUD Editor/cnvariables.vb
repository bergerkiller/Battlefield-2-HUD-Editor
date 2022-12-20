
Public Class cnvariables
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "Clear" Then ComboBox1.SelectedIndex = -1
        Nodes(CurrentIndex).CompassNodeData.ValueVariable = ComboBox1.SelectedItem
    End Sub
    Private Sub cnvariables_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetCBSelectedItem(ComboBox1, Nodes(CurrentIndex).CompassNodeData.ValueVariable)
        Form1.VariablesButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        ViewedDialog = 6
    End Sub
    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        Nodes(CurrentIndex).CompassNodeData.Value = TrackBar1.Value * 0.001
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
