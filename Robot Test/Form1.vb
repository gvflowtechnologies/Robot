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
    Dim Back1() As Single = {187.434, -41.349, -108.231, -262.7}
    Dim Front1() As Single = {260.733, 302.082, -111.144, -148.46}
    Public Inside1() As Single = {57.302, 183.312, -111.334, -178.046}
    Public Outside1() As Single = {390.0, 75.958, -110.722, -185.602}
    Public rows As Integer = 19
    Public columns As Integer = 21
    Dim startx As Single
    Dim starty As Single
    Dim PauseRequest As Boolean
    Dim ResumeMotion As Boolean
    Dim tmr_Timeout As Stopwatch
    Dim vacuumtip As TipState
    Const canistercheck As Integer = 16

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
        Scara.SetPoint(scalepoint, -2.1, 268.4, -98, 0, 0, RCAPINet.SpelHand.Lefty)
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
        Scara.Speed(30) '60 is production
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
        '     Set U angle
        For r = 0 To rows - 1
            If r > 10 Then ucord = -180
            For c = 0 To columns - 1

                xcord = Back1(0) - c * xinc1 - r * xinc2
                ycord = Back1(1) - c * yinc1 - r * yinc2

                ' Check for Presence of Canister
                If PauseRequest = True Then Controlled_Pause()
                Scara.SetPoint(1, xcord, ycord, zcord + canistercheck, ucord, 0, RCAPINet.SpelHand.Lefty)
                Scara.Jump(1)

                'vacuumtip = TipState.Off
                'Scara.Out(1, vacuumtip)

                Scara.WaitSw(8, True, 0.5)

                Dim whatreading As Integer
                whatreading = Scara.In(1)
                If whatreading = 9 Then

                    If PauseRequest = True Then Controlled_Pause()


                    ' Pick up Canister
                    Scara.SetPoint(1, xcord, ycord, zcord + 10, ucord, 0, RCAPINet.SpelHand.Lefty)

                    Scara.Move(1)
                    Scara.Out(1, 2) ' TURN ON VACUUM
                    tmr_Timeout.Restart() ' Reset timer to zero to prevent to long of a time 

                    Do Until Scara.In(2) = 1

                        Select Case tmr_Timeout.ElapsedMilliseconds
                            Case 250 To 1000
                                Scara.SetPoint(1, xcord, ycord, zcord + 5, ucord, 0, RCAPINet.SpelHand.Lefty)

                            Case 1001 To 2000
                                Scara.SetPoint(1, xcord, ycord, zcord + 3, ucord, 0, RCAPINet.SpelHand.Lefty)

                            Case 2001 To 3000
                                Scara.SetPoint(1, xcord, ycord, zcord + 1, ucord, 0, RCAPINet.SpelHand.Lefty)

                            Case Is > 3001
                                ' Exit Do
                        End Select
                        Scara.Move(1)
                        Application.DoEvents()
                        Thread.Sleep(1)

                    Loop


                    '     If PauseRequest = True Then Controlled_Pause()
                    ' Move to Scale and spit out canister
                    Scara.SetPoint(scalepoint, -2.1, 268.4, -96, 0, 0, RCAPINet.SpelHand.Lefty)
                    Scara.Jump(scalepoint)
                    Scara.Out(1, 1)
                    Thread.Sleep(250)

                    tmr_Timeout.Restart()
                    Scara.Out(1, 2)

                    tmr_Timeout.Restart()
                    Do Until Scara.In(2) = 1
                        If PauseRequest = True Then Controlled_Pause()
                        Select Case tmr_Timeout.ElapsedMilliseconds
                            Case 250 To 500
                                Scara.SetPoint(scalepoint, -2.1, 268.4, -96, 0, 0, RCAPINet.SpelHand.Lefty)
                                Scara.Move(scalepoint)
                            Case 501 To 750
                                Scara.SetPoint(scalepoint, -2.1, 268.4, -98, 0, 0, RCAPINet.SpelHand.Lefty)
                                Scara.Move(scalepoint)
                            Case 751 To 1000
                                Scara.SetPoint(scalepoint, -2.1, 268.4, -102, 0, 0, RCAPINet.SpelHand.Lefty)
                                Scara.Move(scalepoint)
                            Case Is > 1000
                                Exit Do
                        End Select

                        Application.DoEvents()
                        Thread.Sleep(1)

                    Loop
                    ' Spit Canister out 

                    Scara.SetPoint(1, xcord, ycord, zcord + 10, ucord, 0, RCAPINet.SpelHand.Lefty)
                    Scara.Jump(1)
                    Scara.Out(1, 1)
                    Thread.Sleep(250)


                    If PauseRequest = True Then Controlled_Pause()
                End If
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
        Scara.Out(1, 0)
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs)
        ' Check if door is open, if the door is open then pause the robot if not already paused.
        ' If door is closesd, then have the robot conitnue if not already running.
        Dim doorreading As Integer
        doorreading = Scara.In(1)
        If Scara.In(1) = 8 Or Scara.In(1) = 9 Then 'Robot should be running
            ' Check if robot is paused or not

            '   If Scara.PauseOn = True Then
            ' If paused is on turn on robot
            ' If make no change
            Scara.Continue()
            'End If

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
