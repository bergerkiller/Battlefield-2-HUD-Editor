
Public Class MainDialog
    Dim isloading As Boolean = True

    Private Sub UpdateState()
        Dim nodes() As Node = Form1.MainScreen.SelectedNodes.ToArray
        isloading = True
        If Nodes.Count = 1 Then
            TextBox1.Enabled = True
            TextBox1.Text = nodes(0).Name
            Label7.Enabled = True
            Panel1.Enabled = Nodes(0).Type <> "Split Node"
            Panel2.Enabled = Panel1.Enabled
            NumericUpDown1.Value = SetValueBounds(nodes(0).Manage.Location.X, -4096, 800)
            NumericUpDown2.Value = SetValueBounds(nodes(0).Manage.Location.Y, -4096, 600)
            NumericUpDown3.Value = SetValueBounds(nodes(0).Manage.Size.Width, 1, 4096)
            NumericUpDown4.Value = SetValueBounds(nodes(0).Manage.Size.Height, 1, 4096)
            Label8.Enabled = True
            Label9.Enabled = True
            ComboBox1.Enabled = True
            ComboBox2.Enabled = True
            ComboBox1.Text = nodes(0).PosXVariable.VariableName
            ComboBox2.Text = nodes(0).PosYVariable.VariableName
        ElseIf Nodes.Count > 1 Then
            NumericUpDown1.Value = SetValueBounds(Form1.MainScreen.SelectionSquare.Location.X, -4096, 800)
            NumericUpDown2.Value = SetValueBounds(Form1.MainScreen.SelectionSquare.Location.Y, -4096, 800)
            NumericUpDown3.Value = SetValueBounds(Form1.MainScreen.SelectionSquare.Size.Width, 1, 4096)
            NumericUpDown4.Value = SetValueBounds(Form1.MainScreen.SelectionSquare.Size.Height, 1, 4096)
            TextBox1.Enabled = False
            TextBox1.Text = ""
            Label7.Enabled = False
            Panel1.Enabled = True
            Panel2.Enabled = False
            Label8.Enabled = False
            Label9.Enabled = False
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False
            ComboBox1.Text = ""
            ComboBox2.Text = ""
        Else
            TextBox1.Text = ""
            TextBox1.Enabled = False
            Label7.Enabled = False
            Panel1.Enabled = False
            Panel2.Enabled = False
            ComboBox1.Text = ""
            ComboBox2.Text = ""
        End If
        isloading = False
    End Sub

    Private Sub MainDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AddHandler Form1.MainScreen.SelectionChanged, AddressOf UpdateState
        AddHandler Form1.MainScreen.SelectionModified, AddressOf UpdateState
        Form1.MainButton.BackColor = Color.Active
        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        For Each v As VariableHandler In Variables
            If v.Type = VariableType.VT_Angle Or v.Type = VariableType.VT_Position Then
                ComboBox1.Items.Add(v.Name)
                ComboBox2.Items.Add(v.Name)
            End If
        Next
        UpdateState()
    End Sub
    Private Sub MainDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.MainButton.BackColor = Color.Control
        RemoveHandler Form1.MainScreen.SelectionChanged, AddressOf UpdateState
        RemoveHandler Form1.MainScreen.SelectionModified, AddressOf UpdateState
        isloading = True
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        NumericUpDown1.Value = 0
        NumericUpDown2.Value = 0
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim snodes() As Node = Form1.MainScreen.SelectedNodes.ToArray
        If snodes.Count = 1 Then
            If snodes(0).Type = "Picture Node" Then
                NumericUpDown3.Value = snodes(0).PictureNode.Texture.Size.Width
                NumericUpDown4.Value = snodes(0).PictureNode.Texture.Size.Height
            ElseIf snodes(0).Type = "Bar Node" Then
                NumericUpDown3.Value = snodes(0).BarNode.FullTexture.Size.Width
                NumericUpDown4.Value = snodes(0).BarNode.FullTexture.Size.Height
            ElseIf snodes(0).Type = "Text Node" Then
                NumericUpDown3.Value = 40
                NumericUpDown4.Value = 10
            ElseIf snodes(0).Type = "Compass Node" Then
                If snodes(0).CompassNode.Style = 3 Then
                    NumericUpDown3.Value = SetValueBounds(snodes(0).CompassNode.TextureSize.Width - snodes(0).CompassNode.Border.Value - snodes(0).CompassNode.Offset.Value, 1, 2048)
                    NumericUpDown4.Value = snodes(0).CompassNode.TextureSize.Height
                ElseIf snodes(0).CompassNode.Style = 0 Then
                    NumericUpDown3.Value = snodes(0).CompassNode.TextureSize.Width
                    NumericUpDown4.Value = SetValueBounds(snodes(0).CompassNode.TextureSize.Height - snodes(0).CompassNode.Border.Value - snodes(0).CompassNode.Offset.Value, 1, 2048)
                End If
                NumericUpDown3.Value = snodes(0).CompassNode.TextureSize.Width
                NumericUpDown4.Value = snodes(0).CompassNode.TextureSize.Height
            End If
        End If
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        NumericUpDown1.Value = 0
        NumericUpDown2.Value = 0
        NumericUpDown3.Value = 800
        NumericUpDown4.Value = 600
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        NumericUpDown1.Value = 400 - (NumericUpDown3.Value * 0.5)
        NumericUpDown2.Value = 300 - (NumericUpDown4.Value * 0.5)
    End Sub
    Private Sub Position_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown2.ValueChanged, NumericUpDown1.ValueChanged
        If isloading = False Then
            Dim snodes() As Node = Form1.MainScreen.SelectedNodes.ToArray
            If snodes.Count = 1 Then
                snodes(0).Manage.Location = New Point(NumericUpDown1.Value, NumericUpDown2.Value)
                UpdateScreen()
            ElseIf snodes.Count > 1 Then
                Dim offsetX As Integer = NumericUpDown1.Value - Form1.MainScreen.SelectionSquare.Location.X
                Dim offsetY As Integer = NumericUpDown2.Value - Form1.MainScreen.SelectionSquare.Location.Y
                For Each Node As Node In snodes
                    Dim pos As Point = Node.Manage.Location
                    pos.X += offsetX
                    pos.Y += offsetY
                    Node.Manage.Location = pos
                    Node.Changed = True
                Next
                UpdateScreen()
            End If
        End If
    End Sub
    Private Sub Size_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown4.ValueChanged, NumericUpDown3.ValueChanged
        If isloading = False Then
            Dim snodes() As Node = Form1.MainScreen.SelectedNodes.ToArray
            If snodes.Count = 1 Then
                snodes(0).Manage.Size = New Size(NumericUpDown3.Value, NumericUpDown4.Value)
                UpdateScreen()
            End If
        End If
    End Sub
    Private Sub Uniform_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown5.ValueChanged
        If isloading = False Then
            Dim snodes() As Node = Form1.MainScreen.SelectedNodes.ToArray
            If snodes.Count = 1 Then
                Dim s As Size = New Size(100, 100)
                If snodes(0).Type = "Picture Node" Then
                    s = snodes(0).PictureNode.Texture.Size
                ElseIf snodes(0).Type = "Bar Node" Then
                    s = snodes(0).BarNode.FullTexture.Size
                End If
                NumericUpDown3.Value = SetValueBounds(s.Width * NumericUpDown5.Value * 0.01, 1, 2048)
                NumericUpDown4.Value = SetValueBounds(s.Height * NumericUpDown5.Value * 0.01, 1, 2048)
            End If
        End If
    End Sub
    Private Sub TextBox1_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        Try
            Dim snodes() As Node = Form1.MainScreen.SelectedNodes.ToArray
            If snodes.Count = 1 Then
                Dim oldname As String = snodes(0).Name
                Dim newname As String = TextBox1.Text.Trim.Replace(" ", "_")
                TextBox1.Text = oldname
                If Val(newname) = 0 And newname.Trim <> "" And newname.ToLower <> oldname.ToLower Then
                    Dim aexist As Boolean = False
                    For Each Node As Node In Form1.MainScreen.Nodes
                        If Node.Name.Trim.Replace(" ", "_").ToLower = newname.ToLower Then aexist = True : Exit For
                    Next
                    If aexist = True Then
                        MsgBox("Can't rename " & Chr(34) & oldname & Chr(34) & " to " & Chr(34) & newname & Chr(34) & " because a node already exists with this name.", MsgBoxStyle.Information)
                    Else
                        snodes(0).Name = newname
                        WriteLog("Name of node " & oldname & " changed to " & newname)
                        Form1.MainScreen.InvokeSelectionChanged()
                    End If
                End If
            End If
        Catch
        End Try
    End Sub
    Private Sub TextBox1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then TextBox1_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub ComboBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.TextChanged, ComboBox1.SelectedIndexChanged
        NumericUpDown1.Enabled = ComboBox1.Text = ""
        If isloading = False Then
            Dim nodes As List(Of Node) = Form1.MainScreen.SelectedNodes
            If nodes.Count = 1 Then
                nodes(0).PosXVariable.VariableName = ComboBox1.Text
            End If
            UpdateScreen()
        End If
    End Sub
    Private Sub ComboBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.TextChanged, ComboBox2.SelectedIndexChanged
        NumericUpDown2.Enabled = ComboBox2.Text = ""
        If isloading = False Then
            Dim nodes As List(Of Node) = Form1.MainScreen.SelectedNodes
            If nodes.Count = 1 Then
                nodes(0).PosYVariable.VariableName = ComboBox2.Text
            End If
            UpdateScreen()
        End If
    End Sub
End Class
