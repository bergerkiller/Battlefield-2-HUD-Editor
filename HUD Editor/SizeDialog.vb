
Public Class SizeDialog
    Dim isloading As Boolean = True
    Private Sub SizeDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If CurrentIndex <> -1 Then
            Form1.SizeButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
            Button2.Enabled = False
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                NumericUpDown1.Value = Nodes(CurrentIndex).PictureNodeData.Size.Width
                NumericUpDown2.Value = Nodes(CurrentIndex).PictureNodeData.Size.Height
                Button2.Enabled = True
            End If
            If Nodes(CurrentIndex).Type = "Text Node" Then
                NumericUpDown1.Value = Nodes(CurrentIndex).TextNodeData.Size.Width
                NumericUpDown2.Value = Nodes(CurrentIndex).TextNodeData.Size.Height
            End If
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                NumericUpDown1.Value = Nodes(CurrentIndex).CompassNodeData.Size.Width
                NumericUpDown2.Value = Nodes(CurrentIndex).CompassNodeData.Size.Height
                Button2.Enabled = True
            End If
            isloading = False
            ViewedDialog = 3
        Else
            Me.Close()
        End If
    End Sub
    Private Sub SizeDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.SizeButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        isloading = True
        ViewedDialog = 0
    End Sub
    Private Sub NumericUpDown3_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown3.ValueChanged
        If isloading = False Then
            Dim s As Size = New Size(100, 100)
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                s = Nodes(CurrentIndex).PictureNodeData.TextureImage.Size
            End If
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                s = Nodes(CurrentIndex).CompassNodeData.TextureImage.Size
            End If
            isloading = True
            NumericUpDown1.Value = SetValueBounds(s.Width * NumericUpDown3.Value * 0.01, 1, 2048)
            isloading = False
            NumericUpDown2.Value = SetValueBounds(s.Height * NumericUpDown3.Value * 0.01, 1, 2048)
        End If
    End Sub
    Private Sub NumericUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged, NumericUpDown2.ValueChanged
        If isloading = False Then
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                Nodes(CurrentIndex).PictureNodeData.Size.Width = NumericUpDown1.Value
                Nodes(CurrentIndex).PictureNodeData.Size.Height = NumericUpDown2.Value
                Nodes(CurrentIndex).PictureNodeData.SizeChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Text Node" Then
                Nodes(CurrentIndex).TextNodeData.Size.Width = NumericUpDown1.Value
                Nodes(CurrentIndex).TextNodeData.Size.Height = NumericUpDown2.Value
                Nodes(CurrentIndex).TextNodeData.Modified = True
            End If
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                Nodes(CurrentIndex).CompassNodeData.Size.Width = NumericUpDown1.Value
                Nodes(CurrentIndex).CompassNodeData.Size.Height = NumericUpDown2.Value
                Nodes(CurrentIndex).CompassNodeData.ValueChanged = True
            End If
            UpdateScreen = True
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Nodes(CurrentIndex).Type = "Picture Node" Then
            NumericUpDown1.Value = Nodes(CurrentIndex).PictureNodeData.TextureImage.Width
            NumericUpDown2.Value = Nodes(CurrentIndex).PictureNodeData.TextureImage.Height
        ElseIf Nodes(CurrentIndex).Type = "Compass Node" Then
            If Nodes(CurrentIndex).CompassNodeData.Type = 3 Then
                NumericUpDown1.Value = SetValueBounds(Nodes(CurrentIndex).CompassNodeData.TextureSize.Width - Nodes(CurrentIndex).CompassNodeData.Border - Nodes(CurrentIndex).CompassNodeData.Offset, 1, 2048)
                NumericUpDown2.Value = Nodes(CurrentIndex).CompassNodeData.TextureSize.Height
            ElseIf Nodes(CurrentIndex).CompassNodeData.Type = 0 Then
                NumericUpDown1.Value = Nodes(CurrentIndex).CompassNodeData.TextureSize.Width
                NumericUpDown2.Value = SetValueBounds(Nodes(CurrentIndex).CompassNodeData.TextureSize.Height - Nodes(CurrentIndex).CompassNodeData.Border - Nodes(CurrentIndex).CompassNodeData.Offset, 1, 2048)
            End If
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        NumericUpDown1.Value = 800
        NumericUpDown2.Value = 600
    End Sub
End Class
