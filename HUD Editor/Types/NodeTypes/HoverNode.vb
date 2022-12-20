Public Class HoverNode
#Region "Internal"
    Public Property Changed() As Boolean
        Get
            If Me.Location.Changed = True Then Return True
            If Me.Size.Changed = True Then Return True
            If Me.HoverInMiddlePos.Changed = True Then Return True
            If Me.HoverWidthLength.Changed = True Then Return True
            Return False
        End Get
        Set(ByVal value As Boolean)
            Me.Location.Changed = value
            Me.Size.Changed = value
            Me.HoverInMiddlePos.Changed = value
            Me.HoverWidthLength.Changed = value
        End Set
    End Property
    Public HoverNodePosX As VariableHandler
    Public HoverNodePosY As VariableHandler
    Public HoverNodeShow As VariableHandler
    Function LoadLine(ByVal Line As String) As Boolean
        Dim rval As Boolean = True
        If Line.ToLower.Trim.StartsWith("hudbuilder.sethoverinmiddlepos") Then
            Me.HoverInMiddlePos.Value = New Point(Val(GetValueAt(Line, 1)), Val(GetValueAt(Line, 2)))
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.sethovermaxvalue") Then
            Me.HoverMaxValue = Val(GetValueAt(Line, 1))
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.sethoverwidthlength") Then
            Me.HoverWidthLength.Value = New Size(Val(GetValueAt(Line, 1)), Val(GetValueAt(Line, 2)))
        Else
            Return False
        End If
        Return True
    End Function
#End Region
    Public Location As New WPoint(0, 0)
    Public Size As New WSize(32, 32)
    Public HoverInMiddlePos As New WPoint(0, 0)
    Public HoverMaxValue As Single
    Public HoverWidthLength As New WSize(32, 32)
End Class
