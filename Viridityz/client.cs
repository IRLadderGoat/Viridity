using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class client
    {
        protected Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        protected string ipAddr = "localhost";
        protected int ipPort = 100;

        private void LoopConnect()
        {
            int attempts = 0;
            while (!_clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    _clientSocket.Connect(ipAddr, ipPort);
                }
                catch (SocketException e)
                {
                    Console.Clear();
                    Console.WriteLine("Times tried connecting: " + attempts.ToString());
                    Console.WriteLine(e.Message);
                }
            }
            //Console.Clear();
            Console.WriteLine("Connected");
        }

    }
}
