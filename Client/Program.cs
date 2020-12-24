using System;
using System.Threading;
using Client.Networking;

namespace Client
{
    class Program
    {
        private static string Ip = "127.0.0.1";
        private static int Port = 61323;
        static ClientSocket RSocket;


        static void Main(){
            RSocket = new ClientSocket(Ip, Port);
            while (true) { }

        }

    }
}
