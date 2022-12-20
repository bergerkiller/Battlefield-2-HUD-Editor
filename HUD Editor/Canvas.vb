Public Class Canvas
    Private _Showbounds As Boolean = False
    Private _DrawRefCross As Boolean = False
    Private _BGImage As Image
    Private _OVImage As Image
    Private _RCImage As Image
    Private _DrawSelSquare As Boolean = True
    Private _UseFixedRes As Boolean = True
    Public Root As New Node("Global", "Split Node")
    Public SelectionSquare As New SelectionSquare
    Public Event SelectionChanged()
    Public Event SelectionModified()

#Region "Mouse moving and events"
    Dim MouseStart As Point
    Dim StartMoveRect As Rectangle
    Dim StartMoveRot As Integer
    Dim ResizeType As SelectionType = 0
    Dim IsResizing As Boolean = False
    Dim beforedcrot As Integer
    Dim beforedcreg As Rectangle
    Dim dcchangemode As Integer = 0
    Dim isDrawingSelRect As Boolean = False

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        SetCursor(SelectionType.Outside, 0)
        If IsResizing = True Then InvokeSelectionChanged(False)
        IsResizing = False
        If isDrawingSelRect = True Then
            Cursor.Current = Cursors.WaitCursor
            Dim rect As Rectangle = SelectionSquare.Rectangle
            rect.X += Me.DrawOffset.X
            rect.Y += Me.DrawOffset.Y
            rect.X *= Me.Scale.X
            rect.Y *= Me.Scale.Y
            rect.Width *= Me.Scale.X
            rect.Height *= Me.Scale.Y
            For Each n As Node In Me.Root.All
                If n.Render = True AndAlso n.Intersects(rect) = True AndAlso n.Type <> "Split Node" Then n.Selected = True
            Next
            isDrawingSelRect = False
            Me.UpdateSelectionSquare()
            Me.InvokeSelectionChanged()
            SelectionSquare.UpdateOnScreen(True)
            Cursor.Current = Cursors.Default
        End If
    End Sub
    Protected Overrides Sub OnMouseDoubleClick(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDoubleClick(e)
        If e.Button = Windows.Forms.MouseButtons.Left And Me._Showbounds = False AndAlso Me.SelectedNodes.Count = 1 Then
            If dcchangemode = 0 Then
                'force middle
                beforedcreg = New Rectangle(Me.SelectedNodes(0).Manage.Location, Me.SelectedNodes(0).Manage.Size)
                beforedcrot = Me.SelectedNodes(0).Manage.Rotation
                Me.SelectedNodes(0).Manage.Location = New Point(400 - beforedcreg.Size.Width * 0.5, 300 - beforedcreg.Size.Height * 0.5)
                Me.SelectedNodes(0).Manage.Size = beforedcreg.Size
                Me.SelectedNodes(0).Manage.Rotation = beforedcrot
                dcchangemode = 1
            ElseIf dcchangemode = 1 Then
                'full screen
                Me.SelectedNodes(0).Manage.Location = New Point(0, 0)
                Me.SelectedNodes(0).Manage.Size = New Size(800, 600)
                Me.SelectedNodes(0).Manage.Rotation = 0
                dcchangemode = 2
            ElseIf dcchangemode = 2 Then
                'reset
                Me.SelectedNodes(0).Manage.Location = beforedcreg.Location
                Me.SelectedNodes(0).Manage.Size = beforedcreg.Size
                Me.SelectedNodes(0).Manage.Rotation = beforedcrot
                dcchangemode = 0
            End If
            UpdateScreen()
            RaiseEvent SelectionModified()
        End If
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If IsInSimulationMode = False And e.Button = Windows.Forms.MouseButtons.Left Then
            MouseStart = Me.CursorPosition()
            If _Showbounds = True Then
                Dim snode As Node = Root.GetNodeAtPoint(CursorPosition(False), True, Node.SelectType.Deselected)
                If IsNothing(snode) Then snode = Root.GetNodeAtPoint(CursorPosition(False), True, Node.SelectType.Selected)
                If IsNothing(snode) Then
                    Me.DeselectAll()
                    isDrawingSelRect = True
                    SelectionSquare.Active = False
                    SelectionSquare.Display = 0
                    SelectionSquare.Rectangle = New Rectangle(0, 0, 1, 1)
                Else
                    If snode.Selected = True Then snode.Selected = False Else snode.Selected = True
                    InvokeSelectionChanged()
                    MyBase.Invalidate()
                End If
                IsResizing = False
            Else
                ResizeType = SelectionSquare.GetSelectionType(Me.CursorPosition())
                StartMoveRect = SelectionSquare.Rectangle
                StartMoveRot = SelectionSquare.Rotation.Value
                If ResizeType <> SelectionType.Outside Then
                    SetCursor(ResizeType, SelectionSquare.Rotation.Value)
                    IsResizing = True
                Else
                    Dim snode As Node = Root.GetNodeAtPoint(CursorPosition(False), False)
                    DeselectAll()
                    If Not IsNothing(snode) Then
                        If snode.Selected = False Then
                            snode.Selected = True
                        ElseIf snode.Type = "Object Marker Node" Then
                            snode.ObjectMarkerNode.DisplayState += 1
                            If snode.ObjectMarkerNode.DisplayState > 3 Then snode.ObjectMarkerNode.DisplayState = 0
                            snode.UpdateOnScreen(True)
                        End If
                    Else
                        isDrawingSelRect = True
                        SelectionSquare.Active = False
                        SelectionSquare.Display = 0
                        SelectionSquare.Rectangle = New Rectangle(0, 0, 1, 1)
                    End If
                    InvokeSelectionChanged()
                End If
            End If
        End If
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        'set button node states
        For Each node As Node In Me.Root.All
            If node.Type = "Button Node" Then
                Dim rect As New Rectangle(node.ButtonNode.Location.Value, node.ButtonNode.Size.Value)
                If node.ButtonNode.UseMouseArea = True Then
                    rect = node.ButtonNode.MouseArea
                    rect.X += node.ButtonNode.Location.X
                    rect.Y += node.ButtonNode.Location.Y
                End If
                Dim pos As Point = CursorPosition()
                pos.X -= rect.X
                pos.Y -= rect.Y
                If pos.X >= 0 And pos.Y >= 0 And pos.X <= rect.Width And pos.Y < rect.Height Then
                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        node.ButtonNode.DisplayState = ButtonNodeState.Clicked
                    Else
                        node.ButtonNode.DisplayState = ButtonNodeState.Hovered
                    End If
                Else
                    node.ButtonNode.DisplayState = ButtonNodeState.Inactive
                End If
            ElseIf node.Type = "Object Marker Node" Then
                Dim pos As Point = Me.CursorPosition
                pos.X -= node.Manage.Location.X
                pos.Y -= node.Manage.Location.Y
                node.ObjectMarkerNode.MarkerLocation.Value = pos
            End If
            If node.Changed = True Then node.UpdateOnScreen()
        Next
        If IsInSimulationMode = False Then
            If e.Button <> Windows.Forms.MouseButtons.Left Then IsResizing = False
            If isDrawingSelRect = True Then
                Dim p As Point = Me.CursorPosition
                SelectionSquare.Location.Value = MouseStart
                If SelectionSquare.Location.X > p.X Then SelectionSquare.Location.X = p.X
                If SelectionSquare.Location.Y > p.Y Then SelectionSquare.Location.Y = p.Y
                SelectionSquare.Size.Width = Math.Abs(MouseStart.X - p.X)
                SelectionSquare.Size.Height = Math.Abs(MouseStart.Y - p.Y)
                SelectionSquare.Active = True
                SelectionSquare.Display = 3
                SelectionSquare.Rotation.Value = 0
                SelectionSquare.RotationEnabled = False
                SelectionSquare.UpdateOnScreen()
            ElseIf IsResizing = False Then
                Dim type As SelectionType = SelectionSquare.GetSelectionType(Me.CursorPosition())
                If type <> SelectionType.Outside And Me._Showbounds = True Then type = SelectionType.Middle
                SetCursor(type, SelectionSquare.Rotation.Value)
            ElseIf IsResizing = True Then
                Try
                    If e.Button = Windows.Forms.MouseButtons.Left And SelectedNodes.Count <> 0 Then
                        Dim offset As Point = Point.Subtract(CursorPosition, MouseStart)

                        If SelectedNodes.Count > 1 Then
                            MouseStart = CursorPosition()
                            For Each Node As Node In SelectedNodes
                                Dim pos As Point = Node.Manage.Location
                                pos.X += offset.X
                                pos.Y += offset.Y
                                Node.Manage.Location = pos
                                Node.UpdateOnScreen()
                            Next
                            UpdateSelectionSquare()
                            SelectionSquare.UpdateOnScreen()
                            RaiseEvent SelectionModified()
                        ElseIf SelectedNodes.Count = 1 And ResizeType <> SelectionType.Outside Then
                            Dim NewPosX As Integer = StartMoveRect.X
                            Dim NewPosY As Integer = StartMoveRect.Y
                            Dim NewSizeW As Integer = StartMoveRect.Width
                            Dim NewSizeH As Integer = StartMoveRect.Height
                            Dim NewRot As Integer = StartMoveRot
                            If ResizeType = SelectionType.Middle Then
                                'Move
                                NewPosX += offset.X
                                NewPosY += offset.Y
                            ElseIf ResizeType = SelectionType.Rotation Then
                                'Rotate to cursor
                                If offset.X <> 0 Or offset.Y <> 0 Then
                                    If offset.X = 0 And offset.Y < 0 Then
                                        NewRot = 0
                                    ElseIf offset.X = 0 And offset.Y > 0 Then
                                        NewRot = 180
                                    ElseIf offset.X > 0 And offset.Y = 0 Then
                                        NewRot = 90
                                    ElseIf offset.X < 0 And offset.Y = 0 Then
                                        NewRot = 270
                                    ElseIf offset.X > 0 And offset.Y < 0 Then
                                        'topright
                                        NewRot = Math.Atan(Math.Abs(offset.X / offset.Y)) * 180 / Math.PI
                                    ElseIf offset.X > 0 And offset.Y > 0 Then
                                        'bottomright
                                        NewRot = Math.Atan(Math.Abs(offset.Y / offset.X)) * 180 / Math.PI + 90
                                    ElseIf offset.X < 0 And offset.Y > 0 Then
                                        'bottomleft
                                        NewRot = Math.Atan(Math.Abs(offset.X / offset.Y)) * 180 / Math.PI + 180
                                    ElseIf offset.X < 0 And offset.Y < 0 Then
                                        'topleft
                                        NewRot = Math.Atan(Math.Abs(offset.Y / offset.X)) * 180 / Math.PI + 270
                                    End If
                                    If NewRot = 360 Then NewRot = 0
                                End If
                            Else
                                'Convert point to square-related
                                Dim point As Point = CursorPosition()
                                Dim Square As Rectangle = StartMoveRect
                                point.X -= Square.X + Square.Width * 0.5
                                point.Y -= Square.Y + Square.Height * 0.5
                                Dim rotradian As Double = StartMoveRot * Math.PI / -180
                                If point.X > 0 Then rotradian += 0.5 * Math.PI
                                If point.X < 0 Then rotradian += 1.5 * Math.PI
                                If point.Y > 0 And point.X = 0 Then rotradian += Math.PI
                                If point.Y <> 0 And point.X <> 0 Then rotradian += Math.Atan(point.Y / point.X)
                                Dim radius As Double = Math.Sqrt(point.X ^ 2 + point.Y ^ 2)
                                offset.X = Math.Sin(rotradian) * radius + Square.Width * 0.5
                                offset.Y = Square.Height * 0.5 - Math.Cos(rotradian) * radius
                                'Disabling offset if not used
                                If ResizeType = SelectionType.TopMiddle Or ResizeType = SelectionType.BottomMiddle Then offset.X = 0
                                If ResizeType = SelectionType.MiddleLeft Or ResizeType = SelectionType.MiddleRight Then offset.Y = 0
                                'Using correct movement; 
                                If ResizeType = SelectionType.TopMiddle Then
                                    If offset.Y >= NewSizeH Then offset.Y = NewSizeH
                                    NewPosY += offset.Y
                                    NewSizeH -= offset.Y
                                ElseIf ResizeType = SelectionType.BottomMiddle Then
                                    If offset.Y < 0 Then offset.Y = 0
                                    offset.Y -= Square.Height
                                    NewSizeH += offset.Y
                                ElseIf ResizeType = SelectionType.MiddleRight Then
                                    If offset.X < 0 Then offset.X = 0
                                    offset.X -= Square.Width
                                    NewSizeW += offset.X
                                ElseIf ResizeType = SelectionType.MiddleLeft Then
                                    If offset.X >= NewSizeW Then offset.X = NewSizeW
                                    NewPosX += offset.X
                                    NewSizeW -= offset.X
                                ElseIf ResizeType = SelectionType.TopLeft Then
                                    If offset.X >= NewSizeW Then offset.X = NewSizeW
                                    If offset.Y >= NewSizeH Then offset.Y = NewSizeH
                                    NewPosX += offset.X
                                    NewPosY += offset.Y
                                    NewSizeW -= offset.X
                                    NewSizeH -= offset.Y
                                ElseIf ResizeType = SelectionType.TopRight Then
                                    If offset.X < 0 Then offset.X = 0
                                    If offset.Y >= NewSizeH Then offset.Y = NewSizeH
                                    NewPosY += offset.Y
                                    NewSizeH -= offset.Y
                                    offset.X -= Square.Width
                                    NewSizeW += offset.X
                                ElseIf ResizeType = SelectionType.BottomRight Then
                                    If offset.X < 0 Then offset.X = 0
                                    If offset.Y < 0 Then offset.Y = 0
                                    offset.X -= Square.Width
                                    NewSizeW += offset.X
                                    offset.Y -= Square.Height
                                    NewSizeH += offset.Y
                                ElseIf ResizeType = SelectionType.BottomLeft Then
                                    If offset.X >= NewSizeW Then offset.X = NewSizeW
                                    If offset.Y < 0 Then offset.Y = 0
                                    NewPosX += offset.X
                                    NewSizeW -= offset.X
                                    offset.Y -= Square.Height
                                    NewSizeH += offset.Y
                                End If
                                NewPosX -= offset.X * 0.5 * (1 - Math.Cos(StartMoveRot / 180 * Math.PI))
                                NewPosY -= offset.Y * 0.5 * (1 - Math.Cos(StartMoveRot / 180 * Math.PI))
                                NewPosX -= offset.Y * 0.5 * Math.Sin(StartMoveRot / 180 * Math.PI)
                                NewPosY -= offset.X * -0.5 * Math.Sin(StartMoveRot / 180 * Math.PI)
                            End If
                            Dim NewPos As New Point(NewPosX, NewPosY)
                            Dim NewSize As New Size(SetValueBounds(NewSizeW, 1, 4096), SetValueBounds(NewSizeH, 1, 4096))
                            SelectedNodes(0).Manage.Location = NewPos
                            SelectedNodes(0).Manage.Size = NewSize
                            SelectedNodes(0).Manage.Rotation = NewRot
                            SelectedNodes(0).UpdateOnScreen()
                            UpdateSelectionSquare()
                            SelectionSquare.UpdateOnScreen()
                            RaiseEvent SelectionModified()
                        End If
                    End If
                Catch ex As Exception
                End Try
            End If
        End If
    End Sub
    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
        MyBase.OnKeyDown(e)
        If _Showbounds <> e.Control And IsInSimulationMode = False Then
            _Showbounds = e.Control
            MyBase.Invalidate()
        End If
        If IsInSimulationMode = False And e.KeyCode = Keys.Escape Then Me.DeselectAll()
    End Sub
    Protected Overrides Sub OnPreviewKeyDown(ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs)
        MyBase.OnPreviewKeyDown(e)
        If IsInSimulationMode = False Then
            If e.KeyCode = Keys.Left Or e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Right Then
                For Each n As Node In SelectedNodes
                    If e.KeyCode = Keys.Left Then n.Manage.Location_X -= 1
                    If e.KeyCode = Keys.Right Then n.Manage.Location_X += 1
                    If e.KeyCode = Keys.Up Then n.Manage.Location_Y -= 1
                    If e.KeyCode = Keys.Down Then n.Manage.Location_Y += 1
                    n.UpdateOnScreen()
                Next
                If e.KeyCode = Keys.Left Then StartMoveRect.X -= 1
                If e.KeyCode = Keys.Right Then StartMoveRect.X += 1
                If e.KeyCode = Keys.Up Then StartMoveRect.Y -= 1
                If e.KeyCode = Keys.Down Then StartMoveRect.Y += 1
                UpdateSelectionSquare()
                SelectionSquare.UpdateOnScreen()
                RaiseEvent SelectionModified()
            ElseIf e.KeyCode = Keys.Oemplus Or e.KeyCode = Keys.Add Then
                OnMouseWheel(New MouseEventArgs(0, 0, 0, 0, 1))
            ElseIf e.KeyCode = Keys.OemMinus Or e.KeyCode = Keys.Subtract Then
                OnMouseWheel(New MouseEventArgs(0, 0, 0, 0, -1))
            End If
        End If
    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As System.Windows.Forms.KeyEventArgs)
        MyBase.OnKeyUp(e)
        If _Showbounds <> e.Control And IsInSimulationMode = False Then
            _Showbounds = e.Control
            MyBase.Invalidate()
        End If
    End Sub
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)
        MyBase.DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        ResetScreenGraphics(e.Graphics)
        If Not IsNothing(Me._BGImage) Then e.Graphics.DrawImage(Me._BGImage, New Point(0, 0))
        If Not IsNothing(Me._RCImage) And Me._DrawRefCross = True Then e.Graphics.DrawImage(Me._RCImage, New Point(0, 0))
        Root.Draw(e.Graphics, New Color(255, 255, 255, 255))
        If Not IsNothing(Me._OVImage) Then e.Graphics.DrawImage(_OVImage, New Point(0, 0))
        If IsInSimulationMode = False Then
            'drawing add. graphics
            Dim snodes As List(Of Node) = SelectedNodes
            If (snodes.Count >= 1 And Me._DrawSelSquare = True) Or isDrawingSelRect = True Then
                If isDrawingSelRect = False Then UpdateSelectionSquare()
                SelectionSquare.Draw(e.Graphics)
            Else
                SelectionSquare.Display = 0
            End If
            If _Showbounds = True Then
                'Render colored boundaries
                For Each Node As Node In Root.All(Node.SelectType.Deselected)
                    Node.DrawBounds(e.Graphics)
                Next
                For Each Node As Node In Root.All(Node.SelectType.Selected)
                    Node.DrawBounds(e.Graphics)
                Next
            End If
        End If
        e.Graphics.ResetTransform()
        If Me.DrawOffset.Y > 0 Then
            e.Graphics.FillRectangle(Brushes.Black, New Rectangle(0, 0, MyBase.Size.Width, Me.DrawOffset.Y))
            e.Graphics.FillRectangle(Brushes.Black, New Rectangle(0, Me.DrawOffset.Y + Me.Scale.Y * 600, MyBase.Size.Width, Me.DrawOffset.Y))
        ElseIf Me.DrawOffset.X > 0 Then
            e.Graphics.FillRectangle(Brushes.Black, New Rectangle(0, 0, Me.DrawOffset.X, MyBase.Size.Height))
            e.Graphics.FillRectangle(Brushes.Black, New Rectangle(Me.DrawOffset.X + Me.Scale.X * 800, 0, Me.DrawOffset.X, MyBase.Size.Height))
        End If
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        If ApplicationLoaded = True Then
            Me.UpdateSelectionSquare()
            SelectionSquare.UpdateOnScreen(True)
            For Each Node As Node In Root.All
                Node.UpdateOnScreen()
            Next
            Me.Invalidate()
        End If
    End Sub
    Protected Overrides Sub OnMouseWheel(ByVal e As Windows.Forms.MouseEventArgs)
        MyBase.OnMouseWheel(e)
        If e.Delta <> 0 Then
            For Each n As Node In Me.SelectedNodes
                Dim newsize As Size = n.Manage.Size
                For i As Integer = 5 To 200
                    Dim factor As Single = 1 + i * 0.01
                    If e.Delta < 0 Then newsize = New Size(n.Manage.Size_Width / factor, n.Manage.Size_Height / factor)
                    If e.Delta > 0 Then newsize = New Size(n.Manage.Size_Width * factor, n.Manage.Size_Height * factor)
                    If Not newsize.Width = n.Manage.Size_Width And Not newsize.Height = n.Manage.Size_Height Then Exit For
                Next
                n.Manage.Location_X -= (newsize.Width - n.Manage.Size_Width) * 0.5
                n.Manage.Location_Y -= (newsize.Height - n.Manage.Size_Height) * 0.5
                n.Manage.Size = newsize
                n.UpdateOnScreen()
            Next
            UpdateSelectionSquare()
            StartMoveRect = SelectionSquare.Rectangle
            SelectionSquare.UpdateOnScreen()
            RaiseEvent SelectionModified()
        End If
    End Sub

    Public ReadOnly Property Nodes() As List(Of Node)
        Get
            Return Me.Root.All
        End Get
    End Property
    Public ReadOnly Property DrawnLocation() As Rectangle
        Get
            Return New Rectangle(Me.DrawOffset, New Size(800 * Me.Scale.X, 600 * Me.Scale.Y))
        End Get
    End Property
    Private Enum SelectionType
        Outside = 0
        TopLeft = 1
        TopMiddle = 2
        TopRight = 3
        MiddleLeft = 4
        MiddleRight = 5
        BottomLeft = 6
        BottomMiddle = 7
        BottomRight = 8
        Middle = 9
        Rotation = 10
    End Enum
    Private Sub SetCursor(ByVal ResizeType As SelectionType, ByVal RotationFactor As Integer)
        If ResizeType = SelectionType.Outside Then
            MyBase.Cursor = Cursors.Default
        ElseIf ResizeType = SelectionType.Middle Then
            MyBase.Cursor = Cursors.SizeAll
        ElseIf ResizeType = SelectionType.Rotation Then
            MyBase.Cursor = Cursors.Hand
        Else
            Dim cmode As Integer = 0
            If ResizeType = SelectionType.TopRight Or ResizeType = SelectionType.BottomLeft Then cmode = 1
            If ResizeType = SelectionType.MiddleLeft Or ResizeType = SelectionType.MiddleRight Then cmode = 2
            If ResizeType = SelectionType.TopLeft Or ResizeType = SelectionType.BottomRight Then cmode = 3
            If RotationFactor >= 23 And RotationFactor < 68 Then cmode += 1
            If RotationFactor >= 68 And RotationFactor < 113 Then cmode += 2
            If RotationFactor >= 113 And RotationFactor < 158 Then cmode += 3
            If RotationFactor >= 203 And RotationFactor < 248 Then cmode += 1
            If RotationFactor >= 248 And RotationFactor < 293 Then cmode += 2
            If RotationFactor >= 293 And RotationFactor < 338 Then cmode += 3
            If cmode > 3 Then cmode -= 4
            If cmode = 0 Then MyBase.Cursor = Cursors.SizeNS
            If cmode = 1 Then MyBase.Cursor = Cursors.SizeNESW
            If cmode = 2 Then MyBase.Cursor = Cursors.SizeWE
            If cmode = 3 Then MyBase.Cursor = Cursors.SizeNWSE
        End If
    End Sub
#End Region

    Public Sub InvokeSelectionChanged(Optional ByVal ClearDC As Boolean = True)
        If ClearDC = True Then dcchangemode = 0
        RaiseEvent SelectionChanged()
    End Sub
    Public Function CursorPosition(Optional ByVal Scaled As Boolean = True) As Point
        Dim p As Point = MyBase.PointToClient(Cursor.Position)
        If Scaled = True Then
            p.X -= Me.DrawOffset.X
            p.Y -= Me.DrawOffset.Y
            p.X /= Scale.X
            p.Y /= Scale.Y
        End If
        Return p
    End Function
    Public Shadows ReadOnly Property Scale() As PointF
        Get
            If Me._UseFixedRes = True Then
                If Me.Width / 800 < Me.Height / 600 Then Return New PointF(Me.Width / 800, Me.Width / 800)
                Return New PointF(Me.Height / 600, Me.Height / 600)
            Else
                Return New PointF(Me.Width / 800, Me.Height / 600)
            End If
        End Get
    End Property
    Public ReadOnly Property DrawOffset() As Point
        Get
            Dim offx As Integer = (Form1.MainScreen.Size.Width - 800 * Form1.MainScreen.Scale.X) * 0.5
            Dim offy As Integer = (Form1.MainScreen.Size.Height - 600 * Form1.MainScreen.Scale.Y) * 0.5
            Return New Point(offx, offy)
        End Get
    End Property
    Public ReadOnly Property SelectedNodes() As List(Of Node)
        Get
            Return Root.All(Node.SelectType.Selected)
        End Get
    End Property
    Public Sub DeselectAll()
        Root.DeselectAll()
        InvokeSelectionChanged()
    End Sub
    Public Shadows Property BackgroundImage() As Image
        Get
            Return _BGImage
        End Get
        Set(ByVal value As Image)
            If IsNothing(value) Then Me._BGImage = value Else Me._BGImage = ResizeImage(value, 800, 600, True)
            Me.Invalidate()
        End Set
    End Property
    Public Property OverlayImage() As Image
        Get
            Return _OVImage
        End Get
        Set(ByVal value As Image)
            If IsNothing(value) Then Me._OVImage = value Else Me._OVImage = ResizeImage(value, 800, 600, True)
            Me.Invalidate()
        End Set
    End Property
    Public Property ReferenceCrossImage() As Image
        Get
            Return Me._RCImage
        End Get
        Set(ByVal value As Image)
            If IsNothing(value) Then Me._RCImage = value Else Me._RCImage = ResizeImage(value, 800, 600, True)
            If Me._DrawRefCross = True Then MyBase.Invalidate()
        End Set
    End Property
    Public Sub HightlightItem(ByVal Name As String)
        Root.Highlighted = False
        If Name <> "" Then Root.GetNodeByName(Name).Highlighted = True
    End Sub

    Public Property ShowReferenceCross() As Boolean
        Get
            Return _DrawRefCross
        End Get
        Set(ByVal value As Boolean)
            If value <> Me._DrawRefCross Then
                Me._DrawRefCross = value
                Me.Invalidate()
            End If
        End Set
    End Property
    Public Property ShowSelectionSquare() As Boolean
        Get
            Return _DrawSelSquare
        End Get
        Set(ByVal value As Boolean)
            _DrawSelSquare = value
            If ApplicationLoaded = True Then SelectionSquare.UpdateOnScreen(True)
        End Set
    End Property
    Public Property UseFixedResolution() As Boolean
        Get
            Return Me._UseFixedRes
        End Get
        Set(ByVal value As Boolean)
            If value <> Me._UseFixedRes Then
                Me._UseFixedRes = value
                MyBase.Invalidate()
            End If
        End Set
    End Property
    Public Sub UpdateSelectionSquare()
        Dim snodes As List(Of Node) = Me.SelectedNodes
        If snodes.Count = 1 Then
            SelectionSquare.Location.Value = SelectedNodes(0).Manage.Location
            SelectionSquare.Size.Value = SelectedNodes(0).Manage.Size
            SelectionSquare.Rotation.Value = SelectedNodes(0).Manage.Rotation
            SelectionSquare.RotationEnabled = False
            If snodes(0).Type = "Picture Node" Then SelectionSquare.RotationEnabled = True
            If Me._DrawSelSquare = True Then
                SelectionSquare.Display = 1
            Else
                SelectionSquare.Display = 0
            End If
            SelectionSquare.Active = snodes(0).Type <> "Split Node"
        ElseIf snodes.Count > 1 Then
            Dim t As Rectangle
            Dim hasnode As Boolean = False
            For Each n As Node In snodes
                If n.Type <> "Split Node" And n.CanBeSaved = True Then
                    If hasnode = True Then t = CombineBounds(t, n.Bounds) Else t = n.Bounds
                    hasnode = True
                    t = CombineBounds(t, n.Bounds)
                End If
            Next
            SelectionSquare.Location.Value = t.Location
            SelectionSquare.Size.Value = t.Size
            SelectionSquare.Rotation.Value = 0
            SelectionSquare.Display = 2
            SelectionSquare.Active = hasnode
        Else
            SelectionSquare.Display = 0
            SelectionSquare.Active = False
        End If
    End Sub

End Class
