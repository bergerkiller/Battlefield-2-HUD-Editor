Public Class Node
#Region "Internal"
    Sub New(ByVal Name As String, ByVal Type As String, Optional ByVal CanBeSaved As Boolean = True)
        Me.CanBeSaved = CanBeSaved
        Name = Name.Trim
        Me.Name = Name
        Me.Type = Type
        If Me.Type = "Picture Node" Then
            Me.PictureNode = New PictureNode
            Me.PictureNode.RotateVariable = New Variable(Me)
        ElseIf Me.Type = "Bar Node" Then
            Me.BarNode = New BarNode
            Me.BarNode.ValueVariable = New Variable(Me)
        ElseIf Me.Type = "Text Node" Then
            Me.TextNode = New TextNode
            Me.TextNode.StringVariable = New Variable(Me)
        ElseIf Me.Type = "Compass Node" Then
            Me.CompassNode = New CompassNode
            Me.CompassNode.ValueVariable = New Variable(Me)
        ElseIf Me.Type = "Button Node" Then
            Me.ButtonNode = New ButtonNode
        ElseIf Me.Type = "Object Marker Node" Then
            Me.ObjectMarkerNode = New ObjectMarkerNode
        ElseIf Me.Type = "Hover Node" Then
            Me.HoverNode = New HoverNode
            Me.HoverNode.HoverNodePosX = New VariableHandler(Me.Name & "HoverNodeXPos", VariableType.VT_Position, 0)
            Me.HoverNode.HoverNodePosY = New VariableHandler(Me.Name & "HoverNodeYPos", VariableType.VT_Position, 0)
        End If
        Me.PosXVariable = New Variable(Me)
        Me.PosYVariable = New Variable(Me)
        Me.ShowVariable = New Variable(Me)
        Me.AlphaVariable = New Variable(Me)
        Me.ShowVariable.Value = True
        Me.Manage = New ManagerClass(Me)
    End Sub

    Public CanBeSaved As Boolean = False
    Private _highlighted As Boolean = False
    Private _selected As Boolean = False
    Private _render As Boolean = True
    Private _NodeData As Object
    Private ReadOnly Property Transform() As System.Drawing.Drawing2D.Matrix
        Get
            Transform = New System.Drawing.Drawing2D.Matrix
            ResetScreenGraphics(Transform)
            Dim locX As Integer = Me.Manage.Location.X
            Dim locY As Integer = Me.Manage.Location.Y
            If Me.PosXVariable.VariableName <> "" Then locX = Me.PosXVariable.Value + 400
            If Me.PosYVariable.VariableName <> "" Then locY = Me.PosYVariable.Value + 300
            If Me.Type = "Picture Node" Then
                Transform.Translate(Me.PictureNode.CenterPoint.X + 400, Me.PictureNode.CenterPoint.Y + 300)
                Transform.Rotate(Val(Me.PictureNode.RotateVariable.Value))
                Transform.Translate(locX - Me.PictureNode.CenterPoint.X - 400 + Me.PictureNode.Size.Width * 0.5, locY - Me.PictureNode.CenterPoint.Y - 300 + Me.PictureNode.Size.Height * 0.5)
                Transform.Rotate(Me.PictureNode.Rotation.Value)
                Transform.Translate(Me.PictureNode.Size.Width * -0.5, Me.PictureNode.Size.Height * -0.5)
            ElseIf Me.Type = "Bar Node" Then
                Transform.Translate(locX, locY)
            ElseIf Me.Type = "Text Node" Then
                Transform.Translate(locX, locY)
                If Me.TextNode.Style = 0 Then
                    Transform.Translate(Me.TextNode.Size.Width * 0.5 - Me.TextNode.TextWidth - 1 + Me.TextNode.TextWidth * 0.5, 0)
                ElseIf Me.TextNode.Style = 1 Then
                    Transform.Translate(Me.TextNode.Size.Width - 1 - Me.TextNode.TextWidth, 0)
                End If
            ElseIf Me.Type = "Compass Node" Then
                Transform.Translate(locX, locY)
            ElseIf Me.Type = "Button Node" Then
                Transform.Translate(locX, locY)
            ElseIf Me.Type = "Object Marker Node" Then
                Transform.Translate(locX, locY)
            ElseIf Me.Type = "Hover Node" Then
                Transform.Translate(locX, locY)
            End If
        End Get
    End Property
    Private OldRegion As New System.Drawing.Region
    Private ReadOnly Property NewRegion() As System.Drawing.Region
        Get
            Dim rect As New Rectangle(New Point(0, 0), Me.Manage.Size)
            If Me.Type = "Text Node" Then rect.Size = New Size(Me.TextNode.TextWidthRegion + 2, 15)
            rect.Inflate(1, 1)
            NewRegion = New System.Drawing.Region(rect)
            NewRegion.Transform(Me.Transform)
        End Get
    End Property
    Public Property Selected() As Boolean
        Get
            Return _selected
        End Get
        Set(ByVal value As Boolean)
            If value <> _selected Then
                _selected = value
                Form1.MainScreen.UpdateSelectionSquare()
                Form1.MainScreen.SelectionSquare.UpdateOnScreen(True)
            End If
        End Set
    End Property
    Public Property Render() As Boolean
        Get
            If IsNothing(Me.Parent) OrElse Me.Parent.Render = True Then Return Me._render Else Return False
        End Get
        Set(ByVal value As Boolean)
            If value <> Me._render Then
                Me._render = value
                Me.UpdateOnScreen()
            End If
        End Set
    End Property
    Public Property Highlighted() As Boolean
        Get
            Return Me._highlighted
        End Get
        Set(ByVal value As Boolean)
            Me._highlighted = value
            Me.UpdateOnScreen()
            For Each n As Node In Me.Children
                n.Highlighted = value
            Next
        End Set
    End Property
    Public FailedLines As String = ""

    Public ReadOnly Property Tree() As String
        Get
            If IsNothing(Me.Parent) Then
                Return Me.Name
            Else
                Return Me.Parent.Tree & "\" & Me.Name
            End If
        End Get
    End Property
    Public Property Index() As Integer
        Get
            If IsNothing(Me.Parent) Then Return -1
            For i As Integer = 0 To Me.Parent.Children.Count - 1
                If Me.Parent.Children(i) Is Me Then Return i
            Next
            Return -1
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(Me.Parent) And value > -1 AndAlso Me.Parent.Children.Count > value Then
                Me.Parent.RemoveChildAt(Me.Index)
                Me.Parent.InsertChild(value, Me)
                Me.UpdateOnScreen()
            End If
        End Set
    End Property
    Public ReadOnly Property Bounds() As Rectangle
        Get
            Dim br As New Point(Me.Manage.Location.X + Me.Manage.Size.Width, Me.Manage.Location.Y + Me.Manage.Size.Height)
            Dim tl As New Point(Me.Manage.Location.X, Me.Manage.Location.Y)
            Dim MiddlePoint As New Point(Me.Manage.Location.X + Me.Manage.Size.Width * 0.5, Me.Manage.Location.Y + Me.Manage.Size.Height * 0.5)
            Dim points(3) As Point
            points(0) = RotatePoint(tl, MiddlePoint, Me.Manage.Rotation)
            points(1) = RotatePoint(br, MiddlePoint, Me.Manage.Rotation)
            points(2) = RotatePoint(New Point(Me.Manage.Location.X + Me.Manage.Size.Width, Me.Manage.Location.Y), MiddlePoint, Me.Manage.Rotation)
            points(3) = RotatePoint(New Point(Me.Manage.Location.X, Me.Manage.Location.Y + Me.Manage.Size.Height), MiddlePoint, Me.Manage.Rotation)
            For Each p As Point In points
                If p.X < tl.X Then tl.X = p.X
                If p.Y < tl.Y Then tl.Y = p.Y
                If p.X > br.X Then br.X = p.X
                If p.Y > br.Y Then br.Y = p.Y
            Next
            Return Rectangle.FromLTRB(tl.X, tl.Y, br.X, br.Y)
        End Get
    End Property
    Public Property Changed() As Boolean
        Get
            If Me.Type = "Picture Node" Then
                If Me.PictureNode.Changed = True Then Return True
            ElseIf Me.Type = "Bar Node" Then
                If Me.BarNode.Changed = True Then Return True
            ElseIf Me.Type = "Text Node" Then
                If Me.TextNode.Changed = True Then Return True
            ElseIf Me.Type = "Compass Node" Then
                If Me.CompassNode.Changed = True Then Return True
            ElseIf Me.Type = "Button Node" Then
                If Me.ButtonNode.Changed = True Then Return True
            ElseIf Me.Type = "Object Marker Node" Then
                If Me.ObjectMarkerNode.Changed = True Then Return True
            End If
            If Me.AlphaVariable.Changed = True Then Return True
            If Me.ShowVariable.Changed = True Then Return True
            If Me.PosXVariable.Changed = True Then Return True
            If Me.PosYVariable.Changed = True Then Return True
            Return Me.Color.Changed
        End Get
        Set(ByVal value As Boolean)
            If Me.Type = "Picture Node" Then Me.PictureNode.Changed = value
            If Me.Type = "Bar Node" Then Me.BarNode.Changed = value
            If Me.Type = "Text Node" Then Me.TextNode.Changed = value
            If Me.Type = "Compass Node" Then Me.CompassNode.Changed = value
            If Me.Type = "Button Node" Then Me.ButtonNode.Changed = value
            If Me.Type = "Object Marker Node" Then Me.ObjectMarkerNode.Changed = value
            Me.Color.Changed = value
            Me.PosXVariable.Changed = value
            Me.PosYVariable.Changed = value
            Me.ShowVariable.Changed = value
            Me.AlphaVariable.Changed = value
        End Set
    End Property
    Public ReadOnly Property Show() As Boolean
        Get
            If Me.ShowVariable.Value = False Then Return False
            If Not IsNothing(Me.Parent) Then Return Me.Parent.Show
            Return True
        End Get
    End Property

    Public Sub Draw(ByRef g As Graphics, ByVal Color As Color)
        ResetScreenGraphics(g)
        'If IsNothing(ColorM) Then ColorM = Me.Color.ToColorMatrix Else ColorM = CombineMatrices(ColorM, Me.Color.ToColorMatrix)
        If IsNothing(Color) Then Color = Me.Color Else Color.CombineWith(Me.Color)
        'If Me.BlendEffectA <> -1 And Me.BlendEffectB <> -1 Then
        '    ApplyBlendEffect(ColorM, Me.BlendEffectA, Me.BlendEffectB)
        'End If
        If Me.Render = True And g.Clip.IsVisible(Me.Bounds) And (Me.Show = True Or IsInSimulationMode = False) Then
            Try
                Dim dhl As Boolean = Me.Highlighted
                If Me.Type = "Picture Node" Then
                    g.Transform = Me.Transform
                    g.DrawImage(Me.PictureNode.SizedImage, New Rectangle(0, 0, Me.PictureNode.SizedImage.Width, Me.PictureNode.SizedImage.Height), 0, 0, Me.PictureNode.SizedImage.Width, Me.PictureNode.SizedImage.Height, GraphicsUnit.Pixel, GetImgAttr(Color.ToColorMatrix))
                ElseIf Me.Type = "Bar Node" Then
                    g.Transform = Me.Transform
                    Dim value As Single = Val(Me.BarNode.ValueVariable.Value)
                    If IsNothing(Me.BarNode.ValueVariable.Value) Then value = 0.5
                    Dim Edestrect As New Rectangle(0, 0, BarNode.Size.Width, BarNode.Size.Height)
                    Dim Fdestrect As New Rectangle(0, 0, BarNode.Size.Width, BarNode.Size.Height)
                    If Me.BarNode.Style = 2 Or Me.BarNode.Style = 3 Then
                        Fdestrect.Width *= value
                        Edestrect.Width *= (1 - value)
                    ElseIf Me.BarNode.Style = 0 Or Me.BarNode.Style = 1 Then
                        Fdestrect.Height *= value
                        Edestrect.Height *= (1 - value)
                    End If
                    If Me.BarNode.Style = 0 Then Fdestrect.Y = (1 - value) * BarNode.Size.Height
                    If Me.BarNode.Style = 1 Then Edestrect.Y = value * BarNode.Size.Height
                    If Me.BarNode.Style = 2 Then Fdestrect.X = (1 - value) * BarNode.Size.Width
                    If Me.BarNode.Style = 3 Then Edestrect.X = value * BarNode.Size.Width
                    g.DrawImage(Me.BarNode.SizedFullImage, Fdestrect, Fdestrect.X, Fdestrect.Y, Fdestrect.Width, Fdestrect.Height, GraphicsUnit.Pixel, GetImgAttr(Color.ToColorMatrix))
                    g.DrawImage(Me.BarNode.SizedEmptyImage, Edestrect, Edestrect.X, Edestrect.Y, Edestrect.Width, Edestrect.Height, GraphicsUnit.Pixel, GetImgAttr(Color.ToColorMatrix))
                ElseIf Me.Type = "Text Node" Then
                    g.Transform = Me.Transform
                    Dim myBrush As New Drawing2D.LinearGradientBrush(New Rectangle(0, 0, 1, 1), Color.ToDrawingColor, Color.ToDrawingColor, Drawing2D.LinearGradientMode.Horizontal)
                    g.DrawString(Me.TextNode.Text, New Font("Arial", 8, FontStyle.Regular), myBrush, 0, 0)
                ElseIf Me.Type = "Compass Node" Then
                    g.Transform = Me.Transform
                    Dim value As Single = Me.CompassNode.ValueVariable.Value()
                    If Me.CompassNode.ValueVariable.VariableType = VariableType.VT_Angle Then value = value / 360
                    Dim bmp As New Bitmap(Me.CompassNode.Size.Width, Me.CompassNode.Size.Height)
                    Dim cg As Graphics = Graphics.FromImage(bmp)
                    If Me.CompassNode.Style = CompassNodeStyle.Horizontal Then
                        Dim offset As Integer = -1 * (Me.CompassNode.TextureSize.Width - Me.CompassNode.Border.Value) * value - Me.CompassNode.Offset.Value
                        cg.DrawImage(Me.CompassNode.SizedImage, New Point(offset, 0))
                        cg.DrawImage(Me.CompassNode.SizedImage, New Point(offset + Me.CompassNode.TextureSize.Width - Me.CompassNode.Border.Value, 0))
                    ElseIf Me.CompassNode.Style = CompassNodeStyle.Vertical Then
                        Dim offset As Integer = -1 * (Me.CompassNode.TextureSize.Height - Me.CompassNode.Border.Value) * value - Me.CompassNode.Offset.Value
                        cg.DrawImage(Me.CompassNode.SizedImage, New Point(0, offset))
                        cg.DrawImage(Me.CompassNode.SizedImage, New Point(0, offset + Me.CompassNode.TextureSize.Height - Me.CompassNode.Border.Value))
                    End If
                    cg.Dispose()
                    g.DrawImage(bmp, New Rectangle(0, 0, Me.CompassNode.Size.Width, Me.CompassNode.Size.Height), 0, 0, Me.CompassNode.Size.Width, Me.CompassNode.Size.Height, GraphicsUnit.Pixel, GetImgAttr(Color.ToColorMatrix))
                ElseIf Me.Type = "Button Node" Then
                    g.Transform = Me.Transform
                    If Me.ButtonNode.DisplayState = ButtonNodeState.Inactive Then
                        g.DrawImage(Me.ButtonNode.SizedOffImage, New Rectangle(0, 0, Me.ButtonNode.SizedOffImage.Width, Me.ButtonNode.SizedOffImage.Height), 0, 0, Me.ButtonNode.SizedOffImage.Width, Me.ButtonNode.SizedOffImage.Height, GraphicsUnit.Pixel, GetImgAttr(Color.ToColorMatrix))
                    Else
                        g.DrawImage(Me.ButtonNode.SizedOnImage, New Rectangle(0, 0, Me.ButtonNode.SizedOnImage.Width, Me.ButtonNode.SizedOnImage.Height), 0, 0, Me.ButtonNode.SizedOnImage.Width, Me.ButtonNode.SizedOnImage.Height, GraphicsUnit.Pixel, GetImgAttr(Color.ToColorMatrix))
                    End If
                Else
                    dhl = False
                End If
                'drawing highlight field
                If dhl = True Then
                    Dim c As System.Drawing.Color = System.Drawing.Color.FromArgb(128, 150, 0, 0)
                    Dim b As New Drawing2D.LinearGradientBrush(New Rectangle(0, 0, 1, 1), c, c, 0)
                    g.FillRectangle(b, New Rectangle(0, 0, Me.Manage.Size_Width, Me.Manage.Size_Height))
                End If
                'updating old region that is painted
                Me.OldRegion = Me.NewRegion
                Me.Changed = False
            Catch ex As Exception
                WriteLog("Failed to render node: " & Me.Name & " (" & Me.Type & ") ; " & ex.Message)
            End Try
        End If
        For Each cnode As Node In Me.Children
            cnode.Draw(g, Color)
        Next
        ResetScreenGraphics(g)
    End Sub
    Public Sub DrawBounds(ByRef g As Graphics)
        ResetScreenGraphics(g)
        Dim b As Rectangle = Me.Bounds
        If Me.Selected = False Then g.DrawRectangle(Pens.Red, b)
        If Me.Selected = True Then g.DrawRectangle(Pens.Green, b)
    End Sub
    Public Sub UpdateOnScreen(Optional ByVal ForceRefresh As Boolean = False)
        Form1.MainScreen.Invalidate(Me.OldRegion)
        If Me.Changed = True Or ForceRefresh = True Then
            Form1.MainScreen.Invalidate(Me.NewRegion)
        End If
        For Each n As Node In Me.Children
            n.UpdateOnScreen(ForceRefresh)
        Next
    End Sub

    Public Sub AddChild(ByVal Node As Node)
        ReDim Preserve Me.Children(Me.Children.Count)
        Node.Parent = Me
        Me.Children(Me.Children.Count - 1) = Node
        Node.UpdateOnScreen(True)
    End Sub
    Public Sub InsertChild(ByVal Index As Integer, ByVal Node As Node)
        ReDim Preserve Me.Children(Me.Children.Count)
        Dim buNodes(Me.Children.Count - 1) As Node
        Me.Children.CopyTo(buNodes, 0)
        For i As Integer = Index To Me.Children.Count - 2
            Me.Children(i + 1) = buNodes(i)
        Next
        buNodes = Nothing
        Me.Children(Index) = Node
    End Sub
    Public Sub RemoveChild(ByVal Node As Node)
        For i As Integer = 0 To Me.Children.Count - 1
            If Me.Children(i) Is Node Then Me.RemoveChildAt(i) : Exit For
        Next
    End Sub
    Public Sub RemoveChildAt(ByVal Index As Integer)
        For i As Integer = Index To Me.Children.Count - 2
            Me.Children(i) = Me.Children(i + 1)
        Next
        ReDim Preserve Me.Children(Me.Children.Count - 2)
    End Sub
    Public Sub BringToFront()
        If Not IsNothing(Me.Parent) Then Me.Index = Me.Parent.Children.Count - 1
    End Sub
    Public Sub SendToBack()
        Me.Index = 0
    End Sub
    Public Sub Delete()
        For Each n As Node In Me.Children
            n.Delete()
        Next
        If Not IsNothing(Me.Parent) Then
            Me.Parent.RemoveChild(Me)
            Me.UpdateOnScreen()
        End If
    End Sub

    Public Sub DeselectAll()
        Me.Selected = False
        For Each cnode As Node In Me.Children
            cnode.DeselectAll()
        Next
    End Sub
    Public Function GetNodeAtPoint(ByVal Point As Point, ByVal UseBounds As Boolean, Optional ByVal SelectType As SelectType = SelectType.Both) As Node
        Dim nodes As List(Of Node) = Me.All(SelectType)
        nodes.Reverse()
        For Each Node As Node In nodes
            If Node.Intersects(Point, UseBounds) Then Return Node
        Next
        Return Nothing
    End Function
    Public Function GetNodeByName(ByVal Name As String) As Node
        If Me.Name.ToLower.Trim = Name.ToLower.Trim Then Return Me
        For Each cnode As Node In Me.Children
            Dim ccnode As Node = cnode.GetNodeByName(Name)
            If Not IsNothing(ccnode) Then Return ccnode
        Next
        Return Nothing
    End Function
    Public Function GetNodesByType(ByVal Type As String) As List(Of Node)
        GetNodesByType = New List(Of Node)
        If Me.Type = Type Then GetNodesByType.Add(Me)
        For Each cnode As Node In Me.Children
            GetNodesByType.AddRange(cnode.GetNodesByType(Type))
        Next
    End Function
    Public Function Intersects(ByVal Point As Point, Optional ByVal UseBounds As Boolean = False) As Boolean
        If Me.Render = False Then Return False
        If Me.Type = "Split Node" Then Return False
        If UseBounds = True Then
            Dim rect As Rectangle = Me.Bounds
            Point.X -= Form1.MainScreen.DrawOffset.X
            Point.Y -= Form1.MainScreen.DrawOffset.Y
            Point.X /= Form1.MainScreen.Scale.X
            Point.Y /= Form1.MainScreen.Scale.Y
            If Point.X >= rect.X And Point.X <= rect.X + rect.Width Then
                Return Point.Y >= rect.Y And Point.Y <= rect.Y + rect.Height
            Else
                Return False
            End If
        Else
            Return Me.NewRegion.IsVisible(Point)
        End If
    End Function
    Public Function Intersects(ByVal Rectangle As Rectangle) As Boolean
        Return Me.NewRegion.IsVisible(Rectangle)
    End Function

    Public Function All(Optional ByVal Mode As SelectType = SelectType.Both) As List(Of Node)
        All = New List(Of Node)
        If Me.Selected = (Mode = SelectType.Selected) Or Mode = SelectType.Both Then All.Add(Me)
        For Each cnode As Node In Me.Children
            For Each ccnode As Node In cnode.All(Mode)
                All.Add(ccnode)
            Next
        Next
    End Function
    Public Enum SelectType
        Both = 0
        Selected = 1
        Deselected = 2
    End Enum
    Public Sub RefreshAllTextures()
        For Each Node As Node In Me.All
            If Node.Type = "Picture Node" Then Node.PictureNode.Texture.Refresh()
            If Node.Type = "Bar Node" Then Node.BarNode.FullTexture.Refresh()
            If Node.Type = "Bar Node" Then Node.BarNode.EmptyTexture.Refresh()
            If Node.Type = "Compass Node" Then Node.CompassNode.Texture.Refresh()
        Next
        UpdateScreen()
    End Sub

    Function SaveData()
        If Me.CanBeSaved = True Then
            Dim rdata As String = ""
            Dim p As String = "Global"
            If Not IsNothing(Me.Parent) Then p = Me.Parent.Name
            If Me.Type = "Picture Node" Then
                With Me.PictureNode
                    rdata = "hudBuilder.createPictureNode		" & p & " " & Me.Name & " "
                    rdata &= .Location.X & " " & .Location.Y & " " & .Size.Width & " " & .Size.Height
                    If .Texture.Path <> "" Then rdata &= vbCrLf & "hudBuilder.setPictureNodeTexture 	" & .Texture.Path
                    If .Rotation.Value <> 0 And .Rotation.Value <> 360 Then rdata &= vbCrLf & "hudBuilder.setPictureNodeRotation 	" & 360 - .Rotation.Value
                    If Me.PosXVariable.VariableName <> "" Then rdata &= vbCrLf & "hudBuilder.setNodePosVariable		0 " & Me.PosXVariable.VariableName
                    If Me.PosYVariable.VariableName <> "" Then rdata &= vbCrLf & "hudBuilder.setNodePosVariable		1 " & Me.PosYVariable.VariableName
                    If .RotateVariable.VariableName <> "" Then rdata &= vbCrLf & "hudBuilder.setPictureNodeRotateVariable " & .RotateVariable.VariableName
                    If .RotateVariable.VariableName <> "" Then rdata &= vbCrLf & "hudBuilder.setPictureNodeCenterPoint 	" & .CenterPoint.X & " " & .CenterPoint.Y
                End With
            ElseIf Me.Type = "Text Node" Then
                With Me.TextNode
                    rdata = "hudBuilder.createTextNode		" & p & " " & Me.Name & " "
                    rdata &= .Location.X & " " & .Location.Y & " " & .Size.Width & " " & .Size.Height
                    rdata &= vbCrLf & "hudBuilder.setTextNodeStyle		Fonts/vehicleHudFont_6.dif " & .Style
                    If .StringVariable.VariableName.Trim = "" Then
                        rdata &= vbCrLf & "hudBuilder.setTextNodeString	"
                        If .StringText.Contains(" ") Then rdata &= Chr(34) & .StringText & Chr(34) Else rdata &= .StringText
                    Else
                        rdata &= vbCrLf & "hudBuilder.setTextNodeStringVariable	" & .StringVariable.VariableName
                    End If
                End With
            ElseIf Me.Type = "Compass Node" Then
                With Me.CompassNode
                    rdata = "hudBuilder.createCompassNode 		" & p & " " & Me.Name & " "
                    rdata &= .Style & " " & .Location.X & " " & .Location.Y & " " & .Size.Width & " " & .Size.Height
                    If .Style = 3 Then rdata &= " 1 0"
                    If .Style = 0 Then rdata &= " 0 1"
                    rdata &= vbCrLf & "hudBuilder.setCompassNodeTexture 	1 " & .Texture.Path
                    rdata &= vbCrLf & "hudBuilder.setCompassNodeTextureSize	" & .TextureSize.Width & " " & .TextureSize.Height
                    If .Style = 3 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeBorder		0 0 0 " & .Border.Value
                    If .Style = 0 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeBorder		0 " & .Border.Value & " 0 0"
                    rdata &= vbCrLf & "hudBuilder.setCompassNodeValueVariable	" & .ValueVariable.VariableName
                    rdata &= vbCrLf & "hudBuilder.setCompassNodeOffset		" & .Offset.Value
                    'If .Style = 3 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeSnapOffset	0 0 " & .OffsetSnapMin & " " & .OffsetSnapMax
                    'If .Style = 0 Then rdata &= vbCrLf & "hudBuilder.setCompassNodeSnapOffset	" & .OffsetSnapMin & " " & .OffsetSnapMax & " 0 0"
                End With
            ElseIf Me.Type = "Split Node" Then
                rdata = "hudBuilder.createSplitNode			" & p & " " & Me.Name
                If Me.BlendEffectA <> 0 And Me.BlendEffectB <> 0 Then rdata &= vbCrLf & "hudBuilder.addNodeBlendEffect		" & Me.BlendEffectA & " " & Me.BlendEffectB
            ElseIf Me.Type = "Bar Node" Then
                With Me.BarNode
                    rdata = "hudBuilder.createBarNode 		" & p & " " & Me.Name & " " & .Style & " "
                    rdata &= .Location.X & " " & .Location.Y & " " & .Size.Width & " " & .Size.Height
                    If .FullTexture.Path <> "" Then rdata &= vbCrLf & "hudbuilder.setBarNodeTexture		1 " & .FullTexture.Path
                    If .EmptyTexture.Path <> "" Then rdata &= vbCrLf & "hudbuilder.setBarNodeTexture		2 " & .EmptyTexture.Path
                    rdata &= vbCrLf & "hudBuilder.setBarNodeValueVariable 	" & .ValueVariable.VariableName
                End With
            ElseIf Me.Type = "Hover Node" Then
                With Me.HoverNode
                    rdata = "hudBuilder.createHoverNode		" & Me.Parent.Name & " " & Me.Name & " "
                    rdata &= .Location.X & " " & .Location.Y & " " & .Size.Width & " " & .Size.Height
                    rdata &= vbCrLf & "hudBuilder.setHoverInMiddlePos		" & .HoverInMiddlePos.X & " " & .HoverInMiddlePos.Y
                    rdata &= vbCrLf & "hudBuilder.setHoverMaxValue		" & .HoverMaxValue
                    rdata &= vbCrLf & "hudBuilder.setHoverWidthLength		" & .HoverWidthLength.Width & " " & .HoverWidthLength.Height
                End With
            ElseIf Me.Type = "Button Node" Then
                With Me.ButtonNode
                    rdata = "hudBuilder.createButtonNode		" & Me.Parent.Name & " " & Me.Name & " "
                    rdata &= .Location.X & " " & .Location.Y & " " & .Size.Width & " " & .Size.Height
                    If .OffTexture.Path <> "" Then rdata &= vbCrLf & "hudBuilder.setButtonNodeTexture 	1 " & .OffTexture.Path
                    If .OnTexture.Path <> "" Then rdata &= vbCrLf & "hudBuilder.setButtonNodeTexture 	2 " & .OnTexture.Path
                    For Each hc As String In .HoverCommands.Split(vbCrLf)
                        hc = hc.Trim
                        If hc <> "" Then rdata &= vbCrLf & "hudBuilder.setButtonNodeConCmd		" & Chr(34) & hc & Chr(34) & " 1"
                    Next
                    For Each pc As String In .PressCommands.Split(vbCrLf)
                        pc = pc.Trim
                        If pc <> "" Then rdata &= vbCrLf & "hudBuilder.setButtonNodeConCmd		" & Chr(34) & pc & Chr(34) & " 0"
                    Next
                    If .UseMouseArea = True Then
                        rdata &= vbCrLf & "hudBuilder.setButtonNodeMouseArea 	" & .MouseArea.X & " " & .MouseArea.Y & " " & .MouseArea.Width & " " & .MouseArea.Height
                    Else
                        rdata &= vbCrLf & "hudBuilder.setButtonNodeMouseArea 	0 0 " & .Size.Width & " " & .Size.Height
                    End If
                End With
            ElseIf Me.Type = "Object Marker Node" Then
                With Me.ObjectMarkerNode
                    rdata = "hudBuilder.createObjectMarkerNode 		" & Me.Parent.Name & " " & Me.Name & " "
                    rdata &= .Location.X & " " & .Location.Y & " " & .Size.Width & " " & .Size.Height
                    If .FriendlyTexture.Path <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTexture 		0 " & .FriendlyTexture.Path
                    If .EnemyTexture.Path <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTexture 		1 " & .EnemyTexture.Path
                    If .LockedTexture.Path <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTexture 		2 " & .LockedTexture.Path
                    If .RangeLineTexture.Path <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTexture 		3 " & .RangeLineTexture.Path
                    If .FriendlyTexture.Path <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTextureSize	0 " & .FriendlyTextureSize.Width & " " & .FriendlyTextureSize.Height
                    If .EnemyTexture.Path <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTextureSize	1 " & .EnemyTextureSize.Width & " " & .EnemyTextureSize.Height
                    If .LockedTexture.Path <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTextureSize	2 " & .LockedTextureSize.Width & " " & .LockedTextureSize.Height
                    If .RangeLineTexture.Path <> "" Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeTextureSize	3 " & .RangeLineTextureSize.Width & " " & .RangeLineTextureSize.Height
                    If .LockOnType <> 0 Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeLockOnType	" & .LockOnType
                    rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeObjects 		" & .omnobjects
                    If .LockTextString <> "" Then
                        rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeLockText 		" & .LockTextStyle & " " & .LockTextString
                        rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeLockTextOffset	" & .LockTextOffSet.X & " " & .LockTextOffSet.Y
                    End If
                    For Each tnode As String In .LockTextNodes.Items
                        rdata &= vbCrLf & "hudBuilder.addObjectMarkerNodeLockTextNode	" & tnode
                    Next
                    If .omnweapon <> 0 Then rdata &= vbCrLf & "hudBuilder.setObjectMarkerNodeWeapon 		" & .omnweapon
                End With
            End If
            If Me.Color.IsWhite = False Then rdata &= vbCrLf & "hudBuilder.setNodeColor		 	" & Me.Color.ToDecimalText
            If Me.InTime <> 0 Then rdata &= vbCrLf & "hudBuilder.setNodeInTime 		" & Me.InTime.ToString.Replace(",", ".")
            If Me.OutTime <> 0 Then rdata &= vbCrLf & "hudBuilder.setNodeOutTime 		" & Me.OutTime.ToString.Replace(",", ".")
            If Me.ShowVariable.VariableName <> "" Then rdata &= vbCrLf & "hudBuilder.setNodeShowVariable 		" & Me.ShowVariable.VariableName
            If Me.AlphaVariable.VariableName <> "" Then rdata &= vbCrLf & "hudBuilder.setNodeAlphaVariable     	" & Me.AlphaVariable.VariableName
            If Me.AlphaShowEffect = True Then rdata &= vbCrLf & "hudBuilder.addNodeAlphaShowEffect"
            For Each line As String In Me.FailedLines.Split(vbCrLf)
                line = line.Trim
                If line <> "" Then rdata &= vbCrLf & line
            Next
            If Me.LogicShowVariables <> "" Then
                For Each line As String In Me.LogicShowVariables.Split(vbCrLf)
                    rdata &= vbCrLf & "hudBuilder.setNodeLogicShowVariable 		" & line
                Next
            End If
            Return rdata
        Else
            Return ""
        End If
    End Function
    Function LoadLine(ByVal Line As String) As Boolean
        If Line.ToLower.Trim.StartsWith("hudbuilder.setnodelogicshowvariable") Then
            If Me.LogicShowVariables <> "" Then Me.LogicShowVariables &= vbCrLf
            Me.LogicShowVariables &= Line.Remove(0, 36).Trim
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setnodeshowvariable") Then
            Me.ShowVariable.VariableName = Line.Remove(0, 31).Trim
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setnodealphavariable") Then
            Me.AlphaVariable.VariableName = GetValueAt(Line, 1)
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.addnodeblendeffect") Then
            Me.BlendEffectA = Val(GetValueAt(Line, 1))
            Me.BlendEffectB = Val(GetValueAt(Line, 2))
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setnodecolor") Then
            Dim R As Integer = SetValueBounds(GetValueAt(Line, 1).Replace(".", ",").Trim(",") * 255, 0, 255)
            Dim G As Integer = SetValueBounds(GetValueAt(Line, 2).Replace(".", ",").Trim(",") * 255, 0, 255)
            Dim B As Integer = SetValueBounds(GetValueAt(Line, 3).Replace(".", ",").Trim(",") * 255, 0, 255)
            Dim A As Integer = SetValueBounds(GetValueAt(Line, 4).Replace(".", ",").Trim(",") * 255, 0, 255)
            Me.Color = New Color(A, R, G, B)
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setnodeposvariable") Then
            If Val(GetValueAt(Line, 1)) = 0 Then Me.PosXVariable.VariableName = GetValueAt(Line, 2)
            If Val(GetValueAt(Line, 1)) = 1 Then Me.PosYVariable.VariableName = GetValueAt(Line, 2)
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setnodeintime") Then
            Me.InTime = SetValueBounds(Val(GetValueAt(Line, 1)), 0, 100)
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setnodeouttime") Then
            Me.OutTime = SetValueBounds(Val(GetValueAt(Line, 1)), 0, 100)
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.addnodealphashoweffect") Then
            Me.AlphaShowEffect = True
        Else
            If Me.Type = "Picture Node" Then Return Me.PictureNode.LoadLine(Line)
            If Me.Type = "Compass Node" Then Return Me.CompassNode.LoadLine(Line)
            If Me.Type = "Text Node" Then Return Me.TextNode.LoadLine(Line)
            If Me.Type = "Bar Node" Then Return Me.BarNode.LoadLine(Line)
            If Me.Type = "Button Node" Then Return Me.ButtonNode.LoadLine(Line)
            If Me.Type = "Object Marker Node" Then Return Me.ObjectMarkerNode.LoadLine(Line)
            If Me.Type = "Hover Node" Then Return Me.HoverNode.LoadLine(Line)
            'If Me.Type = "Split Node" Then rval = Me.SplitNodeData.LoadLine(Line)
        End If
        Return True
    End Function

#End Region

    Public Name As String = ""
    Public Type As String = ""
    Public Children(-1) As Node
    Public Parent As Node
    Public Color As New Color(255, 255, 255, 255)
    Public Manage As ManagerClass
    Public BlendEffectA As Integer = -1
    Public BlendEffectB As Integer = -1
    Public InTime As Single = 0
    Public OutTime As Single = 0
    Public AlphaShowEffect As Boolean = False
    Public MoveShowEffect As String = ""
    Public Class ManagerClass
        Sub New(ByVal P As Node)
            Me.Parent = P
        End Sub
        Private Parent As Node
        Public Property Location() As Point
            Get
                Location = New Point(0, 0)
                If Me.Parent.Type = "Picture Node" Then Location = Me.Parent.PictureNode.Location.Value
                If Me.Parent.Type = "Bar Node" Then Location = Me.Parent.BarNode.Location.Value
                If Me.Parent.Type = "Text Node" Then Location = Me.Parent.TextNode.Location.Value
                If Me.Parent.Type = "Compass Node" Then Location = Me.Parent.CompassNode.Location.Value
                If Me.Parent.Type = "Button Node" Then Location = Me.Parent.ButtonNode.Location.Value
                If Me.Parent.Type = "Object Marker Node" Then Location = Me.Parent.ObjectMarkerNode.Location.Value
                If Me.Parent.Type = "Hover Node" Then Location = Me.Parent.HoverNode.Location.Value
                If Me.Parent.PosXVariable.VariableName <> "" Then Location.X = Me.Parent.PosXVariable.Value + 400
                If Me.Parent.PosYVariable.VariableName <> "" Then Location.Y = Me.Parent.PosYVariable.Value + 300
            End Get
            Set(ByVal value As Point)
                If Me.Parent.PosXVariable.VariableName <> "" Then value.X = Me.Parent.PosXVariable.Value + 400
                If Me.Parent.PosYVariable.VariableName <> "" Then value.Y = Me.Parent.PosYVariable.Value + 300
                If Me.Parent.Type = "Picture Node" Then Me.Parent.PictureNode.Location.Value = value
                If Me.Parent.Type = "Bar Node" Then Me.Parent.BarNode.Location.Value = value
                If Me.Parent.Type = "Text Node" Then Me.Parent.TextNode.Location.Value = value
                If Me.Parent.Type = "Compass Node" Then Me.Parent.CompassNode.Location.Value = value
                If Me.Parent.Type = "Button Node" Then Me.Parent.ButtonNode.Location.Value = value
                If Me.Parent.Type = "Object Marker Node" Then Me.Parent.ObjectMarkerNode.Location.Value = value
                If Me.Parent.Type = "Hover Node" Then Me.Parent.HoverNode.Location.Value = value
            End Set
        End Property
        Public Property Location_X() As Integer
            Set(ByVal value As Integer)
                If Me.Parent.PosXVariable.VariableName = "" Then
                    If Me.Parent.Type = "Picture Node" Then Me.Parent.PictureNode.Location.X = value
                    If Me.Parent.Type = "Bar Node" Then Me.Parent.BarNode.Location.X = value
                    If Me.Parent.Type = "Text Node" Then Me.Parent.TextNode.Location.X = value
                    If Me.Parent.Type = "Compass Node" Then Me.Parent.CompassNode.Location.X = value
                    If Me.Parent.Type = "Button Node" Then Me.Parent.ButtonNode.Location.X = value
                    If Me.Parent.Type = "Object Marker Node" Then Me.Parent.ObjectMarkerNode.Location.X = value
                    If Me.Parent.Type = "Hover Node" Then Me.Parent.HoverNode.Location.X = value
                End If
            End Set
            Get
                Return Me.Location.X
            End Get
        End Property
        Public Property Location_Y() As Integer
            Set(ByVal value As Integer)
                If Me.Parent.PosYVariable.VariableName = "" Then
                    If Me.Parent.Type = "Picture Node" Then Me.Parent.PictureNode.Location.Y = value
                    If Me.Parent.Type = "Bar Node" Then Me.Parent.BarNode.Location.Y = value
                    If Me.Parent.Type = "Text Node" Then Me.Parent.TextNode.Location.Y = value
                    If Me.Parent.Type = "Compass Node" Then Me.Parent.CompassNode.Location.Y = value
                    If Me.Parent.Type = "Button Node" Then Me.Parent.ButtonNode.Location.Y = value
                    If Me.Parent.Type = "Object Marker Node" Then Me.Parent.ObjectMarkerNode.Location.Y = value
                    If Me.Parent.Type = "Hover Node" Then Me.Parent.HoverNode.Location.Y = value
                End If
            End Set
            Get
                Return Me.Location.Y
            End Get
        End Property
        Public Property Size() As Size
            Get
                If Me.Parent.Type = "Picture Node" Then Return Me.Parent.PictureNode.Size.Value
                If Me.Parent.Type = "Bar Node" Then Return Me.Parent.BarNode.Size.Value
                If Me.Parent.Type = "Text Node" Then Return Me.Parent.TextNode.Size.Value
                If Me.Parent.Type = "Compass Node" Then Return Me.Parent.CompassNode.Size.Value
                If Me.Parent.Type = "Button Node" Then Return Me.Parent.ButtonNode.Size.Value
                If Me.Parent.Type = "Object Marker Node" Then Return Me.Parent.ObjectMarkerNode.Size.Value
                If Me.Parent.Type = "Hover Node" Then Return Me.Parent.HoverNode.Size.Value
                Return New Size(0, 0)
            End Get
            Set(ByVal value As Size)
                If Me.Parent.Type = "Picture Node" Then Me.Parent.PictureNode.Size.Value = value
                If Me.Parent.Type = "Bar Node" Then Me.Parent.BarNode.Size.Value = value
                If Me.Parent.Type = "Text Node" Then Me.Parent.TextNode.Size.Value = value
                If Me.Parent.Type = "Compass Node" Then Me.Parent.CompassNode.Size.Value = value
                If Me.Parent.Type = "Button Node" Then Me.Parent.ButtonNode.Size.Value = value
                If Me.Parent.Type = "Object Marker Node" Then Me.Parent.ObjectMarkerNode.Size.Value = value
                If Me.Parent.Type = "Hover Node" Then Me.Parent.HoverNode.Size.Value = value
            End Set
        End Property
        Public Property Size_Width() As Integer
            Set(ByVal value As Integer)
                If Me.Parent.Type = "Picture Node" Then Me.Parent.PictureNode.Size.Width = value
                If Me.Parent.Type = "Bar Node" Then Me.Parent.BarNode.Size.Width = value
                If Me.Parent.Type = "Text Node" Then Me.Parent.TextNode.Size.Width = value
                If Me.Parent.Type = "Compass Node" Then Me.Parent.CompassNode.Size.Width = value
                If Me.Parent.Type = "Button Node" Then Me.Parent.ButtonNode.Size.Width = value
                If Me.Parent.Type = "Object Marker Node" Then Me.Parent.ObjectMarkerNode.Size.Width = value
                If Me.Parent.Type = "Hover Node" Then Me.Parent.HoverNode.Size.Width = value
            End Set
            Get
                Return Me.Size.Width
            End Get
        End Property
        Public Property Size_Height() As Integer
            Set(ByVal value As Integer)
                If Me.Parent.Type = "Picture Node" Then Me.Parent.PictureNode.Size.Height = value
                If Me.Parent.Type = "Bar Node" Then Me.Parent.BarNode.Size.Height = value
                If Me.Parent.Type = "Text Node" Then Me.Parent.TextNode.Size.Height = value
                If Me.Parent.Type = "Compass Node" Then Me.Parent.CompassNode.Size.Height = value
                If Me.Parent.Type = "Button Node" Then Me.Parent.ButtonNode.Size.Height = value
                If Me.Parent.Type = "Object Marker Node" Then Me.Parent.ObjectMarkerNode.Size.Height = value
                If Me.Parent.Type = "Hover Node" Then Me.Parent.HoverNode.Size.Height = value
            End Set
            Get
                Return Me.Size.Height
            End Get
        End Property
        Public Property Rotation() As Integer
            Get
                If Me.Parent.Type = "Picture Node" Then Return Me.Parent.PictureNode.Rotation.Value
                Return 0
            End Get
            Set(ByVal value As Integer)
                If Me.Parent.Type = "Picture Node" Then Me.Parent.PictureNode.Rotation.Value = value
            End Set
        End Property
    End Class
    Public Property PictureNode() As PictureNode
        Get
            Return Me._NodeData
        End Get
        Set(ByVal value As PictureNode)
            Me._NodeData = value
        End Set
    End Property
    Public Property BarNode() As BarNode
        Get
            Return Me._NodeData
        End Get
        Set(ByVal value As BarNode)
            Me._NodeData = value
        End Set
    End Property
    Public Property TextNode() As TextNode
        Get
            Return Me._NodeData
        End Get
        Set(ByVal value As TextNode)
            Me._NodeData = value
        End Set
    End Property
    Public Property CompassNode() As CompassNode
        Get
            Return Me._NodeData
        End Get
        Set(ByVal value As CompassNode)
            Me._NodeData = value
        End Set
    End Property
    Public Property ButtonNode() As ButtonNode
        Get
            Return Me._NodeData
        End Get
        Set(ByVal value As ButtonNode)
            Me._NodeData = value
        End Set
    End Property
    Public Property HoverNode() As HoverNode
        Get
            Return Me._NodeData
        End Get
        Set(ByVal value As HoverNode)
            Me._NodeData = value
        End Set
    End Property
    Public Property ObjectMarkerNode() As ObjectMarkerNode
        Get
            Return Me._NodeData
        End Get
        Set(ByVal value As ObjectMarkerNode)
            Me._NodeData = value
        End Set
    End Property

#Region "Variables"
    Public PosXVariable As Variable
    Public PosYVariable As Variable
    Public ShowVariable As Variable
    Public AlphaVariable As Variable
    Public LogicShowVariables As String = ""
#End Region

End Class