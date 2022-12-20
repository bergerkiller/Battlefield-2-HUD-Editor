
Public Class RotationDialog
    Dim isloading As Boolean = True
    Dim selnode As Node
    Private Sub UpdateState()
        Dim nodes() As Node = Form1.MainScreen.SelectedNodes.ToArray
        isloading = True
        If nodes.Count = 1 AndAlso nodes(0).Type = "Picture Node" Then
            selnode = nodes(0)
            NumericUpDown1.Value = SetValueBounds(selnode.PictureNode.Rotation.Value, 0, 360)
            NumericUpDown2.Value = SetValueBounds(selnode.PictureNode.CenterPoint.X, -1000, 1000)
            NumericUpDown3.Value = SetValueBounds(selnode.PictureNode.CenterPoint.Y, -1000, 1000)
            ComboBox1.Text = selnode.PictureNode.RotateVariable.VariableName
        End If
        isloading = False
    End Sub

    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged, NumericUpDown2.ValueChanged, NumericUpDown3.ValueChanged, ComboBox1.TextChanged, ComboBox1.SelectedIndexChanged
        If isloading = False Then
            If NumericUpDown1.Value = -1 Then
                NumericUpDown1.Value = 359
            ElseIf NumericUpDown1.Value = 360 Then
                NumericUpDown1.Value = 0
            Else
                selnode.PictureNode.Rotation.Value = NumericUpDown1.Value
                selnode.PictureNode.CenterPoint.X = NumericUpDown2.Value
                selnode.PictureNode.CenterPoint.Y = NumericUpDown3.Value
                selnode.PictureNode.RotateVariable.VariableName = ComboBox1.Text
                selnode.UpdateOnScreen(True)
                Form1.MainScreen.UpdateSelectionSquare()
                Form1.MainScreen.SelectionSquare.UpdateOnScreen()
            End If
        End If
    End Sub
    Private Sub RotationDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UpdateState()
        AddHandler Form1.MainScreen.SelectionChanged, AddressOf UpdateState
        AddHandler Form1.MainScreen.SelectionModified, AddressOf UpdateState
        Form1.RotationButton.BackColor = Color.Active
        ComboBox1.Items.Clear()
        For Each v As VariableHandler In Variables
            If v.Type = VariableType.VT_Angle Then ComboBox1.Items.Add(v.Name)
        Next
    End Sub
    Private Sub RotationDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.RotationButton.BackColor = Color.Control
        selnode.PictureNode.RotateVariable.Reset()
        isloading = True
    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        selnode.PictureNode.RotateVariable.Value = TrackBar1.Value
        selnode.UpdateOnScreen(True)
        Form1.MainScreen.UpdateSelectionSquare()
        Form1.MainScreen.SelectionSquare.UpdateOnScreen()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        NumericUpDown2.Value = selnode.PictureNode.Location.X + selnode.PictureNode.Size.Width * 0.5 - 400
        NumericUpDown3.Value = selnode.PictureNode.Location.Y + selnode.PictureNode.Size.Height * 0.5 - 300
    End Sub
End Class
