Public Class CompassNode
#Region "Internal"
    Private _Changed As Boolean = False
    Private _Style As Integer = 0
    Public Property Changed() As Boolean
        Get
            If Me._Changed = True Then Return True
            If Me.Location.Changed = True Then Return True
            If Me.Size.Changed = True Then Return True
            If Me.ValueVariable.Changed = True Then Return True
            If Me.Texture.Changed = True Then Return True
            If Me.Offset.Changed = True Then Return True
            If Me.Border.Changed = True Then Return True
            Return False
        End Get
        Set(ByVal value As Boolean)
            Me._Changed = value
            Me.Location.Changed = value
            Me.Size.Changed = value
            Me.ValueVariable.Changed = value
            Me.Texture.Changed = value
            Me.Border.Changed = value
            Me.Offset.Changed = value
        End Set
    End Property
    Public ReadOnly Property SizedImage() As Image
        Get
            Return Me.Texture.GetSized(Me.TextureSize.Value)
        End Get
    End Property
    Function LoadLine(ByVal Line As String) As Boolean
        Dim rval As Boolean = True
        If Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodetexturesize") Then
            Me.TextureSize.Width = Val(GetValueAt(Line, 1))
            Me.TextureSize.Height = Val(GetValueAt(Line, 2))
            If Me.TextureSize.Width = 0 Then Me.TextureSize.Width = 32
            If Me.TextureSize.Height = 0 Then Me.TextureSize.Height = 32
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodetexture") Then
            Me.Texture.Path = GetValueAt(Line, 2)
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodeborder") Then
            If Me.Style = 3 Then Me.Border.Value = Val(GetValueAt(Line, 4))
            If Me.Style = 0 Then Me.Border.Value = Val(GetValueAt(Line, 2))
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodevaluevariable") Then
            Me.ValueVariable.VariableName = GetValueAt(Line, 1)
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodesnapoffset") Then
            'If Me.Style = 0 Then Me.OffsetSnapMin = Val(GetValueAt(Line, 1))
            'If Me.Style = 0 Then Me.OffsetSnapMax = Val(GetValueAt(Line, 2))
            'If Me.Style = 3 Then Me.OffsetSnapMin = Val(GetValueAt(Line, 3))
            'If Me.Style = 3 Then Me.OffsetSnapMax = Val(GetValueAt(Line, 4))
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setcompassnodeoffset") Then
            Me.Offset.Value = Val(GetValueAt(Line, 1))
        Else
            Return False
        End If
        Return True
    End Function
#End Region
    Public Location As New WPoint(0, 0)
    Public Size As New WSize(32, 32)
    Public Texture As New Texture("")
    Public TextureSize As New WSize(32, 32)
    Public Border As New WInteger(0)
    Public Offset As New WInteger(0)
    Public ValueVariable As Variable
    Public Property Style() As CompassNodeStyle
        Get
            Return Me._Style
        End Get
        Set(ByVal value As CompassNodeStyle)
            If value <> Me._Style Then
                Me._Style = value
                Me._Changed = True
            End If
        End Set
    End Property
End Class
