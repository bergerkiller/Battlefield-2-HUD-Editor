
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
        If Nodes(CurrentIndex).Type = "Bar Node" Then
            NumericUpDown1.Value = Nodes(CurrentIndex).BarNodeData.Position.X
            NumericUpDown2.Value = Nodes(CurrentIndex).BarNodeData.Position.Y
            NumericUpDown3.Value = Nodes(CurrentIndex).BarNodeData.Size.Width
            NumericUpDown4.Value = Nodes(CurrentIndex).BarNodeData.Size.Height
        End If
        If Nodes(CurrentIndex).Type = "Hover Node" Then
            NumericUpDown1.Value = Nodes(CurrentIndex).HoverNodeData.Position.X
            NumericUpDown2.Value = Nodes(CurrentIndex).HoverNodeData.Position.Y
            NumericUpDown3.Value = Nodes(CurrentIndex).HoverNodeData.Size.Width
            NumericUpDown4.Value = Nodes(CurrentIndex).HoverNodeData.Size.Height
        End If
        If Nodes(CurrentIndex).Type = "Object Marker Node" Then
            NumericUpDown1.Value = Nodes(CurrentIndex).ObjectMarkerNodeData.Position.X
            NumericUpDown2.Value = Nodes(CurrentIndex).ObjectMarkerNodeData.Position.Y
            NumericUpDown3.Value = Nodes(CurrentIndex).ObjectMarkerNodeData.Size.Width
            NumericUpDown4.Value = Nodes(CurrentIndex).ObjectMarkerNodeData.Size.Height
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
        ElseIf Nodes(CurrentIndex).Type = "Bar Node" Then
            NumericUpDown3.Value = Nodes(CurrentIndex).BarNodeData.FullTextureImage.Width
            NumericUpDown4.Value = Nodes(CurrentIndex).BarNodeData.FullTextureImage.Height
        Else
            NumericUpDown3.Value = (400 - NumericUpDown1.Value) * 2
            NumericUpDown4.Value = (300 - NumericUpDown2.Value) * 2
        End If
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        NumericUpDown1.Value = 0
        NumericUpDown2.Value = 0
        NumericUpDown3.Value = 800
        NumericUpDown4.Value = 600
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        NumericUpDown1.Value = 400 - (NumericUpDown3.Value * 0.5)
        NumericUpDown2.Value = 300 - (NumericUpDown4.Value * 0.5)
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
            If Nodes(CurrentIndex).Type = "Bar Node" Then
                Nodes(CurrentIndex).BarNodeData.Position.X = NumericUpDown1.Value
                Nodes(CurrentIndex).BarNodeData.Position.Y = NumericUpDown2.Value
                Nodes(CurrentIndex).BarNodeData.ValueChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Hover Node" Then
                Nodes(CurrentIndex).HoverNodeData.Position.X = NumericUpDown1.Value
                Nodes(CurrentIndex).HoverNodeData.Position.Y = NumericUpDown2.Value
            End If
            If Nodes(CurrentIndex).Type = "Object Marker Node" Then
                Nodes(CurrentIndex).ObjectMarkerNodeData.Position.X = NumericUpDown1.Value
                Nodes(CurrentIndex).ObjectMarkerNodeData.Position.Y = NumericUpDown2.Value
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
            If Nodes(CurrentIndex).Type = "Bar Node" Then
                Nodes(CurrentIndex).BarNodeData.Size.Width = NumericUpDown3.Value
                Nodes(CurrentIndex).BarNodeData.Size.Height = NumericUpDown4.Value
                Nodes(CurrentIndex).BarNodeData.SizeChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Hover Node" Then
                Nodes(CurrentIndex).HoverNodeData.Size.Width = NumericUpDown3.Value
                Nodes(CurrentIndex).HoverNodeData.Size.Height = NumericUpDown4.Value
            End If
            If Nodes(CurrentIndex).Type = "Object Marker Node" Then
                Nodes(CurrentIndex).ObjectMarkerNodeData.Size.Width = NumericUpDown3.Value
                Nodes(CurrentIndex).ObjectMarkerNodeData.Size.Height = NumericUpDown4.Value
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
            If Nodes(CurrentIndex).Type = "Bar Node" Then
                s = Nodes(CurrentIndex).BarNodeData.FullTextureImage.Size
            End If
            isloading = True
            NumericUpDown3.Value = SetValueBounds(s.Width * NumericUpDown5.Value * 0.01, 1, 2048)
            isloading = False
            NumericUpDown4.Value = SetValueBounds(s.Height * NumericUpDown5.Value * 0.01, 1, 2048)
        End If
    End Sub
    Private Sub TextBox1_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        Try
            Dim oldname As String = Nodes(CurrentIndex).Name
            Dim newname As String = TextBox1.Text.Trim.Replace(" ", "_")
            TextBox1.Text = oldname
            If Val(newname) = 0 And newname.Trim <> "" And newname.ToLower <> oldname.ToLower Then
                Dim aexist As Boolean = False
                For i As Integer = 1 To Nodes.Count - 1
                    If Nodes(i).Name.Trim.Replace(" ", "_").ToLower = newname.ToLower Then aexist = True : Exit For
                Next
                If aexist = True Then
                    MsgBox("Can't rename " & Chr(34) & oldname & Chr(34) & " to " & Chr(34) & newname & Chr(34) & " because a node already exists with this name.", MsgBoxStyle.Information)
                Else
                    Nodes(CurrentIndex).Name = newname
                    WriteLog("Name of node " & oldname & " (" & CurrentIndex & ") changed to " & newname)
                    Form1.Text = "HUD Editor - " & newname & " (" & Nodes(CurrentIndex).Type & ")"
                    For i As Integer = 1 To Nodes.Count - 1
                        If Nodes(i).Parent = oldname Then Nodes(i).Parent = newname
                    Next
                    TextBox1.Text = newname
                    If Not IsNothing(Form1.TreeView1.SelectedNode) Then Form1.TreeView1.SelectedNode.Text = newname
                End If
            End If
        Catch
        End Try
    End Sub

End Class
