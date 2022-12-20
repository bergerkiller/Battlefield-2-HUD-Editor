<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LCForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LCForm))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RemoveImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditPathToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddNewImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Button3 = New System.Windows.Forms.Button
        Me.OpenFileDialog2 = New System.Windows.Forms.OpenFileDialog
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SetBackgroundToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CheckboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BlackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GreyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WhiteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button5 = New System.Windows.Forms.Button
        Me.OpenFileDialog3 = New System.Windows.Forms.OpenFileDialog
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Textures:"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(420, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Preview"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(453, 288)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(116, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Save texture library"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Button2.Location = New System.Drawing.Point(246, 288)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Add image"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "Texture Library|*.texlib"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "All|*.dds;*.tga;*.bmp;*.jpeg;*.png;*.ico;*.psd;*.jif;*.tiff|*.dds|Direct draw sur" & _
            "face|*.tga|TARGA|*.bmp|Bitmap|*.jpeg|JPEG|*.png|PNG|*.ico|Icons|*.psd|Photoshop " & _
            "PSD|*.jif|JIF|*.tiff|TIFF"
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.ListView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HideSelection = False
        Me.ListView1.LabelEdit = True
        Me.ListView1.Location = New System.Drawing.Point(12, 25)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(295, 256)
        Me.ListView1.TabIndex = 6
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Files"
        Me.ColumnHeader1.Width = 285
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveImageToolStripMenuItem, Me.EditPathToolStripMenuItem, Me.AddNewImageToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(158, 70)
        '
        'RemoveImageToolStripMenuItem
        '
        Me.RemoveImageToolStripMenuItem.Name = "RemoveImageToolStripMenuItem"
        Me.RemoveImageToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.RemoveImageToolStripMenuItem.Text = "Remove image"
        '
        'EditPathToolStripMenuItem
        '
        Me.EditPathToolStripMenuItem.Name = "EditPathToolStripMenuItem"
        Me.EditPathToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.EditPathToolStripMenuItem.Text = "Edit Path"
        '
        'AddNewImageToolStripMenuItem
        '
        Me.AddNewImageToolStripMenuItem.Name = "AddNewImageToolStripMenuItem"
        Me.AddNewImageToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.AddNewImageToolStripMenuItem.Text = "Add new image"
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button3.Location = New System.Drawing.Point(12, 288)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(114, 23)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "Load texture library"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'OpenFileDialog2
        '
        Me.OpenFileDialog2.Filter = "Texture Libraries|*.texlib"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.ContextMenuStrip = Me.ContextMenuStrip2
        Me.PictureBox1.Location = New System.Drawing.Point(313, 25)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(256, 256)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SetBackgroundToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(158, 26)
        '
        'SetBackgroundToolStripMenuItem
        '
        Me.SetBackgroundToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CheckboardToolStripMenuItem, Me.BlackToolStripMenuItem, Me.GreyToolStripMenuItem, Me.WhiteToolStripMenuItem})
        Me.SetBackgroundToolStripMenuItem.Name = "SetBackgroundToolStripMenuItem"
        Me.SetBackgroundToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.SetBackgroundToolStripMenuItem.Text = "Set Background"
        '
        'CheckboardToolStripMenuItem
        '
        Me.CheckboardToolStripMenuItem.Name = "CheckboardToolStripMenuItem"
        Me.CheckboardToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.CheckboardToolStripMenuItem.Text = "Checkboard"
        '
        'BlackToolStripMenuItem
        '
        Me.BlackToolStripMenuItem.Name = "BlackToolStripMenuItem"
        Me.BlackToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.BlackToolStripMenuItem.Text = "Black"
        '
        'GreyToolStripMenuItem
        '
        Me.GreyToolStripMenuItem.Name = "GreyToolStripMenuItem"
        Me.GreyToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.GreyToolStripMenuItem.Text = "Grey"
        '
        'WhiteToolStripMenuItem
        '
        Me.WhiteToolStripMenuItem.Name = "WhiteToolStripMenuItem"
        Me.WhiteToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.WhiteToolStripMenuItem.Text = "White"
        '
        'Button4
        '
        Me.Button4.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Button4.Location = New System.Drawing.Point(327, 288)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(91, 23)
        Me.Button4.TabIndex = 8
        Me.Button4.Text = "Remove image"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button5.Location = New System.Drawing.Point(132, 288)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(81, 23)
        Me.Button5.TabIndex = 9
        Me.Button5.Text = "Load atlas"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'OpenFileDialog3
        '
        Me.OpenFileDialog3.Filter = "Texture Atlasses|*.tai"
        '
        'LCForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(581, 323)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(542, 361)
        Me.Name = "LCForm"
        Me.Text = "Texture Library Creator"
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog2 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RemoveImageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditPathToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddNewImageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SetBackgroundToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CheckboardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BlackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GreyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WhiteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog3 As System.Windows.Forms.OpenFileDialog

End Class
