Imports System.Threading


Public Class SimulateDialog
    Dim threadsimulate As Thread
    Dim simfolder As String = ""
    Dim speed As Integer = 50


    Private Sub Simulator()
        Try
            If System.IO.Directory.Exists("Simulation\" & simfolder) Then
                Dim Variablevalues As New ListView
                Dim valuecount As Integer = -1
                'Loading variable lists
                For Each file As String In System.IO.Directory.GetFiles("Simulation\" & simfolder)
                    Dim item As New ListViewItem
                    item.Text = IO.Path.GetFileNameWithoutExtension(file)
                    Dim reader As New System.IO.StreamReader(file)
                    Do While reader.Peek <> -1
                        item.SubItems.Add(Val(reader.ReadLine))
                    Loop
                    If item.SubItems.Count < valuecount Or valuecount = -1 Then valuecount = item.SubItems.Count
                    Variablevalues.Items.Add(item)
                    reader.Close()
                Next
                '----end---
                valuecount -= 1
                Dim simulatedimages(valuecount - 1) As Image
                Dim loadperc As Integer = 0
                For i As Integer = 0 To valuecount - 1
                    loadperc = i * 100 \ valuecount
                    If loadperc >= 100 Then loadperc = 99
                    ProgressAdapter(loadperc)
                    '--Generate every single frame here--
                    Dim finimage As New Bitmap(800, 600)
                    Dim g As Graphics = Graphics.FromImage(finimage)
                    For nodeindex As Integer = 0 To NodeInformation.Items.Count - 1
                        Dim item As ListViewItem = NodeInformation.Items(nodeindex)
                        If NodesToRender.Items.Contains(item.Text.ToLower) Then
                            'Process this node and then node types
                            If item.SubItems(1).Text = "Picture Node" Then
                                Dim posX As Integer = Val(NodeInformation.Items(nodeindex).SubItems(9).Text)
                                Dim posY As Integer = Val(NodeInformation.Items(nodeindex).SubItems(10).Text)
                                Dim staticrot As Integer = Val(NodeInformation.Items(nodeindex).SubItems(11).Text)
                                Dim rotvarmidX As Integer = Val(NodeInformation.Items(nodeindex).SubItems(12).Text)
                                Dim rotvarmidY As Integer = Val(NodeInformation.Items(nodeindex).SubItems(13).Text)

                                Dim offvarXvalue As Integer = 0
                                Dim offvarYvalue As Integer = 0
                                Dim rotvarangle As Integer = 0
                                'Getting variable values
                                Dim offXvar As String = NodeInformation.Items(nodeindex).SubItems(14).Text
                                Dim offYvar As String = NodeInformation.Items(nodeindex).SubItems(15).Text
                                Dim rotvar As String = NodeInformation.Items(nodeindex).SubItems(16).Text
                                For Each variable As ListViewItem In Variablevalues.Items
                                    If variable.Text.Trim.ToLower = offXvar.Trim.ToLower Then
                                        'Set offset X var
                                        offvarXvalue = Val(variable.SubItems(i + 1).Text)
                                    End If
                                    If variable.Text.Trim.ToLower = offYvar.Trim.ToLower Then
                                        'Set offset Y var
                                        offvarYvalue = Val(variable.SubItems(i + 1).Text)
                                    End If
                                    If variable.Text.Trim.ToLower = rotvar.Trim.ToLower Then
                                        'Set rot var angle
                                        rotvarangle = Val(variable.SubItems(i + 1).Text)
                                    End If
                                Next
                                g.DrawImage(RenderPictureNode(SizedImage(nodeindex), staticrot, posX, posY, rotvarmidX, rotvarmidY, rotvarangle, offvarXvalue, offvarYvalue), New Point(0, 0))
                            End If
                        End If
                    Next
                    g.DrawImage(OVImage, New Point(0, 0))
                    simulatedimages(i) = finimage
                    g.Dispose()
                    '--end--
                Next
                ProgressAdapter(100)
                Do
                    Try
                        For i As Integer = 0 To simulatedimages.Count - 1
                            PictureBoxAdapter(simulatedimages(i), Me)
                            Thread.Sleep(speed)
                        Next
                    Catch
                    End Try
                Loop
            End If
        Catch
        End Try
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

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        simfolder = ComboBox1.SelectedItem
        Button1.Enabled = ComboBox1.SelectedIndex <> -1
    End Sub
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
        Try
            threadsimulate.Abort()
        Catch
        End Try
        ProgressBar1.Value = 0
        ProgressBar1.Visible = False
        ComboBox1.Enabled = True
        Button2.Enabled = False
        Button1.Enabled = True
        Edited = True
    End Sub
    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.ValueChanged
        speed = 1000 - TrackBar1.Value
    End Sub

    Private Sub ComboBox1_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.DropDown
        Dim selitem As String = ComboBox1.SelectedItem
        ComboBox1.Items.Clear()
        If System.IO.Directory.Exists("Simulation") Then
            For Each folder As String In System.IO.Directory.GetDirectories("Simulation")
                ComboBox1.Items.Add(IO.Path.GetFileName(folder))
            Next
            ComboBox1.SelectedItem = selitem
        End If
    End Sub

    Private Sub SimulateDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            threadsimulate.Abort()
        Catch
        End Try
        ProgressBar1.Value = 0
        ProgressBar1.Visible = False
        ComboBox1.Enabled = True
        Button2.Enabled = False
        Button1.Enabled = True
        Edited = True
    End Sub
End Class
