Imports System.Windows.Forms

Public Class ColorDialog
    Dim isloading As Boolean = True
    Private Sub TrackBarA_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarA.Scroll
        NumericUpDownAc.Value = TrackBarA.Value / 255
    End Sub
    Private Sub TrackBarR_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarR.Scroll
        NumericUpDownRc.Value = TrackBarR.Value / 255
    End Sub
    Private Sub TrackBarG_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarG.Scroll
        NumericUpDownGc.Value = TrackBarG.Value / 255
    End Sub
    Private Sub TrackBarB_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarB.Scroll
        NumericUpDownBc.Value = TrackBarB.Value / 255
    End Sub
    Private Sub SetColor(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDownAc.ValueChanged, NumericUpDownRc.ValueChanged, NumericUpDownGc.ValueChanged, NumericUpDownBc.ValueChanged
        TrackBarA.Value = NumericUpDownAc.Value * 255
        TrackBarR.Value = NumericUpDownRc.Value * 255
        TrackBarG.Value = NumericUpDownGc.Value * 255
        TrackBarB.Value = NumericUpDownBc.Value * 255
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
            If Nodes(CurrentIndex).Type = "Bar Node" Then
                Nodes(CurrentIndex).BarNodeData.Color = Color.FromArgb(NumericUpDownAc.Value * 255, NumericUpDownRc.Value * 255, NumericUpDownGc.Value * 255, NumericUpDownBc.Value * 255)
                Nodes(CurrentIndex).BarNodeData.ColorChanged = True
            End If
            UpdateScreen = True
        End If
    End Sub
    Private Sub ColorDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Form1.ColorButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        NumericUpDownAc.Value = GetNodeColor(Nodes(CurrentIndex)).A / 255
        NumericUpDownRc.Value = GetNodeColor(Nodes(CurrentIndex)).R / 255
        NumericUpDownGc.Value = GetNodeColor(Nodes(CurrentIndex)).G / 255
        NumericUpDownBc.Value = GetNodeColor(Nodes(CurrentIndex)).B / 255
        isloading = False
        ViewedDialog = 2
    End Sub
    Private Sub ColorDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.ColorButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        ViewedDialog = 0
        isloading = True
    End Sub
    Private Function GetNodeColor(ByVal Node As Node) As Color
        If Node.Type = "Picture Node" Then
            Return Node.PictureNodeData.Color
        End If
        If Node.Type = "Text Node" Then
            Return Node.TextNodeData.Color
        End If
        If Node.Type = "Compass Node" Then
            Return Node.CompassNodeData.Color
        End If
        If Node.Type = "Bar Node" Then
            Return Node.BarNodeData.Color
        End If
        Return Nothing
    End Function
    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        Dim newcustomcolors As New List(Of Integer)
        For i As Integer = 1 To Nodes.Count - 1
            If Not IsNothing(GetNodeColor(Nodes(i))) Then
                Dim cole As Integer = System.Drawing.ColorTranslator.ToOle(GetNodeColor(Nodes(i)))
                If Not newcustomcolors.Contains(cole) And newcustomcolors.Count <> 16 Then newcustomcolors.Add(cole)
            End If
        Next
        Do While newcustomcolors.Count < 16
            newcustomcolors.Add(16777215)
        Loop
        ColorDialog1.CustomColors = newcustomcolors.ToArray
        ColorDialog1.Color = Color.FromArgb(255, NumericUpDownRc.Value * 255, NumericUpDownGc.Value * 255, NumericUpDownBc.Value * 255)
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            NumericUpDownRc.Value = ColorDialog1.Color.R / 255
            NumericUpDownGc.Value = ColorDialog1.Color.G / 255
            NumericUpDownBc.Value = ColorDialog1.Color.B / 255
        End If
    End Sub
End Class
