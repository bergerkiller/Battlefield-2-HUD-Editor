Imports System.Windows.Forms

Public Class ColorDialog
    Dim IsLoading As Boolean = True


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
    Private Sub NumericUpDownAc_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDownAc.ValueChanged
        TrackBarA.Value = NumericUpDownAc.Value * 1000
        Dim ebm As New Bitmap(32, 64)
        Dim g As Graphics = Graphics.FromImage(ebm)
        g.FillRectangle(New SolidBrush(Color.FromArgb(NumericUpDownAc.Value * 255, NumericUpDownRc.Value * 255, NumericUpDownGc.Value * 255, NumericUpDownBc.Value * 255)), New Rectangle(0, 0, 32, 64))
        PictureBox2.Image = ebm
    End Sub
    Private Sub NumericUpDownRc_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDownRc.ValueChanged
        TrackBarR.Value = NumericUpDownRc.Value * 1000
        Dim ebm As New Bitmap(32, 64)
        Dim g As Graphics = Graphics.FromImage(ebm)
        g.FillRectangle(New SolidBrush(Color.FromArgb(NumericUpDownAc.Value * 255, NumericUpDownRc.Value * 255, NumericUpDownGc.Value * 255, NumericUpDownBc.Value * 255)), New Rectangle(0, 0, 32, 64))
        PictureBox2.Image = ebm
    End Sub
    Private Sub NumericUpDownGc_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDownGc.ValueChanged
        TrackBarG.Value = NumericUpDownGc.Value * 1000
        Dim ebm As New Bitmap(32, 64)
        Dim g As Graphics = Graphics.FromImage(ebm)
        g.FillRectangle(New SolidBrush(Color.FromArgb(NumericUpDownAc.Value * 255, NumericUpDownRc.Value * 255, NumericUpDownGc.Value * 255, NumericUpDownBc.Value * 255)), New Rectangle(0, 0, 32, 64))
        PictureBox2.Image = ebm
    End Sub
    Private Sub NumericUpDownBc_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDownBc.ValueChanged
        TrackBarB.Value = NumericUpDownBc.Value * 1000
        Dim ebm As New Bitmap(32, 64)
        Dim g As Graphics = Graphics.FromImage(ebm)
        g.FillRectangle(New SolidBrush(Color.FromArgb(NumericUpDownAc.Value * 255, NumericUpDownRc.Value * 255, NumericUpDownGc.Value * 255, NumericUpDownBc.Value * 255)), New Rectangle(0, 0, 32, 64))
        PictureBox2.Image = ebm
    End Sub
    Private Sub ColorDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If CurrentIndex <> -1 Then
            If NodeType = "Picture Node" Then
                NumericUpDownAc.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(3).Text) * 0.001
                NumericUpDownRc.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(4).Text) * 0.001
                NumericUpDownGc.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(5).Text) * 0.001
                NumericUpDownBc.Value = Val(NodeInformation.Items(CurrentIndex).SubItems(6).Text) * 0.001
            End If
            IsLoading = False
        Else
            Me.Close()
        End If
    End Sub
    Private Sub ColorDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        IsLoading = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Cursor.Current = Cursors.WaitCursor
        If CurrentIndex <> -1 And IsLoading = False Then
            With NodeInformation.Items(CurrentIndex)
                If NodeType = "Picture Node" Then
                    .SubItems(3).Text = TrackBarA.Value
                    .SubItems(4).Text = TrackBarR.Value
                    .SubItems(5).Text = TrackBarG.Value
                    .SubItems(6).Text = TrackBarB.Value
                End If
            End With
            ProcessPictureNodeImage(True, True)
        End If
        Cursor.Current = Cursors.Default
    End Sub
End Class
