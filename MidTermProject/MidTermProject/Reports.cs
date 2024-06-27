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
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace MidTermProject
{
    public partial class Reports : Form
    {
        public Reports()
        {
            InitializeComponent();
        }

        private void Reports_Load(object sender, EventArgs e)
        {

        }

        private void StudentID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) && (!char.IsLetter(e.KeyChar)) && (!char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void Show_Click(object sender, EventArgs e)
        {
            if (ReportType.SelectedItem != null)
            {
                if (ReportType.SelectedItem.ToString() == "Attendance Report")
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("SELECT s.FirstName,s.LastName, s.Contact,s.Email,sa.AttendanceStatus,ca.AttendanceDate FROM StudentAttendance sa JOIN ClassAttendance ca ON sa.AttendanceId = ca.Id JOIN Student s ON sa.StudentId = s.Id ORDER BY sa.AttendanceId ASC; ", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                else if (ReportType.SelectedItem.ToString() == "Rubrics Report")
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Select c.Name,r.CloID,r.ID RubricID,r.Details RubricDetails,rl.ID RubricLevelID,rl.Details RubricLevelDetails from Clo c join Rubric r on c.Id=r.CloId join RubricLevel rl on rl.RubricId=r.Id;", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                else if (ReportType.SelectedItem.ToString() == "Student Assesment Report")
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("SELECT  s.FirstName, s.LastName, ac.Name AS ComponentName,  a.Title AS AssessmentTitle, a.TotalMarks AS AssessmentTotalMarks, a.TotalWeightage AS AssessmentTotalWeightage, sr.EvaluationDate, rl.Details AS RubricDetails, rl.MeasurementLevel AS RubricMeasurementLevel FROM  Student s JOIN StudentResult sr ON s.Id = sr.StudentId JOIN AssessmentComponent ac ON sr.AssessmentComponentId = ac.Id JOIN Assessment a ON ac.AssessmentId = a.Id JOIN Rubric r ON ac.RubricId = r.Id JOIN RubricLevel rl ON r.Id = rl.RubricId ORDER BY s.LastName ASC, s.FirstName ASC, a.Title ASC;", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                else if (ReportType.SelectedItem.ToString() == "Active Students Report")
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Select * From Student Where Status = '5'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                else if (ReportType.SelectedItem.ToString() == "Student Result Report")
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Select Assessment.Id as Assessment, AssessmentComponent.Id as Components, AssessmentComponent.TotalMarks, StudentResult.StudentId, StudentResult.RubricMeasurementId, RubricLevel.MeasurementLevel, StudentResult.EvaluationDate, (cast(2 as decimal(10,2))/4 *AssessmentComponent.TotalMarks) as ObtainedMarks From Assessment Join AssessmentComponent on Assessment.Id = AssessmentComponent.AssessmentId Join StudentResult on AssessmentComponent.Id = StudentResult.AssessmentComponentId Join RubricLevel on StudentResult.RubricMeasurementId = RubricLevel.Id;", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            else
            {
                MessageBox.Show("Please Select a Report Type");
            }
        }

        private void GeneratePDF_Click(object sender, EventArgs e)
        {
            ExportGridtoPdf(dataGridView1, "Report");
        }

        public static void ExportGridtoPdf(DataGridView dgw, string filename)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            PdfPTable pdftable = new PdfPTable(dgw.Columns.Count);
            pdftable.DefaultCell.Padding = 3;
            pdftable.WidthPercentage = 100;
            pdftable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.DefaultCell.BorderWidth = 1;
            iTextSharp.text.Font text = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

            foreach (DataGridViewColumn column in dgw.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, text));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                pdftable.AddCell(cell);
            }

            foreach (DataGridViewRow row in dgw.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    pdftable.AddCell(new Phrase(cell.Value.ToString(), text));
                }
            }

            var savefiledialogs = new SaveFileDialog();
            savefiledialogs.FileName = filename;
            savefiledialogs.DefaultExt = ".pdf";

            if (savefiledialogs.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefiledialogs.FileName, FileMode.Create))
                {
                    Document pdfdoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    PdfWriter.GetInstance(pdfdoc, stream);
                    pdfdoc.Open();
                    pdfdoc.Add(pdftable);
                    pdfdoc.Close();
                    stream.Close();
                }
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {

        }
    }
}
