
Public Class Progress

    Private Sub Progress_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If IgnoreProgressClose = False Then
            If MessageBox.Show("Abort loading?", "Abort", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.Yes Then
                AbortProcess = True
            Else
                AbortProcess = False
                e.Cancel = True
            End If
        End If
    End Sub

End Class
