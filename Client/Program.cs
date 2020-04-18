using System;
using System.Net;
using System.Net.Sockets;
using Client.Networking;
using System.Threading;

namespace Client
{
    class Program
    {
        private static string ipAddr = "127.0.0.1";
        private static int ipPort = 5600;

        static void Main(string[] args)
        {
            ClientSocket cs = new ClientSocket(ipAddr, ipPort);
            while (true)
                Console.ReadKey();
        }

        private static void AttemptConnect()
        {
            int attempts = 0;
            for (int i = 50; i > attempts;)
                try
                {
                    attempts++;
                    ClientSocket cs = new ClientSocket(ipAddr, ipPort);
                    Thread.Sleep(5000);
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("Times tried connecting: " + attempts.ToString());
                    Console.WriteLine(e.Message);
                }   
        }
    }
}
