using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.Closed += (s, args) => this.Close();
            f1.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            if (customers.searchName(textBox2.Text))
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Give a Name");
                }
                else {
                    try
                    {                       
                        con.Open();
                        string sql = "UPDATE Customers SET Gender = @Gender, PhoneNumber = @Phone, FaxNumber = @Fax, Address = @Address, Email = @Mail, Notes = @Notes WHERE FullName LIKE @Name";
                        SqlCommand com = new SqlCommand(sql, con);
                        com.Parameters.AddWithValue("@Id", customers.idfinder(textBox2.Text));
                        com.Parameters.AddWithValue("@Name", textBox2.Text);
                        com.Parameters.AddWithValue("@Gender", comboBox1.Text);
                        com.Parameters.AddWithValue("@Phone", textBox3.Text);
                        com.Parameters.AddWithValue("@Fax", textBox6.Text);
                        com.Parameters.AddWithValue("@Address", textBox7.Text);
                        com.Parameters.AddWithValue("@Mail", textBox5.Text);
                        com.Parameters.AddWithValue("@Notes", richTextBox1.Text);
                        com.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Database Updated");
                        Form2_Load(sender, e);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Database Error");
                    }
                }
                
            }
            else {
                try
                {
                    int k = count() + 1;
                    con.Open();
                    string sql = "INSERT INTO Customers(Id, FullName, Gender, PhoneNumber, FaxNumber, Address, Email, Notes)";
                    sql += " VALUES (@Id, @Name, @Gender, @Phone, @Fax, @Address, @Mail, @Notes)";
                    SqlCommand com = new SqlCommand(sql, con);
                    com.Parameters.AddWithValue("@Id", k);
                    com.Parameters.AddWithValue("@Name", textBox2.Text);
                    com.Parameters.AddWithValue("@Gender", comboBox1.Text);
                    com.Parameters.AddWithValue("@Phone", textBox3.Text);
                    com.Parameters.AddWithValue("@Fax", textBox6.Text);
                    com.Parameters.AddWithValue("@Address", textBox7.Text);
                    com.Parameters.AddWithValue("@Mail", textBox5.Text);
                    com.Parameters.AddWithValue("@Notes", richTextBox1.Text);
                    com.ExecuteNonQuery();
                    con.Close();
                    Form2_Load(sender, e);
                    MessageBox.Show("Database Updated");
                }
                catch (Exception)
                {
                    MessageBox.Show("Database error2");
                }
            }
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            this.button4.Image = (Image)(new Bitmap(WindowsFormsApp1.Properties.Resources.outlook, new Size(32, 32)));
            textBox1.AutoCompleteCustomSource.Clear();
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            string sql = "SELECT * FROM Customers";
            SqlCommand com = new SqlCommand(sql, con);
            con.Open();
            SqlDataReader sdr = com.ExecuteReader();
            AutoCompleteStringCollection autotext = new AutoCompleteStringCollection();
            while (sdr.Read()) {
                autotext.Add(sdr.GetString(1));
                autotext.Add(sdr.GetString(2));
                autotext.Add(sdr.GetString(3));
                autotext.Add(sdr.GetString(4));
                autotext.Add(sdr.GetString(5));
                autotext.Add(sdr.GetString(6));
            }
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox1.AutoCompleteCustomSource = autotext;
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("None found");
            }
            else {
                if (!search(textBox1.Text, true))
                {
                    MessageBox.Show("None Found");
                }
            }
            
        }
        private Boolean search(string param,bool f) {
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            string sql = "SELECT * FROM Customers";
            SqlCommand com = new SqlCommand(sql, con);
            con.Open();
            SqlDataReader sdr = com.ExecuteReader();
            AutoCompleteStringCollection autotext = new AutoCompleteStringCollection();
            while (sdr.Read())
            {
                if (textBox1.Text == sdr.GetValue(1).ToString() || textBox1.Text == sdr.GetValue(2).ToString() || textBox1.Text == sdr.GetValue(3).ToString() || textBox1.Text == sdr.GetValue(4).ToString() || textBox1.Text == sdr.GetValue(5).ToString() || textBox1.Text == sdr.GetValue(6).ToString()) {
                    if (f) {
                        textBox2.Text = sdr.GetValue(1).ToString();
                        textBox3.Text = sdr.GetValue(3).ToString();
                        comboBox1.Text = sdr.GetValue(2).ToString();
                        textBox5.Text = sdr.GetValue(6).ToString();
                        if (sdr.GetValue(7) != null)
                        {
                            richTextBox1.Text = sdr.GetValue(7).ToString();
                        }

                        if (sdr.GetValue(4) != null)
                        {
                            textBox6.Text = sdr.GetValue(4).ToString();
                        }
                        textBox7.Text = sdr.GetValue(5).ToString();
                    }
                    return true;
                }
            }
            return false;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2_Click(sender, e);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (search(textBox2.Text, false))
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete "+textBox2.Text +"?","Confirm", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    SqlConnection con = new SqlConnection(connectionstring);
                    string sql = "DELETE FROM Customers WHERE FullName LIKE @name";
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    com.Parameters.AddWithValue("@name", textBox2.Text);
                    com.ExecuteNonQuery();
                    con.Close();
                    customers.fixID(customers.idfinder(textBox2.Text));
                    Form2_Load(sender, e);
                }
                
            }
            else {
                MessageBox.Show("This Customer doesn't exist in the database");
            }
        }


        private int count() {
            int k = 0;
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {
                string sql = "SELECT * FROM Customers";
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                SqlDataReader sdr = com.ExecuteReader();
                while (sdr.Read()) {
                    k++;
                }
                con.Close();
            }
            catch (Exception) {
                MessageBox.Show("Database Error Count");
            }
            
            return k;
        }

        private void backToMainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu f2 = new MainMenu();
            f2.Closed += (s, args) => this.Close();
            f2.Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(sender,e);

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                MessageBox.Show("You didn't give an email address");
            }
            else {
                Outlook.Application oApp = new Outlook.Application();
                Outlook._MailItem oMailItem = (Outlook._MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMailItem.To = textBox5.Text;
                oMailItem.Display(true);
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PDF File |*.pdf";
            saveFileDialog1.Title = "Save customers to PDF file";
            saveFileDialog1.FileName = "Customers.pdf";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "") {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                SqlConnection con = new SqlConnection(connectionstring);
                try
                {
                    string sql = "SELECT * FROM Customers";
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader sdr = com.ExecuteReader();
                    while (sdr.Read())
                    {
                        byte[] fileData = (byte[])sdr.GetValue(0);
                    }
                    con.Close();
                    using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs))

                    {

                        //bw.Write(fileData);

                        bw.Close();

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Database Error Count");
                }
            }
                
        }
    }
}
