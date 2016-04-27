Imports RCAPINet

Public Class Form1

    Const scalepoint As Integer = 10
    Const goodpoint1 As Integer = 11
    Const badpoint1 As Integer = 12
    Dim Back1() As Single = {187.804, -40.051, -111.231, -262.7}
    Dim Front1() As Single = {260.733, 302.082, -111.144, -148.46}
    Public Inside1() As Single = {57.937, 184.83, -111.334, -178.046}
    Public Outside1() As Single = {391.447, 76.419, -110.722, -185.602}
    Public rows As Integer = 19
    Public columns As Integer = 21
    Dim startx As Single
    Dim starty As Single

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Scara.Dispose()
        Scara.Out(1, 0)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Epson_SPEL.InitApp()

        If Not Scara.MotorsOn Then
            Scara.MotorsOn = True
        End If
        Scara.SetPoint(scalepoint, -2.1, 268.4, -100, 0, 0, RCAPINet.SpelHand.Lefty)


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
        zcord = Back1(2) + 5
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



                Scara.SetPoint(1, xcord, ycord, zcord, ucord, 0, RCAPINet.SpelHand.Lefty)

                ' Scara.SetPoint(2, xcord, ycord, zcord, -181.0, 0, RCAPINet.SpelHand.Lefty)

                Scara.Jump(1)
                
                Scara.SetPoint(1, xcord, ycord, zcord + 10, ucord, 0, RCAPINet.SpelHand.Lefty)
                Scara.Jump(1)
                Scara.Out(1, 1)
                Scara.Delay(1000)


                Scara.SetPoint(1, xcord, ycord, zcord, ucord, 0, RCAPINet.SpelHand.Lefty)

                Scara.Jump(1)
                Scara.Out(1, 2)
                Scara.Delay(250)



                Scara.Jump(scalepoint)
                'Scara.Out(1, 1)
                'Scara.Delay(250)
                'Scara.Out(1, 2)
                'Scara.Delay(250)

            Next

        Next
        Scara.Out(1, 0)

    End Sub

    Private Sub Btn_Pause_Click(sender As Object, e As EventArgs) Handles Btn_Pause.Click
        Scara.Pause()
        Btn_Pause.Enabled = False
        Button2.Enabled = True
        Btn_Continue.Enabled = True

    End Sub

    Private Sub Btn_Continue_Click(sender As Object, e As EventArgs) Handles Btn_Continue.Click
        Scara.Continue()
        Btn_Pause.Enabled = True
        Btn_Continue.Enabled = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Not Scara.MotorsOn Then
            Scara.MotorsOn = True
        Else
            Scara.MotorsOn = False
        End If
    End Sub

End Class
