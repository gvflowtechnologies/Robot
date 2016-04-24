Module Epson_SPEL
    Public WithEvents Scara As RCAPINet.Spel
    Public Sub InitApp()
        Scara = New RCAPINet.Spel
        With Scara
            .Initialize()
            .Project = "C:\EpsonRC70\projects\VBCONTORL\VBCONTORL.sprj"
            .TLSet(1, -16.01, -0.303, 0, 0, 0, 0)
        End With
    End Sub

    Public Sub EventReceived(ByVal sender As Object, ByVal e As RCAPINet.SpelEventArgs) Handles Scara.EventReceived
        MsgBox("received event " & e.Event)
    End Sub

End Module
