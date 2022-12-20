Public Structure WPoint
    Sub New(ByVal X As Integer, ByVal Y As Integer)
        Me.p = New Point(X, Y)
        Me.Changed = False
    End Sub
    Sub New(ByVal P As Point)
        Me.p = P
        Me.Changed = False
    End Sub
    Public Changed As Boolean
    Private p As Point
    Public Property X() As Integer
        Get
            Return Me.p.X
        End Get
        Set(ByVal value As Integer)
            If Me.p.X <> value Then
                Me.p.X = value
                Me.Changed = True
            End If
        End Set
    End Property
    Public Property Y() As Integer
        Get
            Return Me.p.Y
        End Get
        Set(ByVal value As Integer)
            If Me.p.Y <> value Then
                Me.p.Y = value
                Me.Changed = True
            End If
        End Set
    End Property
    Public Sub Offset(ByVal dx As Integer, ByVal dy As Integer)
        If dx <> 0 Or dy <> 0 Then
            Me.p.X += dx
            Me.p.Y += dy
            Me.Changed = True
        End If
    End Sub
    Public Property Value() As Point
        Get
            Return Me.p
        End Get
        Set(ByVal value As Point)
            If Me.p <> value Then
                Me.p = value
                Me.Changed = True
            End If
        End Set
    End Property
End Structure
Public Structure WSize
    Sub New(ByVal Width As Integer, ByVal Height As Integer)
        If Width > 2048 Then Width = 2048
        If Width < 1 Then Width = 1
        If Height > 2048 Then Height = 2048
        If Height < 1 Then Height = 1
        Me.s = New Size(Width, Height)
        Me.Changed = False
    End Sub
    Public Changed As Boolean
    Private s As Size
    Public Property Width() As Integer
        Get
            Return Me.s.Width
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then value = 1
            If value > 4096 Then value = 4096
            If Me.s.Width <> value Then
                Me.s.Width = value
                Me.Changed = True
            End If
        End Set
    End Property
    Public Property Height() As Integer
        Get
            Return Me.s.Height
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then value = 1
            If value > 4096 Then value = 4096
            If Me.s.Height <> value Then
                Me.s.Height = value
                Me.Changed = True
            End If
        End Set
    End Property
    Public Sub Add(ByVal dw As Integer, ByVal dh As Integer)
        If dw <> 0 Or dh <> 0 Then
            Me.s.Width += dw
            Me.s.Height += dh
            If Me.s.Width < 1 Then Me.s.Width = 1
            If Me.s.Width > 4096 Then Me.s.Width = 4096
            If Me.s.Height < 1 Then Me.s.Height = 1
            If Me.s.Height > 4096 Then Me.s.Height = 4096
            Me.Changed = True
        End If
    End Sub
    Public Property Value() As Size
        Get
            Return Me.s
        End Get
        Set(ByVal value As Size)
            If value.Width < 1 Then value.Width = 1
            If value.Width > 4096 Then value.Width = 4096
            If value.Height < 1 Then value.Height = 1
            If value.Height > 4096 Then value.Height = 4096
            If Me.s <> value Then
                Me.s = value
                Me.Changed = True
            End If
        End Set
    End Property
End Structure
Public Structure WInteger
    Public Changed As Boolean
    Private val As Integer
    Sub New(ByVal Value As Integer)
        Me.val = Value
        Me.Changed = False
    End Sub
    Public Property Value() As Integer
        Get
            Return Me.val
        End Get
        Set(ByVal value As Integer)
            If Me.val <> value Then
                Me.val = value
                Me.Changed = True
            End If
        End Set
    End Property
End Structure
Public Structure Texture
    Sub New(ByVal Path As String, Optional ByVal Image As Image = Nothing)
        Me._path = Path
        If IsNothing(Image) Then Me._Image = LoadImage(Path) Else Me._Image = Image
        Me.Changed = False
    End Sub

    Public Changed As Boolean
    Private _Image As Image
    Private _path As String
    Private _OldSized As Image
    Public Property Path() As String
        Get
            Return _path
        End Get
        Set(ByVal value As String)
            If _path.ToLower.Trim <> value.ToLower.Trim Then
                _path = value.Trim
                Me.Image = LoadImage(value)
                Me.Changed = True
            End If
        End Set
    End Property
    Public Property Image()
        Get
            If IsNothing(Me._Image) Then Me._Image = LoadImage(Me._path)
            If IsNothing(Me._Image) Then Me._Image = New Bitmap(32, 32)
            Return Me._Image
        End Get
        Set(ByVal value)
            Me._Image = value
        End Set
    End Property
    Public Sub Refresh()
        Me.Image = LoadImage(Me._path)
        Me.Changed = True
    End Sub
    Public Function GetSized(ByVal sze As Size) As Image
        If IsNothing(Me._OldSized) OrElse Me._OldSized.Size <> sze Or Me.Changed = True Then
            Me._OldSized = New Bitmap(sze.Width, sze.Height)
            Dim g As Graphics = Graphics.FromImage(Me._OldSized)
            g.DrawImage(Me.Image, 0, 0, sze.Width, sze.Height)
            g.Dispose()
        End If
        Return Me._OldSized
    End Function
    Public ReadOnly Property Size() As Size
        Get
            Return Me.Image.Size
        End Get
    End Property
End Structure
Public Class ImagePointer
    Sub New(ByVal Path As String, ByVal SourceFile As String, ByVal ImageOffset As Long, ByVal ImageLength As Long)
        Me.Path = Path
        Me.SourceFile = SourceFile
        Me.ImageOffset = ImageOffset
        Me.ImageLength = ImageLength
    End Sub
    Public Path As String = ""
    Public SourceFile As String = ""
    Public ImageOffset As Long = 0
    Public ImageLength As Long = 0
    Public Function GetImage() As Image
        Dim s As New System.IO.FileStream(Me.SourceFile, IO.FileMode.Open)
        Dim imgbytes(Me.ImageLength) As Byte
        s.Position = Me.ImageOffset
        s.Read(imgbytes, 0, Me.ImageLength)
        s.Close()
        Dim ms As New System.IO.MemoryStream(imgbytes, 0, imgbytes.Length)
        ms.Write(imgbytes, 0, imgbytes.Length)
        GetImage = Image.FromStream(ms)
        ms.Close()
    End Function
End Class
Public Class ImageData
    Sub New(ByVal Path As String, ByVal Image As Image)
        Me.Image = Image
        Me.Path = Path
    End Sub
    Public Path As String = ""
    Private _Image As Image
    Public Property Image() As Image
        Get
            Return Me._Image
        End Get
        Set(ByVal value As Image)
            Me._Image = value
            Dim ms As New System.IO.MemoryStream()
            value.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
            Me.imageBytes = ms.ToArray
            ms.Close()
        End Set
    End Property
    Public imageBytes() As Byte
    Public Function GetHeader() As String
        Return Me.imageBytes.Length & "|" & Me.Path
    End Function
End Class
Public Enum BarNodeStyle
    IncFromBelow = 0
    IncFromAbove = 1
    IncFromRight = 2
    IncFromLeft = 3
End Enum
Public Enum CompassNodeStyle
    Vertical = 0
    Horizontal = 3
End Enum
Public Enum TextNodeStyle
    Center = 0
    Right = 1
    Left = 2
End Enum
Public Enum SelectionSquareMode
    Invisible = 0
    SquareAndDots = 1
    Square = 2
    TranslucentSquare = 3
End Enum
Public Enum VariableType
    VT_Angle = 0 'integer
    VT_Position = 1
    VT_Value = 2 'single 0 - 1
    VT_String = 3 'string ""
    VT_Color = 4 'integer 0 - 255
    VT_Show = 5
End Enum
Public Enum GameMode
    Battlefield2 = 0
    Battlefield2142 = 1
    Both = 2
End Enum
Public Enum ButtonNodeState
    Inactive = 0
    Hovered = 1
    Clicked = 2
End Enum
Public Enum ObjectMarkerState
    None = 0
    Enemy = 1
    Friendly = 2
    Locked = 3
End Enum