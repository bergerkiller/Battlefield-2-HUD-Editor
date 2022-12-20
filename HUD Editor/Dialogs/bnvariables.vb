Public Class bnvariables
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.TextUpdate
        If ComboBox1.SelectedItem = "Clear" Then ComboBox1.SelectedIndex = -1
        Nodes(CurrentIndex).BarNodeData.ValueVariable = ComboBox1.Text
    End Sub
    Private Sub cnvariables_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetCBSelectedItem(ComboBox1, Nodes(CurrentIndex).BarNodeData.ValueVariable, False)
        ComboBox1.Text = Nodes(CurrentIndex).BarNodeData.ValueVariable
        Form1.VariablesButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        ViewedDialog = 6
    End Sub
    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        Nodes(CurrentIndex).BarNodeData.Value = TrackBar1.Value * 0.001
        Nodes(CurrentIndex).BarNodeData.ValueChanged = True
        UpdateScreen = True
    End Sub
    Private Sub tnvariables_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.VariablesButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        Nodes(CurrentIndex).BarNodeData.Value = 0.5
        Nodes(CurrentIndex).BarNodeData.ValueChanged = True
        UpdateScreen = True
        ViewedDialog = 0
    End Sub
End Class
