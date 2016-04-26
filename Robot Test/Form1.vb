Imports RCAPINet

Public Class Form1

    Const scalepoint As Integer = 10
    Const goodpoint1 As Integer = 11
    Const badpoint1 As Integer = 12
    Dim Back1() As Single = {187.804, -40.051, -111.231, -262.7}
    Dim Front1() As Single = {260.733, 302.082, -111.144, -148.46}
    Public Inside1() As Single = {57.937, 184.83, -111.334, -178.046}
    Public Outside1() As Single = {391.447, 76.419, -110.722, -185.602}
    Public rows As Integer = 20
    Public columns As Integer = 20
    Dim startx As Single
    Dim starty As Single

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Scara.Dispose()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Epson_SPEL.InitApp()

        If Not Scara.MotorsOn Then
            Scara.MotorsOn = True
        End If
        Scara.SetPoint(scalepoint, -2.1, 268.4, -70, -175, 0, RCAPINet.SpelHand.Lefty)


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Scara.Stop()



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Btn_Pause.Enabled = True
        Button2.Enabled = False
        Scara.PowerHigh = False
        Dim xcord As Single
        Dim ycord As Single
        Dim zcord As Single
        xcord = CSng(TextBox1.Text)
        ycord = CSng(TextBox2.Text)
        zcord = CSng(TextBox3.Text)
        Scara.Tool(1)

        Btn_Continue.Enabled = False

        Scara.SetPoint(1, -2.1, 268.4, -70, -175, 0, RCAPINet.SpelHand.Lefty)
        Dim r As Integer
        Dim c As Integer

        Dim count As Integer
        For r = 0 To 
            For c = 1 To 20


                Scara.SetPoint(2, xcord, ycord, zcord, -181.0, 0, RCAPINet.SpelHand.Lefty)

                Scara.Jump(scalepoint)


            Next
        Next


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
