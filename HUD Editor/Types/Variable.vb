Public Class Variable
    Sub New(ByVal Parent As Node)
        Me.Parent = Parent
    End Sub
    Private OldVariable As VariableHandler
    Public Changed As Boolean = False
    Private _Value As Object
    Private _Name As String = ""

    Public Property Value() As Object
        Get
            Return Me._Value
        End Get
        Set(ByVal value As Object)
            If Me._Value <> value Then
                Me._Value = value
            End If
        End Set
    End Property
    Public Parent As Node
    Public Sub SetVariable(ByRef V As VariableHandler)
        If Not IsNothing(OldVariable) Then
            RemoveHandler OldVariable.Changed, AddressOf Change
        End If
        AddHandler V.Changed, AddressOf Change
        Me._Name = V.Name
        Me._Value = V.Value
        Me.Changed = True
        OldVariable = V
    End Sub
    Public Sub Change(ByVal NewValue As Object)
        If Me.Value <> NewValue Then
            Me._Value = NewValue
            Me.Changed = True
        End If
    End Sub
    Public Property VariableName() As String
        Get
            If Not IsNothing(Me.OldVariable) Then Return Me.OldVariable.Name
            Return _Name
        End Get
        Set(ByVal value As String)
            If value.ToLower.Trim <> Me._Name.ToLower.Trim Then
                Me._Name = value
                If Not IsNothing(OldVariable) Then
                    RemoveHandler OldVariable.Changed, AddressOf Change
                    OldVariable = Nothing
                End If
                For Each v As VariableHandler In Variables
                    If v.Name.ToLower.Trim = value.ToLower.Trim Then
                        Me.SetVariable(v)
                        Exit For
                    End If
                Next
            End If
        End Set
    End Property
    Public Sub Reset()
        If IsNothing(Me.OldVariable) Then
            Me.Value = 0
        Else
            Me.Value = Me.OldVariable.Value
        End If
    End Sub
    Public ReadOnly Property VariableType() As VariableType
        Get
            If IsNothing(Me.OldVariable) Then
                Return 0
            Else
                Return Me.OldVariable.Type
            End If
        End Get
    End Property
End Class
