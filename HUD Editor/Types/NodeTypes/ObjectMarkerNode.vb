Public Class ObjectMarkerNode
#Region "Internal"
    Private _disp As New WInteger(0)
    Public Property Changed() As Boolean
        Get
            If Me.Location.Changed = True Then Return True
            If Me.Size.Changed = True Then Return True
            If Me.FriendlyTexture.Changed = True Then Return True
            If Me.EnemyTexture.Changed = True Then Return True
            If Me.LockedTexture.Changed = True Then Return True
            If Me.RangeLineTexture.Changed = True Then Return True
            If Me.FriendlyTextureSize.Changed = True Then Return True
            If Me.EnemyTextureSize.Changed = True Then Return True
            If Me.LockedTextureSize.Changed = True Then Return True
            If Me.RangeLineTextureSize.Changed = True Then Return True
            Return False
        End Get
        Set(ByVal value As Boolean)
            Me.Location.Changed = value
            Me.Size.Changed = value
            Me.FriendlyTexture.Changed = value
            Me.EnemyTexture.Changed = value
            Me.LockedTexture.Changed = value
            Me.RangeLineTexture.Changed = value
            Me.FriendlyTextureSize.Changed = value
            Me.EnemyTextureSize.Changed = value
            Me.LockedTextureSize.Changed = value
            Me.RangeLineTextureSize.Changed = value
        End Set
    End Property
    Public ReadOnly Property SizedFriendlyImage() As Image
        Get
            Return Me.FriendlyTexture.GetSized(Me.FriendlyTextureSize.Value)
        End Get
    End Property
    Public ReadOnly Property SizedEnemyImage() As Image
        Get
            Return Me.EnemyTexture.GetSized(Me.EnemyTextureSize.Value)
        End Get
    End Property
    Public ReadOnly Property SizedRangeLineImage() As Image
        Get
            Return Me.RangeLineTexture.GetSized(Me.RangeLineTextureSize.Value)
        End Get
    End Property
    Public ReadOnly Property SizedLockedImage() As Image
        Get
            Return Me.LockedTexture.GetSized(Me.LockedTextureSize.Value)
        End Get
    End Property
    Function LoadLine(ByVal Line As String) As Boolean
        If Line.ToLower.Trim.StartsWith("hudbuilder.setobjectmarkernodetexture") Then
            Dim tpath As String = FixTexturePath(GetValueAt(Line, 2))
            If Val(GetValueAt(Line, 1)) = 0 Then
                Me.FriendlyTexture.Path = tpath
            ElseIf Val(GetValueAt(Line, 1)) = 1 Then
                Me.EnemyTexture.Path = tpath
            ElseIf Val(GetValueAt(Line, 1)) = 2 Then
                Me.LockedTexture.Path = tpath
            ElseIf Val(GetValueAt(Line, 1)) = 3 Then
                Me.RangeLineTexture.Path = tpath
            End If
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setobjectmarkernodetexturesize") Then
            Dim tsize As Size = New Size(SetValueBounds(Val(GetValueAt(Line, 2)), 1, 2048), SetValueBounds(Val(GetValueAt(Line, 2)), 1, 2048))
            If Val(GetValueAt(Line, 1)) = 0 Then
                Me.FriendlyTextureSize.Value = tsize
            ElseIf Val(GetValueAt(Line, 1)) = 1 Then
                Me.EnemyTextureSize.Value = tsize
            ElseIf Val(GetValueAt(Line, 1)) = 2 Then
                Me.LockedTextureSize.Value = tsize
            ElseIf Val(GetValueAt(Line, 1)) = 3 Then
                Me.RangeLineTextureSize.Value = tsize
            End If
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setobjectmarkernodeobjects") Then
            Me.omnobjects = Val(GetValueAt(Line, 1))
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setobjectmarkernodeweapon") Then
            Me.omnweapon = Val(GetValueAt(Line, 1))
        ElseIf Line.ToLower.Trim.StartsWith("hudbuilder.setobjectmarkernodelockontype") Then
            Me.LockOnType = Val(GetValueAt(Line, 1))
        ElseIf Line.ToLower.Trim.StartsWith("hudBuilder.setobjectmarkernodelocktextoffset") Then
            Me.LockTextOffSet = New Point(Val(GetValueAt(Line, 1)), Val(GetValueAt(Line, 2)))
        ElseIf Line.ToLower.Trim.StartsWith("hudBuilder.setobjectmarkernodelocktext") Then
            Me.LockTextStyle = Val(GetValueAt(Line, 1))
            Me.LockTextString = GetValueAt(Line, 2)
        ElseIf Line.ToLower.StartsWith("hudbuilder.addobjectmarkernodelocktextnode") Then
            Me.LockTextNodes.Items.Add(GetValueAt(Line, 1))
        Else
            Return False
        End If
        Return True
    End Function
    Public Property DisplayState() As ObjectMarkerState
        Get
            Return Me._disp.Value
        End Get
        Set(ByVal value As ObjectMarkerState)
            Me._disp.Value = value
        End Set
    End Property
    Public MarkerLocation As New WPoint(0, 0)
#End Region
    Public Location As New WPoint(0, 0)
    Public Size As New WSize(32, 32)
    Public FriendlyTexture As New Texture("")
    Public EnemyTexture As New Texture("")
    Public RangeLineTexture As New Texture("")
    Public LockedTexture As New Texture("")
    Public FriendlyTextureSize As New WSize(32, 32)
    Public EnemyTextureSize As New WSize(32, 32)
    Public RangeLineTextureSize As New WSize(32, 32)
    Public LockedTextureSize As New WSize(32, 32)
    Public omnobjects As Integer = 4
    Public omnweapon As Integer = 0
    Public LockOnType As Integer = 0
    Public LockTextString As String = ""
    Public LockTextStyle As Integer = 0
    Public LockTextOffSet As New Point(0, 0)
    Public LockTextNodes As New ListBox
End Class
