using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Share;

namespace Server.Networking
{
    public class Server
    {
        //Global varials for buffer, list of clients and the socket for the server
        private byte[] _buffer = new byte[2048];
        public List<Socket> _clientSockets { get; private set; }
        
        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        //Initialize the clietsocket list and the server with random IP:100
        public Server()
        {
            _clientSockets = new List<Socket>();
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
                //PackageManager(DataBuf);
                //DataManager(new Packet(DataBuf));
                PacketHandler(new Packet(DataBuf));    

                //Clear buffer and start accepting again
                _buffer = new byte[2048];
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
            }
            //If there are errors close socket and remove from list
            else
            {
                _socket.Close(1);
                _clientSockets.Remove(_socket);
                return;
            }
        }

        public string[,] getSocketInfo()
        {
            string[,] socketInfo = new string[_clientSockets.Count,3];

            for (int i = 0; i < _clientSockets.Count; i++)
            {
                socketInfo[i, 0] = i.ToString();
                socketInfo[i, 1] = _clientSockets[i].RemoteEndPoint.ToString();
                socketInfo[i, 2] = _clientSockets[i].Handle.ToString();
            }
            return socketInfo;
        }
    }
}
