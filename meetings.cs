using System;
using System.Collections;
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
    public partial class meetings : Form
    {
        public meetings()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f2 = new Form1();
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

        private void meetings_Load(object sender, EventArgs e)
        {

            deleteOld();
            if (checkEmpty())
            {
                Label label = new Label();
                label.Location = new System.Drawing.Point(20, 118);
                label.Name = "label1";
                label.Text = "Schedule is empty";
                label.Font = new Font("Lucida Console", 14, FontStyle.Regular);
                label.AutoSize = true;
                this.Controls.Add(label);
            }
            else {
                string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                SqlConnection con = new SqlConnection(connectionstring);
                string sql = "SELECT * FROM Meetings";
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                SqlDataReader sdr = com.ExecuteReader();
                int k = 0;
                int l = 118;
                while (sdr.Read())
                {
                    k++;
                    Label label = new Label();
                    label.Location = new System.Drawing.Point(20, l);
                    label.Name = "label" + k.ToString();
                    label.Text = "With " + sdr.GetValue(3).ToString() + " at " + sdr.GetValue(1).ToString() + " " + sdr.GetValue(2).ToString();
                    label.Font = new Font("Lucida Console", 14, FontStyle.Regular);
                    label.AutoSize = true;
                    this.Controls.Add(label);

                    Label label2 = new Label();
                    label2.Location = new System.Drawing.Point(-3, l + 20);
                    label2.Name = "label" + k + 1.ToString();
                    label2.Text = line.Text;
                    label2.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
                    label2.AutoSize = true;
                    this.Controls.Add(label2);
                    l += 50;
                }
                con.Close();
            }
            
        }

        private void deleteMeeting(string customer,string date) {
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            string sql = "DELETE FROM Meetings WHERE Customer LIKE @name AND Date LIKE @date";
            con.Open();
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@name", customer);
            com.Parameters.AddWithValue("@date", date);
            com.ExecuteNonQuery();
            con.Close();
        }

        private void deleteOld() {
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            string sql = "SELECT * FROM Meetings";
            SqlCommand com = new SqlCommand(sql, con);
            con.Open();
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read()) {
                DateTime date = DateTime.Parse(sdr.GetValue(1).ToString());
                if (date.Year < DateTime.Today.Year)
                {
                    int id = int.Parse(sdr.GetValue(0).ToString());
                    deleteMeeting(sdr.GetValue(3).ToString(), sdr.GetValue(1).ToString());
                    fixId(id);
                }
                else if (date.Year == DateTime.Today.Year) {
                    if (date.Month < DateTime.Today.Month)
                    {
                        int id = int.Parse(sdr.GetValue(0).ToString());
                        deleteMeeting(sdr.GetValue(3).ToString(), sdr.GetValue(1).ToString());
                        fixId(id);
                    }
                    else if (date.Month == DateTime.Today.Month) {
                        if (date.Day < DateTime.Today.Day) {
                            int id = int.Parse(sdr.GetValue(0).ToString());
                            deleteMeeting(sdr.GetValue(3).ToString(), sdr.GetValue(1).ToString());
                            fixId(id);
                        }
                    }
                }
            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            NewMeeting f2 = new NewMeeting();
            f2.Closed += (s, args) => this.Close();
            f2.Show();
        }

        private void fixId(int id) {
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            string sql = "UPDATE Meetings SET Id = Id - 1 WHERE Id > @Id";
            con.Open();
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@Id", id);
            com.ExecuteNonQuery();
            con.Close();
        }

        private Boolean checkEmpty() {
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            string sql = "SELECT * FROM Meetings";
            con.Open();
            SqlCommand com = new SqlCommand(sql, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read()) {
                con.Close();
                return false;
            }
            con.Close();
            return true;
        }
    }
}
