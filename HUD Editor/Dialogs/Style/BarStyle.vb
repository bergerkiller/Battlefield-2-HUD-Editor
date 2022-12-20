
Public Class BarStyle
    Dim isloading As Boolean = True

    Private Sub BarStyle_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim snodes As List(Of Node) = Form1.MainScreen.SelectedNodes
        If snodes.Count = 1 Then
            If snodes(0).BarNode.Style = 0 Then RadioButton1.Checked = True
            If snodes(0).BarNode.Style = 1 Then RadioButton2.Checked = True
            If snodes(0).BarNode.Style = 2 Then RadioButton3.Checked = True
            If snodes(0).BarNode.Style = 3 Then RadioButton4.Checked = True
            ComboBox1.Items.Clear()
            For Each v As VariableHandler In Variables
                If v.Type = VariableType.VT_Value Then ComboBox1.Items.Add(v.Name)
            Next
            ComboBox1.Text = snodes(0).BarNode.ValueVariable.VariableName
            isloading = False
            Form1.StyleButton.BackColor = Color.Active
        Else
            Me.Close()
        End If
    End Sub
    Private Sub BarStyle_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.StyleButton.BackColor = Color.Control
        isloading = True
    End Sub
    Private Sub RadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged, RadioButton3.CheckedChanged, RadioButton2.CheckedChanged, RadioButton1.CheckedChanged, ComboBox1.TextChanged, ComboBox1.SelectedIndexChanged
        If isloading = False Then
            Dim snodes As List(Of Node) = Form1.MainScreen.SelectedNodes
            If snodes.Count = 1 Then
                If RadioButton1.Checked = True Then snodes(0).BarNode.Style = 0 'below
                If RadioButton2.Checked = True Then snodes(0).BarNode.Style = 1 'above
                If RadioButton3.Checked = True Then snodes(0).BarNode.Style = 2 'right
                If RadioButton4.Checked = True Then snodes(0).BarNode.Style = 3 'left
                snodes(0).BarNode.ValueVariable.VariableName = ComboBox1.Text
                snodes(0).UpdateOnScreen()
            End If
        End If
    End Sub
    Private Sub TrackBar1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.ValueChanged
        Dim snodes As List(Of Node) = Form1.MainScreen.SelectedNodes
        If snodes.Count = 1 Then
            snodes(0).BarNode.ValueVariable.Value = TrackBar1.Value / 1000
            snodes(0).UpdateOnScreen()
        End If
    End Sub
End Class
