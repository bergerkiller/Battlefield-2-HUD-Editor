Public Class BarNode
#Region "Internal"
    Private _Changed As Boolean = False
    Private _Style As Integer = 0
    Public Property Changed() As Boolean
        Get
            If Me._Changed = True Then Return True
            If Me.Location.Changed = True Then Return True
            If Me.Size.Changed = True Then Return True
            If Me.ValueVariable.Changed = True Then Return True
            If Me.FullTexture.Changed = True Then Return True
            If Me.EmptyTexture.Changed = True Then Return True
            Return False
        End Get
        Set(ByVal value As Boolean)
            Me._Changed = value
            Me.Location.Changed = value
            Me.Size.Changed = value
            Me.ValueVariable.Changed = value
            Me.FullTexture.Changed = value
            Me.EmptyTexture.Changed = value
        End Set
    End Property
    Public ReadOnly Property SizedFullImage() As Image
        Get
            Return Me.FullTexture.GetSized(Me.Size.Value)
        End Get
    End Property
    Public ReadOnly Property SizedEmptyImage() As Image
        Get
            Return Me.EmptyTexture.GetSized(Me.Size.Value)
        End Get
    End Property
    Function LoadLine(ByVal Line As String) As Boolean
        Dim rval As Boolean = True
        If Line.ToLower.Trim.StartsWith("hudbuilder.setbarnodetexture") Then
            If Val(GetValueAt(Line, 1)) = 1 Then
                Me.FullTexture.Path = GetValueAt(Line, 2)
            ElseIf Val(GetValueAt(Line, 1)) = 2 Then
                Me.EmptyTexture.Path = GetValueAt(Line, 2)
            End If
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setbarnodevaluevariable") Then
            Me.ValueVariable.VariableName = GetValueAt(Line, 1)
        Else
            Return False
        End If
        Return True
    End Function
#End Region

    Public Location As New WPoint(0, 0)
    Public Size As New WSize(32, 32)
    Public FullTexture As New Texture("Ingame\GeneralIcons\full.dds")
    Public EmptyTexture As New Texture("Ingame\GeneralIcons\empty.dds")
    Public ValueVariable As Variable
    Public Property Style() As BarNodeStyle
        Get
            Return Me._Style
        End Get
        Set(ByVal value As BarNodeStyle)
            If value <> Me._Style Then
                Me._Style = value
                Me._Changed = True
            End If
        End Set
    End Property
End Class
