using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MidTermProject
{
    public partial class MarkAttendence : Form
    {
        public MarkAttendence()
        {
            InitializeComponent();
        }

        private void MarkAttendence_Load(object sender, EventArgs e)
        {
            LoadDate();
            LoadDataIntoComboBox();
        }

        private void Date_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) &&  (!char.IsDigit(e.KeyChar)) && (!char.IsPunctuation(e.KeyChar)) && (!char.IsWhiteSpace(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void LoadDate()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id, RegistrationNumber, CONCAT(FirstName, ' ' ,LastName) as StudentName from Student Where Status = '5'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void LoadDataIntoComboBox()
        {
            string Category = "ATTENDANCE_STATUS";
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select LookupId from Lookup Where Category=@Category", con);
            cmd.Parameters.AddWithValue("@Category", Category);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                Status.Items.Add(dr["LookupId"].ToString());
            }
        }

        private void ConfirmAttendance_Click(object sender, EventArgs e)
        {
            if (Date != null)
            {
                var con = Configuration.getInstance().getConnection();
                DateTime date = Date.Value;
                SqlCommand cmd = new SqlCommand("Insert into ClassAttendance values (@date)", con);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.ExecuteNonQuery();

                int AttendanceId=0;
                var con2 = Configuration.getInstance().getConnection();
                SqlCommand cmd2 = new SqlCommand("Select Id from ClassAttendance Where YEAR(AttendanceDate) = @Year1 AND MONTH(AttendanceDate) = @Month AND Day(AttendanceDate) = @Day", con2);
                cmd2.Parameters.AddWithValue("@Year1", date.Year);
                cmd2.Parameters.AddWithValue("@Month", date.Month);
                cmd2.Parameters.AddWithValue("@Day", date.Day);
                SqlDataReader reader = cmd2.ExecuteReader();
                if(reader.Read())
                {
                    AttendanceId = reader.GetInt32(0);
                    reader.Close();
                }
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    AttendanceId = int.Parse(dr["Id"].ToString());
                //}
                //cmd2.ExecuteNonQuery();

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    //int AttendanceStatus = int.Parse(row.Cells[0].Value.ToString());
                    int StudentId = int.Parse(row.Cells[1].Value.ToString());
                    DataGridViewComboBoxCell cell0 = row.Cells[0] as DataGridViewComboBoxCell;
                    int AttendanceStatus = int.Parse(cell0.Value.ToString());
                    

                    var con3 = Configuration.getInstance().getConnection();
                    SqlCommand cmd3 = new SqlCommand("Insert into StudentAttendance values (@AttendanceId, @StudentId, @AttendanceStatus)", con3);
                    cmd3.Parameters.AddWithValue("@AttendanceId", AttendanceId);
                    cmd3.Parameters.AddWithValue("@StudentId", StudentId);
                    cmd3.Parameters.AddWithValue("@AttendanceStatus", AttendanceStatus);
                    cmd3.ExecuteNonQuery();
                }

                MessageBox.Show("Attendance Marked Successfully");

            }
            else
            {
                MessageBox.Show("Please fill in the Date Text Box");
            }
        }
    }
}
