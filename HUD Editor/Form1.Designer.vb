<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.ToolStripDropDownButton2 = New System.Windows.Forms.ToolStripDropDownButton
        Me.LoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.StyleButton = New System.Windows.Forms.ToolStripButton
        Me.TextureButton = New System.Windows.Forms.ToolStripButton
        Me.TSizeButton = New System.Windows.Forms.ToolStripButton
        Me.ColorButton = New System.Windows.Forms.ToolStripButton
        Me.SizeButton = New System.Windows.Forms.ToolStripButton
        Me.PositionButton = New System.Windows.Forms.ToolStripButton
        Me.RotationButton = New System.Windows.Forms.ToolStripButton
        Me.VariablesButton = New System.Windows.Forms.ToolStripButton
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton
        Me.DrawReferenceCrossToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DrawSelectionSquareToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackgroundToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripComboBox2 = New System.Windows.Forms.ToolStripComboBox
        Me.OverlayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripComboBox3 = New System.Windows.Forms.ToolStripComboBox
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.ForceUpdateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SimulateButton = New System.Windows.Forms.ToolStripButton
        Me.TrackBarXPos = New System.Windows.Forms.TrackBar
        Me.TrackBarYpos = New System.Windows.Forms.TrackBar
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.ToolStrip1.SuspendLayout()
        CType(Me.TrackBarXPos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBarYpos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripDropDownButton2, Me.ToolStripButton1, Me.ToolStripSeparator2, Me.StyleButton, Me.TextureButton, Me.TSizeButton, Me.ColorButton, Me.SizeButton, Me.PositionButton, Me.RotationButton, Me.VariablesButton, Me.ToolStripDropDownButton1, Me.SimulateButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(844, 25)
        Me.ToolStrip1.TabIndex = 60
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripDropDownButton2
        '
        Me.ToolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripDropDownButton2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadToolStripMenuItem, Me.SaveToolStripMenuItem})
        Me.ToolStripDropDownButton2.Image = CType(resources.GetObject("ToolStripDropDownButton2.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton2.Name = "ToolStripDropDownButton2"
        Me.ToolStripDropDownButton2.Size = New System.Drawing.Size(38, 22)
        Me.ToolStripDropDownButton2.Text = "File"
        '
        'LoadToolStripMenuItem
        '
        Me.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem"
        Me.LoadToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.LoadToolStripMenuItem.Text = "Load"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(74, 22)
        Me.ToolStripButton1.Text = "Select Node"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'StyleButton
        '
        Me.StyleButton.Image = CType(resources.GetObject("StyleButton.Image"), System.Drawing.Image)
        Me.StyleButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.StyleButton.Name = "StyleButton"
        Me.StyleButton.Size = New System.Drawing.Size(52, 22)
        Me.StyleButton.Text = "Style"
        Me.StyleButton.Visible = False
        '
        'TextureButton
        '
        Me.TextureButton.Image = CType(resources.GetObject("TextureButton.Image"), System.Drawing.Image)
        Me.TextureButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TextureButton.Name = "TextureButton"
        Me.TextureButton.Size = New System.Drawing.Size(66, 22)
        Me.TextureButton.Text = "Texture"
        Me.TextureButton.Visible = False
        '
        'TSizeButton
        '
        Me.TSizeButton.Image = CType(resources.GetObject("TSizeButton.Image"), System.Drawing.Image)
        Me.TSizeButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSizeButton.Name = "TSizeButton"
        Me.TSizeButton.Size = New System.Drawing.Size(89, 22)
        Me.TSizeButton.Text = "Texture Size"
        Me.TSizeButton.Visible = False
        '
        'ColorButton
        '
        Me.ColorButton.BackColor = System.Drawing.SystemColors.Control
        Me.ColorButton.Image = CType(resources.GetObject("ColorButton.Image"), System.Drawing.Image)
        Me.ColorButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ColorButton.Name = "ColorButton"
        Me.ColorButton.Size = New System.Drawing.Size(56, 22)
        Me.ColorButton.Text = "Color"
        Me.ColorButton.Visible = False
        '
        'SizeButton
        '
        Me.SizeButton.Image = CType(resources.GetObject("SizeButton.Image"), System.Drawing.Image)
        Me.SizeButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SizeButton.Name = "SizeButton"
        Me.SizeButton.Size = New System.Drawing.Size(47, 22)
        Me.SizeButton.Text = "Size"
        Me.SizeButton.Visible = False
        '
        'PositionButton
        '
        Me.PositionButton.Image = CType(resources.GetObject("PositionButton.Image"), System.Drawing.Image)
        Me.PositionButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PositionButton.Name = "PositionButton"
        Me.PositionButton.Size = New System.Drawing.Size(70, 22)
        Me.PositionButton.Text = "Position"
        Me.PositionButton.Visible = False
        '
        'RotationButton
        '
        Me.RotationButton.Image = CType(resources.GetObject("RotationButton.Image"), System.Drawing.Image)
        Me.RotationButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.RotationButton.Name = "RotationButton"
        Me.RotationButton.Size = New System.Drawing.Size(72, 22)
        Me.RotationButton.Text = "Rotation"
        Me.RotationButton.Visible = False
        '
        'VariablesButton
        '
        Me.VariablesButton.Image = CType(resources.GetObject("VariablesButton.Image"), System.Drawing.Image)
        Me.VariablesButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.VariablesButton.Name = "VariablesButton"
        Me.VariablesButton.Size = New System.Drawing.Size(74, 22)
        Me.VariablesButton.Text = "Variables"
        Me.VariablesButton.Visible = False
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DrawReferenceCrossToolStripMenuItem, Me.DrawSelectionSquareToolStripMenuItem, Me.BackgroundToolStripMenuItem, Me.OverlayToolStripMenuItem, Me.ToolStripSeparator3, Me.ForceUpdateToolStripMenuItem})
        Me.ToolStripDropDownButton1.Image = CType(resources.GetObject("ToolStripDropDownButton1.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(61, 22)
        Me.ToolStripDropDownButton1.Text = "View"
        '
        'DrawReferenceCrossToolStripMenuItem
        '
        Me.DrawReferenceCrossToolStripMenuItem.CheckOnClick = True
        Me.DrawReferenceCrossToolStripMenuItem.Name = "DrawReferenceCrossToolStripMenuItem"
        Me.DrawReferenceCrossToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.DrawReferenceCrossToolStripMenuItem.Text = "Draw Reference Cross"
        '
        'DrawSelectionSquareToolStripMenuItem
        '
        Me.DrawSelectionSquareToolStripMenuItem.Checked = True
        Me.DrawSelectionSquareToolStripMenuItem.CheckOnClick = True
        Me.DrawSelectionSquareToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.DrawSelectionSquareToolStripMenuItem.Name = "DrawSelectionSquareToolStripMenuItem"
        Me.DrawSelectionSquareToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.DrawSelectionSquareToolStripMenuItem.Text = "Draw Selection Square"
        '
        'BackgroundToolStripMenuItem
        '
        Me.BackgroundToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripComboBox2})
        Me.BackgroundToolStripMenuItem.Name = "BackgroundToolStripMenuItem"
        Me.BackgroundToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.BackgroundToolStripMenuItem.Text = "Background"
        '
        'ToolStripComboBox2
        '
        Me.ToolStripComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBox2.Name = "ToolStripComboBox2"
        Me.ToolStripComboBox2.Size = New System.Drawing.Size(200, 23)
        '
        'OverlayToolStripMenuItem
        '
        Me.OverlayToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripComboBox3})
        Me.OverlayToolStripMenuItem.Name = "OverlayToolStripMenuItem"
        Me.OverlayToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.OverlayToolStripMenuItem.Text = "Overlay"
        '
        'ToolStripComboBox3
        '
        Me.ToolStripComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBox3.Name = "ToolStripComboBox3"
        Me.ToolStripComboBox3.Size = New System.Drawing.Size(200, 23)
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(188, 6)
        '
        'ForceUpdateToolStripMenuItem
        '
        Me.ForceUpdateToolStripMenuItem.Name = "ForceUpdateToolStripMenuItem"
        Me.ForceUpdateToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.ForceUpdateToolStripMenuItem.Text = "Force Update"
        '
        'SimulateButton
        '
        Me.SimulateButton.Image = CType(resources.GetObject("SimulateButton.Image"), System.Drawing.Image)
        Me.SimulateButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SimulateButton.Name = "SimulateButton"
        Me.SimulateButton.Size = New System.Drawing.Size(73, 22)
        Me.SimulateButton.Text = "Simulate"
        '
        'TrackBarXPos
        '
        Me.TrackBarXPos.Enabled = False
        Me.TrackBarXPos.LargeChange = 400
        Me.TrackBarXPos.Location = New System.Drawing.Point(-1, 634)
        Me.TrackBarXPos.Maximum = 800
        Me.TrackBarXPos.Name = "TrackBarXPos"
        Me.TrackBarXPos.Size = New System.Drawing.Size(824, 45)
        Me.TrackBarXPos.TabIndex = 62
        Me.TrackBarXPos.TickFrequency = 400
        Me.TrackBarXPos.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'TrackBarYpos
        '
        Me.TrackBarYpos.Enabled = False
        Me.TrackBarYpos.LargeChange = 300
        Me.TrackBarYpos.Location = New System.Drawing.Point(809, 19)
        Me.TrackBarYpos.Maximum = 0
        Me.TrackBarYpos.Minimum = -600
        Me.TrackBarYpos.Name = "TrackBarYpos"
        Me.TrackBarYpos.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.TrackBarYpos.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.TrackBarYpos.Size = New System.Drawing.Size(45, 626)
        Me.TrackBarYpos.TabIndex = 63
        Me.TrackBarYpos.TabStop = False
        Me.TrackBarYpos.TickFrequency = 300
        Me.TrackBarYpos.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Location = New System.Drawing.Point(10, 34)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(800, 600)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 61
        Me.PictureBox1.TabStop = False
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "Text files|*.txt"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(844, 667)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.TrackBarXPos)
        Me.Controls.Add(Me.TrackBarYpos)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "HUD Editor - No Node Selected"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.TrackBarXPos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBarYpos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripDropDownButton2 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents LoadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TextureButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ColorButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SizeButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents PositionButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents RotationButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents VariablesButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripDropDownButton1 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents DrawReferenceCrossToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackgroundToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripComboBox2 As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents OverlayToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripComboBox3 As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SimulateButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents TrackBarXPos As System.Windows.Forms.TrackBar
    Friend WithEvents TrackBarYpos As System.Windows.Forms.TrackBar
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents DrawSelectionSquareToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ForceUpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StyleButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents TSizeButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog

End Class
