<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ColorDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ColorDialog))
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.TrackBarB = New System.Windows.Forms.TrackBar
        Me.TrackBarG = New System.Windows.Forms.TrackBar
        Me.TrackBarR = New System.Windows.Forms.TrackBar
        Me.TrackBarA = New System.Windows.Forms.TrackBar
        Me.NumericUpDownBc = New System.Windows.Forms.NumericUpDown
        Me.NumericUpDownGc = New System.Windows.Forms.NumericUpDown
        Me.NumericUpDownRc = New System.Windows.Forms.NumericUpDown
        Me.NumericUpDownAc = New System.Windows.Forms.NumericUpDown
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBarB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBarG, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBarR, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBarA, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDownBc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDownGc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDownRc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDownAc, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox2
        '
        Me.PictureBox2.BackgroundImage = CType(resources.GetObject("PictureBox2.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(318, 12)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(55, 95)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 53
        Me.PictureBox2.TabStop = False
        '
        'TrackBarB
        '
        Me.TrackBarB.LargeChange = 1
        Me.TrackBarB.Location = New System.Drawing.Point(55, 88)
        Me.TrackBarB.Maximum = 1000
        Me.TrackBarB.Name = "TrackBarB"
        Me.TrackBarB.Size = New System.Drawing.Size(203, 45)
        Me.TrackBarB.TabIndex = 52
        Me.TrackBarB.TickFrequency = 80
        Me.TrackBarB.TickStyle = System.Windows.Forms.TickStyle.None
        Me.TrackBarB.Value = 1
        '
        'TrackBarG
        '
        Me.TrackBarG.LargeChange = 1
        Me.TrackBarG.Location = New System.Drawing.Point(55, 62)
        Me.TrackBarG.Maximum = 1000
        Me.TrackBarG.Name = "TrackBarG"
        Me.TrackBarG.Size = New System.Drawing.Size(203, 45)
        Me.TrackBarG.TabIndex = 51
        Me.TrackBarG.TickFrequency = 80
        Me.TrackBarG.TickStyle = System.Windows.Forms.TickStyle.None
        Me.TrackBarG.Value = 1
        '
        'TrackBarR
        '
        Me.TrackBarR.LargeChange = 1
        Me.TrackBarR.Location = New System.Drawing.Point(55, 36)
        Me.TrackBarR.Maximum = 1000
        Me.TrackBarR.Name = "TrackBarR"
        Me.TrackBarR.Size = New System.Drawing.Size(203, 45)
        Me.TrackBarR.TabIndex = 50
        Me.TrackBarR.TickFrequency = 80
        Me.TrackBarR.TickStyle = System.Windows.Forms.TickStyle.None
        Me.TrackBarR.Value = 1
        '
        'TrackBarA
        '
        Me.TrackBarA.LargeChange = 1
        Me.TrackBarA.Location = New System.Drawing.Point(55, 12)
        Me.TrackBarA.Maximum = 1000
        Me.TrackBarA.Name = "TrackBarA"
        Me.TrackBarA.Size = New System.Drawing.Size(203, 45)
        Me.TrackBarA.TabIndex = 49
        Me.TrackBarA.TickFrequency = 80
        Me.TrackBarA.TickStyle = System.Windows.Forms.TickStyle.None
        Me.TrackBarA.Value = 1
        '
        'NumericUpDownBc
        '
        Me.NumericUpDownBc.DecimalPlaces = 3
        Me.NumericUpDownBc.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.NumericUpDownBc.Location = New System.Drawing.Point(264, 88)
        Me.NumericUpDownBc.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDownBc.Name = "NumericUpDownBc"
        Me.NumericUpDownBc.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDownBc.TabIndex = 48
        '
        'NumericUpDownGc
        '
        Me.NumericUpDownGc.DecimalPlaces = 3
        Me.NumericUpDownGc.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.NumericUpDownGc.Location = New System.Drawing.Point(264, 62)
        Me.NumericUpDownGc.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDownGc.Name = "NumericUpDownGc"
        Me.NumericUpDownGc.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDownGc.TabIndex = 47
        '
        'NumericUpDownRc
        '
        Me.NumericUpDownRc.DecimalPlaces = 3
        Me.NumericUpDownRc.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.NumericUpDownRc.Location = New System.Drawing.Point(264, 36)
        Me.NumericUpDownRc.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDownRc.Name = "NumericUpDownRc"
        Me.NumericUpDownRc.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDownRc.TabIndex = 46
        '
        'NumericUpDownAc
        '
        Me.NumericUpDownAc.DecimalPlaces = 3
        Me.NumericUpDownAc.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.NumericUpDownAc.Location = New System.Drawing.Point(264, 12)
        Me.NumericUpDownAc.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDownAc.Name = "NumericUpDownAc"
        Me.NumericUpDownAc.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDownAc.TabIndex = 45
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(19, 44)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(30, 13)
        Me.Label8.TabIndex = 41
        Me.Label8.Text = "Red:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 42
        Me.Label2.Text = "Green:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 18)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(37, 13)
        Me.Label10.TabIndex = 44
        Me.Label10.Text = "Alpha:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(18, 96)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(31, 13)
        Me.Label11.TabIndex = 43
        Me.Label11.Text = "Blue:"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(316, 114)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(60, 23)
        Me.Button1.TabIndex = 54
        Me.Button1.Text = "Set Color"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ColorDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(385, 145)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.TrackBarB)
        Me.Controls.Add(Me.TrackBarG)
        Me.Controls.Add(Me.TrackBarR)
        Me.Controls.Add(Me.TrackBarA)
        Me.Controls.Add(Me.NumericUpDownBc)
        Me.Controls.Add(Me.NumericUpDownGc)
        Me.Controls.Add(Me.NumericUpDownRc)
        Me.Controls.Add(Me.NumericUpDownAc)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label11)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ColorDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Color"
        Me.TopMost = True
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBarB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBarG, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBarR, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBarA, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDownBc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDownGc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDownRc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDownAc, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents TrackBarB As System.Windows.Forms.TrackBar
    Friend WithEvents TrackBarG As System.Windows.Forms.TrackBar
    Friend WithEvents TrackBarR As System.Windows.Forms.TrackBar
    Friend WithEvents TrackBarA As System.Windows.Forms.TrackBar
    Friend WithEvents NumericUpDownBc As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDownGc As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDownRc As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDownAc As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
