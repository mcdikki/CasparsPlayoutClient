using System;
using System.Threading;
using System.Net;
using Bespoke.Common;
using Bespoke.Common.Osc;

namespace Transmitter
{
    public class TcpTransmitter : ITransmitter
    {
        public void Start(OscPacket packet)
        {
            Assert.ParamIsNotNull(packet);

            mOscClient = new OscClient(Destination);
            mOscClient.Connect();

            mPacket = packet;
            mPacket.Client = mOscClient;
            mSendMessages = true;

            mTransmitterThread = new Thread(RunWorker);
            mTransmitterThread.Start();
        }

        public void Stop()
        {
            mSendMessages = false;
            mTransmitterThread.Join();

            mOscClient.Close();
        }

        private void RunWorker()
        {
            try
            {
                while (mSendMessages)
                {
                    mPacket.Send();

                    mTransmissionCount++;
                    Console.Clear();
                    Console.WriteLine("Osc Transmitter: Tcp");
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

        private static readonly IPEndPoint Destination = new IPEndPoint(IPAddress.Loopback, Program.Port);

        private volatile bool mSendMessages;
        private Thread mTransmitterThread;
        private OscPacket mPacket;
        private OscClient mOscClient;
        private int mTransmissionCount;        
    }
}
