Imports System.Windows.Forms

Public Class ColorDialog
    Dim isloading As Boolean = True
    Private Sub TrackBarA_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarA.Scroll
        NumericUpDownAc.Value = TrackBarA.Value * 0.001
    End Sub
    Private Sub TrackBarR_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarR.Scroll
        NumericUpDownRc.Value = TrackBarR.Value * 0.001
    End Sub
    Private Sub TrackBarG_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarG.Scroll
        NumericUpDownGc.Value = TrackBarG.Value * 0.001
    End Sub
    Private Sub TrackBarB_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarB.Scroll
        NumericUpDownBc.Value = TrackBarB.Value * 0.001
    End Sub
    Private Sub SetColor(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDownAc.ValueChanged, NumericUpDownRc.ValueChanged, NumericUpDownGc.ValueChanged, NumericUpDownBc.ValueChanged
        TrackBarA.Value = NumericUpDownAc.Value * 1000
        TrackBarR.Value = NumericUpDownRc.Value * 1000
        TrackBarG.Value = NumericUpDownGc.Value * 1000
        TrackBarB.Value = NumericUpDownBc.Value * 1000
        Dim ebm As New Bitmap(32, 64)
        Graphics.FromImage(ebm).FillRectangle(New SolidBrush(Color.FromArgb(NumericUpDownAc.Value * 255, NumericUpDownRc.Value * 255, NumericUpDownGc.Value * 255, NumericUpDownBc.Value * 255)), New Rectangle(0, 0, 32, 64))
        PictureBox2.Image = ebm
        If isloading = False Then
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                Nodes(CurrentIndex).PictureNodeData.Color = Color.FromArgb(NumericUpDownAc.Value * 255, NumericUpDownRc.Value * 255, NumericUpDownGc.Value * 255, NumericUpDownBc.Value * 255)
                Nodes(CurrentIndex).PictureNodeData.ColorChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Text Node" Then
                Nodes(CurrentIndex).TextNodeData.Color = Color.FromArgb(NumericUpDownAc.Value * 255, NumericUpDownRc.Value * 255, NumericUpDownGc.Value * 255, NumericUpDownBc.Value * 255)
                Nodes(CurrentIndex).TextNodeData.Modified = True
            End If
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                Nodes(CurrentIndex).CompassNodeData.Color = Color.FromArgb(NumericUpDownAc.Value * 255, NumericUpDownRc.Value * 255, NumericUpDownGc.Value * 255, NumericUpDownBc.Value * 255)
                Nodes(CurrentIndex).CompassNodeData.ColorChanged = True
            End If
            UpdateScreen = True
        End If
    End Sub
    Private Sub ColorDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Form1.ColorButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        If Nodes(CurrentIndex).Type = "Picture Node" Then
            NumericUpDownAc.Value = Nodes(CurrentIndex).PictureNodeData.Color.A / 255
            NumericUpDownRc.Value = Nodes(CurrentIndex).PictureNodeData.Color.R / 255
            NumericUpDownGc.Value = Nodes(CurrentIndex).PictureNodeData.Color.G / 255
            NumericUpDownBc.Value = Nodes(CurrentIndex).PictureNodeData.Color.B / 255
        End If
        If Nodes(CurrentIndex).Type = "Text Node" Then
            NumericUpDownAc.Value = Nodes(CurrentIndex).TextNodeData.Color.A / 255
            NumericUpDownRc.Value = Nodes(CurrentIndex).TextNodeData.Color.R / 255
            NumericUpDownGc.Value = Nodes(CurrentIndex).TextNodeData.Color.G / 255
            NumericUpDownBc.Value = Nodes(CurrentIndex).TextNodeData.Color.B / 255
        End If
        If Nodes(CurrentIndex).Type = "Compass Node" Then
            NumericUpDownAc.Value = Nodes(CurrentIndex).CompassNodeData.Color.A / 255
            NumericUpDownRc.Value = Nodes(CurrentIndex).CompassNodeData.Color.R / 255
            NumericUpDownGc.Value = Nodes(CurrentIndex).CompassNodeData.Color.G / 255
            NumericUpDownBc.Value = Nodes(CurrentIndex).CompassNodeData.Color.B / 255
        End If
        isloading = False
        ViewedDialog = 2
    End Sub
    Private Sub ColorDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.ColorButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        ViewedDialog = 0
        isloading = True
    End Sub
End Class
