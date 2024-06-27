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
    public partial class StudentResult : Form
    {
        public List<string> list = new List<string>();

        public StudentResult()
        {
            InitializeComponent();
        }

        private void StudentResult_Load(object sender, EventArgs e)
        {
            LoadDataIntoStudentIDComboBox();
            LoadDataIntoAssessmnetIDComboBox();
            LoadDataIntoRubricMeasurementIDComboBox();
        }

        private void LoadDataIntoStudentIDComboBox()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from Student Where Status='5'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow r in dt.Rows)
            {
                StudentID.Items.Add(r["Id"].ToString());
            }
        }

        private void LoadDataIntoAssessmnetIDComboBox()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from Assessment", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                AssessmentID.Items.Add(row["Id"].ToString());
                list.Add(row["Id"].ToString());

            }
        }

        private void LoadDataIntoRubricMeasurementIDComboBox()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select MeasurementLevel from RubricLevel", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow r in dt.Rows)
            {
                RubricMeasurementLevel.Items.Add(r["MeasurementLevel"].ToString());
            }
        }

        private void AssessmentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (AssessmentID.Text == list[i].ToString())
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Select * from AssessmentComponent where AssessmentId = " + AssessmentID.Text + "", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    AssessmentComponentID.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        AssessmentComponentID.Items.Add(row["Id"].ToString());
                    }
                }
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("INSERT INTO StudentResult(StudentId, AssessmentComponentId, RubricMeasurementId, EvaluationDate) VALUES (@StudentId, @AssessmentComponentId, @RubricMeasurementId, @EvaluationDate)", con);
            cmd.Parameters.AddWithValue("@StudentId", int.Parse(StudentID.Text));
            cmd.Parameters.AddWithValue("@AssessmentComponentId", int.Parse(AssessmentComponentID.Text));
            cmd.Parameters.AddWithValue("@RubricMeasurementId", int.Parse(RubricMeasurementLevel.Text));
            cmd.Parameters.AddWithValue("@EvaluationDate", SqlDbType.Date).Value = EvaluationDate.Value.Date;

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully Added");
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UPDATE StudentResult SET StudentId = @StudentId, AssessmentComponentId = @AssessmentComponentId, RubricMeasurementId = @RubricMeasurementId, EvaluationDate = @EvaluationDate", con);
            cmd.Parameters.AddWithValue("@StudentId", int.Parse(StudentID.Text));
            cmd.Parameters.AddWithValue("@AssessmentComponentId", int.Parse(AssessmentComponentID.Text));
            cmd.Parameters.AddWithValue("@RubricMeasurementId", int.Parse(RubricMeasurementLevel.Text));
            cmd.Parameters.AddWithValue("@EvaluationDate", SqlDbType.Date).Value = EvaluationDate.Value.Date;

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully Updated");
        }

        private void Show_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from StudentResult", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow r = dataGridView1.Rows[index];
            StudentID.Text = r.Cells[0].Value + "";
            AssessmentComponentID.Text = r.Cells[1].Value + "";
            RubricMeasurementLevel.Text = r.Cells[2].Value + "";
            EvaluationDate.Text = r.Cells[3].Value + "";
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            StudentID.Text = "";
            AssessmentComponentID.Text = "";
            AssessmentID.Text = "";
            RubricMeasurementLevel.Text = "";
            EvaluationDate.Text = "";
        }
    }
}
