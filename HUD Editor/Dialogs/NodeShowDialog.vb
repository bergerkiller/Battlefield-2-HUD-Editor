Public Class NodeShowDialog

    Private Sub ShowDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim nodes() As Node = Form1.MainScreen.SelectedNodes.ToArray
        If nodes.Count = 1 Then
            ComboBox1.Text = nodes(0).ShowVariable.VariableName
            TextBox1.Text = nodes(0).LogicShowVariables
            CheckBox1.Checked = nodes(0).AlphaShowEffect
            NumericUpDown1.Value = SetValueBounds(nodes(0).InTime, 0, 100)
            NumericUpDown2.Value = SetValueBounds(nodes(0).OutTime, 0, 100)
            ComboBox1.Items.Clear()
            For Each v As VariableHandler In Variables
                If v.Type = VariableType.VT_Show Then ComboBox1.Items.Add(v.Name)
            Next
        End If
    End Sub
    Private Sub NodeShowDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Dim nodes() As Node = Form1.MainScreen.SelectedNodes.ToArray
        If nodes.Count = 1 Then
            nodes(0).ShowVariable.VariableName = ComboBox1.Text
            nodes(0).LogicShowVariables = TextBox1.Text
            nodes(0).AlphaShowEffect = CheckBox1.Checked
            nodes(0).InTime = NumericUpDown1.Value
            nodes(0).OutTime = NumericUpDown2.Value
        End If
    End Sub
End Class