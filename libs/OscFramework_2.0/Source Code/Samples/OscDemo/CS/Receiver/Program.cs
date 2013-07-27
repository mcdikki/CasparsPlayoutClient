using System;
using System.Net;
using System.Collections.Generic;
using Bespoke.Common;
using Bespoke.Common.Osc;

namespace Receiver
{
    public enum DemoType
    {
        Udp,
        Tcp,
        Multicast
    }

	public class Program
	{
		public static void Main(string[] args)
		{
            OscServer oscServer;
            DemoType demoType = GetDemoType();
            switch (demoType)
            {
                case DemoType.Udp:
                    oscServer = new OscServer(TransportType.Udp, IPAddress.Loopback, Port);
                    break;

                case DemoType.Tcp:
                    oscServer = new OscServer(TransportType.Tcp, IPAddress.Loopback, Port);
                    break;

                case DemoType.Multicast:
                    oscServer = new OscServer(IPAddress.Parse("224.25.26.27"), Port);
                    break;

                default:
                    throw new Exception("Unsupported receiver type.");
            }
            
            oscServer.FilterRegisteredMethods = false;
			oscServer.RegisterMethod(AliveMethod);
            oscServer.RegisterMethod(TestMethod);
            oscServer.BundleReceived += new EventHandler<OscBundleReceivedEventArgs>(oscServer_BundleReceived);
			oscServer.MessageReceived += new EventHandler<OscMessageReceivedEventArgs>(oscServer_MessageReceived);
            oscServer.ReceiveErrored += new EventHandler<ExceptionEventArgs>(oscServer_ReceiveErrored);
            oscServer.ConsumeParsingExceptions = false;

            oscServer.Start();

			Console.WriteLine("Osc Receiver: " + demoType.ToString());
			Console.WriteLine("Press any key to exit.");
			Console.ReadKey();
			oscServer.Stop();
		}

        private static DemoType GetDemoType()
        {
            Dictionary<ConsoleKey, DemoType> keyMappings = new Dictionary<ConsoleKey, DemoType>();
            keyMappings.Add(ConsoleKey.D1, DemoType.Udp);
            keyMappings.Add(ConsoleKey.D2, DemoType.Tcp);
            keyMappings.Add(ConsoleKey.D3, DemoType.Multicast);

            Console.WriteLine("\nWelcome to the Bespoke Osc Receiver Demo.\nPlease select the type of receiver you would like to use:");
            Console.WriteLine("  1. Udp\n  2. Tcp\n  3. Udp Multicast");

            ConsoleKeyInfo key = Console.ReadKey();
            while (keyMappings.ContainsKey(key.Key) == false)
            {
                Console.WriteLine("\nInvalid selection\n");
                Console.WriteLine("  1. Udp\n  2. Tcp\n  3. Udp Multicast");
                key = Console.ReadKey();
            }

            Console.Clear();

            return keyMappings[key.Key];
        }

        private static void oscServer_BundleReceived(object sender, OscBundleReceivedEventArgs e)
        {
            sBundlesReceivedCount++;

            OscBundle bundle = e.Bundle;
            Console.WriteLine(string.Format("\nBundle Received [{0}:{1}]: Nested Bundles: {2} Nested Messages: {3}", bundle.SourceEndPoint.Address, bundle.TimeStamp, bundle.Bundles.Count, bundle.Messages.Count));
            Console.WriteLine("Total Bundles Received: {0}", sBundlesReceivedCount);
        }

		private static void oscServer_MessageReceived(object sender, OscMessageReceivedEventArgs e)
		{
            sMessagesReceivedCount++;

            OscMessage message = e.Message;

            Console.WriteLine(string.Format("\nMessage Received [{0}]: {1}", message.SourceEndPoint.Address, message.Address));
            Console.WriteLine(string.Format("Message contains {0} objects.", message.Data.Count));

            for (int i = 0; i < message.Data.Count; i++)
            {
                string dataString;

                if (message.Data[i] == null)
                {
                    dataString = "Nil";
                }
                else
                {
                    dataString = (message.Data[i] is byte[] ? BitConverter.ToString((byte[])message.Data[i]) : message.Data[i].ToString());
                }
                Console.WriteLine(string.Format("[{0}]: {1}", i, dataString));
            }

            Console.WriteLine("Total Messages Received: {0}", sMessagesReceivedCount);
		}

        private static void oscServer_ReceiveErrored(object sender, ExceptionEventArgs e)
        {
            Console.WriteLine("Error during reception of packet: {0}", e.Exception.Message);
        }

		private static readonly int Port = 5103;
		private static readonly string AliveMethod = "/osctest/alive";
        private static readonly string TestMethod = "/osctest/test";

        private static int sBundlesReceivedCount;
        private static int sMessagesReceivedCount;
	}
}
