
Public Class ReplaceDiag

    Private Sub CheckBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.TextChanged
        Dim amount As String = CheckBox1.Text.Split(" ").Last
        amount = amount.Trim("(").Trim(")").Trim
        If Val(amount) = 0 Then
            Me.Size = New Size(426, 250)
        Else
            Me.Size = New Size(426, 276)
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If CheckBox1.Checked = False Then Me.DialogResult = 0 Else Me.DialogResult = 3
        Me.Close()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If CheckBox1.Checked = False Then Me.DialogResult = 1 Else Me.DialogResult = 4
        Me.Close()
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If CheckBox1.Checked = False Then Me.DialogResult = 2 Else Me.DialogResult = 5
        Me.Close()
    End Sub
    Private Sub ReplaceDiag_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CheckBox1.Checked = False
        Me.DialogResult = 0
    End Sub
End Class
