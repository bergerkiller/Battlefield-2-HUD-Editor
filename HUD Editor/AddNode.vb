Public Class AddNode
    Dim existingnames As New ListBox

    Private Sub AddNode_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        existingnames.Items.Clear()
        For Each Node As Node In Nodes
            existingnames.Items.Add(Node.Name.ToLower.Trim)
        Next
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged, ComboBox1.SelectedIndexChanged
        Button1.Enabled = TextBox1.Text <> "" And Not existingnames.Items.Contains(TextBox1.Text.ToLower.Trim) And ComboBox1.SelectedIndex <> -1
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class