using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidTermProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Students f = new Students();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MarkAttendence f = new MarkAttendence();
            f.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Clo f = new Clo();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Rubric f = new Rubric();
            f.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            RubricLevel f = new RubricLevel();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Assessment f = new Assessment();
            f.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AssessmentComponent f = new AssessmentComponent();
            f.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            StudentResult f = new StudentResult();
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reports f = new Reports();
            f.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
