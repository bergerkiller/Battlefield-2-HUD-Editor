Public Structure Color
    Sub New(ByVal A As Integer, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer)
        Me._A = A
        Me._R = R
        Me._G = G
        Me._B = B
        Me.Changed = False
    End Sub
    Sub New(ByVal Color As System.Drawing.Color)
        _A = Color.A
        _R = Color.R
        _G = Color.G
        _B = Color.B
        Me.Changed = False
    End Sub
    Sub New(ByVal Ole As Integer)
        Dim c As System.Drawing.Color = System.Drawing.ColorTranslator.FromOle(Ole)
        Me._A = c.A
        Me._R = c.R
        Me._G = c.G
        Me._B = c.B
        Me.Changed = False
        c = Nothing
    End Sub

    Public Changed As Boolean
    Private _A As Integer
    Private _R As Integer
    Private _G As Integer
    Private _B As Integer

    Public Shared Function FromArgb(ByVal A As Integer, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Color
        Return New Color(A, R, G, B)
    End Function
    Public Shared Function FromOle(ByVal Ole As Integer) As Color
        Return New Color(System.Drawing.ColorTranslator.FromOle(Ole))
    End Function
    Public Shared Function FromDrawingColor(ByVal Color As System.Drawing.Color) As Color
        Return New Color(Color)
    End Function
    Public Shared Function FromDecimalText(ByVal Text As String, Optional ByVal Delimiter As String = " ") As Color
        Text = Text.Trim
        FromDecimalText = New Color
        If Text.Split(Delimiter).Count >= 3 Then
            FromDecimalText._R = Val(Text.Split(Delimiter)(0)) * 255
            FromDecimalText._G = Val(Text.Split(Delimiter)(1)) * 255
            FromDecimalText._B = Val(Text.Split(Delimiter)(2)) * 255
            If Text.Split(Delimiter).Count >= 4 Then
                FromDecimalText._A = Val(Text.Split(Delimiter)(4)) * 255
            End If
            If FromDecimalText._R > 255 Then FromDecimalText._R /= 255
            If FromDecimalText._G > 255 Then FromDecimalText._G /= 255
            If FromDecimalText._B > 255 Then FromDecimalText._B /= 255
            If FromDecimalText._A > 255 Then FromDecimalText._A /= 255
            Do While FromDecimalText._R > 255 Or FromDecimalText._G > 255 Or FromDecimalText._B > 255 Or FromDecimalText._A > 255
                FromDecimalText._R -= 255
                FromDecimalText._G -= 255
                FromDecimalText._B -= 255
                FromDecimalText._A -= 255
            Loop
            If FromDecimalText._R < 0 Then FromDecimalText._R = 0
            If FromDecimalText._G < 0 Then FromDecimalText._G = 0
            If FromDecimalText._B < 0 Then FromDecimalText._B = 0
            If FromDecimalText._A < 0 Then FromDecimalText._A = 0
        End If
    End Function

    Public Shared Function ToColorMatrix(ByVal Color As System.Drawing.Color) As System.Drawing.Imaging.ColorMatrix
        ToColorMatrix = New System.Drawing.Imaging.ColorMatrix
        ToColorMatrix.Matrix00 = Color.R / 255
        ToColorMatrix.Matrix11 = Color.G / 255
        ToColorMatrix.Matrix22 = Color.B / 255
        ToColorMatrix.Matrix33 = Color.A / 255
    End Function
    Public Shared Function ToColorMatrix(ByVal Color As Color) As System.Drawing.Imaging.ColorMatrix
        Return ToColorMatrix(Color.ToDrawingColor)
    End Function
    Public Function ToColorMatrix() As System.Drawing.Imaging.ColorMatrix
        Return ToColorMatrix(Me)
    End Function

    Public Shared Function ToOle(ByVal Color As Color) As Integer
        Return Color.ToOle(Color.ToDrawingColor)
    End Function
    Public Shared Function ToOle(ByVal Color As System.Drawing.Color) As Integer
        Return System.Drawing.ColorTranslator.ToOle(Color)
    End Function
    Public Function ToOle() As Integer
        Return System.Drawing.ColorTranslator.ToOle(Me.ToDrawingColor)
    End Function
    Public Shared Function ToDrawingColor(ByVal Color As Color) As System.Drawing.Color
        Return System.Drawing.Color.FromArgb(Color.A, Color.R, Color.G, Color.B)
    End Function
    Public Function ToDrawingColor() As System.Drawing.Color
        Return Color.ToDrawingColor(Me)
    End Function
    Public Shared Function ToDecimalText(ByVal Color As Color, Optional ByVal Delimiter As String = " ") As String
        Return Color.ToDecimalText(Color.ToDrawingColor, Delimiter)
    End Function
    Public Shared Function ToDecimalText(ByVal Color As System.Drawing.Color, Optional ByVal Delimiter As String = " ") As String
        Dim rval As String = Math.Round(Color.R / 255, 6)
        rval &= Delimiter & Math.Round(Color.G / 255, 6)
        rval &= Delimiter & Math.Round(Color.B / 255, 6)
        rval &= Delimiter & Math.Round(Color.A / 255, 6)
        rval = rval.Replace(",", ".")
        Return rval
    End Function
    Public Function ToDecimalText()
        Return ToDecimalText(Me)
    End Function
    Public Shared Function Combine(ByVal Color1 As Color, ByVal Color2 As Color) As Color
        Return New Color(Color1.A * Color2.A / 255, Color1.R * Color2.R / 255, Color1.G * Color2.G / 255, Color1.B * Color2.B / 255)
    End Function
    Public Sub CombineWith(ByVal Color As Color)
        SetColor(Combine(Me, Color).ToDrawingColor)
    End Sub
    Public Shared Function Merge(ByVal Color1 As Color, ByVal Color2 As Color) As Color
        Return New Color((Color1.A + Color2.A) * 0.5, (Color1.R + Color2.R) * 0.5, (Color1.G + Color2.G) * 0.5, (Color1.B + Color2.B) * 0.5)
    End Function
    Public Sub MergeWith(ByVal Color As Color)
        SetColor(Merge(Me, Color).ToDrawingColor)
    End Sub
    Public ReadOnly Property IsWhite() As Boolean
        Get
            If Me._R <> 255 Then Return False
            If Me._G <> 255 Then Return False
            If Me._B <> 255 Then Return False
            If Me._A <> 255 Then Return False
            Return True
        End Get
    End Property
    Public Sub SetColor(ByVal A As Integer, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer)
        Dim ch As Boolean = A <> Me._A Or R <> Me._R Or G <> Me._G Or B <> Me._B
        Me._A = A
        Me._R = R
        Me._G = G
        Me._B = B
        If ch = True Then Me.Changed = True
    End Sub
    Public Sub SetColor(ByVal Color As System.Drawing.Color)
        Dim ch As Boolean = Color.A <> Me._A Or Color.R <> Me._R Or Color.G <> Me._G Or Color.B <> Me._B
        Me._A = Color.A
        Me._R = Color.R
        Me._G = Color.G
        Me._B = Color.B
        If ch = True Then Me.Changed = True
    End Sub
    Public Sub SetColor(ByVal Ole As Integer)
        Dim Color As System.Drawing.Color = System.Drawing.ColorTranslator.FromOle(Ole)
        Dim ch As Boolean = Color.A <> Me._A Or Color.R <> Me._R Or Color.G <> Me._G Or Color.B <> Me._B
        Me._A = Color.A
        Me._R = Color.R
        Me._G = Color.G
        Me._B = Color.B
        If ch = True Then Me.Changed = True
    End Sub
    Public Sub SetColor(ByVal Text As String, Optional ByVal Delimiter As String = " ")
        Dim Color As Color = Color.FromDecimalText(Text, Delimiter)
        Dim ch As Boolean = Color.A <> Me._A Or Color.R <> Me._R Or Color.G <> Me._G Or Color.B <> Me._B
        Me._A = Color.A
        Me._R = Color.R
        Me._G = Color.G
        Me._B = Color.B
        If ch = True Then Me.Changed = True
    End Sub

    Public Property A() As Integer
        Get
            Return _A
        End Get
        Set(ByVal value As Integer)
            Dim ch As Boolean = value <> _A
            _A = value
            If ch = True Then Changed = True
        End Set
    End Property
    Public Property R() As Integer
        Get
            Return _R
        End Get
        Set(ByVal value As Integer)
            Dim ch As Boolean = value <> _R
            _R = value
            If ch = True Then Changed = True
        End Set
    End Property
    Public Property G() As Integer
        Get
            Return _G
        End Get
        Set(ByVal value As Integer)
            Dim ch As Boolean = value <> _G
            _G = value
            If ch = True Then Changed = True
        End Set
    End Property
    Public Property B() As Integer
        Get
            Return _B
        End Get
        Set(ByVal value As Integer)
            Dim ch As Boolean = value <> _B
            _B = value
            If ch = True Then Changed = True
        End Set
    End Property

    Public Shared ReadOnly Property White() As System.Drawing.Color
        Get
            Return System.Drawing.Color.FromArgb(255, 255, 255, 255)
        End Get
    End Property
    Public Shared ReadOnly Property Grey() As System.Drawing.Color
        Get
            Return System.Drawing.Color.FromArgb(255, 128, 128, 128)
        End Get
    End Property
    Public Shared ReadOnly Property Black() As System.Drawing.Color
        Get
            Return System.Drawing.Color.FromArgb(255, 0, 0, 0)
        End Get
    End Property
    Public Shared ReadOnly Property Control() As System.Drawing.Color
        Get
            Return System.Drawing.Color.FromKnownColor(KnownColor.Control)
        End Get
    End Property
    Public Shared ReadOnly Property Active() As System.Drawing.Color
        Get
            Return System.Drawing.Color.FromKnownColor(KnownColor.ActiveCaption)
        End Get
    End Property
End Structure
