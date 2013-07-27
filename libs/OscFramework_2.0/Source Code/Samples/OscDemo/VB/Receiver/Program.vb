Module Program
    Public Enum DemoType
        Udp
        Tcp
        Multicast
    End Enum

    Sub Main()
        Dim demoType As DemoType = GetDemoType()

        Select Case demoType
            Case Program.DemoType.Udp
                sOscServer = New OscServer(TransportType.Udp, IPAddress.Loopback, Port)

            Case Program.DemoType.Tcp
                sOscServer = New OscServer(TransportType.Tcp, IPAddress.Loopback, Port)

            Case Program.DemoType.Multicast
                sOscServer = New OscServer(IPAddress.Parse("224.25.26.27"), Port)

            Case Else
                Throw New Exception("Unsupported receiver type.")
        End Select

        sOscServer.FilterRegisteredMethods = False
        sOscServer.RegisterMethod(AliveMethod)
        sOscServer.RegisterMethod(TestMethod)
        sOscServer.ConsumeParsingExceptions = False

        sOscServer.Start()

        Console.WriteLine("Osc Receiver: " + demoType.ToString())
        Console.WriteLine("Press any key to exit.")
        Console.ReadKey()

        sOscServer.Stop()
    End Sub

    Private Sub sOscServer_BundleReceived(ByVal sender As Object, ByVal e As OscBundleReceivedEventArgs) Handles sOscServer.BundleReceived
        sBundlesReceivedCount += 1

        Dim bundle As OscBundle = e.Bundle

        Console.WriteLine(String.Format("{0}Bundle Received [{1}:{2}]: Nested Bundles: {3} Nested Messages: {4}", Environment.NewLine, bundle.SourceEndPoint.Address, bundle.TimeStamp, bundle.Bundles.Count, bundle.Messages.Count))
        Console.WriteLine("Total Bundles Received: {0}", sBundlesReceivedCount)
    End Sub

    Private Sub sOscServer_MessageReceived(ByVal sender As Object, ByVal e As OscMessageReceivedEventArgs) Handles sOscServer.MessageReceived
        sMessagesReceivedCount += 1

        Dim message As OscMessage = e.Message

        Console.WriteLine(String.Format("{0}Message Received [{1}]: {2}", Environment.NewLine, message.SourceEndPoint.Address, message.Address))
        Console.WriteLine(String.Format("Message contains {0} objects.", message.Data.Count))

        For i As Integer = 0 To message.Data.Count - 1
            Dim dataString As String

            If message.Data(i) Is Nothing Then
                dataString = "Nil"
            Else
                dataString = If(TypeOf message.Data(i) Is Byte(), BitConverter.ToString(message.Data(i)), message.Data(i).ToString())
            End If

            Console.WriteLine(String.Format("[{0}]: {1}", i, dataString))
        Next

        Console.WriteLine("Total Messages Received: {0}", sMessagesReceivedCount)
    End Sub

    Private Sub sOscServer_ReceiveErrored(ByVal sender As Object, ByVal e As ExceptionEventArgs) Handles sOscServer.ReceiveErrored
        Console.WriteLine("Error during reception of packet: {0}", e.Exception.Message)
    End Sub

    Private Function GetDemoType() As DemoType
        Dim keyMappings As Dictionary(Of ConsoleKey, DemoType) = New Dictionary(Of ConsoleKey, DemoType)()
        keyMappings.Add(ConsoleKey.D1, DemoType.Udp)
        keyMappings.Add(ConsoleKey.D2, DemoType.Tcp)
        keyMappings.Add(ConsoleKey.D3, DemoType.Multicast)

        Console.WriteLine("{0}Welcome to the Bespoke Osc Receiver Demo.{0}Please select the type of receiver you would like to use:", Environment.NewLine)
        Console.WriteLine("  1. Udp{0}  2. Tcp{0}  3. Udp Multicast", Environment.NewLine)

        Dim key As ConsoleKeyInfo = Console.ReadKey()
        While keyMappings.ContainsKey(key.Key) = False
            Console.WriteLine("{0}Invalid selection{0}", Environment.NewLine)
            Console.WriteLine("  1. Udp{0}  2. Tcp{0}  3. Udp Multicast", Environment.NewLine)
            key = Console.ReadKey()
        End While

        Console.Clear()

        Return keyMappings(key.Key)
    End Function

    Private ReadOnly Port As Integer = 5103

    Private ReadOnly AliveMethod As String = "/osctest/alive"
    Private ReadOnly TestMethod As String = "/osctest/test"

    Private WithEvents sOscServer As OscServer
    Private sBundlesReceivedCount As Integer
    Private sMessagesReceivedCount As Integer
End Module
