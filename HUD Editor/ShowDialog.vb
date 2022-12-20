
Public Class ShowDialog
    Private Sub ShowDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Nodes(CurrentIndex).LogicShowVariables.Items.Clear()
        Dim prevstring As String = "EQUAL GuiIndex "
        Dim usedindices As New List(Of Integer)
        For Each index As String In TextBox1.Text.Split(",")
            index = index.Trim
            Dim guii As Integer = Val(index)
            If guii <> 0 Then
                If Not usedindices.Contains(guii) Then Nodes(CurrentIndex).LogicShowVariables.Items.Add(prevstring & guii)
                usedindices.Add(guii)
                prevstring = "OR GuiIndex "
            End If
        Next
        ViewedDialog = 0
        Form1.ShowButton.BackColor = Color.FromKnownColor(KnownColor.Control)
    End Sub
    Private Sub ShowDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = ""
        For Each var As String In Nodes(CurrentIndex).LogicShowVariables.Items
            var = var.ToLower.Trim
            If var.StartsWith("equal guiindex ") Then
                TextBox1.Text &= ", " & var.Remove(0, 15).Trim
            ElseIf var.StartsWith("or guiindex ") Then
                TextBox1.Text &= ", " & var.Remove(0, 12).Trim
            End If
        Next
        TextBox1.Text = TextBox1.Text.Trim(",").Trim
        ViewedDialog = 8
        Form1.ShowButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
    End Sub
End Class
