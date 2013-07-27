using System;
using System.Threading;
using System.Net;
using Bespoke.Common;
using Bespoke.Common.Osc;

namespace Transmitter
{
    public class MulticastTransmitter : ITransmitter
    {
        public void Start(OscPacket packet)
        {
            Assert.ParamIsNotNull(packet);

            mPacket = packet;
            mSendMessages = true;

            mTransmitterThread = new Thread(RunWorker);
            mTransmitterThread.Start();
        }

        public void Stop()
        {
            mSendMessages = false;
            mTransmitterThread.Join();
        }

        private void RunWorker()
        {
            try
            {
                while (mSendMessages)
                {
                    mPacket.Send(Destination);

                    mTransmissionCount++;
                    Console.Clear();
                    Console.WriteLine("Osc Transmitter: Multicast");
                    Console.WriteLine("Transmission Count: {0}\n", mTransmissionCount);
                    Console.WriteLine("Press any key to exit.");

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static readonly IPEndPoint Destination = new IPEndPoint(IPAddress.Parse("224.25.26.27"), Program.Port);

        private volatile bool mSendMessages;
        private Thread mTransmitterThread;
        private OscPacket mPacket;
        private int mTransmissionCount;
    }
}
