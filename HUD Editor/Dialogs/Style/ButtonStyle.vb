Public Class ButtonStyle
    Dim Isloading As Boolean = True

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        NumericUpDown1.Enabled = CheckBox1.Checked
        NumericUpDown2.Enabled = CheckBox1.Checked
        NumericUpDown3.Enabled = CheckBox1.Checked
        NumericUpDown4.Enabled = CheckBox1.Checked
    End Sub
    Private Sub ButtonStyle_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim snodes As List(Of Node) = Form1.MainScreen.SelectedNodes
        If snodes.Count = 1 Then
            NumericUpDown1.Value = SetValueBounds(snodes(0).ButtonNode.MouseArea.X, -2048, 2048)
            NumericUpDown2.Value = SetValueBounds(snodes(0).ButtonNode.MouseArea.Y, -2048, 2048)
            NumericUpDown3.Value = SetValueBounds(snodes(0).ButtonNode.MouseArea.Width, 1, 2048)
            NumericUpDown4.Value = SetValueBounds(snodes(0).ButtonNode.MouseArea.Height, 1, 2048)
            CheckBox1.Checked = snodes(0).ButtonNode.UseMouseArea
            TextBox1.Text = snodes(0).ButtonNode.HoverCommands
            TextBox2.Text = snodes(0).ButtonNode.PressCommands
            Form1.StyleButton.BackColor = Color.Active
            Isloading = False
        Else
            Me.Close()
        End If
    End Sub
    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown4.ValueChanged, NumericUpDown3.ValueChanged, NumericUpDown2.ValueChanged, NumericUpDown1.ValueChanged, CheckBox1.CheckedChanged, TextBox1.TextChanged, TextBox2.TextChanged
        If isloading = False Then
            Dim snodes As List(Of Node) = Form1.MainScreen.SelectedNodes
            If snodes.Count = 1 Then
                snodes(0).ButtonNode.MouseArea = New Rectangle(NumericUpDown1.Value, NumericUpDown2.Value, NumericUpDown3.Value, NumericUpDown4.Value)
                snodes(0).ButtonNode.UseMouseArea = CheckBox1.Checked
                snodes(0).ButtonNode.HoverCommands = TextBox1.Text
                snodes(0).ButtonNode.PressCommands = TextBox2.Text
                snodes(0).UpdateOnScreen()
            End If
        End If
    End Sub
    Private Sub ButtonStyle_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Isloading = True
        Form1.StyleButton.BackColor = Color.Control
    End Sub
End Class
