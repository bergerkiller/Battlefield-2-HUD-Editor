
Public Class tnvariables

    Private Sub tnvariables_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If CurrentIndex <> -1 Then
            If Nodes(CurrentIndex).Type = "Text Node" Then
                SetCBSelectedItem(ComboBox1, Nodes(CurrentIndex).TextNodeData.StringVariable, False)
                SetCBSelectedItem(ComboBox2, Nodes(CurrentIndex).TextNodeData.StringText, False)
                ComboBox1.Text = Nodes(CurrentIndex).TextNodeData.StringVariable
                ComboBox2.Text = Nodes(CurrentIndex).TextNodeData.StringText
                Form1.VariablesButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
                RadioButton1.Checked = Nodes(CurrentIndex).TextNodeData.StringVariable = ""
                RadioButton2.Checked = Nodes(CurrentIndex).TextNodeData.StringText = ""
                ViewedDialog = 6
            Else
                Me.Close()
            End If
        Else
            Me.Close()
        End If
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "Clear" Then ComboBox1.SelectedIndex = -1
    End Sub
    Private Sub tnvariables_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If CurrentIndex <> -1 Then
            Nodes(CurrentIndex).TextNodeData.Text = "100"
            If RadioButton1.Checked = True Then Nodes(CurrentIndex).TextNodeData.StringVariable = ""
            If RadioButton2.Checked = True Then Nodes(CurrentIndex).TextNodeData.StringText = ""
            Nodes(CurrentIndex).TextNodeData.Modified = True
            UpdateScreen = True
        End If
        Form1.VariablesButton.BackColor = Color.FromKnownColor(KnownColor.Control)
        ViewedDialog = 0
    End Sub
    Private Sub RadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged
        If CurrentIndex <> -1 Then
            If Nodes(CurrentIndex).Type = "Text Node" Then
                If RadioButton1.Checked = True Then
                    Nodes(CurrentIndex).TextNodeData.Text = GetTextString(ComboBox2.Text)
                    Nodes(CurrentIndex).TextNodeData.Modified = True
                    UpdateScreen = True
                End If
                If RadioButton2.Checked = True Then
                    Nodes(CurrentIndex).TextNodeData.Text = TrackBar1.Value
                    Nodes(CurrentIndex).TextNodeData.Modified = True
                    UpdateScreen = True
                End If
                ComboBox2.Enabled = RadioButton1.Checked
                ComboBox1.Enabled = RadioButton2.Checked
                TrackBar1.Enabled = RadioButton2.Checked
            End If
        End If
    End Sub
    Private Sub ComboBox2_TextUpdate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.TextUpdate, ComboBox2.TextUpdate, TrackBar1.Scroll
        If CurrentIndex <> -1 Then
            If Nodes(CurrentIndex).Type = "Text Node" Then
                If RadioButton1.Checked = True Then
                    Nodes(CurrentIndex).TextNodeData.StringText = ComboBox2.Text
                    Nodes(CurrentIndex).TextNodeData.Text = ComboBox2.Text
                    Nodes(CurrentIndex).TextNodeData.Modified = True
                    UpdateScreen = True
                End If
                If RadioButton2.Checked = True Then
                    Nodes(CurrentIndex).TextNodeData.StringVariable = ComboBox1.Text
                    Nodes(CurrentIndex).TextNodeData.Text = TrackBar1.Value
                    Nodes(CurrentIndex).TextNodeData.Modified = True
                    UpdateScreen = True
                End If
            End If
        End If
    End Sub
End Class
