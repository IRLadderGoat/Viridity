using System;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Server.Forms {
    public partial class DirectoryForm : Form {
        Networking.Server server;
        Socket clientSoc;
        string tempPath;
        public DirectoryForm ( Networking.Server server, Socket client ) {
            InitializeComponent();
            this.server = server;
            clientSoc = client;

            Share.Packet p = new Share.Packet(Share.PacketType.FileList, server.SERVER_ID.ToString());
            server.SendRequest(p, clientSoc);

            Thread.Sleep(200);
            fillListView(server.CurrentRequestedDirectory);
        }

        private void listView1_SelectedIndexChanged ( object sender, EventArgs e ) {

        }

        private void fillListView ( (string[], string[], string) tp ) {

            (string[] directories, string[] files, string path) = tp;

            if (directories == null || files == null || path == null)
                return;

            textBox1.Text = path;
            tempPath = path;

            listView1.Clear();

            for (int i = 0; i < directories.Length; i++) {
                listView1.Items.Add(directories[i], 0);
            }
            for (int i = 0; i < files.Length; i++) {
                listView1.Items.Add(files[i], 1);
            }
        }

        private void button1_Click ( object sender, EventArgs e ) {
            string[] temp = tempPath.Split('\\');
            if (temp.Length == 1)
                return;

            string requestFolder = "";
            for (int i = 0; i < temp.Length - 1; i++) {
                requestFolder += temp[i];
                if (i != temp.Length - 2 || temp.Length == 2) {
                    requestFolder += '\\';
                }

            }
            Share.Packet p = new Share.Packet(Share.PacketType.FileList, server.SERVER_ID.ToString());
            p.PData.Add(requestFolder);

            server.SendRequest(p, clientSoc);

            Thread.Sleep(200);
            fillListView(server.CurrentRequestedDirectory);
        }

        private void listView1_MouseDoubleClick ( object sender, MouseEventArgs e ) {
            if (listView1.SelectedItems.Count != 1)
                return;

            string requestFolder = tempPath + '\\' + listView1.SelectedItems[0].Text;

            Share.Packet p = new Share.Packet(Share.PacketType.FileList, server.SERVER_ID.ToString());
            p.PData.Add(requestFolder);

            server.SendRequest(p, clientSoc);

            Thread.Sleep(200);
            
            fillListView(server.CurrentRequestedDirectory);
        }
        private void goto_click ( object sender, EventArgs e ) {
            Share.Packet p = new Share.Packet(Share.PacketType.FileList, server.SERVER_ID.ToString());
            p.PData.Add(textBox1.Text);

            server.SendRequest(p, clientSoc);

            Thread.Sleep(200);
            fillListView(server.CurrentRequestedDirectory);
        }
    }
}
