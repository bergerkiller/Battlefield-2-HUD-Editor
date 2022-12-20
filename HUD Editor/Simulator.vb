Imports System.Threading

Public Class Simulator
    Dim simfile As String = ""
    Dim threadsimulate As Thread
    Dim speed As Integer = 50

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        threadsimulate = New Thread(AddressOf Simulator)
        threadsimulate.IsBackground = True
        threadsimulate.Start()
        ProgressBar1.Value = 0
        ProgressBar1.Visible = True
        ComboBox1.Enabled = False
        Button2.Enabled = True
        Button1.Enabled = False
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        StopSimulation()
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Button1.Enabled = ComboBox1.SelectedIndex <> -1
        simfile = "Simulation\" & ComboBox1.SelectedItem & ".hesf"
    End Sub
    Private Delegate Sub myProgressAdapter(ByVal Percentage As Integer)
    Private Sub ProgressAdapter(ByVal Percentage As Integer)
        Try
            If Me.InvokeRequired Then
                Dim d As New myProgressAdapter(AddressOf ProgressAdapter)
                Me.Invoke(d, Percentage)
            Else
                ProgressBar1.Value = Percentage
                If Percentage = 100 Then ProgressBar1.Visible = False
            End If
        Catch
        End Try
    End Sub
    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.ValueChanged
        speed = 1000 - TrackBar1.Value
    End Sub
    Private Sub Simulator_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        For Each SimFile As String In System.IO.Directory.GetFiles("Simulation", "*.hesf")
            ComboBox1.Items.Add(IO.Path.GetFileNameWithoutExtension(SimFile))
        Next
        Button1.Enabled = False
        Form1.SimulateButton.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
    End Sub
    Private Sub Simulator_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        StopSimulation()
        Form1.SimulateButton.BackColor = Color.FromKnownColor(KnownColor.Control)
    End Sub
    Private Sub StopSimulation()
        Try
            threadsimulate.Abort()
        Catch
        End Try
        ProgressBar1.Value = 0
        ProgressBar1.Visible = False
        ComboBox1.Enabled = True
        Button2.Enabled = False
        Button1.Enabled = True
        ResetNodeSimvars()
    End Sub
    Private Sub Simulator()
        If System.IO.File.Exists(simfile) Then
            Dim SimulationScript As New ListView
            Dim HasReadHeader As Boolean = False
            Dim reader As New System.IO.StreamReader(simfile)
            Do While reader.Peek <> -1
                Dim textline As String = reader.ReadLine
                If Not textline.StartsWith("#") Then
                    If HasReadHeader = True Then
                        Dim i As Integer = 0
                        For Each Value As String In textline.Split(ControlChars.Tab)
                            If Not Value.Trim = "" Then
                                SimulationScript.Items(i).SubItems.Add(Value)
                                i += 1
                                If SimulationScript.Items.Count = i Then Exit For
                            End If
                        Next
                        'Make sure all variables have even amount of values
                        Dim requestedamount As Integer = 0
                        For Each item As ListViewItem In SimulationScript.Items
                            If item.SubItems.Count > requestedamount Then requestedamount = item.SubItems.Count
                        Next
                        For Each item As ListViewItem In SimulationScript.Items
                            If item.SubItems.Count < requestedamount Then
                                If item.SubItems.Count = 1 Then item.SubItems.Add("0") Else item.SubItems.Add(item.SubItems(item.SubItems.Count - 1).Text)
                            End If
                        Next
                    Else
                        For Each Variable As String In textline.Split(ControlChars.Tab)
                            If Not Variable.Trim = "" Then
                                Dim item As New ListViewItem
                                item.Text = Variable
                                SimulationScript.Items.Add(item)
                            End If
                        Next
                        If SimulationScript.Items.Count = 0 Then Exit Do
                        HasReadHeader = True
                    End If
                End If
            Loop
            If SimulationScript.Items.Count Then
                'Simulation script loaded
                Dim simulationimages(SimulationScript.Items(0).SubItems.Count - 1) As Image
                For i As Integer = 1 To SimulationScript.Items(0).SubItems.Count - 1
                    For Each VarItem As ListViewItem In SimulationScript.Items
                        Dim Variable As String = VarItem.Text.ToLower.Trim
                        Dim Value As String = VarItem.SubItems(i).Text
                        For ni As Integer = 0 To Nodes.Count - 1
                            If Nodes(ni).Render = True Then
                                If Nodes(ni).Type = "Picture Node" Then
                                    With Nodes(ni).PictureNodeData
                                        If .DOffsetXVar.ToLower.Trim = Variable Then .DOffsetX = Val(Value)
                                        If .DOffsetYVar.ToLower.Trim = Variable Then .DOffsetY = Val(Value)
                                        If .DRotationVar.ToLower.Trim = Variable Then .DRotation = Val(Value)
                                        .PosRotChanged = True
                                    End With
                                ElseIf Nodes(ni).Type = "Text Node" Then
                                    With Nodes(ni).TextNodeData
                                        If .StringVariable.ToLower.Trim = Variable Then
                                            .Text = Value
                                            .Modified = True
                                        End If
                                    End With
                                ElseIf Nodes(ni).Type = "Compass Node" Then
                                    With Nodes(ni).CompassNodeData
                                        If .ValueVariable.ToLower.Trim = Variable Then
                                            .Value = Value
                                            .ValueChanged = True
                                        End If
                                    End With
                                End If
                            End If
                        Next
                    Next
                    'Render this variable scene
                    Dim screenimage As New Bitmap(800, 600)
                    Dim g As Graphics = Graphics.FromImage(screenimage)
                    RenderNodes(g)
                    g.Dispose()
                    simulationimages(i) = screenimage


                    ProgressAdapter(i * 100 \ (SimulationScript.Items(0).SubItems.Count))
                Next
                ProgressAdapter(100)
                Dim simindex As Integer = 1
                Do
                    PictureBoxAdapter(simulationimages(simindex), Me)
                    simindex += 1
                    If simindex = simulationimages.Count Then simindex = 1
                    Thread.Sleep(speed)
                Loop

            Else
                MsgBox("Simulation script is invalid.", MsgBoxStyle.Critical)
            End If
        Else
            MsgBox("Simulation file not found.", MsgBoxStyle.Critical)
        End If
    End Sub
End Class
