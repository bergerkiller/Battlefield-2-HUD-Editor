Public Class TextNode
#Region "Internal"
    Private _Style As Integer = 0
    Private _Changed As Boolean = False
    Private _StringText As String = ""
    Private _TextChanged As Boolean = True
    Private _OldLength As Integer = 0
    Private _OldRLength As Integer = 0
    Public Property Changed() As Boolean
        Get
            If Me._Changed = True Then Return True
            If Me._TextChanged = True Then Return True
            If Me.Location.Changed = True Then Return True
            If Me.Size.Changed = True Then Return True
            If Me.StringVariable.Changed = True Then Return True
            Return False
        End Get
        Set(ByVal value As Boolean)
            Me._Changed = value
            Me.Location.Changed = value
            Me.Size.Changed = value
            Me._TextChanged = value
            Me.StringVariable.Changed = value
        End Set
    End Property
    Public ReadOnly Property Text() As String
        Get
            Text = Me.StringText
            If Text = "" Then
                If IsNothing(Me.StringVariable.Value) Then Text = "100" Else Text = Me.StringVariable.Value
            End If
        End Get
    End Property
    Public ReadOnly Property TextWidth()
        Get
            If Me._TextChanged = True Or Me.StringVariable.Changed = True Then
                Dim tbox As New TextBox
                tbox.Font = New Font("Arial", 8, FontStyle.Regular)
                tbox.Text = Me.Text & "_"
                Me._OldLength = tbox.GetPositionFromCharIndex(tbox.Text.Length - 1).X
            End If
            Return Me._OldLength
        End Get
    End Property
    Public ReadOnly Property TextWidthRegion()
        Get
            If Me._TextChanged = True Or Me.StringVariable.Changed = True Then
                Dim tbox As New TextBox
                tbox.Font = New Font("Arial", 9, FontStyle.Regular)
                tbox.Text = Me.Text & "_"
                Me._OldRLength = tbox.GetPositionFromCharIndex(tbox.Text.Length - 1).X
            End If
            Return Me._OldRLength
        End Get
    End Property
    Function LoadLine(ByVal Line As String) As Boolean
        If Line.ToLower.Trim.StartsWith("hudbuilder.settextnodestyle") Then
            Me.Style = SetValueBounds(Val(GetValueAt(Line, 2)), 0, 2)
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.settextnodestringvariable") Then
            Me.StringVariable.VariableName = GetValueAt(Line, 1)
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.settextnodestring") Then
            Me.StringText = GetValueAt(Line, 1).Trim(Chr(34))
        Else
            Return False
        End If
        Return True
    End Function
#End Region
    Public Location As New WPoint(0, 0)
    Public Size As New WSize(40, 10)
    Public StringVariable As Variable
    Public Property StringText() As String
        Get
            Return Me._StringText
        End Get
        Set(ByVal value As String)
            If value <> Me._StringText Then
                Me._StringText = value
                Me._TextChanged = True
            End If
        End Set
    End Property
    Public Property Style() As TextNodeStyle
        Get
            Return Me._Style
        End Get
        Set(ByVal value As TextNodeStyle)
            If value <> Me._Style Then
                Me._Style = value
                Me._Changed = True
            End If
        End Set
    End Property
End Class
