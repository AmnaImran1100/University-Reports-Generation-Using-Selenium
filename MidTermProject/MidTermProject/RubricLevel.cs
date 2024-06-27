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
    public partial class RubricLevel : Form
    {
        public static int Id = 0;

        public RubricLevel()
        {
            InitializeComponent();
        }

        private void RubricLevel_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadDataIntoComboBox();
        }

        private void Details_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) && (!char.IsLetter(e.KeyChar)) && (!char.IsWhiteSpace(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void MeasurementLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) && (!char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void LoadData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from RubricLevel", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void LoadDataIntoComboBox()
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

        private void Add_Click(object sender, EventArgs e)
        {
            if (Id != null && RubricID.Text != null && Details.Text != null && MeasurementLevel.Text != null)
            {

                var con1 = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("Select Details from Rubric where Details=@a ", con1);
                cmd1.Parameters.AddWithValue("a", Details.Text);
                SqlDataReader reader1;
                reader1 = cmd1.ExecuteReader();
                if (reader1.Read())
                {
                    MessageBox.Show("Rubric Level with this detail already exists");
                    con1.Close();
                }
                else
                {
                    con1.Close();
                    var con = Configuration.getInstance().getConnection();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO RubricLevel (RubricId, Details, MeasurementLevel) VALUES (@RubricId, @Details, @MeasurementLevel);", con);
                    cmd.Parameters.AddWithValue("@RubricId", int.Parse(RubricID.Text));
                    cmd.Parameters.AddWithValue("@Details", Details.Text);
                    cmd.Parameters.AddWithValue("@MeasurementLevel", int.Parse(MeasurementLevel.Text));
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
            if (Id != null && RubricID.Text != null && Details.Text != null && MeasurementLevel.Text != null)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("UPDATE RubricLevel SET RubricId = @RubricId, Details = @Details, MeasurementLevel = @MeasurementLevel WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@RubricId", RubricID.Text);
                cmd.Parameters.AddWithValue("@Details", Details.Text);
                cmd.Parameters.AddWithValue("@MeasurementLevel", MeasurementLevel.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Updated");
                LoadData();
            }
            else
            {
                MessageBox.Show("Please fill all the Text Boxes");
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {

        }

        private void Show_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from RubricLevel", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from RubricLevel WHERE Id LIKE '%" + Id + "%'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            RubricID.Text = "";
            Details.Text = "";
            MeasurementLevel.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow r = dataGridView1.Rows[index];
            Id = int.Parse(r.Cells[0].Value.ToString());
            RubricID.Text = r.Cells[1].Value + "";
            Details.Text = r.Cells[2].Value + "";
            MeasurementLevel.Text = r.Cells[3].Value + "";
        }
    }
}
