using System;
using System.Threading;
using Client.Networking;

namespace Client
{
    class Program
    {
        private static string Ip = "83.93.223.103";
        private static int Port = 61323;
        static ClientSocket RSocket;


        static void Main(){
            RSocket = new ClientSocket(Ip, Port);
            while (true) { }

        }

    }
}
