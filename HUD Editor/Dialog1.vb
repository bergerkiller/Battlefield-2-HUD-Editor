Imports System.Windows.Forms

Public Class Dialog1


    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        Form1.MainScreen.SelectedNodes(0).CompassNode.Border.Value = NumericUpDown1.Value
        UpdateScreen()
    End Sub

    Private Sub NumericUpDown2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        Variables(0).Value = NumericUpDown2.Value
        UpdateScreen()
    End Sub
End Class
