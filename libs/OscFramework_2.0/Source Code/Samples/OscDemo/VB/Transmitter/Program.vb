Module Program
    Public Enum DemoType
        Udp
        Tcp
        Multicast
    End Enum

    Public ReadOnly Port As Integer = 5103

    Sub Main()
        Dim bundle As OscBundle = CreateTestBundle()
        Dim demoType As DemoType = GetDemoType()

        Dim transmitter As ITransmitter
        Select Case demoType
            Case Program.DemoType.Udp
                transmitter = New UdpTransmitter()

            Case Program.DemoType.Tcp
                transmitter = New TcpTransmitter()

            Case Program.DemoType.Multicast
                transmitter = New MulticaseTransmitter()

            Case Else
                Throw New Exception("Unsupported transmitter type.")
        End Select

        transmitter.Start(bundle)

        ' Stop the transmitter, and exit, when a key is pressed.
        Console.ReadKey()
        transmitter.Stop()
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

    Private Function CreateTestBundle() As OscBundle
        Dim sourceEndPoint As IPEndPoint = New IPEndPoint(IPAddress.Loopback, Port)
        Dim bundle As OscBundle = New OscBundle(sourceEndPoint)

        Dim nestedBundle As OscBundle = New OscBundle(sourceEndPoint)
        Dim nestedMessage As OscMessage = New OscMessage(sourceEndPoint, TestMethod)

        nestedMessage.AppendNil()
        nestedMessage.Append("Some String")
        nestedMessage.Append(10)
        nestedMessage.Append(100000L)
        nestedMessage.Append(1234.567F)
        nestedMessage.Append(10.0012345)
        nestedMessage.Append(New Byte() {1, 2, 3, 4})
        nestedMessage.Append(New OscTimeTag())
        nestedMessage.Append("c"c)
        nestedMessage.Append(Color.DarkGoldenrod)
        nestedMessage.Append(True)
        nestedMessage.Append(False)
        nestedMessage.Append(Single.PositiveInfinity)
        nestedBundle.Append(nestedMessage)
        bundle.Append(nestedBundle)

        Dim message As OscMessage = New OscMessage(sourceEndPoint, AliveMethod)
        message.Append(9876.543F)
        bundle.Append(message)

        Return bundle
    End Function

    Private ReadOnly AliveMethod As String = "/osctest/alive"
    Private ReadOnly TestMethod As String = "/osctest/alive"
End Module
