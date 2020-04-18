using System;
using System.Windows.Forms;


namespace Server
{
    public partial class Form1 : Form
    {
        public static Networking.Server serverSocket = new Networking.Server();

        public Form1()
        {
            InitializeComponent();
            
        }

        private void ListClients()
        {
            
            listView1.Items.Clear();

            string[,] socketInfo = serverSocket.getSocketInfo();
            for (int i = 0; i < socketInfo.GetLength(0); i++)
            {
                ListViewItem lvi = new ListViewItem{ Text = socketInfo[i,0] };
                lvi.SubItems.Add(socketInfo[i,1]);
                lvi.SubItems.Add(socketInfo[i,2]);
                listView1.Items.Add(lvi);
            }

            /*
            foreach (Socket _socket in serverSocket._clientSockets)
            {

                ListViewItem lvi = new ListViewItem
                {
                    Text = i.ToString()
                };
                lvi.SubItems.Add(_socket.RemoteEndPoint.ToString());
                lvi.SubItems.Add(_socket.Handle.ToString());
                listView1.Items.Add(lvi);
                i++;
            }
            */
            
        }
        private void ListClients(string pcName)
        {
            /*
            MethodInvoker mi = delegate
            {
                int i = 0;
                listView1.Items.Clear();
                foreach (Socket _socket in serverSocket._clientSockets)
                {
                    ListViewItem lvi = new ListViewItem
                    {
                        Text = i.ToString()
                    };
                    lvi.SubItems.Add(_socket.RemoteEndPoint.ToString());
                    lvi.SubItems.Add(_socket.Handle.ToString());
                    listView1.Items.Add(lvi);
                    i++;
                }
            };
            if (InvokeRequired)
                this.Invoke(mi);
            */
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListClients();
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
