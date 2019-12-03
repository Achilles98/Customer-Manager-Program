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
    public partial class NewMeeting : Form
    {
        public NewMeeting()
        {
            InitializeComponent();
        }

        private void NewMeeting_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = DateTime.Now;
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            string sql = "SELECT * FROM Customers";
            SqlCommand com = new SqlCommand(sql, con);
            con.Open();
            SqlDataReader sdr = com.ExecuteReader();
            AutoCompleteStringCollection autotext = new AutoCompleteStringCollection();
            while (sdr.Read())
            {
                autotext.Add(sdr.GetString(1));               
            }
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox1.AutoCompleteCustomSource = autotext;
            con.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string time;
            if (int.Parse(dateTimePicker2.Value.Minute.ToString()) < 10)
            {
                time = dateTimePicker2.Value.Hour.ToString() + ":0" + dateTimePicker2.Value.Minute.ToString();
            }
            else {
                time = dateTimePicker2.Value.Hour.ToString() + ":" + dateTimePicker2.Value.Minute.ToString();
            }
           
            string date = dateTimePicker1.Value.Day.ToString() + "/" + dateTimePicker1.Value.Month.ToString() + "/" + dateTimePicker1.Value.Year.ToString();
            

            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            string sql = "INSERT INTO Meetings(Id,Date,time,Customer) VALUES(@Id,@date,@time,@Customer)";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@Customer", textBox1.Text);
            com.Parameters.AddWithValue("@date", date);
            com.Parameters.AddWithValue("@time", time);
            com.Parameters.AddWithValue("@Id", count() + 1);
            com.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Meeting Saved");
            this.Hide();
            meetings f2 = new meetings();
            f2.Closed += (s, args) => this.Close();
            f2.Show();
        }
        private int count() {
            int k = 0;
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            string sql = "SELECT * FROM Meetings";
            SqlCommand com = new SqlCommand(sql, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read()) {
                k++;
            }
            return k;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            meetings f2 = new meetings();
            f2.Closed += (s, args) => this.Close();
            f2.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void backToMeetingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            meetings f2 = new meetings();
            f2.Closed += (s, args) => this.Close();
            f2.Show();
        }

        private void backToMainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu f2 = new MainMenu();
            f2.Closed += (s, args) => this.Close();
            f2.Show();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f2 = new Form1();
            f2.Closed += (s, args) => this.Close();
            f2.Show();
        }
    }
}
