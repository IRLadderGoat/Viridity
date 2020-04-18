using Share;
using System;
using System.Net;
using System.Net.Sockets;

namespace Client.Networking
{
    public class ClientSocket
    {
        private static Socket _clientSocket;
        private static IPEndPoint _remoteAddr;
        private static byte[] _buffer;

        public ClientSocket(string ipAddr, int ipPort)
        {
            _remoteAddr = new IPEndPoint(IPAddress.Parse(ipAddr), ipPort);
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Connecting to {0}...", _remoteAddr.ToString());
            _clientSocket.BeginConnect(_remoteAddr, ConnectCallBack, null);
        }

        void ConnectCallBack(IAsyncResult result)
        {
            if (_clientSocket.Connected)
            {
                Console.WriteLine("Connected");
                _buffer = new byte[2048];
                _clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), null);
            }
            else
            {
                Console.WriteLine("trying again connect again to {0}...", _remoteAddr.ToString());
                _clientSocket.BeginConnect(_remoteAddr, ConnectCallBack, null);
            }
        }
        void RecieveCallback(IAsyncResult result)
        {
            byte[] packet = new byte[_clientSocket.EndReceive(result)];
            Array.Copy(_buffer, packet, packet.Length);
            Packet p = new Packet(packet);
            DataManager(p);
        }

        void DataManager(Packet packet)
        {
            switch (packet.PType)
            {
                case PacketType.ListClient:
                    Packet r = new Packet(PacketType.RegisterClient, _clientSocket.LocalEndPoint.ToString());

                    //r.PData.Add
                    //_clientSocket.Send(r.ToBytes());
                    break;

            }
        }
    }
}
