
Public Class PositionDialog
    Dim IsLoading As Boolean = True
    Private Sub NumericUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown4.ValueChanged, NumericUpDown3.ValueChanged
        If IsLoading = False Then
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                Nodes(CurrentIndex).PictureNodeData.Position.X = NumericUpDown3.Value
                Nodes(CurrentIndex).PictureNodeData.Position.Y = NumericUpDown4.Value
                Nodes(CurrentIndex).PictureNodeData.PosRotChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Text Node" Then
                Nodes(CurrentIndex).TextNodeData.Position.X = NumericUpDown3.Value
                Nodes(CurrentIndex).TextNodeData.Position.Y = NumericUpDown4.Value
                Nodes(CurrentIndex).TextNodeData.Modified = True
            End If
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                Nodes(CurrentIndex).CompassNodeData.Position.X = NumericUpDown3.Value
                Nodes(CurrentIndex).CompassNodeData.Position.Y = NumericUpDown4.Value
                Nodes(CurrentIndex).CompassNodeData.ValueChanged = True
            End If
            UpdateScreen = True
        End If
        Form1.TrackBarXPos.Value = SetValueBounds(NumericUpDown3.Value, 0, 800)
        Form1.TrackBarYpos.Value = SetValueBounds(NumericUpDown4.Value * -1, -600, 0)
    End Sub
    Private Sub PositionDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Nodes(CurrentIndex).Type = "Picture Node" Then
            NumericUpDown3.Value = Nodes(CurrentIndex).PictureNodeData.Position.X
            NumericUpDown4.Value = Nodes(CurrentIndex).PictureNodeData.Position.Y
        End If
        If Nodes(CurrentIndex).Type = "Text Node" Then
            NumericUpDown3.Value = Nodes(CurrentIndex).TextNodeData.Position.X
            NumericUpDown4.Value = Nodes(CurrentIndex).TextNodeData.Position.Y
        End If
        If Nodes(CurrentIndex).Type = "Compass Node" Then
            NumericUpDown3.Value = Nodes(CurrentIndex).CompassNodeData.Position.X
            NumericUpDown4.Value = Nodes(CurrentIndex).CompassNodeData.Position.Y
        End If
        Form1.PositionButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        Form1.TrackBarXPos.Enabled = True
        Form1.TrackBarYpos.Enabled = True
        IsLoading = False
        ViewedDialog = 4
    End Sub
    Private Sub PositionDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        IsLoading = True
        Form1.TrackBarXPos.Enabled = False
        Form1.TrackBarYpos.Enabled = False
        Form1.PositionButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        ViewedDialog = 0
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If Nodes(CurrentIndex).Type = "Picture Node" Then
            NumericUpDown3.Value = 400 - (Nodes(CurrentIndex).PictureNodeData.Size.Width * 0.5)
            NumericUpDown4.Value = 300 - (Nodes(CurrentIndex).PictureNodeData.Size.Height * 0.5)
        End If
        If Nodes(CurrentIndex).Type = "Text Node" Then
            NumericUpDown3.Value = 400 - (Nodes(CurrentIndex).TextNodeData.Size.Width * 0.5)
            NumericUpDown4.Value = 300 - (Nodes(CurrentIndex).TextNodeData.Size.Height * 0.5)
        End If
        UpdateScreen = True
    End Sub
End Class
