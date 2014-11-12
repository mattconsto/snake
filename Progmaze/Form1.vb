Public Class Form1

    'Dim maze(,) As Integer = { _
    '                            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
    '                            {1, 0, 1, 0, 1, 1, 1, 1, 1, 1}, _
    '                            {0, 0, 1, 0, 0, 0, 0, 0, 0, 0}, _
    '                            {1, 1, 1, 1, 1, 0, 0, 0, 1, 1}, _
    '                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
    '                            {1, 1, 1, 0, 0, 1, 0, 1, 1, 1}, _
    '                            {0, 0, 0, 1, 0, 0, 0, 0, 1, 1}, _
    '                            {0, 1, 0, 1, 1, 1, 0, 0, 0, 0}, _
    '                            {0, 1, 0, 0, 0, 0, 0, 0, 1, 0}, _
    '                            {1, 1, 1, 1, 1, 1, 1, 0, 1, 0} _
    '                        }
    Dim locX As Integer = 5
    Dim locY As Integer = 4

    Dim locations() As Integer = {5 = 4, 5 = 3, 5 = 2}
    Dim direction As System.Windows.Forms.Keys = Keys.Right
    Dim scaleup As Double = 10

    Private Sub Render() Handles MyBase.Load
        Dim bm As New Bitmap(Me.Width, Me.Height)
        Dim X As Integer
        Dim Y As Integer

        For X = 0 To bm.Width - 1
            For Y = 0 To bm.Height - 1
                Dim newX = CStr(X / scaleup)
                Dim newY = CStr(Y / scaleup)
                If newX.Contains(".") Then newX = newX.Substring(0, newX.IndexOf("."))
                If newY.Contains(".") Then newY = newY.Substring(0, newY.IndexOf("."))
                'If maze(newX, newY) = 1 Then
                'bm.SetPixel(X, Y, Color.FromArgb(0, 0, 0))
                'Else
                bm.SetPixel(X, Y, Color.FromArgb(255, 255, 255))
                'End If
                If locX = newX And locY = newY Then
                    bm.SetPixel(X, Y, Color.FromArgb(0, 255, 0))
                End If
            Next Y
        Next X

        picCanvas.Image = bm
    End Sub

    Private Sub Render_Blob(ByVal newX, ByVal newY, ByVal oldX, ByVal oldY)
        Dim bm As New Bitmap(picCanvas.Image)
        Dim X As Integer
        Dim Y As Integer

        For X = (scaleup * (newX + 1)) - scaleup To scaleup * (newX + 1) - 1
            For Y = (scaleup * (newY + 1)) - scaleup To scaleup * (newY + 1) - 1
                bm.SetPixel(X, Y, Color.FromArgb(0, 255, 0))
            Next
        Next

        For X = (scaleup * (oldX + 1)) - scaleup To scaleup * (oldX + 1) - 1
            For Y = (scaleup * (oldY + 1)) - scaleup To scaleup * (oldY + 1) - 1
                'If maze(newX, newY) = 1 Then
                'bm.SetPixel(X, Y, Color.FromArgb(0, 0, 0))
                'Else
                bm.SetPixel(X, Y, Color.FromArgb(255, 255, 255))
                'End If
            Next
        Next

        picCanvas.Image = bm
    End Sub

    Private Sub Move_Blob() Handles Timer.Tick
        If locY = 0 Or locY = picCanvas.Height / scaleup - 1 _
            Or locX = 0 Or locX = picCanvas.Width / scaleup - 1 Then
            Exit Sub
        End If
        Dim tempV = 0
        Select Case direction
            Case Keys.Up
                tempV = locY
                locY = locY - 1
            Case Keys.Right
                tempV = locX
                locX = locX + 1
            Case Keys.Down
                tempV = locY
                locY = locY + 1
            Case Keys.Left
                tempV = locX
                locX = locX - 1
        End Select
        Render_Blob(locX, locY, tempV, locY)
    End Sub

    Private Sub Key_Handle(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode >= 37 And e.KeyCode <= 40 Then direction = e.KeyValue
    End Sub

End Class