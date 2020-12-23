using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Share;

namespace Server.Networking
{
    public class Server
    {

        const int SERVER_ID = 0;

        //Global varials for buffer, list of clients and the socket for the server
        private byte[] _buffer = new byte[2048];
        public List<Socket> _clientSockets { get; private set; }

        public Dictionary<Socket, string[]> _clientInfo { get; private set; }


        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        //Initialize the clietsocket list and the server with random IP:100
        public Server()
        {
            _clientSockets = new List<Socket>();
            _clientInfo = new Dictionary<Socket, string[]>();
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 5600));
            _serverSocket.Listen(500);

            //Begin accepting connections and call AcceptCallBack as result
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }

        //Accept Client async
        private void AcceptCallBack(IAsyncResult AR)
        {
            //Initialize new socket equal to passed as parameter
            Socket socket = _serverSocket.EndAccept(AR);

            //Add socket to list to keep track
            _clientSockets.Add(socket);

            // Ask for client info
            SendRequest(PacketType.ListClient, socket);

            //Begin recieving data and call RecieveCallBack as result
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallBack), socket);

            //Start accepting connections again
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }


        private void RecieveCallBack(IAsyncResult AR)
        {
            //Initialize new socket equal to the one passed as parameter
            Socket _socket = (Socket)AR.AsyncState;


            //Get ready to check for Socket errors
            SocketError SE;
            int leg = _socket.EndReceive(AR, out SE);

            //If there are no errors proceed
            if (SE == SocketError.Success) {
                byte[] DataBuf = new byte[leg];
                Array.Copy(_buffer, DataBuf, DataBuf.Length);

                //Manage the data recieved
                DataManager(new Packet(DataBuf), _socket);

                //Clear buffer and start accepting again
                _buffer = new byte[2048];
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
            }
            //If there are errors close socket and remove from list
            else
            {
                RemoveClient(_socket);
                _buffer = new byte[2048];
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
            }
        }

        public void DataManager(Packet p, Socket s) {
            //if (p.PData.Count == 0 p.PType == ) return;

            switch (p.PType) {
                case PacketType.ListClient:
                    if (!_clientInfo.ContainsKey(s)) {
                        _clientInfo.Add(s, new string[4]);
                    }
                    int numElements = p.PData.Count;
                    for (int i = 0; i < numElements; i++) {
                        _clientInfo[s][i] = p.PData[i];
                    }
                    _clientInfo[s][numElements] = s.RemoteEndPoint.ToString();
                    break;
            }
        }
        
        public void SendRequest(PacketType pType, params Socket[] sockets) {
            Packet p = new Packet(pType, SERVER_ID.ToString());
            foreach (Socket s in sockets) {
                try {
                    s.Send(p.ToBytes());
                } catch {
                    RemoveClient(s);
                }
            }
        }

        public void RemoveClient(Socket s) {
            s.Close(1);
            _clientSockets.Remove(s);
            _clientInfo.Remove(s);
        }

        public long PingClient(Socket s) {
            Ping ping = new Ping();
            string endpoint = ((IPEndPoint)s.RemoteEndPoint).Address.ToString();
            PingReply pr = ping.Send(endpoint, 120, new byte[0], null);
            return pr.RoundtripTime;
        }
    }
}
