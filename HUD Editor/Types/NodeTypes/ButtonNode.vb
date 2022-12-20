Public Class ButtonNode
#Region "Internal"
    Private _Dstate As New WInteger(0)
    Public Property Changed() As Boolean
        Get
            If Me.Location.Changed = True Then Return True
            If Me.Size.Changed = True Then Return True
            If Me.OnTexture.Changed = True Then Return True
            If Me.OffTexture.Changed = True Then Return True
            If Me._Dstate.Changed = True Then Return True
            Return False
        End Get
        Set(ByVal value As Boolean)
            Me.Location.Changed = value
            Me.Size.Changed = value
            Me.OnTexture.Changed = value
            Me.OffTexture.Changed = value
            Me._Dstate.Changed = value
        End Set
    End Property
    Public ReadOnly Property SizedOnImage() As Image
        Get
            Return Me.OnTexture.GetSized(Me.Size.Value)
        End Get
    End Property
    Public ReadOnly Property SizedOffImage() As Image
        Get
            Return Me.OffTexture.GetSized(Me.Size.Value)
        End Get
    End Property
    Function LoadLine(ByVal Line As String) As Boolean
        If Line.ToLower.Trim.StartsWith("hudbuilder.setbuttonnodetexture") Then
            If Val(GetValueAt(Line, 1)) = 1 Then
                Me.OffTexture.Path = FixTexturePath(GetValueAt(Line, 2))
            ElseIf Val(GetValueAt(Line, 1)) = 2 Then
                Me.OnTexture.Path = FixTexturePath(GetValueAt(Line, 2))
            End If
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setbuttonnodeconcmd") Then
            Line = Line.Trim.Remove(0, 30).Trim
            If Line.EndsWith("0") Then
                Line = StrReverse(StrReverse(Line).Remove(0, 1)).Trim.Trim(Chr(34))
                If Me.PressCommands <> "" Then Me.PressCommands &= vbCrLf
                Me.PressCommands &= Line
            ElseIf Line.EndsWith("1") Then
                Line = StrReverse(StrReverse(Line).Remove(0, 1)).Trim.Trim(Chr(34))
                If Me.HoverCommands <> "" Then Me.HoverCommands &= vbCrLf
                Me.HoverCommands &= Line
            End If
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setbuttonnodemousearea") Then
            Me.UseMouseArea = True
            Me.MouseArea = New Rectangle(Val(GetValueAt(Line, 1)), Val(GetValueAt(Line, 2)), Val(GetValueAt(Line, 3)), Val(GetValueAt(Line, 4)))
        Else
            Return False
        End If
        Return True
    End Function
#End Region
    Public Location As New WPoint(0, 0)
    Public Size As New WSize(32, 32)
    Public OnTexture As New Texture("")
    Public OffTexture As New Texture("")
    Public PressCommands As String = ""
    Public HoverCommands As String = ""
    Public MouseArea As New Rectangle(0, 0, 1, 1)
    Public UseMouseArea As Boolean = False
    Public Property DisplayState() As ButtonNodeState
        Get
            Return Me._Dstate.Value
        End Get
        Set(ByVal value As ButtonNodeState)
            Me._Dstate.Value = value
        End Set
    End Property
End Class
