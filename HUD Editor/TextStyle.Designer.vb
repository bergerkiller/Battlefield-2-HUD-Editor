<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TextStyle
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.RBStyle2 = New System.Windows.Forms.RadioButton
        Me.RBStyle0 = New System.Windows.Forms.RadioButton
        Me.RBStyle1 = New System.Windows.Forms.RadioButton
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(46, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Text alignment"
        '
        'RBStyle2
        '
        Me.RBStyle2.AutoSize = True
        Me.RBStyle2.Location = New System.Drawing.Point(12, 35)
        Me.RBStyle2.Name = "RBStyle2"
        Me.RBStyle2.Size = New System.Drawing.Size(43, 17)
        Me.RBStyle2.TabIndex = 1
        Me.RBStyle2.Text = "Left"
        Me.RBStyle2.UseVisualStyleBackColor = True
        '
        'RBStyle0
        '
        Me.RBStyle0.AutoSize = True
        Me.RBStyle0.Checked = True
        Me.RBStyle0.Location = New System.Drawing.Point(61, 35)
        Me.RBStyle0.Name = "RBStyle0"
        Me.RBStyle0.Size = New System.Drawing.Size(56, 17)
        Me.RBStyle0.TabIndex = 2
        Me.RBStyle0.TabStop = True
        Me.RBStyle0.Text = "Center"
        Me.RBStyle0.UseVisualStyleBackColor = True
        '
        'RBStyle1
        '
        Me.RBStyle1.AutoSize = True
        Me.RBStyle1.Location = New System.Drawing.Point(123, 35)
        Me.RBStyle1.Name = "RBStyle1"
        Me.RBStyle1.Size = New System.Drawing.Size(50, 17)
        Me.RBStyle1.TabIndex = 3
        Me.RBStyle1.Text = "Right"
        Me.RBStyle1.UseVisualStyleBackColor = True
        '
        'TextStyle
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(174, 68)
        Me.Controls.Add(Me.RBStyle1)
        Me.Controls.Add(Me.RBStyle0)
        Me.Controls.Add(Me.RBStyle2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TextStyle"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Style"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RBStyle2 As System.Windows.Forms.RadioButton
    Friend WithEvents RBStyle0 As System.Windows.Forms.RadioButton
    Friend WithEvents RBStyle1 As System.Windows.Forms.RadioButton

End Class
