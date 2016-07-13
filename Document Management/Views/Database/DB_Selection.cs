using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DocumentManagement
{
    public partial class DB_Selection : Form
    {
        // Array of all available Databases
        private Dictionary<int, Database> allDBs = new Dictionary<int, Database>();

        // Help-String for catching the Database-Object from the Form2
        public static string addDBjson;

        System.Windows.Forms.Timer statusTimer = null;

        public DB_Selection()
        {
            InitializeComponent();
        }


        class DatabaseItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }


        private void DB_Selection_Load(object sender, EventArgs e)
        {
            
            GlobalApp.Options = new GlobalOptions();
            GlobalApp.Encryption = new Encryption();

            // FIXME loading databases dynamically from a xlsx file
            addDatabase(@"[
                    { 
                        'Name': 'Test', 
                        'Password' : '' 
                    },
                    { 
                        'Name': 'Test123', 
                        'Password' : '' 
                    },
                    { 
                        'Id' : 5,
                        'Name': 'ID Test', 
                        'Password' : '' 
                    }
                  ]");
        }


        private void addDatabase(string jsonData)
        {
            dynamic jsonArray = JsonConvert.DeserializeObject(jsonData);

            foreach (dynamic element in jsonArray)
            {
                if (element.Name != null && element.Password != null)
                {
                    Database tmpDB;
                    string name = element.Name;
                    string password = element.Password;

                    if (element.Id != null)
                    {
                        int id = element.Id;
                        if (allDBs.ContainsKey(id))
                        {
                            updateStatusBar(String.Format("Database {0} is corrupted! ID already exists!", name), Color.Green);
                            tmpDB = new Database(name, password);
                        } else
                        {
                            tmpDB = new Database(id, name, password);
                        }
                        
                    } else
                    {
                        tmpDB = new Database(name, password);
                    }

                    allDBs.Add(tmpDB.getId(), tmpDB);

                    DatabaseItem item = new DatabaseItem();
                    item.Text = tmpDB.getName();
                    item.Value = tmpDB.getId();

                    comboBox1.Items.Add(item);
                }
            }
        }


        private void updateStatusBar(string text, Color color)
        {
            toolStripStatusLabel1.ForeColor = color;
            toolStripStatusLabel1.Text = text;

            // Reset a already running timer to prevent asynchron problems
            if (statusTimer != null)
            {
                statusTimer.Stop();
                statusTimer = null;
            }

            // Setting a 5 second timer to hide the message
            statusTimer = new System.Windows.Forms.Timer();
            statusTimer.Interval = 5000; 
            statusTimer.Tick += new EventHandler(timer_resetStatusBar);
            statusTimer.Start();
        }

        private void timer_resetStatusBar (object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";

            // Reset Timer
            statusTimer.Stop();
            statusTimer = null;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DB_New newForm = new DB_New();

            // New Database
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                addDatabase(addDBjson);

                updateStatusBar("Database successfully added!", Color.Green);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Open Database
            if (((DatabaseItem)comboBox1.SelectedItem) != null)
            {
                MessageBox.Show(((DatabaseItem)comboBox1.SelectedItem).Value.ToString());
                // TODO Close this form and open the main Screen
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Delete 
            if (((DatabaseItem)comboBox1.SelectedItem) != null)
            {
                allDBs.Remove(((DatabaseItem)comboBox1.SelectedItem).Value);
                comboBox1.Items.Remove(comboBox1.SelectedItem);

                updateStatusBar("Database successfully deleted!", Color.Green);
            } else
            {
                updateStatusBar("Please select a database!", Color.Red);
            }

            GlobalApp.Options.editOption("test_bla", "Haha funzt!");

            GlobalApp.Options.saveOptions();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB_Options newForm = new DB_Options();

            if (newForm.ShowDialog() == DialogResult.OK)
            {
                updateStatusBar("Options successfully edited!", Color.Green);
            }
        }
    }
}
