using Share;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        public static Networking.Server serverSocket = new Networking.Server();

        public Form1()
        {
            InitializeComponent();

            Timer timer = new Timer();
            timer.Tick += new EventHandler(ListClients);
            timer.Interval = 10000;
            timer.Start();
            //System.Threading.Timer timer = new System.Threading.Timer((TimerCallback)ListClients, null, 10000, 10000);
        }

        private void ListClients(object sender, EventArgs e)
        {
            
            listView1.Items.Clear();
            serverSocket.SendRequest(PacketType.ListClient, serverSocket._clientSockets.ToArray());

            foreach (KeyValuePair<Socket,string[]> client in serverSocket._clientInfo) {
                string ping = serverSocket.PingClient(client.Key).ToString();
                ListViewItem lvi = new ListViewItem { Text = client.Value[client.Value.Length - 1] };

                for (int i = 0; i < client.Value.Length - 1; i++) {
                    lvi.SubItems.Add(client.Value[i]);
                }
                lvi.SubItems.Add(ping);
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
    }
}
