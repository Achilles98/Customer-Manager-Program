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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (login(textBox1.Text,textBox2.Text))
            {
                if (checkBox1.Checked)
                {
                    setter("true");
                }
                else {
                    setter("false");
                }
                this.Hide();
                MainMenu f2 = new MainMenu();
                f2.Closed += (s, args) => this.Close();
                f2.Show();
            }
            else
            {
                MessageBox.Show("Username or Password are incorrect");
                textBox1.Clear();
                textBox2.Clear();
            }
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                button1_Click(sender,e);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private Boolean login(string username, string password) {

            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {
                con.Open();
                string sql = "SELECT * FROM Users";
                SqlCommand com = new SqlCommand(sql, con);
                SqlDataReader sdr = com.ExecuteReader();
                
                while (sdr.Read())
                {
                    
                    if (username == sdr.GetValue(0).ToString() && password == sdr.GetValue(1).ToString())
                    {
                        return true;
                    }
                }
            }
            catch (Exception e) {
              MessageBox.Show("Database error");
            }
            return false;
        }
        private Boolean checkrem() {
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {
                con.Open();
                string sql = "SELECT * FROM Users";
                SqlCommand com = new SqlCommand(sql, con);
                SqlDataReader sdr = com.ExecuteReader();

                while (sdr.Read())
                {

                    if ("true" == sdr.GetValue(3).ToString())
                    {
                        textBox1.Text = sdr.GetValue(0).ToString();
                        textBox2.Text = sdr.GetValue(1).ToString();
                        return true;
                    }
                    else {
                        return false;
                    }
                    
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Database error");
            }
            return false;
        }
        private void setter(string f) {
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {
                con.Open();
                string sql = "UPDATE Users SET Remember = @rem WHERE Username LIKE @user";
                SqlCommand com = new SqlCommand(sql,con);
                com.Parameters.AddWithValue("@rem", f);
                com.Parameters.AddWithValue("@user", textBox1.Text);
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception) {
                MessageBox.Show("Database Error");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            if (checkrem()) {
                checkBox1.Checked = true;
            }

        }
    }
}
