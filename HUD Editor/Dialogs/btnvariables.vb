Public Class btnvariables

    Private Sub btnvariables_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If CurrentIndex <> -1 Then
            If Nodes(CurrentIndex).Type = "Button Node" Then
                TextBox1.Text = Nodes(CurrentIndex).ButtonNodeData.HoverCommands
                TextBox2.Text = Nodes(CurrentIndex).ButtonNodeData.PressCommands
                Form1.VariablesButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
            Else
                Me.Close()
            End If
        Else
            Me.Close()
        End If
    End Sub
    Private Sub btnvariables_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If CurrentIndex <> -1 Then
            If Nodes(CurrentIndex).Type = "Button Node" Then
                Nodes(CurrentIndex).ButtonNodeData.HoverCommands = TextBox1.Text.Trim
                Nodes(CurrentIndex).ButtonNodeData.PressCommands = TextBox2.Text.Trim
            End If
        End If
        Form1.VariablesButton.BackColor = Color.FromKnownColor(KnownColor.Control)
    End Sub
End Class