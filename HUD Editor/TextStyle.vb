
Public Class TextStyle
    Dim isloading As Boolean = True

    Private Sub TextStyle_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Nodes(CurrentIndex).Type = "Text Node" Then
            If Nodes(CurrentIndex).TextNodeData.Style = 0 Then RBStyle0.Checked = True
            If Nodes(CurrentIndex).TextNodeData.Style = 1 Then RBStyle1.Checked = True
            If Nodes(CurrentIndex).TextNodeData.Style = 2 Then RBStyle2.Checked = True
        End If
        ViewedDialog = 7
        isloading = False
        Form1.StyleButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
    End Sub
    Private Sub TextStyle_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.StyleButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        ViewedDialog = 0
        isloading = True
    End Sub
    Private Sub RBStyle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RBStyle2.CheckedChanged, RBStyle1.CheckedChanged, RBStyle0.CheckedChanged
        If isloading = False Then
            If Nodes(CurrentIndex).Type = "Text Node" Then
                If RBStyle0.Checked = True Then Nodes(CurrentIndex).TextNodeData.Style = 0
                If RBStyle1.Checked = True Then Nodes(CurrentIndex).TextNodeData.Style = 1
                If RBStyle2.Checked = True Then Nodes(CurrentIndex).TextNodeData.Style = 2
                Nodes(CurrentIndex).TextNodeData.Modified = True
            End If
            UpdateScreen = True
        End If
    End Sub
End Class
