Public Class Form

    Dim locations() As String = {"40.27", "40.28", "40.29", "40.30"} 'Start positions
    Dim direction As Integer = 0 'Keycode for direction
    Dim scaleup As Double = 10 'Scaling of image
    Dim roomsize() As Integer = {80, 60}
    Dim random As New Random
    Dim obsticals() As String
    Dim food() As Integer = {random.Next(1, roomsize(0) - 1), random.Next(1, roomsize(1) - 1)}
    Dim score As Integer = 0 'Score displayed to user (Blobs eaten)

    Private Sub Render_Load() Handles MyBase.Load
        'Creates a bitmat the size of the render
        Dim bm As New Bitmap(picCanvas.Width, picCanvas.Height)
        picCanvas.Image = bm
        'Render snake and food
        Render_Snake()
        Render_Food()
    End Sub

    Private Sub Render_Snake()
        'This sub render the snake and only the snake when its called
        Dim bm As New Bitmap(picCanvas.Image) 'We load the existing image as we only want to modify it

        For count As Integer = 0 To locations.Length - 1
            'Skips to the end so it doesnt hog CPU. and is playable with a moderate score.
            'We dont do this as all it doesnt is turn green green
            If count = 3 Then count = locations.Length - 1

            'This is for every part of the snake
            Dim touched As Boolean = False
            Dim cloc() As String = Split(locations(count), ".")
            If cloc.Length > 1 Then 'This is just to ensure that we dont end up with an invalid input which can crash the game
                For X = (scaleup * cloc(0)) - scaleup To scaleup * cloc(0) - 1 'Some scaling which means that its visible
                    For Y = (scaleup * cloc(1)) - scaleup To scaleup * cloc(1) - 1
                        If X >= 0 And Y >= 0 Then 'So that it doesnt crash when you leave the screen
                            If count = 0 And bm.GetPixel(X, Y) = Color.FromArgb(0, 0, 255) Then
                                'If the head goes over the food (blue block)
                                If Not touched Then 'We only want to activate it once so we do this
                                    'Generate a new peice of food in another random location
                                    touched = True
                                    ReDim food(1)
                                    food(0) = random.Next(2, roomsize(0))
                                    food(1) = random.Next(2, roomsize(1))

                                    'Increase the players score and snake length
                                    score += 1
                                    lblScore.Text = "Score: " + CStr(score)
                                    Increase_Snake(cloc)
                                    picCanvas.Image = bm
                                    Render_Food()
                                    bm = picCanvas.Image
                                End If
                                'Create obstical that kills us if we crash, added difficulty
                                bm.SetPixel(X, Y, Color.FromArgb(255, 0, 0))
                            ElseIf count = 0 And (bm.GetPixel(X, Y) = Color.FromArgb(255, 0, 0) Or bm.GetPixel(X, Y) = Color.FromArgb(0, 255, 0)) Then 'If it is red/green
                                'If the head (count = 0) touched either a red block or its own body (green)
                                'We kill the game and let the user start again
                                Reset_Game()
                                Exit Sub
                            ElseIf Not bm.GetPixel(X, Y) = Color.FromArgb(255, 0, 0) Then
                                'If the current pixel isnt red we render the actual snake here, obsticals are handled above
                                Select Case count
                                    Case 0 : bm.SetPixel(X, Y, Color.FromArgb(0, 150, 0)) 'Head
                                    Case locations.Length - 1 : bm.SetPixel(X, Y, Color.FromArgb(255, 255, 255)) 'Behind (Sets a pixel the same colour as the background)
                                    Case Else : bm.SetPixel(X, Y, Color.FromArgb(0, 255, 0)) 'Body
                                End Select
                            End If
                        End If
                    Next
                Next
            End If
        Next
        'Show the user the updated game
        picCanvas.Image = bm

    End Sub

    Private Sub Increase_Snake(ByVal cloc)
        Console.WriteLine("Increased")
        cloc = Split(locations(locations.Length - 1), ".")

        'The locations of the snake are stored in an array which I would make multidimensional
        'But cannot seem how to redim it. So I have used this. I feel dirty.
        ReDim Preserve locations(locations.Length)
        Select Case direction
            'This adds an extra element to the array that is in a different place depedning upon the direction
            Case 37 : locations(locations.Length - 1) = CStr(cloc(0) + 1) + "." + CStr(cloc(1)) 'Left
            Case 38 : locations(locations.Length - 1) = CStr(cloc(0)) + "." + CStr(cloc(1) + 1) 'Up
            Case 39 : locations(locations.Length - 1) = CStr(cloc(0) - 1) + "." + CStr(cloc(1)) 'Right
            Case 40 : locations(locations.Length - 1) = CStr(cloc(0)) + "." + CStr(cloc(1) - 1) 'Down
        End Select
    End Sub

    Private Sub Render_Food()
        Dim bm As New Bitmap(picCanvas.Image)

        For X = (scaleup * food(0)) - scaleup To scaleup * food(0) - 1
            For Y = (scaleup * food(1)) - scaleup To scaleup * food(1) - 1
                If X >= 0 And Y >= 0 Then
                    bm.SetPixel(X, Y, Color.FromArgb(0, 0, 255))
                End If
            Next
        Next

        picCanvas.Image = bm
    End Sub

    Private Sub Timer_Tick() Handles Timer.Tick
        If direction >= 37 And direction <= 40 Then
            For count As Integer = locations.Length - 1 To 0 Step -1
                If count = 0 Then
                    Dim cloc() As String = Split(locations(count), ".")
                    If (cloc(0) = 1 Or cloc(1) = 1 Or cloc(0) = roomsize(0) Or cloc(1) = roomsize(1)) And Not direction = 0 Then
                        Reset_Game()
                    Else
                        Select Case direction
                            Case 37 : locations(count) = CStr(cloc(0) - 1) + "." + CStr(cloc(1))
                            Case 38 : locations(count) = CStr(cloc(0)) + "." + CStr(cloc(1) - 1)
                            Case 39 : locations(count) = CStr(cloc(0) + 1) + "." + CStr(cloc(1))
                            Case 40 : locations(count) = CStr(cloc(0)) + "." + CStr(cloc(1) + 1)
                        End Select
                    End If
                Else
                    locations(count) = locations(count - 1)
                End If
            Next
            Render_Snake()
        End If
    End Sub

    Private Sub Reset_Game()
        Timer.Enabled = False
        MsgBox("You scored " + CStr(score) + " points", Nothing, "Snake")
        direction = 0
        ReDim locations(3)
        locations(0) = "40.27"
        locations(1) = "40.28"
        locations(2) = "40.29"
        locations(3) = "40.30"
        score = 0
        lblScore.Text = "Score: " + CStr(score)
        Render_Load()
        Render_Snake()
        Render_Food()
        Timer.Enabled = True
    End Sub

    Private Sub Key_Handle(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If (e.KeyCode = 37 And Not direction = 39) Or _
                (e.KeyCode = 38 And Not direction = 40) Or _
                (e.KeyCode = 39 And Not direction = 37) Or _
                (e.KeyCode = 40 And Not direction = 38 And Not direction = 0) Then
            direction = e.KeyCode
        End If
        '37 = left, 38 = up, 39 = right, 40 = down

        Select Case e.KeyCode
            Case Keys.D
                For Each loca In locations
                    Console.Write(loca.ToString + " ")
                Next
                Console.Write(vbCrLf)
            Case Keys.F
                ReDim food(1)
                food(0) = random.Next(2, roomsize(0))
                food(1) = random.Next(2, roomsize(1))
                Render_Food()
            Case Keys.P
                direction = 0
        End Select
    End Sub

End Class