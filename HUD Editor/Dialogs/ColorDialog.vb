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
        Dim c As System.Drawing.Color = System.Drawing.Color.FromArgb(NumericUpDownAc.Value, NumericUpDownRc.Value, NumericUpDownGc.Value, NumericUpDownBc.Value)
        Graphics.FromImage(ebm).FillRectangle(New SolidBrush(c), New Rectangle(0, 0, 32, 64))
        PictureBox2.Image = ebm
        If isloading = False Then
            For Each Node As Node In Form1.MainScreen.SelectedNodes
                Node.Color.SetColor(c)
            Next
            UpdateScreen()
        End If
    End Sub
    Private Sub ColorDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AddHandler Form1.MainScreen.SelectionChanged, AddressOf SelectionChanged
        SelectionChanged()
        Form1.ColorButton.BackColor = Color.Active
        isloading = False
    End Sub
    Private Sub ColorDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.ColorButton.BackColor = Color.Control
        isloading = True
        RemoveHandler Form1.MainScreen.SelectionChanged, AddressOf SelectionChanged
    End Sub
    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        Dim newcustomcolors As New List(Of Integer)
        For Each Node As Node In Form1.MainScreen.Nodes
            Dim cole As Integer = Node.Color.ToOle
            If Not newcustomcolors.Contains(cole) And newcustomcolors.Count <> 16 Then newcustomcolors.Add(cole)
        Next
        Do While newcustomcolors.Count < 16
            newcustomcolors.Add(16777215)
        Loop
        ColorDialog1.CustomColors = newcustomcolors.ToArray
        ColorDialog1.Color = System.Drawing.Color.FromArgb(255, NumericUpDownRc.Value, NumericUpDownGc.Value, NumericUpDownBc.Value)
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            NumericUpDownRc.Value = ColorDialog1.Color.R
            NumericUpDownGc.Value = ColorDialog1.Color.G
            NumericUpDownBc.Value = ColorDialog1.Color.B
        End If
    End Sub

    Private Sub SelectionChanged()
        Dim n() As Node = Form1.MainScreen.SelectedNodes.ToArray
        isloading = True
        'If n.Count = 1 Then
        '    ComboBox1.SelectedIndex = 0
        '    If n(0).BlendEffectA = 0 And n(0).BlendEffectB = 0 Then ComboBox1.SelectedIndex = 1
        '    If n(0).BlendEffectA = 0 And n(0).BlendEffectB = 1 Then ComboBox1.SelectedIndex = 2
        '    If n(0).BlendEffectA = 0 And n(0).BlendEffectB = 3 Then ComboBox1.SelectedIndex = 3
        'Else
        '    ComboBox1.SelectedIndex = 0
        'End If
        If n.Count >= 1 Then
            Dim c As Color = n(0).Color
            For Each Node As Node In n
                c.MergeWith(Node.Color)
            Next
            NumericUpDownAc.Value = c.A
            NumericUpDownRc.Value = c.R
            NumericUpDownGc.Value = c.G
            NumericUpDownBc.Value = c.B
        End If
        SetEnabled(n.Count <> 0)
        isloading = False
    End Sub

    Private Sub SetEnabled(ByVal Enabled As Boolean)
        TrackBarA.Enabled = Enabled
        TrackBarR.Enabled = Enabled
        TrackBarG.Enabled = Enabled
        TrackBarB.Enabled = Enabled
        NumericUpDownAc.Enabled = Enabled
        NumericUpDownRc.Enabled = Enabled
        NumericUpDownGc.Enabled = Enabled
        NumericUpDownBc.Enabled = Enabled
        PictureBox2.Enabled = Enabled
        ComboBox1.Enabled = Enabled
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        'If isloading = False Then
        '    For Each Node As Node In Form1.MainScreen.SelectedNodes
        '        If ComboBox1.SelectedIndex = 0 Then
        '            Node.BlendEffectA = -1
        '            Node.BlendEffectB = -1
        '        ElseIf ComboBox1.SelectedIndex = 1 Then
        '            Node.BlendEffectA = 0
        '            Node.BlendEffectB = 0
        '        ElseIf ComboBox1.SelectedIndex = 2 Then
        '            Node.BlendEffectA = 0
        '            Node.BlendEffectB = 1
        '        ElseIf ComboBox1.SelectedIndex = 3 Then
        '            Node.BlendEffectA = 0
        '            Node.BlendEffectB = 3
        '        End If
        '        Node.Changed = True
        '    Next
        '    UpdateScreen()
        'End If
    End Sub
End Class
