Imports System.Windows.Forms

Public Class ColorDialog
    Dim isloading As Boolean = True
    Private Sub TrackBarA_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarA.Scroll
        NumericUpDownAc.Value = TrackBarA.Value
    End Sub
    Private Sub TrackBarR_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarR.Scroll
        NumericUpDownRc.Value = TrackBarR.Value
    End Sub
    Private Sub TrackBarG_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarG.Scroll
        NumericUpDownGc.Value = TrackBarG.Value
    End Sub
    Private Sub TrackBarB_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarB.Scroll
        NumericUpDownBc.Value = TrackBarB.Value
    End Sub
    Private Sub SetColor(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDownAc.ValueChanged, NumericUpDownRc.ValueChanged, NumericUpDownGc.ValueChanged, NumericUpDownBc.ValueChanged
        TrackBarA.Value = NumericUpDownAc.Value
        TrackBarR.Value = NumericUpDownRc.Value
        TrackBarG.Value = NumericUpDownGc.Value
        TrackBarB.Value = NumericUpDownBc.Value
        Dim ebm As New Bitmap(32, 64)
        Graphics.FromImage(ebm).FillRectangle(New SolidBrush(Color.FromArgb(NumericUpDownAc.Value, NumericUpDownRc.Value, NumericUpDownGc.Value, NumericUpDownBc.Value)), New Rectangle(0, 0, 32, 64))
        PictureBox2.Image = ebm
        If isloading = False Then
            If Nodes(CurrentIndex).Type = "Picture Node" Then
                Nodes(CurrentIndex).PictureNodeData.Color = Color.FromArgb(NumericUpDownAc.Value, NumericUpDownRc.Value, NumericUpDownGc.Value, NumericUpDownBc.Value)
                Nodes(CurrentIndex).PictureNodeData.ColorChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Text Node" Then
                Nodes(CurrentIndex).TextNodeData.Color = Color.FromArgb(NumericUpDownAc.Value, NumericUpDownRc.Value, NumericUpDownGc.Value, NumericUpDownBc.Value)
                Nodes(CurrentIndex).TextNodeData.Modified = True
            End If
            If Nodes(CurrentIndex).Type = "Compass Node" Then
                Nodes(CurrentIndex).CompassNodeData.Color = Color.FromArgb(NumericUpDownAc.Value, NumericUpDownRc.Value, NumericUpDownGc.Value, NumericUpDownBc.Value)
                Nodes(CurrentIndex).CompassNodeData.ColorChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Bar Node" Then
                Nodes(CurrentIndex).BarNodeData.Color = Color.FromArgb(NumericUpDownAc.Value, NumericUpDownRc.Value, NumericUpDownGc.Value, NumericUpDownBc.Value)
                Nodes(CurrentIndex).BarNodeData.ColorChanged = True
            End If
            If Nodes(CurrentIndex).Type = "Button Node" Then
                Nodes(CurrentIndex).ButtonNodeData.Color = Color.FromArgb(NumericUpDownAc.Value, NumericUpDownRc.Value, NumericUpDownGc.Value, NumericUpDownBc.Value)
                Nodes(CurrentIndex).ButtonNodeData.ColorChanged = True
            End If
            UpdateScreen = True
        End If
    End Sub
    Private Sub ColorDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Form1.ColorButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        NumericUpDownAc.Value = Nodes(CurrentIndex).GetValue(Node.ValueType.Color).A
        NumericUpDownRc.Value = Nodes(CurrentIndex).GetValue(Node.ValueType.Color).R
        NumericUpDownGc.Value = Nodes(CurrentIndex).GetValue(Node.ValueType.Color).G
        NumericUpDownBc.Value = Nodes(CurrentIndex).GetValue(Node.ValueType.Color).B
        isloading = False
        ViewedDialog = 2
    End Sub
    Private Sub ColorDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.ColorButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        ViewedDialog = 0
        isloading = True
    End Sub
    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        Dim newcustomcolors As New List(Of Integer)
        For i As Integer = 1 To Nodes.Count - 1
            If Not IsNothing(Nodes(i).GetValue(Node.ValueType.Color)) Then
                Dim cole As Integer = System.Drawing.ColorTranslator.ToOle(Nodes(i).GetValue(Node.ValueType.Color))
                If Not newcustomcolors.Contains(cole) And newcustomcolors.Count <> 16 Then newcustomcolors.Add(cole)
            End If
        Next
        Do While newcustomcolors.Count < 16
            newcustomcolors.Add(16777215)
        Loop
        ColorDialog1.CustomColors = newcustomcolors.ToArray
        ColorDialog1.Color = Color.FromArgb(255, NumericUpDownRc.Value, NumericUpDownGc.Value, NumericUpDownBc.Value)
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            NumericUpDownRc.Value = ColorDialog1.Color.R
            NumericUpDownGc.Value = ColorDialog1.Color.G
            NumericUpDownBc.Value = ColorDialog1.Color.B
        End If
    End Sub
End Class
