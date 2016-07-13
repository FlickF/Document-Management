using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DocumentManagement
{
    public partial class DB_New : Form
    {
        public DB_New()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                bool confirmResult = true;

                if (textBox2.Text == "")
                {
                    var confirm = MessageBox.Show(
                        "Are you sure to create a new Database without a password?", 
                        "", 
                        MessageBoxButtons.YesNo
                    );

                    if (confirm == DialogResult.No)
                    {
                        confirmResult = false;
                    }
                }

                if (confirmResult) 
                {
                    dynamic[] newDB = { new ExpandoObject() };
                    newDB[0].Name = textBox1.Text;
                    newDB[0].Password = textBox2.Text;

                    DB_Selection.addDBjson = JsonConvert.SerializeObject(newDB);

                    this.DialogResult = DialogResult.OK;

                    textBox1.Text = "";
                    textBox2.Text = "";
                }
            } else
            {
                MessageBox.Show("Please enter a name for the new Database!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }

        private void DB_New_Load(object sender, EventArgs e)
        {

        }
    }
}
