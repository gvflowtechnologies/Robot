Public Class Form1



    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Scara.Dispose()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Epson_SPEL.InitApp()

        '  Scara.AvoidSingularity = True
        If Not Scara.MotorsOn Then
            Scara.MotorsOn = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Scara.Stop()



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Btn_Pause.Enabled = True
        Button2.Enabled = False
        Dim xcord As Single
        Dim ycord As Single
        Dim zcord As Single
        xcord = CSng(TextBox1.Text)
        ycord = CSng(TextBox2.Text)
        zcord = CSng(TextBox3.Text)
        Scara.Tool(1)

        Btn_Continue.Enabled = False
        Scara.Pallet(0, )
        Scara.SetPoint(1, -2.1, 268.4, -70, -175, 0, RCAPINet.SpelHand.Lefty)

        Dim count As Integer
        For count = 1 To 10
            Scara.SetPoint(2, xcord, ycord, zcord, -181.0, 0, RCAPINet.SpelHand.Lefty)
            Scara.Jump(1)
            Scara.Jump(2)
            count += 1
            xcord = xcord + 25
            ycord = ycord + 25
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
