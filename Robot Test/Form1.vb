Imports RCAPINet
Imports System.Threading

Public Class Form1

    Public Enum TipState
        Off = 0
        BlowOff = 1
        Vacuum = 2
    End Enum

    Const scalepoint As Integer = 10
    Const goodpoint1 As Integer = 11
    Const badpoint1 As Integer = 12
    Const pausepoint As Integer = 13
    Dim Back1() As Single = {187.804, -40.051, -111.231, -262.7}
    Dim Front1() As Single = {260.733, 302.082, -111.144, -148.46}
    Public Inside1() As Single = {57.937, 184.83, -111.334, -178.046}
    Public Outside1() As Single = {391.447, 76.419, -110.722, -185.602}
    Public rows As Integer = 19
    Public columns As Integer = 21
    Dim startx As Single
    Dim starty As Single
    Dim PauseRequest As Boolean
    Dim ResumeMotion As Boolean
    Dim tmr_Timeout As Stopwatch
    Dim vacuumtip As TipState

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Scara.Stop()
        Scara.Out(1, 0)
        Scara.Dispose()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Epson_SPEL.InitApp()
        PauseRequest = False
        ResumeMotion = False
        Btn_Continue.Enabled = False
        If Not Scara.MotorsOn Then
            Scara.MotorsOn = True
        End If
        Scara.SetPoint(scalepoint, -2.1, 268.4, -100, 0, 0, RCAPINet.SpelHand.Lefty)
        Scara.SetPoint(pausepoint, 100, 100, -70, 0, 0, RCAPINet.SpelHand.Lefty)
        tmr_Timeout = New Stopwatch
        tmr_Timeout.Start()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Scara.Stop()



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Btn_Pause.Enabled = True
        Button2.Enabled = False
        Scara.PowerHigh = True
        Dim xcord As Single
        Dim ycord As Single
        Dim zcord As Single
        Dim xinc1 As Single
        Dim yinc1 As Single
        Dim xinc2 As Single
        Dim yinc2 As Single

        xinc1 = (Back1(0) - Inside1(0)) / (columns - 1)
        yinc1 = (Back1(1) - Inside1(1)) / (columns - 1)
        xinc2 = (Back1(0) - Outside1(0)) / (rows - 1)
        yinc2 = (Back1(1) - Outside1(1)) / (rows - 1)




        'xcord = CSng(TextBox1.Text)
        'ycord = CSng(TextBox2.Text)
        'zcord = CSng(TextBox3.Text)

        Scara.Tool(1)
        Scara.LimZ(-65)
        Scara.Speed(60)
        Scara.Accel(30, 30)
        Btn_Continue.Enabled = False

        xcord = Back1(0)
        ycord = Back1(1)
        zcord = Back1(2) + 0
        Dim ucord As Single

        Scara.SetPoint(1, xcord, ycord, zcord, -175, 0, RCAPINet.SpelHand.Lefty)
        Dim r As Integer
        Dim c As Integer

        ucord = 0
        '      Scara.PowerHigh = True
        For r = 0 To rows - 1
            If r > 10 Then ucord = -180
            For c = 0 To columns - 1

                xcord = Back1(0) - c * xinc1 - r * xinc2
                ycord = Back1(1) - c * yinc1 - r * yinc2

                ' Go to pallet cordinate and spit canister out.
                ' Spit out one

                Scara.SetPoint(1, xcord, ycord, zcord + 15, ucord, 0, RCAPINet.SpelHand.Lefty)
                Scara.Jump(1)
                Scara.Out(1, 1)
                ' Wait until spit out
                Do Until Scara.In(2) = 1

                    Application.DoEvents()
                    Thread.Sleep(1)

                Loop
                vacuumtip = TipState.Off
                Scara.Out(1, vacuumtip)


                If PauseRequest = True Then Controlled_Pause()

                ' Pick up one
                Scara.SetPoint(1, xcord, ycord, zcord + 8, ucord, 0, RCAPINet.SpelHand.Lefty)

                Scara.Jump(1)
                Scara.Out(1, 2)
                tmr_Timeout.Restart() ' Reset timer to zero to prevent to long of a time 

                Do Until Scara.In(2) = 0

                    Select Case tmr_Timeout.ElapsedMilliseconds
                        Case 250 To 500
                            Scara.SetPoint(1, xcord, ycord, zcord + 6, ucord, 0, RCAPINet.SpelHand.Lefty)
                            Scara.Jump(1)
                        Case 501 To 750
                            Scara.SetPoint(1, xcord, ycord, zcord + 4, ucord, 0, RCAPINet.SpelHand.Lefty)
                            Scara.Jump(1)
                        Case 751 To 1000
                            Scara.SetPoint(1, xcord, ycord, zcord + 2, ucord, 0, RCAPINet.SpelHand.Lefty)
                            Scara.Jump(1)
                        Case Is > 1000
                            Exit Do
                    End Select

                    Application.DoEvents()
                    Thread.Sleep(1)

                Loop


                If PauseRequest = True Then Controlled_Pause()

                Scara.SetPoint(scalepoint, -2.1, 268.4, -98, 0, 0, RCAPINet.SpelHand.Lefty)
                Scara.Jump(scalepoint)
                Scara.Out(1, 1)

                tmr_Timeout.Restart()

                Do Until Scara.In(2) = 1
                    If tmr_Timeout.ElapsedMilliseconds > 1000 Then Exit Do

                    Application.DoEvents()
                    Thread.Sleep(1)
                Loop
                Scara.Out(1, 0)

                tmr_Timeout.Restart()
                Do Until Scara.In(2) = 0
                    If PauseRequest = True Then Controlled_Pause()
                    Select Case tmr_Timeout.ElapsedMilliseconds
                        Case 250 To 500
                            Scara.SetPoint(scalepoint, -2.1, 268.4, -100, 0, 0, RCAPINet.SpelHand.Lefty)
                            Scara.Jump(scalepoint)
                        Case 501 To 750
                            Scara.SetPoint(scalepoint, -2.1, 268.4, -102, 0, 0, RCAPINet.SpelHand.Lefty)
                            Scara.Jump(scalepoint)
                        Case 751 To 1000
                            Scara.SetPoint(scalepoint, -2.1, 268.4, -104, 0, 0, RCAPINet.SpelHand.Lefty)
                            Scara.Jump(scalepoint)
                        Case Is > 1000
                            Exit Do
                    End Select
 
                    Application.DoEvents()
                    Thread.Sleep(1)

                Loop


                If PauseRequest = True Then Controlled_Pause()

            Next

        Next
        Scara.Out(1, 0)

    End Sub

    Private Sub Btn_Pause_Click(sender As Object, e As EventArgs) Handles Btn_Pause.Click
        ' Set flag to request pause
        PauseRequest = True
        Me.BackColor = Color.IndianRed
        ResumeMotion = False
        Btn_Pause.Enabled = False
        Btn_Continue.Enabled = True
    End Sub

    Private Sub Btn_Continue_Click(sender As Object, e As EventArgs) Handles Btn_Continue.Click
        ResumeMotion = True
    End Sub
    Private Sub Controlled_Pause()

        ' 1. Jump to location.
        ' 2. Enable button to start
        ' 3. Wait for continue to be pressed


        Scara.Jump(pausepoint)

        Do
            Application.DoEvents()
            Thread.Sleep(1)

        Loop Until ResumeMotion = True

        Btn_Pause.Enabled = True
        Btn_Continue.Enabled = False
        PauseRequest = False
        Me.BackColor = Color.BurlyWood

    End Sub
 

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' Check if door is open, if the door is open then pause the robot if not already paused.
        ' If door is closesd, then have the robot conitnue if not already running.

        If Scara.In(2) = 8 Or Scara.In(2) = 9 Then 'Robot should be running
            ' Check if robot is paused or not

            If Scara.PauseOn = True Then
                ' If paused is on turn on robot
                ' If make no change
                Scara.Continue()
            End If

        Else ' Robot should not be running
            ' Check to see if robot is paused or not.

            If Scara.PauseOn = False Then
                ' If the robot is not paused, then pause it.
                ' Otherwise make no change
                Scara.Pause()

            End If

        End If

    End Sub
End Class
