Public Class VariableHandler
    Sub New(ByVal Name As String, ByVal Type As VariableType, Optional ByVal Value As Object = Nothing)
        Me.Name = Name
        Me.Type = Type
        Me._value = Value
        Me._OldValue = Value
    End Sub
    Public Event Changed(ByVal NewValue As Object)
    Private _name As String = ""
    Private _value As Object
    Private _OldValue As Object
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal VarName As String)
            VarName = VarName.Trim
            _name = VarName
            ResetValue()
        End Set
    End Property
    Public Sub ResetValue()
        Me.Value = _OldValue
    End Sub
    Public Property Value() As Object
        Get
            Return _value
        End Get
        Set(ByVal value As Object)
            If value IsNot _value Then
                _value = value
                RaiseEvent Changed(value)
            End If
        End Set
    End Property
    Public Type As VariableType

End Class
