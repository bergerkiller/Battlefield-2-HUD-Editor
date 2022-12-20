
Public Class TextStyle
    Dim isloading As Boolean = True

    Private Sub TextStyle_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim snodes As List(Of Node) = Form1.MainScreen.SelectedNodes
        If snodes.Count = 1 Then
            If snodes(0).TextNode.Style = 0 Then RBStyle0.Checked = True
            If snodes(0).TextNode.Style = 1 Then RBStyle1.Checked = True
            If snodes(0).TextNode.Style = 2 Then RBStyle2.Checked = True
            ComboBox1.Text = snodes(0).TextNode.StringText
            ComboBox2.Text = snodes(0).TextNode.StringVariable.VariableName
            isloading = False
            ComboBox2.Items.Clear()
            For Each v As VariableHandler In Variables
                If v.Type = VariableType.VT_String Then ComboBox2.Items.Add(v.Name)
            Next
            Form1.StyleButton.BackColor = Color.Active
        Else
            Me.Close()
        End If
    End Sub
    Private Sub TextStyle_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.StyleButton.BackColor = Color.Control
        isloading = True
    End Sub
    Private Sub RBStyle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RBStyle2.CheckedChanged, RBStyle1.CheckedChanged, RBStyle0.CheckedChanged, ComboBox1.TextChanged, ComboBox2.TextChanged, ComboBox1.SelectedIndexChanged, ComboBox2.SelectedIndexChanged
        If isloading = False Then
            Dim snodes As List(Of Node) = Form1.MainScreen.SelectedNodes
            If snodes.Count = 1 Then
                If RBStyle0.Checked = True Then snodes(0).TextNode.Style = 0
                If RBStyle1.Checked = True Then snodes(0).TextNode.Style = 1
                If RBStyle2.Checked = True Then snodes(0).TextNode.Style = 2
                snodes(0).TextNode.StringText = ComboBox1.Text
                snodes(0).TextNode.StringVariable.VariableName = ComboBox2.Text
                ComboBox2.Enabled = ComboBox1.Text.Trim = ""
                snodes(0).UpdateOnScreen()
            End If
        End If
    End Sub
End Class
