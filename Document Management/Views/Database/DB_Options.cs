using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DocumentManagement
{
    public partial class DB_Options : Form
    {
        public DB_Options()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GlobalApp.Options.editOption("db_dir", textBox1.Text);
            GlobalApp.Options.saveOptions();

            this.DialogResult = DialogResult.OK;
        }

        private void DB_Options_Load(object sender, EventArgs e)
        {
            textBox1.Text = GlobalApp.Options.getOption("db_dir");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FileInfo fi1 = new FileInfo(GlobalApp.Options.getOption("db_dir"));
            List<string> list = new List<string> { "Dies ist ein streng Geheimer Text", "Mein Passwort lautet Banane" };
            GlobalApp.Encryption.encrypt(fi1, "test", list);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FileInfo fi1 = new FileInfo(GlobalApp.Options.getOption("db_dir"));
            string output = GlobalApp.Encryption.decrypt(fi1, "test");

            MessageBox.Show(output);
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}
