using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace dataviewBindToMySQL
{
    public partial class Form1 : Form
    {
        private MySqlConnection connection;
        MySqlDataAdapter dAdapter;
        DataTable dTable;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;
        private string connectionString;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //  server = "localhost";
            //  database = "dd_parts";
            //  uid = "";
            //  password = null;
            //string  connectionString;
            //connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            //connection = new MySqlConnection(connectionString);

            Initialize();

            //LoadTable("47_001");

           // CloseConnection();
        }

        private void LoadTable(string table)
        {
            //create the connection string
            //string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\myDatabase.mdb";

            //create the database query
            string query = string.Format("SELECT * FROM {0}",table);

            //create an OleDbDataAdapter to execute the query

            //OleDbDataAdapter dAdapter = new OleDbDataAdapter(query, connString);
            dAdapter = new MySqlDataAdapter(query, connectionString);

            //create a command builder
            //OleDbCommandBuilder cBuilder = new OleDbCommandBuilder(dAdapter);
            MySqlCommandBuilder cBuilder = new MySqlCommandBuilder(dAdapter);

            //create a DataTable to hold the query results
            dTable = new DataTable();

            //---------------------------

            //fill the DataTable
            dAdapter.Fill(dTable);

            //the DataGridView
            //DataGridView dgView = new DataGridView();

            //BindingSource to sync DataTable and DataGridView
            BindingSource bSource = new BindingSource();

            //set the BindingSource DataSource
            bSource.DataSource = dTable;

            //set the DataGridView DataSource
            dataGridView1.DataSource = bSource;
        }

        private void Initialize()
        {
            server = "10.1.0.15";
            database = "dd_parts";
            uid = "dev";
            password = null;
            port = "3306";

            connectionString = "SERVER=" + server + ";" + "Port=" + port + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dAdapter.Update(dTable);
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject d = dataGridView1.GetClipboardContent();
                Clipboard.SetDataObject(d);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int noLines = lines.Count();
                //for (int i = 0; i < noLines; i++)
                //{
                //    dTable.Rows.Add();
                //}

                int row = dataGridView1.CurrentCell.RowIndex;
                int col = dataGridView1.CurrentCell.ColumnIndex;
                //int row = 0;
                //int col = 0;

                foreach (string line in lines)
                {
                    if (row < dataGridView1.RowCount && line.Length >0)
                    {
                        string[] cells = line.Split('\t');
                        for (int i = 0; i < cells.GetLength(0); ++i)
                        {
                            if (col + i <this.dataGridView1.ColumnCount)
                            {
                                try
                                {
                                    dataGridView1[col + i, row].Value = Convert.ChangeType(cells[i], dataGridView1[col + i, row].ValueType);
                                }
                                catch (Exception error)
                                {
                                    MessageBox.Show(error.Message);
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        row++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public void getTables()
        {

          
MySqlCommand command = connection.CreateCommand();
command.CommandText = "SHOW TABLES;";
MySqlDataReader Reader;
connection.Open();
Reader = command.ExecuteReader();

List<String> Tablenames = new List<String>();
while (Reader.Read())
{
    Tablenames.Add(Reader.GetString(0));
    //string row = "";
    //for (int i = 0; i < Reader.FieldCount; i++)
    //    row += Reader.GetValue(i).ToString() + ", ";
   // Console.WriteLine(row);
}

comboBox1.DataSource = Tablenames;
connection.Close();
            //string query = "SHOW TABLES;";
            
            ////Create a list to store the result
            //List<string> list = new List<string>();
            ////list[0] = new List<string>();
            ////list[1] = new List<string>();
            ////list[2] = new List<string>();

            ////Open connection
            //if (this.OpenConnection() == true)
            //{
            //    //Create Command
            //    MySqlCommand cmd = new MySqlCommand(query, connection);
            //    //Create a data reader and Execute the command
            //    MySqlDataReader dataReader = cmd.ExecuteReader();

            //    //Read the data and store them in the list
            //    while (dataReader.Read())
            //    {
                    
                    
            //        //list.Add(dataReader["PartNumber"] + "");
            //        // list[1].Add(dataReader["name"] + "");
            //        //list[2].Add(dataReader["age"] + "");
            //    }

            //    //close Data Reader
            //    dataReader.Close();

            //    //close Connection
            //    this.CloseConnection();

            //    //return list to be displayed
            //   // return list;
            //}
            //else
            //{
            //   // return list;
            //}
        }



        private void comboBox1_Click(object sender, EventArgs e)
        {
           // OpenConnection();

            // string tables = "SELECT TABLE_NAME FROM Information_Schema.Tables where Table_Type = 'BASE TABLE'";
            getTables();
           // CloseConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string form = string.Format("{0}", comboBox1.SelectedValue);
            LoadTable(form);
        }
    }
}
