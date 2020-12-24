using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Forms {
    public partial class DirectoryForm : Form {
        public DirectoryForm() {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void fillListView((string[], string[], string) tp) {
            (string[] directories, string[] files, string path) = tp;
            textBox1.Text = path;

            listView1.Clear();
            for (int i = 0; i < directories.Length; i++) {
                listView1.Items.Add(directories[i]);
            }
            for (int i = 0; i < files.Length; i++) {
                listView1.Items.Add(files[i]);
            }

        }
    }
}
