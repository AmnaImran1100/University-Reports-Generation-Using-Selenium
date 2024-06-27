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
    public partial class AssessmentComponent : Form
    {
        public static int Id = 0;

        public AssessmentComponent()
        {
            InitializeComponent();
        }

        private void AssessmentComponent_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadDataIntoComboBox1();
            LoadDataIntoComboBox2();
        }


        private void Name_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) && (!char.IsLetter(e.KeyChar)) && (!char.IsWhiteSpace(e.KeyChar)) && (!char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void TotalMarks_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) &&  (!char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }


        private void DateUpdated_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) && (!char.IsDigit(e.KeyChar)) && (!char.IsPunctuation(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void DateCreated_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) && (!char.IsDigit(e.KeyChar)) && (!char.IsPunctuation(e.KeyChar)))
            {
                e.Handled = true;
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LoadData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from AssessmentComponent", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void LoadDataIntoComboBox1()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from Rubric", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow r in dt.Rows)
            {
                RubricID.Items.Add(r["Id"].ToString());
            }
        }

        private void LoadDataIntoComboBox2()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from Assessment", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow r in dt.Rows)
            {
                AssessmentId.Items.Add(r["Id"].ToString());
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (Id != null && AssessmentComponentName.Text != null && RubricID.Text != null && TotalMarks.Text != null && DateCreated.Text != null && DateUpdated.Text != null && AssessmentId.Text != null)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("Select Name from AssessmentComponent where Name=@a", con);
                cmd1.Parameters.AddWithValue("a", AssessmentComponentName.Text);
                SqlDataReader reader1;

                reader1 = cmd1.ExecuteReader();
                if (reader1.Read())
                {
                    MessageBox.Show("AssessmentComponent with this Name already exists");
                    con.Close();
                }
                else
                {
                    con.Close();
                    var con1 = Configuration.getInstance().getConnection();
                    con1.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO AssessmentComponent (Name, RubricId, TotalMarks, DateCreated, DateUpdated, AssessmentID) VALUES (@Name, @RubricId, @TotalMarks, @DateCreated, @DateUpdated, @AssessmentID);", con1);
                    cmd.Parameters.AddWithValue("@Name", AssessmentComponentName.Text);
                    cmd.Parameters.AddWithValue("@RubricId", RubricID.Text);
                    cmd.Parameters.AddWithValue("@TotalMarks", TotalMarks.Text);
                    cmd.Parameters.AddWithValue("@DateCreated", SqlDbType.Date).Value = DateCreated.Value.Date;
                    cmd.Parameters.AddWithValue("@DateUpdated", SqlDbType.Date).Value = DateUpdated.Value.Date;
                    cmd.Parameters.AddWithValue("@AssessmentID", AssessmentId.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Added");
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Fill in all the Text Boxes");
            }
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            if (Id != null && AssessmentComponentName.Text != null && RubricID.Text != null && TotalMarks.Text != null && DateCreated.Text != null && DateUpdated.Text != null && AssessmentId.Text != null)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("UPDATE AssessmentComponent SET Name = @Name, RubricId = @RubricId, TotalMarks = @TotalMarks, DateUpdated = @DateUpdated, AssessmentID = @AssessmentID Where Id = @Id;", con);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@Name", AssessmentComponentName.Text);
                cmd.Parameters.AddWithValue("@RubricId", RubricID.Text);
                cmd.Parameters.AddWithValue("@TotalMarks", TotalMarks.Text);
                cmd.Parameters.AddWithValue("@DateUpdated", SqlDbType.Date).Value = DateUpdated.Value.Date;
                cmd.Parameters.AddWithValue("@AssessmentID", AssessmentId.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Updated");
                LoadData();
            }
            else
            {
                MessageBox.Show("Make sure that you have filled all text boxes!");
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {

        }

        private void Show_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from AssessmentComponent", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from AssessmentComponent WHERE Id LIKE '%" + Id + "%'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            AssessmentComponentName.Text = "";
            RubricID.Text = "";
            TotalMarks.Text = "";
            AssessmentId.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow r = dataGridView1.Rows[index];
            Id = int.Parse(r.Cells[0].Value.ToString());
            AssessmentComponentName.Text = r.Cells[1].Value + "";
            RubricID.Text = r.Cells[2].Value + "";
            TotalMarks.Text = r.Cells[3].Value + "";
            DateCreated.Text = r.Cells[4].Value + "";
            DateUpdated.Text = r.Cells[5].Value + "";
            AssessmentId.Text = r.Cells[6].Value + "";
        }
    }
}
