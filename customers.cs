using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class customers
    {
        public static int idfinder(string name) {
            
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            try {
                string sql = "SELECT * FROM Customers";
                SqlCommand com = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader sdr = com.ExecuteReader();
                while (sdr.Read()) {
                    
                    if (sdr.GetValue(1).ToString() == name) {
                        int k = int.Parse(sdr.GetValue(0).ToString());
                        return k;
                    }
                }
                con.Close();
            }
            catch (Exception) {
                MessageBox.Show("Database Error idfinder");
                }
            return 0;
        }
        public static Boolean searchName(string name) {
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            
                string sql = "SELECT * FROM Customers";
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                SqlDataReader sdr = com.ExecuteReader();
                while (sdr.Read())
                {
                    if (sdr.GetValue(1).ToString() == name)
                    {
                        con.Close();
                        return true;
                    }
                }
            
            return false;
        }

        public static void fixID(int id) {
            string connectionstring = "Data Source=localhost;Initial Catalog=StorageDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionstring);
            string sql = "UPDATE Customers SET Id = Id - 1 WHERE Id > @Id";
            con.Open();
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@Id", id);
            com.ExecuteNonQuery();
            con.Close();
        }
    }
}
