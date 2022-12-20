Public Class SelectionSquare
    Public Property Changed() As Boolean
        Get
            If Me.Location.Changed = True Then Return True
            If Me.Size.Changed = True Then Return True
            If Me.Rotation.Changed = True Then Return True
            Return False
        End Get
        Set(ByVal value As Boolean)
            Me.Location.Changed = value
            Me.Size.Changed = value
            Me.Rotation.Changed = value
        End Set
    End Property
    Public Location As New WPoint(0, 0)
    Public Size As New WSize(0, 0)
    Public Rotation As WInteger = New WInteger(0)
    Public Display As SelectionSquareMode = SelectionSquareMode.Invisible
    Public Active As Boolean = True
    Public RotationEnabled As Boolean = True
    Public Property Rectangle() As Rectangle
        Get
            Return New Rectangle(Me.Location.Value, Me.Size.Value)
        End Get
        Set(ByVal value As Rectangle)
            Me.Location.Value = value.Location
            Me.Size.Value = value.Size
        End Set
    End Property
    Public ReadOnly Property Transform() As System.Drawing.Drawing2D.Matrix
        Get
            Transform = New System.Drawing.Drawing2D.Matrix
            ResetScreenGraphics(Transform)
            Transform.Translate(Me.Location.X + Me.Size.Width * 0.5, Me.Location.Y + Me.Size.Height * 0.5)
            Transform.Rotate(Me.Rotation.Value)
            Transform.Translate(Me.Size.Width * -0.5, Me.Size.Height * -0.5)
        End Get
    End Property


    Private OldRegion As New System.Drawing.Region
    Private ReadOnly Property NewRegion() As System.Drawing.Region
        Get
            Dim m As New System.Drawing.Drawing2D.Matrix
            Dim rect As New Rectangle(Me.Location.Value, Me.Size.Value)
            rect.Inflate(5, 5)
            ResetScreenGraphics(m)
            m.Translate(rect.X + rect.Width * 0.5, rect.Y + rect.Height * 0.5)
            m.Rotate(Me.Rotation.Value)
            m.Translate(rect.Width * -0.5, rect.Height * -0.5)
            NewRegion = New System.Drawing.Region(New Rectangle(New Point(0, 0), rect.Size))
            NewRegion.Transform(m)
        End Get
    End Property

    Public Sub UpdateOnScreen(Optional ByVal ForceRefresh As Boolean = False)
        If Me.Active = True Or ForceRefresh = True Then
            Form1.MainScreen.Invalidate(Me.OldRegion)
            If Me.Changed = True Or ForceRefresh = True Then
                Form1.MainScreen.Invalidate(Me.NewRegion)
                Me.OldRegion = Me.NewRegion
            End If
        End If
    End Sub
    Public Sub Draw(ByRef g As Graphics)
        If Me.Active = True Then
            g.Transform = Me.Transform
            If Me.Display = SelectionSquareMode.TranslucentSquare Then
                Dim c As System.Drawing.Color = System.Drawing.Color.FromArgb(128, 0, 150, 255)
                Dim b As New Drawing2D.LinearGradientBrush(New Rectangle(0, 0, 1, 1), c, c, 0)
                g.FillRectangle(b, 0, 0, Me.Size.Width, Me.Size.Height)
                Dim p As New Pen(System.Drawing.Color.Red)
                g.DrawRectangle(p, New Rectangle(0, 0, Me.Size.Width, Me.Size.Height))
                p.Color = Drawing.Color.Black
                p.DashStyle = Drawing2D.DashStyle.Dash
                g.DrawRectangle(p, New Rectangle(0, 0, Me.Size.Width, Me.Size.Height))
            Else
                Dim p As New Pen(System.Drawing.Color.Red)
                g.DrawRectangle(p, New Rectangle(0, 0, Me.Size.Width, Me.Size.Height))
                p.Color = Drawing.Color.Black
                p.DashStyle = Drawing2D.DashStyle.Dash
                g.DrawRectangle(p, New Rectangle(0, 0, Me.Size.Width, Me.Size.Height))
                If Me.Display = SelectionSquareMode.SquareAndDots Then
                    g.FillRectangle(New SolidBrush(System.Drawing.Color.White), New Rectangle(-3, -3, 6, 6))
                    g.FillRectangle(New SolidBrush(System.Drawing.Color.White), New Rectangle(Me.Size.Width - 3, -3, 6, 6))
                    g.FillRectangle(New SolidBrush(System.Drawing.Color.White), New Rectangle(Me.Size.Width - 3, Me.Size.Height - 3, 6, 6))
                    g.FillRectangle(New SolidBrush(System.Drawing.Color.White), New Rectangle(-3, Me.Size.Height - 3, 6, 6))
                    g.DrawRectangle(Pens.Black, New Rectangle(-3, -3, 6, 6))
                    g.DrawRectangle(Pens.Black, New Rectangle(Me.Size.Width - 3, -3, 6, 6))
                    g.DrawRectangle(Pens.Black, New Rectangle(Me.Size.Width - 3, Me.Size.Height - 3, 6, 6))
                    g.DrawRectangle(Pens.Black, New Rectangle(-3, Me.Size.Height - 3, 6, 6))
                    If Me.Size.Width > 31 Then
                        g.FillRectangle(New SolidBrush(System.Drawing.Color.White), New Rectangle(Me.Size.Width * 0.5 - 3, -3, 6, 6))
                        g.DrawRectangle(Pens.Black, New Rectangle(Me.Size.Width * 0.5 - 3, -3, 6, 6))
                        g.FillRectangle(New SolidBrush(System.Drawing.Color.White), New Rectangle(Me.Size.Width * 0.5 - 3, Me.Size.Height - 3, 6, 6))
                        g.DrawRectangle(Pens.Black, New Rectangle(Me.Size.Width * 0.5 - 3, Me.Size.Height - 3, 6, 6))
                    End If
                    If Me.Size.Height > 31 Then
                        g.FillRectangle(New SolidBrush(System.Drawing.Color.White), New Rectangle(-3, Me.Size.Height * 0.5 - 3, 6, 6))
                        g.DrawRectangle(Pens.Black, New Rectangle(-3, Me.Size.Height * 0.5 - 3, 6, 6))
                        g.FillRectangle(New SolidBrush(System.Drawing.Color.White), New Rectangle(Me.Size.Width - 3, Me.Size.Height * 0.5 - 3, 6, 6))
                        g.DrawRectangle(Pens.Black, New Rectangle(Me.Size.Width - 3, Me.Size.Height * 0.5 - 3, 6, 6))
                    End If
                    If Me.RotationEnabled = True And Me.Size.Width > 40 And Me.Size.Height > 40 Then
                        Dim imgpos As Point = New Point(Me.Size.Width * 0.5 - 5, Me.Size.Height * 0.5 - 5)
                        g.FillPie(Brushes.Red, New Rectangle(imgpos, New Size(10, 10)), 0, 180)
                    End If
                End If
            End If
            If Me.Changed = True Then
                Me.OldRegion = Me.NewRegion
                Me.Changed = False
            End If
        End If
    End Sub
    Public Function GetSelectionType(ByVal Point As Point) As Integer
        If Me.Active = False Then Return 0
        'Convert point to square-related
        'Point.X -= Form1.MainScreen.DrawOffset.X
        'Point.Y -= Form1.MainScreen.DrawOffset.Y
        Point.X -= Me.Location.X + Me.Size.Width * 0.5
        Point.Y -= Me.Location.Y + Me.Size.Height * 0.5
        Dim rotradian As Double = Me.Rotation.Value * Math.PI / -180
        If Point.X > 0 Then rotradian += 0.5 * Math.PI
        If Point.X < 0 Then rotradian += 1.5 * Math.PI
        If Point.Y <> 0 And Point.X <> 0 Then rotradian += Math.Atan(Point.Y / Point.X)
        Dim radius As Double = Math.Sqrt(Point.X ^ 2 + Point.Y ^ 2)
        If Point.Y > 0 And Point.X = 0 Then rotradian += Math.PI
        Point.X = Math.Sin(rotradian) * radius + Me.Size.Width * 0.5
        Point.Y = Me.Size.Height * 0.5 - Math.Cos(rotradian) * radius
        'Evaluate
        If Me.Display = SelectionSquareMode.SquareAndDots Then
            Dim TL As New Point(0, 0)
            Dim TM As Point = New Point(Me.Size.Width * 0.5, 0)
            Dim TR As Point = New Point(Me.Size.Width, 0)
            Dim ML As Point = New Point(0, Me.Size.Height * 0.5)
            Dim MR As Point = New Point(Me.Size.Width, Me.Size.Height * 0.5)
            Dim BL As Point = New Point(0, Me.Size.Height)
            Dim BM As Point = New Point(Me.Size.Width * 0.5, Me.Size.Height)
            Dim BR As Point = New Point(Me.Size.Width, Me.Size.Height)
            Dim MM As Point = New Point(Me.Size.Width * 0.5, Me.Size.Height * 0.5)
            If Math.Sqrt((TL.X - Point.X) ^ 2 + (TL.Y - Point.Y) ^ 2) <= 5 Then Return 1 'Top left
            If Math.Sqrt((TM.X - Point.X) ^ 2 + (TM.Y - Point.Y) ^ 2) <= 5 Then Return 2 'Top Middle
            If Math.Sqrt((TR.X - Point.X) ^ 2 + (TR.Y - Point.Y) ^ 2) <= 5 Then Return 3 'Top Right
            If Math.Sqrt((ML.X - Point.X) ^ 2 + (ML.Y - Point.Y) ^ 2) <= 5 Then Return 4 'Middle Left
            If Math.Sqrt((MR.X - Point.X) ^ 2 + (MR.Y - Point.Y) ^ 2) <= 5 Then Return 5 'Middle Right
            If Math.Sqrt((BL.X - Point.X) ^ 2 + (BL.Y - Point.Y) ^ 2) <= 5 Then Return 6 'Bottom Left
            If Math.Sqrt((BM.X - Point.X) ^ 2 + (BM.Y - Point.Y) ^ 2) <= 5 Then Return 7 'Botton Middle
            If Math.Sqrt((BR.X - Point.X) ^ 2 + (BR.Y - Point.Y) ^ 2) <= 5 Then Return 8 'Bottom Right
            If Me.Size.Width > 40 And Me.Size.Height > 40 And Me.RotationEnabled = True Then
                If Math.Sqrt((MM.X - Point.X) ^ 2 + (MM.Y - Point.Y) ^ 2) <= 8 Then Return 10 'Middle rotation
            End If
        End If
        If Point.X >= 0 And Point.Y >= 0 And Point.X <= Me.Size.Width And Point.Y <= Me.Size.Height Then Return 9 'Middle
        Return 0 'Outside
    End Function
End Class