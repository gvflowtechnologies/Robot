Module Epson_SPEL
    Public WithEvents Scara As RCAPINet.Spel
    Public Sub InitApp()
        Scara = New RCAPINet.Spel
        With Scara
            .Initialize()
            .Project = "c:\EpsonRC70\projects\API_Demos\Demo1 \demo1.sprj"
        End With
    End Sub

    Public Sub EventReceived(ByVal sender As Object, ByVal e As RCAPINet.SpelEventArgs) Handles Scara.EventReceived
        MsgBox("received event " & e.Event)
    End Sub

End Module
