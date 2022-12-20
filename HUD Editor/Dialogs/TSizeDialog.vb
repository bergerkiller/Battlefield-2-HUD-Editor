
Public Class TSizeDialog
    Dim isloading As Boolean = True

    Private Sub TSizeDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Form1.TSizeButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        If Nodes(CurrentIndex).Type = "Compass Node" Then
            NumericUpDown1.Value = Nodes(CurrentIndex).CompassNodeData.TextureSize.Width
            NumericUpDown2.Value = Nodes(CurrentIndex).CompassNodeData.TextureSize.Height
        End If
        isloading = False
        ViewedDialog = 4
    End Sub
    Private Sub TSizeDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.TSizeButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        isloading = True
        ViewedDialog = 0
    End Sub
    Private Sub NumericUpDown3_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown3.ValueChanged
        If isloading = False Then
            Dim s As Size = New Size(100, 100)
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                s = Nodes(CurrentIndex).CompassNodeData.TextureImage.Size
            End If
            NumericUpDown1.Value = SetValueBounds(s.Width * NumericUpDown3.Value * 0.01, 1, 2048)
            NumericUpDown2.Value = SetValueBounds(s.Height * NumericUpDown3.Value * 0.01, 1, 2048)
        End If
    End Sub
    Private Sub NumericUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged, NumericUpDown2.ValueChanged
        If isloading = False Then
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                Nodes(CurrentIndex).CompassNodeData.TextureSize.Width = NumericUpDown1.Value
                Nodes(CurrentIndex).CompassNodeData.TextureSize.Height = NumericUpDown2.Value
                Nodes(CurrentIndex).CompassNodeData.SizeChanged = True
            End If
            UpdateScreen = True
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Nodes(CurrentIndex).Type = "Picture Node" Then
            NumericUpDown1.Value = Nodes(CurrentIndex).PictureNodeData.TextureImage.Width
            NumericUpDown2.Value = Nodes(CurrentIndex).PictureNodeData.TextureImage.Height
        End If
        If Nodes(CurrentIndex).Type = "Compass Node" Then
            NumericUpDown1.Value = Nodes(CurrentIndex).CompassNodeData.TextureImage.Width
            NumericUpDown2.Value = Nodes(CurrentIndex).CompassNodeData.TextureImage.Height
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        NumericUpDown1.Value = 800
        NumericUpDown2.Value = 600
    End Sub
End Class
