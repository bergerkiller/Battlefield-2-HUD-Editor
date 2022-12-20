
Public Class BarStyle
    Dim isloading As Boolean = True

    Private Sub BarStyle_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Nodes(CurrentIndex).Type = "Bar Node" Then
            If Nodes(CurrentIndex).BarNodeData.Style = 0 Then RadioButton1.Checked = True
            If Nodes(CurrentIndex).BarNodeData.Style = 1 Then RadioButton2.Checked = True
            If Nodes(CurrentIndex).BarNodeData.Style = 2 Then RadioButton3.Checked = True
            If Nodes(CurrentIndex).BarNodeData.Style = 3 Then RadioButton4.Checked = True
        End If
        ViewedDialog = 7
        isloading = False
        Form1.StyleButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
    End Sub
    Private Sub BarStyle_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.StyleButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        ViewedDialog = 0
        isloading = True
    End Sub
    Private Sub RadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged, RadioButton3.CheckedChanged, RadioButton2.CheckedChanged, RadioButton1.CheckedChanged
        If isloading = False Then
            If Nodes(CurrentIndex).Type = "Bar Node" Then
                If RadioButton1.Checked = True Then Nodes(CurrentIndex).BarNodeData.Style = 0
                If RadioButton2.Checked = True Then Nodes(CurrentIndex).BarNodeData.Style = 1
                If RadioButton3.Checked = True Then Nodes(CurrentIndex).BarNodeData.Style = 2
                If RadioButton4.Checked = True Then Nodes(CurrentIndex).BarNodeData.Style = 3
                Nodes(CurrentIndex).BarNodeData.ValueChanged = True
            End If
            UpdateScreen = True
        End If
    End Sub
End Class
