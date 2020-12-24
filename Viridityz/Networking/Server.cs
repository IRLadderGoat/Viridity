using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Xml.Linq;
using Share;

namespace Server.Networking
{
    public class Server
    {

        public readonly int SERVER_ID = 0;

        //Global varials for buffer, list of clients and the socket for the server
        private byte[] _buffer = new byte[2048];
        public List<Socket> ClientSockets { get; private set; }
        public Dictionary<Socket, string[]> ClientInfo { get; private set; }

        public (string[], string[], string) CurrentRequestedDirectory { get; private set; }


        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        //Initialize the clietsocket list and the server with random IP:100
        public Server()
        {
            ClientSockets = new List<Socket>();
            ClientInfo = new Dictionary<Socket, string[]>();
            CurrentRequestedDirectory = (null, null, null);

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
            ClientSockets.Add(socket);

            // Ask for client info
            //Packet p = new Packet(PacketType.ListClient, SERVER_ID.ToString());
            //SendRequest(p, socket);

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
                    if (!ClientInfo.ContainsKey(s)) {
                        ClientInfo.Add(s, new string[4]);
                    }
                    int n_info = p.PData.Count;
                    for (int i = 0; i < n_info; i++) {
                        ClientInfo[s][i] = p.PData[i];
                    }
                    ClientInfo[s][n_info] = s.RemoteEndPoint.ToString();
                    break;
                case PacketType.FileDirectoryList:
                    
                    string[] directories = new string[p.PNum];
                    for (int i = 0; i < p.PNum; i++) {
                        directories[i] = p.PData[i];
                    }
                    int n_files = p.PData.Count;
                    string[] files = new string[(n_files - p.PNum) - 1];
                    for (int i = p.PNum; i < n_files; i++) {
                        files[i] = p.PData[i];
                    }
                    string path = p.PData[n_files];
                    CurrentRequestedDirectory = (directories, files, path);
                    break;
            }
        }
        
        public void SendRequest(Packet p, params Socket[] sockets) {
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
            ClientSockets.Remove(s);
            ClientInfo.Remove(s);
        }
    }
}
