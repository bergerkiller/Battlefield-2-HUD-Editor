Public Class PictureNode
#Region "Internal"
    Public Property Changed() As Boolean
        Get
            If Me.Location.Changed = True Then Return True
            If Me.Size.Changed = True Then Return True
            If Me.Rotation.Changed = True Then Return True
            If Me.CenterPoint.Changed = True Then Return True
            If Me.RotateVariable.Changed = True Then Return True
            If Me.Texture.Changed = True Then Return True
            Return False
        End Get
        Set(ByVal value As Boolean)
            Me.Location.Changed = value
            Me.Size.Changed = value
            Me.CenterPoint.Changed = value
            Me.Rotation.Changed = value
            Me.RotateVariable.Changed = value
            Me.Texture.Changed = value
        End Set
    End Property
    Public ReadOnly Property SizedImage() As Image
        Get
            Return Me.Texture.GetSized(Me.Size.Value)
        End Get
    End Property
    Function LoadLine(ByVal Line As String) As Boolean
        If Line.ToLower.Trim.StartsWith("hudbuilder.setpicturenodetexture") Then
            Me.Texture.Path = GetValueAt(Line, 1)
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setpicturenoderotatevariable") Then
            Me.RotateVariable.VariableName = GetValueAt(Line, 1)
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setpicturenodecenterpoint") Then
            Me.CenterPoint = New WPoint(Val(GetValueAt(Line, 1)), Val(GetValueAt(Line, 2)))
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setpicturenoderotation") Then
            Dim srotation As Integer = Val(GetValueAt(Line, 1))
            Do While srotation < 0
                srotation += 360
            Loop
            If srotation = 0 Then srotation = 360
            Me.Rotation.Value = 360 - srotation
        Else
            Return False
        End If
        Return True
    End Function
#End Region
    Public Location As New WPoint(0, 0)
    Public Size As New WSize(32, 32)
    Public Rotation As New WInteger(0)
    Public CenterPoint As New WPoint(0, 0)
    Public RotateVariable As Variable
    Public Texture As New Texture("")
End Class