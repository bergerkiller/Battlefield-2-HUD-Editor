
Public Class MainDialog
    Dim isloading As Boolean = True

    Private Sub MainDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Form1.MainButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        If Nodes(CurrentIndex).Type = "Picture Node" Then
            NumericUpDown1.Value = Nodes(CurrentIndex).PictureNodeData.Position.X
            NumericUpDown2.Value = Nodes(CurrentIndex).PictureNodeData.Position.Y
            NumericUpDown3.Value = Nodes(CurrentIndex).PictureNodeData.Size.Width
            NumericUpDown4.Value = Nodes(CurrentIndex).PictureNodeData.Size.Height
        End If
        If Nodes(CurrentIndex).Type = "Text Node" Then
            NumericUpDown1.Value = Nodes(CurrentIndex).TextNodeData.Position.X
            NumericUpDown2.Value = Nodes(CurrentIndex).TextNodeData.Position.Y
            NumericUpDown3.Value = Nodes(CurrentIndex).TextNodeData.Size.Width
            NumericUpDown4.Value = Nodes(CurrentIndex).TextNodeData.Size.Height
        End If
        If Nodes(CurrentIndex).Type = "Compass Node" Then
            NumericUpDown1.Value = Nodes(CurrentIndex).CompassNodeData.Position.X
            NumericUpDown2.Value = Nodes(CurrentIndex).CompassNodeData.Position.Y
            NumericUpDown3.Value = Nodes(CurrentIndex).CompassNodeData.Size.Width
            NumericUpDown4.Value = Nodes(CurrentIndex).CompassNodeData.Size.Height
        End If
        TextBox1.Text = Nodes(CurrentIndex).Name
        isloading = False
    End Sub
    Private Sub MainDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.MainButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        ViewedDialog = 0
        isloading = True
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        NumericUpDown1.Value = 0
        NumericUpDown2.Value = 0
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Nodes(CurrentIndex).Type = "Picture Node" Then
            NumericUpDown3.Value = Nodes(CurrentIndex).PictureNodeData.TextureImage.Size.Width
            NumericUpDown4.Value = Nodes(CurrentIndex).PictureNodeData.TextureImage.Size.Height
        ElseIf Nodes(CurrentIndex).Type = "Compass Node" Then
            If Nodes(CurrentIndex).CompassNodeData.Type = 3 Then
                NumericUpDown3.Value = SetValueBounds(Nodes(CurrentIndex).CompassNodeData.TextureSize.Width - Nodes(CurrentIndex).CompassNodeData.Border - Nodes(CurrentIndex).CompassNodeData.Offset, 1, 2048)
                NumericUpDown4.Value = Nodes(CurrentIndex).CompassNodeData.TextureSize.Height
            ElseIf Nodes(CurrentIndex).CompassNodeData.Type = 0 Then
                NumericUpDown3.Value = Nodes(CurrentIndex).CompassNodeData.TextureSize.Width
                NumericUpDown4.Value = SetValueBounds(Nodes(CurrentIndex).CompassNodeData.TextureSize.Height - Nodes(CurrentIndex).CompassNodeData.Border - Nodes(CurrentIndex).CompassNodeData.Offset, 1, 2048)
            End If
        ElseIf Nodes(CurrentIndex).Type = "Text Node" Then
            NumericUpDown3.Value = 40
            NumericUpDown4.Value = 10
        End If
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        NumericUpDown1.Value = 0
        NumericUpDown2.Value = 0
        NumericUpDown3.Value = 800
        NumericUpDown4.Value = 600
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Nodes(CurrentIndex).Type = "Picture Node" Then
            NumericUpDown1.Value = 400 - (Nodes(CurrentIndex).PictureNodeData.Size.Width * 0.5)
            NumericUpDown2.Value = 300 - (Nodes(CurrentIndex).PictureNodeData.Size.Height * 0.5)
        ElseIf Nodes(CurrentIndex).Type = "Text Node" Then
            NumericUpDown1.Value = 400 - (Nodes(CurrentIndex).TextNodeData.Size.Width * 0.5)
            NumericUpDown2.Value = 300 - (Nodes(CurrentIndex).TextNodeData.Size.Height * 0.5)
        ElseIf Nodes(CurrentIndex).Type = "Compass Node" Then
            NumericUpDown1.Value = 400 - (Nodes(CurrentIndex).CompassNodeData.Size.Width * 0.5)
            NumericUpDown2.Value = 300 - (Nodes(CurrentIndex).CompassNodeData.Size.Height * 0.5)
        End If
    End Sub
    Private Sub Position_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown2.ValueChanged, NumericUpDown1.ValueChanged
        If isloading = False Then
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                Nodes(CurrentIndex).PictureNodeData.Position.X = NumericUpDown1.Value
                Nodes(CurrentIndex).PictureNodeData.Position.Y = NumericUpDown2.Value
                Nodes(CurrentIndex).PictureNodeData.PosRotChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Text Node" Then
                Nodes(CurrentIndex).TextNodeData.Position.X = NumericUpDown1.Value
                Nodes(CurrentIndex).TextNodeData.Position.Y = NumericUpDown2.Value
                Nodes(CurrentIndex).TextNodeData.Modified = True
            End If
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                Nodes(CurrentIndex).CompassNodeData.Position.X = NumericUpDown1.Value
                Nodes(CurrentIndex).CompassNodeData.Position.Y = NumericUpDown2.Value
                Nodes(CurrentIndex).CompassNodeData.ValueChanged = True
            End If
            UpdateScreen = True
        End If
    End Sub
    Private Sub Size_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown4.ValueChanged, NumericUpDown3.ValueChanged
        If isloading = False Then
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                Nodes(CurrentIndex).PictureNodeData.Size.Width = NumericUpDown3.Value
                Nodes(CurrentIndex).PictureNodeData.Size.Height = NumericUpDown4.Value
                Nodes(CurrentIndex).PictureNodeData.SizeChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Text Node" Then
                Nodes(CurrentIndex).TextNodeData.Size.Width = NumericUpDown3.Value
                Nodes(CurrentIndex).TextNodeData.Size.Height = NumericUpDown4.Value
                Nodes(CurrentIndex).TextNodeData.Modified = True
            End If
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                Nodes(CurrentIndex).CompassNodeData.Size.Width = NumericUpDown3.Value
                Nodes(CurrentIndex).CompassNodeData.Size.Height = NumericUpDown4.Value
                Nodes(CurrentIndex).CompassNodeData.ValueChanged = True
            End If
            UpdateScreen = True
        End If
    End Sub
    Private Sub Uniform_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown5.ValueChanged
        If isloading = False Then
            Dim s As Size = New Size(100, 100)
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                s = Nodes(CurrentIndex).PictureNodeData.TextureImage.Size
            End If
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                s = Nodes(CurrentIndex).CompassNodeData.TextureImage.Size
            End If
            If Nodes(CurrentIndex).Type = "Text Node" Then
                s = Nodes(CurrentIndex).TextNodeData.Size
            End If
            isloading = True
            NumericUpDown3.Value = SetValueBounds(s.Width * NumericUpDown5.Value * 0.01, 1, 2048)
            isloading = False
            NumericUpDown4.Value = SetValueBounds(s.Height * NumericUpDown5.Value * 0.01, 1, 2048)
        End If
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If isloading = False Then
            Dim olds As Integer = TextBox1.SelectionStart
            TextBox1.Text = TextBox1.Text.Replace(" ", "_")
            TextBox1.SelectionStart = olds
            Nodes(CurrentIndex).Name = TextBox1.Text
            Form1.Text = "HUD Editor - " & TextBox1.Text & " (" & Nodes(CurrentIndex).Type & ")"
        End If
    End Sub
End Class
