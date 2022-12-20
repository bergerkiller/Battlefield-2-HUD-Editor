
Public Class CompassStyle
    Dim isloading As Boolean = True

    Private Sub CompassStyle_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim snodes As List(Of Node) = Form1.MainScreen.SelectedNodes
        If snodes.Count = 1 Then
            If snodes(0).CompassNode.Style = 3 Then RadioButton1.Checked = True
            If snodes(0).CompassNode.Style = 0 Then RadioButton2.Checked = True
            NumericUpDown1.Value = snodes(0).CompassNode.Border.Value
            NumericUpDown2.Value = snodes(0).CompassNode.Offset.Value
            NumericUpDown3.Value = snodes(0).CompassNode.TextureSize.Width
            NumericUpDown4.Value = snodes(0).CompassNode.TextureSize.Height
            ComboBox1.Text = snodes(0).CompassNode.ValueVariable.VariableName
            isloading = False
            Form1.StyleButton.BackColor = Color.Active
            ComboBox1.Items.Clear()
            For Each v As VariableHandler In Variables
                If v.Type = VariableType.VT_Value Or v.Type = VariableType.VT_Angle Then ComboBox1.Items.Add(v.Name)
            Next
        Else
            Me.Close()
        End If
    End Sub
    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged, RadioButton1.CheckedChanged, NumericUpDown1.ValueChanged, NumericUpDown2.ValueChanged, ComboBox1.TextChanged, ComboBox1.SelectedIndexChanged, NumericUpDown3.ValueChanged, NumericUpDown4.ValueChanged
        If isloading = False Then
            Dim snodes As List(Of Node) = Form1.MainScreen.SelectedNodes
            If snodes.Count = 1 Then
                If RadioButton1.Checked = True Then snodes(0).CompassNode.Style = 3
                If RadioButton2.Checked = True Then snodes(0).CompassNode.Style = 0
                snodes(0).CompassNode.Border.Value = NumericUpDown1.Value
                snodes(0).CompassNode.Offset.Value = NumericUpDown2.Value
                snodes(0).CompassNode.ValueVariable.VariableName = ComboBox1.Text
                snodes(0).CompassNode.TextureSize.Value = New Size(NumericUpDown3.Value, NumericUpDown4.Value)
                snodes(0).UpdateOnScreen()
            End If
        End If
    End Sub
    Private Sub CompassStyle_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.StyleButton.BackColor = Color.Control
        isloading = True
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim snodes As List(Of Node) = Form1.MainScreen.SelectedNodes
        If snodes.Count = 1 Then
            NumericUpDown3.Value = snodes(0).CompassNode.Texture.Size.Width
            NumericUpDown4.Value = snodes(0).CompassNode.Texture.Size.Height
        End If
    End Sub
    Private Sub TrackBar1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.ValueChanged
        Dim snodes As List(Of Node) = Form1.MainScreen.SelectedNodes
        If snodes.Count = 1 Then
            With snodes(0).CompassNode.ValueVariable
                If .VariableType = VariableType.VT_Angle Then .Value = TrackBar1.Value
                If .VariableType = VariableType.VT_Value Then .Value = TrackBar1.Value / 360
            End With
            snodes(0).UpdateOnScreen()
        End If
    End Sub
End Class
