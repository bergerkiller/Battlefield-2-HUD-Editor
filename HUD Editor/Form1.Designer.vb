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
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveSnapshotToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.MainButton = New System.Windows.Forms.ToolStripButton
        Me.ColorButton = New System.Windows.Forms.ToolStripButton
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton
        Me.DrawReferenceCrossToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DrawSelectionSquareToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackgroundToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripComboBox2 = New System.Windows.Forms.ToolStripComboBox
        Me.OverlayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripComboBox3 = New System.Windows.Forms.ToolStripComboBox
        Me.FullScreenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UseFixedResolutionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TextureFilterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.TextureLibraryCreatorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ForceUpdateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ResetScreenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ViewLogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TextureButton1 = New System.Windows.Forms.ToolStripButton
        Me.TextureButton2 = New System.Windows.Forms.ToolStripButton
        Me.StyleButton = New System.Windows.Forms.ToolStripButton
        Me.RotationButton = New System.Windows.Forms.ToolStripButton
        Me.SimulateButton = New System.Windows.Forms.ToolStripButton
        Me.ShowButton = New System.Windows.Forms.ToolStripButton
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewNodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SelectNodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ShowAllNodesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SendToBackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BringToFrontToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.DeselectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Button1 = New System.Windows.Forms.Button
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.DeleteButton = New System.Windows.Forms.Button
        Me.AddButton = New System.Windows.Forms.Button
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SaveFileDialog2 = New System.Windows.Forms.SaveFileDialog
        Me.Label2 = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.MainScreen = New HUD_Editor.Canvas
        Me.ToolStrip1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripDropDownButton2, Me.ToolStripSeparator2, Me.MainButton, Me.ColorButton, Me.ToolStripDropDownButton1, Me.TextureButton1, Me.TextureButton2, Me.StyleButton, Me.RotationButton, Me.SimulateButton, Me.ShowButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(801, 25)
        Me.ToolStrip1.TabIndex = 60
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripDropDownButton2
        '
        Me.ToolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripDropDownButton2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.LoadToolStripMenuItem, Me.SaveToolStripMenuItem, Me.SaveSnapshotToolStripMenuItem, Me.ToolStripSeparator4, Me.ExitToolStripMenuItem})
        Me.ToolStripDropDownButton2.Image = CType(resources.GetObject("ToolStripDropDownButton2.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton2.Name = "ToolStripDropDownButton2"
        Me.ToolStripDropDownButton2.Size = New System.Drawing.Size(38, 22)
        Me.ToolStripDropDownButton2.Text = "File"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'LoadToolStripMenuItem
        '
        Me.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem"
        Me.LoadToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.LoadToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.LoadToolStripMenuItem.Text = "Load"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'SaveSnapshotToolStripMenuItem
        '
        Me.SaveSnapshotToolStripMenuItem.Name = "SaveSnapshotToolStripMenuItem"
        Me.SaveSnapshotToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.SaveSnapshotToolStripMenuItem.Text = "Save snapshot"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(146, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
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
        '
        'ColorButton
        '
        Me.ColorButton.BackColor = System.Drawing.SystemColors.Control
        Me.ColorButton.Image = CType(resources.GetObject("ColorButton.Image"), System.Drawing.Image)
        Me.ColorButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ColorButton.Name = "ColorButton"
        Me.ColorButton.Size = New System.Drawing.Size(56, 22)
        Me.ColorButton.Text = "Color"
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DrawReferenceCrossToolStripMenuItem, Me.DrawSelectionSquareToolStripMenuItem, Me.BackgroundToolStripMenuItem, Me.OverlayToolStripMenuItem, Me.FullScreenToolStripMenuItem, Me.UseFixedResolutionToolStripMenuItem, Me.TextureFilterToolStripMenuItem, Me.ToolStripSeparator3, Me.TextureLibraryCreatorToolStripMenuItem, Me.ForceUpdateToolStripMenuItem, Me.ResetScreenToolStripMenuItem, Me.ViewLogToolStripMenuItem})
        Me.ToolStripDropDownButton1.Image = CType(resources.GetObject("ToolStripDropDownButton1.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(78, 22)
        Me.ToolStripDropDownButton1.Text = "Settings"
        '
        'DrawReferenceCrossToolStripMenuItem
        '
        Me.DrawReferenceCrossToolStripMenuItem.CheckOnClick = True
        Me.DrawReferenceCrossToolStripMenuItem.Name = "DrawReferenceCrossToolStripMenuItem"
        Me.DrawReferenceCrossToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.DrawReferenceCrossToolStripMenuItem.Text = "Draw Reference Cross"
        '
        'DrawSelectionSquareToolStripMenuItem
        '
        Me.DrawSelectionSquareToolStripMenuItem.Checked = True
        Me.DrawSelectionSquareToolStripMenuItem.CheckOnClick = True
        Me.DrawSelectionSquareToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.DrawSelectionSquareToolStripMenuItem.Name = "DrawSelectionSquareToolStripMenuItem"
        Me.DrawSelectionSquareToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.DrawSelectionSquareToolStripMenuItem.Text = "Draw Selection Square"
        '
        'BackgroundToolStripMenuItem
        '
        Me.BackgroundToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripComboBox2})
        Me.BackgroundToolStripMenuItem.Name = "BackgroundToolStripMenuItem"
        Me.BackgroundToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
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
        Me.OverlayToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.OverlayToolStripMenuItem.Text = "Overlay"
        '
        'ToolStripComboBox3
        '
        Me.ToolStripComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBox3.Name = "ToolStripComboBox3"
        Me.ToolStripComboBox3.Size = New System.Drawing.Size(200, 23)
        '
        'FullScreenToolStripMenuItem
        '
        Me.FullScreenToolStripMenuItem.CheckOnClick = True
        Me.FullScreenToolStripMenuItem.Name = "FullScreenToolStripMenuItem"
        Me.FullScreenToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.FullScreenToolStripMenuItem.Text = "Full Screen"
        '
        'UseFixedResolutionToolStripMenuItem
        '
        Me.UseFixedResolutionToolStripMenuItem.CheckOnClick = True
        Me.UseFixedResolutionToolStripMenuItem.Name = "UseFixedResolutionToolStripMenuItem"
        Me.UseFixedResolutionToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.UseFixedResolutionToolStripMenuItem.Text = "Fixed Resolution"
        '
        'TextureFilterToolStripMenuItem
        '
        Me.TextureFilterToolStripMenuItem.Name = "TextureFilterToolStripMenuItem"
        Me.TextureFilterToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.TextureFilterToolStripMenuItem.Text = "Texture Filter"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(191, 6)
        '
        'TextureLibraryCreatorToolStripMenuItem
        '
        Me.TextureLibraryCreatorToolStripMenuItem.Name = "TextureLibraryCreatorToolStripMenuItem"
        Me.TextureLibraryCreatorToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.TextureLibraryCreatorToolStripMenuItem.Text = "Texture Library Creator"
        '
        'ForceUpdateToolStripMenuItem
        '
        Me.ForceUpdateToolStripMenuItem.Name = "ForceUpdateToolStripMenuItem"
        Me.ForceUpdateToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.ForceUpdateToolStripMenuItem.Text = "Force Update"
        '
        'ResetScreenToolStripMenuItem
        '
        Me.ResetScreenToolStripMenuItem.Name = "ResetScreenToolStripMenuItem"
        Me.ResetScreenToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.ResetScreenToolStripMenuItem.Text = "Reset Screen"
        '
        'ViewLogToolStripMenuItem
        '
        Me.ViewLogToolStripMenuItem.Name = "ViewLogToolStripMenuItem"
        Me.ViewLogToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.ViewLogToolStripMenuItem.Text = "View Log"
        '
        'TextureButton1
        '
        Me.TextureButton1.Image = CType(resources.GetObject("TextureButton1.Image"), System.Drawing.Image)
        Me.TextureButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TextureButton1.Name = "TextureButton1"
        Me.TextureButton1.Size = New System.Drawing.Size(66, 22)
        Me.TextureButton1.Text = "Texture"
        Me.TextureButton1.Visible = False
        '
        'TextureButton2
        '
        Me.TextureButton2.Image = CType(resources.GetObject("TextureButton2.Image"), System.Drawing.Image)
        Me.TextureButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TextureButton2.Name = "TextureButton2"
        Me.TextureButton2.Size = New System.Drawing.Size(66, 22)
        Me.TextureButton2.Text = "Texture"
        Me.TextureButton2.Visible = False
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
        'RotationButton
        '
        Me.RotationButton.Image = CType(resources.GetObject("RotationButton.Image"), System.Drawing.Image)
        Me.RotationButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.RotationButton.Name = "RotationButton"
        Me.RotationButton.Size = New System.Drawing.Size(72, 22)
        Me.RotationButton.Text = "Rotation"
        Me.RotationButton.Visible = False
        '
        'SimulateButton
        '
        Me.SimulateButton.Image = CType(resources.GetObject("SimulateButton.Image"), System.Drawing.Image)
        Me.SimulateButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SimulateButton.Name = "SimulateButton"
        Me.SimulateButton.Size = New System.Drawing.Size(73, 22)
        Me.SimulateButton.Text = "Simulate"
        '
        'ShowButton
        '
        Me.ShowButton.Image = CType(resources.GetObject("ShowButton.Image"), System.Drawing.Image)
        Me.ShowButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ShowButton.Name = "ShowButton"
        Me.ShowButton.Size = New System.Drawing.Size(56, 22)
        Me.ShowButton.Text = "Show"
        Me.ShowButton.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewNodeToolStripMenuItem, Me.SelectNodeToolStripMenuItem, Me.ShowAllNodesToolStripMenuItem, Me.HideToolStripMenuItem, Me.SendToBackToolStripMenuItem, Me.BringToFrontToolStripMenuItem, Me.ToolStripSeparator1, Me.DeselectToolStripMenuItem, Me.SelectAllToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.CutToolStripMenuItem, Me.DeleteToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(189, 274)
        '
        'NewNodeToolStripMenuItem
        '
        Me.NewNodeToolStripMenuItem.Name = "NewNodeToolStripMenuItem"
        Me.NewNodeToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.NewNodeToolStripMenuItem.Text = "Add Node"
        '
        'SelectNodeToolStripMenuItem
        '
        Me.SelectNodeToolStripMenuItem.Name = "SelectNodeToolStripMenuItem"
        Me.SelectNodeToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.SelectNodeToolStripMenuItem.Text = "Select node"
        '
        'ShowAllNodesToolStripMenuItem
        '
        Me.ShowAllNodesToolStripMenuItem.Name = "ShowAllNodesToolStripMenuItem"
        Me.ShowAllNodesToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.ShowAllNodesToolStripMenuItem.Text = "Show all nodes"
        '
        'HideToolStripMenuItem
        '
        Me.HideToolStripMenuItem.Name = "HideToolStripMenuItem"
        Me.HideToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.HideToolStripMenuItem.Text = "Hide selected node(s)"
        '
        'SendToBackToolStripMenuItem
        '
        Me.SendToBackToolStripMenuItem.Name = "SendToBackToolStripMenuItem"
        Me.SendToBackToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.SendToBackToolStripMenuItem.Text = "Send to back"
        '
        'BringToFrontToolStripMenuItem
        '
        Me.BringToFrontToolStripMenuItem.Name = "BringToFrontToolStripMenuItem"
        Me.BringToFrontToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.BringToFrontToolStripMenuItem.Text = "Bring to front"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(185, 6)
        '
        'DeselectToolStripMenuItem
        '
        Me.DeselectToolStripMenuItem.Name = "DeselectToolStripMenuItem"
        Me.DeselectToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.DeselectToolStripMenuItem.Text = "Deselect All"
        '
        'SelectAllToolStripMenuItem
        '
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        Me.SelectAllToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.SelectAllToolStripMenuItem.Text = "Select All"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.CopyToolStripMenuItem.Text = "Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.PasteToolStripMenuItem.Text = "Paste"
        '
        'CutToolStripMenuItem
        '
        Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
        Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.CutToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.CutToolStripMenuItem.Text = "Cut"
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
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
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.SystemColors.Info
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.TreeView1)
        Me.Panel1.Controls.Add(Me.DeleteButton)
        Me.Panel1.Controls.Add(Me.AddButton)
        Me.Panel1.Controls.Add(Me.ComboBox1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(-1, 26)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(326, 600)
        Me.Panel1.TabIndex = 63
        Me.Panel1.Visible = False
        '
        'Button1
        '
        Me.Button1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(310, -1)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(17, 604)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = ">>>>>>"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TreeView1
        '
        Me.TreeView1.AllowDrop = True
        Me.TreeView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TreeView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.TreeView1.CheckBoxes = True
        Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.TreeView1.FullRowSelect = True
        Me.TreeView1.LabelEdit = True
        Me.TreeView1.Location = New System.Drawing.Point(6, 3)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.ShowNodeToolTips = True
        Me.TreeView1.Size = New System.Drawing.Size(302, 537)
        Me.TreeView1.TabIndex = 0
        '
        'DeleteButton
        '
        Me.DeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DeleteButton.BackgroundImage = CType(resources.GetObject("DeleteButton.BackgroundImage"), System.Drawing.Image)
        Me.DeleteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.DeleteButton.Enabled = False
        Me.DeleteButton.ForeColor = System.Drawing.Color.DarkRed
        Me.DeleteButton.Location = New System.Drawing.Point(50, 540)
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
        Me.AddButton.Location = New System.Drawing.Point(18, 540)
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
        Me.ComboBox1.Location = New System.Drawing.Point(127, 578)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(50, 21)
        Me.ComboBox1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 581)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(102, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Rendered GuiIndex:"
        '
        'SaveFileDialog2
        '
        Me.SaveFileDialog2.Filter = "Targa|*.tga|PNG|*.png"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(758, 607)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 13)
        Me.Label2.TabIndex = 64
        Me.Label2.Text = "20 fps"
        Me.Label2.Visible = False
        '
        'Timer1
        '
        Me.Timer1.Interval = 20
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(81, 546)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(179, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Nodes under ""Chat"" are bf2142-only"
        '
        'MainScreen
        '
        Me.MainScreen.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MainScreen.BackColor = System.Drawing.Color.Black
        Me.MainScreen.ContextMenuStrip = Me.ContextMenuStrip1
        Me.MainScreen.Location = New System.Drawing.Point(0, 26)
        Me.MainScreen.Name = "MainScreen"
        Me.MainScreen.OverlayImage = Nothing
        Me.MainScreen.ReferenceCrossImage = Nothing
        Me.MainScreen.ShowReferenceCross = False
        Me.MainScreen.ShowSelectionSquare = True
        Me.MainScreen.Size = New System.Drawing.Size(800, 600)
        Me.MainScreen.TabIndex = 0
        Me.MainScreen.UseFixedResolution = False
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(801, 627)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.MainScreen)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(432, 365)
        Me.Name = "Form1"
        Me.Text = "HUD Editor - No Nodes Selected"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
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
    Friend WithEvents ColorButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripDropDownButton1 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents DrawReferenceCrossToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackgroundToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripComboBox2 As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents OverlayToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripComboBox3 As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DrawSelectionSquareToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ForceUpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
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
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents ResetScreenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewLogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveSnapshotToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveFileDialog2 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents DeselectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextureLibraryCreatorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainScreen As HUD_Editor.Canvas
    Friend WithEvents FullScreenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UseFixedResolutionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TextureFilterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TextureButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ShowAllNodesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TextureButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StyleButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents RotationButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ShowButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents SimulateButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label3 As System.Windows.Forms.Label

End Class
