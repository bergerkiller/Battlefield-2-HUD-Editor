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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.ToolStripDropDownButton2 = New System.Windows.Forms.ToolStripDropDownButton
        Me.LoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.MainButton = New System.Windows.Forms.ToolStripButton
        Me.StyleButton = New System.Windows.Forms.ToolStripButton
        Me.TextureButton = New System.Windows.Forms.ToolStripButton
        Me.TSizeButton = New System.Windows.Forms.ToolStripButton
        Me.ColorButton = New System.Windows.Forms.ToolStripButton
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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewNodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SelectNodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SendToBackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BringToFrontToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.Button1 = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.DeleteButton = New System.Windows.Forms.Button
        Me.AddButton = New System.Windows.Forms.Button
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ToolStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripDropDownButton2, Me.ToolStripSeparator2, Me.MainButton, Me.StyleButton, Me.TextureButton, Me.TSizeButton, Me.ColorButton, Me.RotationButton, Me.VariablesButton, Me.ToolStripDropDownButton1, Me.SimulateButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(816, 25)
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
        Me.LoadToolStripMenuItem.Size = New System.Drawing.Size(100, 22)
        Me.LoadToolStripMenuItem.Text = "Load"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(100, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'MainButton
        '
        Me.MainButton.Image = CType(resources.GetObject("MainButton.Image"), System.Drawing.Image)
        Me.MainButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MainButton.Name = "MainButton"
        Me.MainButton.Size = New System.Drawing.Size(54, 22)
        Me.MainButton.Text = "Main"
        Me.MainButton.Visible = False
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
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.PictureBox1.Location = New System.Drawing.Point(15, 26)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(800, 600)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 61
        Me.PictureBox1.TabStop = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewNodeToolStripMenuItem, Me.SelectNodeToolStripMenuItem, Me.SendToBackToolStripMenuItem, Me.BringToFrontToolStripMenuItem, Me.ToolStripSeparator1, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.CutToolStripMenuItem, Me.DeleteToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(146, 186)
        '
        'NewNodeToolStripMenuItem
        '
        Me.NewNodeToolStripMenuItem.Name = "NewNodeToolStripMenuItem"
        Me.NewNodeToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.NewNodeToolStripMenuItem.Text = "Add Node"
        '
        'SelectNodeToolStripMenuItem
        '
        Me.SelectNodeToolStripMenuItem.Name = "SelectNodeToolStripMenuItem"
        Me.SelectNodeToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.SelectNodeToolStripMenuItem.Text = "Select node"
        '
        'SendToBackToolStripMenuItem
        '
        Me.SendToBackToolStripMenuItem.Name = "SendToBackToolStripMenuItem"
        Me.SendToBackToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.SendToBackToolStripMenuItem.Text = "Send to back"
        '
        'BringToFrontToolStripMenuItem
        '
        Me.BringToFrontToolStripMenuItem.Name = "BringToFrontToolStripMenuItem"
        Me.BringToFrontToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.BringToFrontToolStripMenuItem.Text = "Bring to front"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(142, 6)
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.CopyToolStripMenuItem.Text = "Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.PasteToolStripMenuItem.Text = "Paste"
        '
        'CutToolStripMenuItem
        '
        Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
        Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.CutToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.CutToolStripMenuItem.Text = "Cut"
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "Con Files|*.con"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "Con files|*.con"
        '
        'Button1
        '
        Me.Button1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(0, 26)
        Me.Button1.Name = "Button1"
        Me.Button1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Button1.Size = New System.Drawing.Size(15, 600)
        Me.Button1.TabIndex = 62
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.SystemColors.Info
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.TreeView1)
        Me.Panel1.Controls.Add(Me.DeleteButton)
        Me.Panel1.Controls.Add(Me.AddButton)
        Me.Panel1.Controls.Add(Me.ComboBox1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(0, 26)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(219, 600)
        Me.Panel1.TabIndex = 63
        Me.Panel1.Visible = False
        '
        'TreeView1
        '
        Me.TreeView1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TreeView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.TreeView1.FullRowSelect = True
        Me.TreeView1.HideSelection = False
        Me.TreeView1.Location = New System.Drawing.Point(19, 3)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(193, 533)
        Me.TreeView1.TabIndex = 0
        '
        'DeleteButton
        '
        Me.DeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DeleteButton.BackgroundImage = CType(resources.GetObject("DeleteButton.BackgroundImage"), System.Drawing.Image)
        Me.DeleteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.DeleteButton.Enabled = False
        Me.DeleteButton.ForeColor = System.Drawing.Color.DarkRed
        Me.DeleteButton.Location = New System.Drawing.Point(50, 536)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(25, 25)
        Me.DeleteButton.TabIndex = 6
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddButton.BackgroundImage = CType(resources.GetObject("AddButton.BackgroundImage"), System.Drawing.Image)
        Me.AddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.AddButton.ForeColor = System.Drawing.Color.DarkOliveGreen
        Me.AddButton.Location = New System.Drawing.Point(18, 536)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(25, 25)
        Me.AddButton.TabIndex = 5
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(127, 574)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(50, 21)
        Me.ComboBox1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 577)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(102, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Rendered GuiIndex:"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(816, 627)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.PictureBox1)
        Me.MinimumSize = New System.Drawing.Size(417, 365)
        Me.Name = "Form1"
        Me.Text = "HUD Editor - No Node Selected"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripDropDownButton2 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents LoadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TextureButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ColorButton As System.Windows.Forms.ToolStripButton
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
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents DrawSelectionSquareToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ForceUpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StyleButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents TSizeButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SelectNodeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SendToBackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BringToFrontToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewNodeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents DeleteButton As System.Windows.Forms.Button

End Class
