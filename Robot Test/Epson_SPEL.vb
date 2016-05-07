Option Explicit On
Imports RCAPINet
Module Epson_SPEL

    Public WithEvents Scara As RCAPINet.Spel
    Public pointpallet01 As SpelPoint
    Public pointpallet02 As SpelPoint
    Public pointpallet03 As SpelPoint
    Public pointpallet04 As SpelPoint


    Dim corner1 = New Single




    Public Sub InitApp()
        Scara = New RCAPINet.Spel
        With Scara
            .Initialize()
            .Project = "C:\EpsonRC70\Projects\vbcontorl\vbcontorl.sprj"
            .TLSet(1, -16.006, -0.568, 0, 0, 0, 0)
            .LimZ(-10)
        End With

        ' pointpallet01 = Scara.GetPoint(1)


        '    Scara.SetPoint(1, pointpallet01)



        Scara.SetPoint(1, 374.314, 76.43, -110.937, 0, 0, SpelHand.Lefty)
        Scara.SetPoint(2, 248.665, 289.621, -111.446, -136.823, 0, SpelHand.Lefty)
        Scara.SetPoint(3, 185.976, -56.505, -111.162, -98.779, 0, SpelHand.Lefty)
        Scara.SetPoint(4, 73.867, 180.489, -111.405, -13.634, 0, SpelHand.Lefty)

        'Scara.SetPoint(1, pointpallet01)
        'Scara.SetPoint(2, pointpallet02)
        'Scara.SetPoint(3, pointpallet03)
        'Scara.SetPoint(4, pointpallet04)
        '   Scara.Pallet(0, 1, 2, 3, 4, 20, 20)


    End Sub



    Public Sub EventReceived(ByVal sender As Object, ByVal e As RCAPINet.SpelEventArgs) Handles Scara.EventReceived
        MsgBox("received event " & e.Event)
    End Sub

End Module
