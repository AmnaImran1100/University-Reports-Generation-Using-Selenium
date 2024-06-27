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
    public partial class Rubric : Form
    {
        public static int Id;

        public Rubric()
        {
            InitializeComponent();
        }
        private void Rubric_Load(object sender, EventArgs e)
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

        private void LoadData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Rubric", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void LoadDataIntoComboBox()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from Clo", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow r in dt.Rows)
            {
                CloID.Items.Add(r["Id"].ToString());
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (Id != null && Details.Text != null && CloID.Text != null)
            {
                var con1 = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("Select Id from Rubric where Id=@a", con1);
                cmd1.Parameters.AddWithValue("a", int.Parse(RubricID.Text));
                SqlDataReader reader1;

                reader1 = cmd1.ExecuteReader();
                if (reader1.Read())
                {
                    MessageBox.Show("Rubric with this Id already exists");
                    con1.Close();
                }
                else
                {
                    con1.Close();
                    var con = Configuration.getInstance().getConnection();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Rubric (Id, Details, CloId) VALUES (@Id, @Details, @CloId);", con);
                    cmd.Parameters.AddWithValue("@Id", int.Parse(RubricID.Text));
                    cmd.Parameters.AddWithValue("@Details", Details.Text);
                    cmd.Parameters.AddWithValue("@CloId", int.Parse(CloID.Text));
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
            if (Id != null && Details.Text != null && CloID.Text != null)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("UPDATE Rubric SET Details = @Details, CloId = @CloId WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", int.Parse(RubricID.Text));
                cmd.Parameters.AddWithValue("@Details", Details.Text);
                cmd.Parameters.AddWithValue("@CloId", CloID.Text);
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
            SqlCommand cmd = new SqlCommand("Select * from Rubric", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Rubric WHERE Details LIKE '%" + Details.Text + "%'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            RubricID.Text = "";
            Details.Text = "";
            CloID.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow r = dataGridView1.Rows[index];
            Id = int.Parse(r.Cells[0].Value.ToString());
            Details.Text = r.Cells[1].Value + "";
            CloID.Text = r.Cells[2].Value + "";
        }
    }
}
