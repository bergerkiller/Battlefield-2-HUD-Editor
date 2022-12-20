﻿
Public Class pnvariables

    Private Sub pnvariables_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetCBSelectedItem(ComboBox1, Nodes(CurrentIndex).PictureNodeData.DOffsetXVar)
        SetCBSelectedItem(ComboBox2, Nodes(CurrentIndex).PictureNodeData.DOffsetYVar)
        SetCBSelectedItem(ComboBox3, Nodes(CurrentIndex).PictureNodeData.DRotationVar)
        NumericUpDown1.Value = Nodes(CurrentIndex).PictureNodeData.DRotationMid.X
        NumericUpDown2.Value = Nodes(CurrentIndex).PictureNodeData.DRotationMid.Y
        Form1.VariablesButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        ViewedDialog = 6
        UpdateScreen = True
    End Sub
    Private Sub pnvariables_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.VariablesButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        Nodes(CurrentIndex).PictureNodeData.DOffsetX = 0
        Nodes(CurrentIndex).PictureNodeData.DOffsetY = 0
        Nodes(CurrentIndex).PictureNodeData.DRotation = 0
        Nodes(CurrentIndex).PictureNodeData.PosRotChanged = True
        ViewedDialog = 0
        UpdateScreen = True
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "Clear" Then ComboBox1.SelectedIndex = -1
        Nodes(CurrentIndex).PictureNodeData.DOffsetXVar = ComboBox1.SelectedItem
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedItem = "Clear" Then ComboBox2.SelectedIndex = -1
        Nodes(CurrentIndex).PictureNodeData.DOffsetYVar = ComboBox2.SelectedItem
    End Sub
    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.SelectedItem = "Clear" Then ComboBox3.SelectedIndex = -1
        Nodes(CurrentIndex).PictureNodeData.DRotationVar = ComboBox3.SelectedItem
    End Sub
    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        Nodes(CurrentIndex).PictureNodeData.DRotationMid.X = NumericUpDown1.Value
        Nodes(CurrentIndex).PictureNodeData.PosRotChanged = True
        UpdateScreen = True
    End Sub
    Private Sub NumericUpDown2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        Nodes(CurrentIndex).PictureNodeData.DRotationMid.Y = NumericUpDown2.Value
        Nodes(CurrentIndex).PictureNodeData.PosRotChanged = True
        UpdateScreen = True
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        NumericUpDown1.Value = Nodes(CurrentIndex).PictureNodeData.Position.X + Nodes(CurrentIndex).PictureNodeData.Size.Width * 0.5 - 400
        NumericUpDown2.Value = Nodes(CurrentIndex).PictureNodeData.Position.Y + Nodes(CurrentIndex).PictureNodeData.Size.Height * 0.5 - 300
    End Sub
    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        Nodes(CurrentIndex).PictureNodeData.DRotation = TrackBar1.Value
        Nodes(CurrentIndex).PictureNodeData.PosRotChanged = True
        UpdateScreen = True
    End Sub
End Class
