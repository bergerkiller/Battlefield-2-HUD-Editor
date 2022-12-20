<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NodeSelect
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NodeSelect))
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.AddButton = New System.Windows.Forms.Button
        Me.DeleteButton = New System.Windows.Forms.Button
        Me.UpButton = New System.Windows.Forms.Button
        Me.DownButton = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CloneButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.CheckBoxes = True
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(-1, 48)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(312, 271)
        Me.ListView1.TabIndex = 6
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Node Name"
        Me.ColumnHeader1.Width = 95
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Node Type"
        Me.ColumnHeader2.Width = 112
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(1, 3)
        Me.Label3.MaximumSize = New System.Drawing.Size(258, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(256, 39)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Check the nodes to render. Nodes are rendered from top to bottom. Use the Up and " & _
            "DOWN buttons to change the render order."
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(-1, 318)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(312, 20)
        Me.TextBox1.TabIndex = 8
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddButton.BackgroundImage = CType(resources.GetObject("AddButton.BackgroundImage"), System.Drawing.Image)
        Me.AddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.AddButton.ForeColor = System.Drawing.Color.DarkOliveGreen
        Me.AddButton.Location = New System.Drawing.Point(317, 242)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(25, 25)
        Me.AddButton.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.AddButton, "Add new node")
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DeleteButton.BackgroundImage = CType(resources.GetObject("DeleteButton.BackgroundImage"), System.Drawing.Image)
        Me.DeleteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.DeleteButton.Enabled = False
        Me.DeleteButton.ForeColor = System.Drawing.Color.DarkRed
        Me.DeleteButton.Location = New System.Drawing.Point(317, 48)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(25, 25)
        Me.DeleteButton.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.DeleteButton, "Delete this node")
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'UpButton
        '
        Me.UpButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpButton.Enabled = False
        Me.UpButton.Location = New System.Drawing.Point(317, 79)
        Me.UpButton.Name = "UpButton"
        Me.UpButton.Size = New System.Drawing.Size(25, 75)
        Me.UpButton.TabIndex = 10
        Me.UpButton.Text = "/\/\/\/\/\/\/\/\/\"
        Me.ToolTip1.SetToolTip(Me.UpButton, "Move node UP")
        Me.UpButton.UseVisualStyleBackColor = True
        '
        'DownButton
        '
        Me.DownButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DownButton.Enabled = False
        Me.DownButton.Location = New System.Drawing.Point(317, 160)
        Me.DownButton.Name = "DownButton"
        Me.DownButton.Size = New System.Drawing.Size(25, 75)
        Me.DownButton.TabIndex = 11
        Me.DownButton.Text = "\/\/\/\/\/\/\/\/\/"
        Me.ToolTip1.SetToolTip(Me.DownButton, "Move node DOWN")
        Me.DownButton.UseVisualStyleBackColor = True
        '
        'CloneButton
        '
        Me.CloneButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloneButton.BackgroundImage = CType(resources.GetObject("CloneButton.BackgroundImage"), System.Drawing.Image)
        Me.CloneButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CloneButton.Enabled = False
        Me.CloneButton.ForeColor = System.Drawing.Color.DarkOliveGreen
        Me.CloneButton.Location = New System.Drawing.Point(317, 315)
        Me.CloneButton.Name = "CloneButton"
        Me.CloneButton.Size = New System.Drawing.Size(25, 25)
        Me.CloneButton.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.CloneButton, "Clone selected node")
        Me.CloneButton.UseVisualStyleBackColor = True
        '
        'NodeSelect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(347, 345)
        Me.Controls.Add(Me.CloneButton)
        Me.Controls.Add(Me.DownButton)
        Me.Controls.Add(Me.UpButton)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ListView1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(290, 310)
        Me.Name = "NodeSelect"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Node"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents UpButton As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents DownButton As System.Windows.Forms.Button
    Friend WithEvents CloneButton As System.Windows.Forms.Button

End Class
