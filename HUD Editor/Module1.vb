
Module Module1
    Public BGImage As New Bitmap(800, 600)
    Public OVImage As New Bitmap(16, 16)

    Public ViewedDialog As Integer = 0

    Public MaxNodeCount As Integer = 100

    Public NodeInformation As New ListView
    Public NodesToRender As New ListBox

    Public NodeType As String = ""
    Public Edited As Boolean = False
    Public CurrentIndex As Integer = -1
    Public Loading As Boolean = True
    Public SplitNodeNames As New ListBox
    Public SplitNodeIndices As New ListBox

    Public ImageSelectorImagePath As String
    Public OriginalImage(MaxNodeCount), ColoredImage(MaxNodeCount), SizedImage(MaxNodeCount) As Image

    'Picturenode Information

    Public Sub ProcessPictureNodeImage(ByVal PerformColoring As Boolean, ByVal PerformSizing As Boolean)
        If Loading = False And CurrentIndex <> -1 Then
            If PerformColoring = True Then ColoredImage(CurrentIndex) = colorImage(OriginalImage(CurrentIndex), Val(NodeInformation.Items(CurrentIndex).SubItems(3).Text) * 0.001, Val(NodeInformation.Items(CurrentIndex).SubItems(4).Text) * 0.001, Val(NodeInformation.Items(CurrentIndex).SubItems(5).Text) * 0.001, Val(NodeInformation.Items(CurrentIndex).SubItems(6).Text) * 0.001)
            If PerformSizing = True Then SizedImage(CurrentIndex) = ResizeImage(ColoredImage(CurrentIndex), Val(NodeInformation.Items(CurrentIndex).SubItems(7).Text), Val(NodeInformation.Items(CurrentIndex).SubItems(8).Text), True)
            Edited = True
        End If
    End Sub
    Public Sub ViewDialog(ByVal Index As Integer)
        ViewedDialog = Index
        SetBarsEnabled(False)
        ColorDialog.Close()
        SizeDialog.Close()
        PositionDialog.Close()
        NodeSelect.Close()
        RotationDialog.Close()
        VariablesDialog.Close()
        If Index = 1 Then ColorDialog.Show()
        If Index = 2 Then SizeDialog.Show()
        If Index = 3 Then PositionDialog.Show() : SetBarsEnabled(True)
        If Index = 4 Then RotationDialog.Show()
        If Index = 5 Then VariablesDialog.Show()
    End Sub
    Public Sub SetBarsEnabled(ByVal EnableBar As Boolean)
        Form1.TrackBarXPos.Enabled = EnableBar
        Form1.TrackBarYpos.Enabled = EnableBar
    End Sub

    Public Delegate Sub myPictureBoxAdapter(ByVal PBImage As Image, ByVal Invokefrom As Control)
    Public Sub PictureBoxAdapter(ByVal PBImage As Image, ByVal Invokefrom As Control)
        Try
            If Invokefrom.InvokeRequired Then
                Dim d As New myPictureBoxAdapter(AddressOf PictureBoxAdapter)
                Invokefrom.Invoke(d, PBImage, Invokefrom)
            Else
                Form1.PictureBox1.Image = PBImage
            End If
        Catch
        End Try
    End Sub

    Public Function ResizeImage(ByVal b As Bitmap, ByVal NewSizeX As Integer, ByVal NewSizeY As Integer, ByVal Stretch As Boolean) As Bitmap
        If NewSizeX < 1 Then NewSizeX = 1
        If NewSizeY < 1 Then NewSizeY = 1
        Dim returnbitmap As New Bitmap(NewSizeX, NewSizeY)
        Dim g As Graphics = Graphics.FromImage(returnbitmap)
        If Stretch = True Then
            g.DrawImage(b, 0, 0, NewSizeX + 1, NewSizeY + 1)
        Else
            Dim scale1 As Integer = NewSizeX * 100000 \ b.Size.Width
            Dim scale2 As Integer = NewSizeY * 100000 \ b.Size.Height
            Dim finalscale As Integer = 1
            If scale1 < scale2 Then finalscale = scale1
            If scale1 >= scale2 Then finalscale = scale2
            g.DrawImage(b, 0, 0, (b.Size.Width * finalscale \ 100000) + 1, (b.Size.Height * finalscale \ 100000) + 1)
        End If
        g.Dispose()
        Return returnbitmap
    End Function
    Public Function LoadImage(ByVal Path As String) As Bitmap
        Dim b As New Bitmap(32, 32)
        If System.IO.File.Exists(Path) Then
            If Path.ToLower.EndsWith(".dds") Then
                b = FreeImageAPI.FreeImage.GetBitmap(FreeImageAPI.FreeImage.LoadEx(Path, FreeImageAPI.FREE_IMAGE_FORMAT.FIF_DDS))
            Else
                If Path.ToLower.EndsWith(".tga") Then
                    b = FreeImageAPI.FreeImage.GetBitmap(FreeImageAPI.FreeImage.LoadEx(Path, FreeImageAPI.FREE_IMAGE_FORMAT.FIF_TARGA))
                Else
                    b = Image.FromFile(Path)
                End If
            End If
        End If
        Return b
    End Function
    Public Function colorImage(ByVal source As Bitmap, ByVal Amult As Single, ByVal Rmult As Single, ByVal Gmult As Single, ByVal Bmult As Single) As Bitmap
        Dim bm As New Bitmap(source.Width, source.Height)
        For y As Integer = 0 To bm.Height - 1
            For x As Integer = 0 To bm.Width - 1
                Dim c As Color = source.GetPixel(x, y)
                Dim Alpha As Integer = SetValueBounds(c.A * Amult, 0, 255)
                Dim Red As Integer = SetValueBounds(c.R * Rmult, 0, 255)
                Dim Green As Integer = SetValueBounds(c.G * Gmult, 0, 255)
                Dim Blue As Integer = SetValueBounds(c.B * Bmult, 0, 255)
                bm.SetPixel(x, y, Color.FromArgb(Alpha, Red, Green, Blue))
            Next
        Next
        Return bm
    End Function
    Public Function SetValueBounds(ByVal Value As Integer, ByVal Min As Integer, ByVal Max As Integer) As Integer
        If Value > Max Then Value = Max
        If Value < Min Then Value = Min
        Return Value
    End Function

    Public Function RenderPictureNode(ByVal b As Bitmap, ByVal Angle As Integer, ByVal PosX As Integer, ByVal PosY As Integer, ByVal RotVarMidX As Integer, ByVal RotVarMidY As Integer, ByVal RotVarAngle As Integer, Optional ByVal OffSetX As Integer = 0, Optional ByVal OffSetY As Integer = 0) As Bitmap
        Dim returnBitmap As New Bitmap(800, 600)
        Dim g As Graphics = Graphics.FromImage(returnBitmap)
        g.TranslateTransform(RotVarMidX + 400, RotVarMidY + 300)
        g.RotateTransform(RotVarAngle)
        g.TranslateTransform((PosX + OffSetX) - RotVarMidX - 400 + b.Width * 0.5, (PosY + OffSetY) - RotVarMidY - 300 + b.Height * 0.5)
        g.RotateTransform(Angle)
        g.TranslateTransform(b.Width * -0.5, b.Height * -0.5)
        g.DrawImage(b, New Point(0, 0))
        g.Dispose()
        Return returnBitmap
    End Function


    Public Function RenderTextNode(ByVal text As String, ByVal Font As System.Drawing.Font, ByVal PosX As Integer, ByVal PosY As Integer) As Bitmap
        Dim returnbitmap As New Bitmap(800, 600)
        Dim g As Graphics = Graphics.FromImage(returnbitmap)
        g.DrawString(text, Font, Brushes.Black, PosX, PosY)
        g.Dispose()
        Return returnbitmap
    End Function

    Public Function ListFolderFiles(ByVal Path As String, ByVal Filter As String) As String()
        Dim rval As New List(Of String)
        For Each file As String In IO.Directory.GetFiles(Path)
            '--Filter--
            Dim add As Boolean = False
            For Each f As String In Filter.Split("|")
                f = f.ToLower.Trim("*")
                If file.EndsWith(f) Then add = True
                If f = "." Then add = True
            Next
            '--end--
            If add Then rval.Add(file)
        Next
        For Each directory As String In IO.Directory.GetDirectories(Path)
            For Each file As String In ListFolderFiles(directory, Filter)
                rval.Add(file)
            Next
        Next
        Dim frval(rval.Count - 1) As String
        For i As Integer = 0 To rval.Count - 1
            frval(i) = rval(i)
        Next
        Return frval
    End Function
    Public Function ListFolderSubFolders(ByVal Path As String) As String()
        Dim rval As New List(Of String)
        For Each directory As String In IO.Directory.GetDirectories(Path)
            rval.Add(directory)
            For Each folder As String In ListFolderSubFolders(directory)
                rval.Add(folder)
            Next
        Next
        Dim frval(rval.Count - 1) As String
        For i As Integer = 0 To rval.Count - 1
            frval(i) = rval(i)
        Next
        Return frval
    End Function
    Public Function PathIsFile(ByVal Path As String) As Boolean
        Return IO.Path.GetFileName(Path) <> IO.Path.GetFileNameWithoutExtension(Path)
    End Function


    Structure Node
        Sub New(ByVal Name As String, ByVal type As String)
            Me.NodeName = Name
            Me.NodeType = type
        End Sub
        Public NodeName As String
        Public NodeParent As String
        Public NodeType As String
        Public Data As Object
    End Structure

    Structure PictureNode
        Sub New(ByVal TexturePath As String)
            Me.TexturePath = TexturePath
            Me.TextureImage = LoadImage(TexturePath)
        End Sub
        Public TexturePath As String
        Public Position As Point
        Public Rotation As Integer
        Public Color As Color
        Public Size As Size
        Public TextureImage As Image
        Public ColoredImage As Image
        Public SizedImage As Image
    End Structure
End Module
