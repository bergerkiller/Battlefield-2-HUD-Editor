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
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Text alignment"
        '
        'RBStyle2
        '
        Me.RBStyle2.AutoSize = True
        Me.RBStyle2.Location = New System.Drawing.Point(94, 7)
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
        Me.RBStyle0.Location = New System.Drawing.Point(143, 7)
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
        Me.RBStyle1.Location = New System.Drawing.Point(205, 7)
        Me.RBStyle1.Name = "RBStyle1"
        Me.RBStyle1.Size = New System.Drawing.Size(50, 17)
        Me.RBStyle1.TabIndex = 3
        Me.RBStyle1.Text = "Right"
        Me.RBStyle1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Variable:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(22, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Text:"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(59, 41)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(196, 21)
        Me.ComboBox1.TabIndex = 6
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(59, 68)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(196, 21)
        Me.ComboBox2.TabIndex = 7
        '
        'TextStyle
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(263, 101)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox

End Class
