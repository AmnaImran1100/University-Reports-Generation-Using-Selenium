using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace MidTermProject
{
    public partial class Students : Form
    {
        int Id;

        public Students()
        {
            InitializeComponent();
        }

        private void Students_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadDataIntoComboBox();
        }

        private void FirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) && (!char.IsLetter(e.KeyChar)) && (!char.IsWhiteSpace(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void LastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) && (!char.IsLetter(e.KeyChar)) && (!char.IsWhiteSpace(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void RegistrationNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar)) && (!char.IsLetter(e.KeyChar)) && (!char.IsDigit(e.KeyChar)) && (!char.IsPunctuation(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void Contact_KeyPress(object sender, KeyPressEventArgs e)
        {
            Regex r = new Regex(@"^[0-9]{10}$");
            if (r.IsMatch(Contact.Text))
            {
                Contact.BackColor = Color.Green;
            }
            else
            {
                Contact.BackColor = Color.Red;
            }
        }

        private void LoadData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Student ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void LoadDataIntoComboBox()
        {
            string Category = "STUDENT_STATUS";
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select LookupId from Lookup Where Category=@Category", con);
            cmd.Parameters.AddWithValue("@Category", Category);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                Status.Items.Add(dr["LookupId"].ToString());
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (Id != null && FirstName.Text != null && LastName.Text != null && Contact.Text != null && Email.Text != null && RegistrationNumber.Text != null && Status.Text != null)
            {
                if (Contact.BackColor == Color.Green)
                {
                    if (Regex.IsMatch(Email.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") == true)
                    {
                        var con = Configuration.getInstance().getConnection();
                        SqlCommand cmd1 = new SqlCommand("Select RegistrationNumber from Student where RegistrationNumber=@a ", con);
                        cmd1.Parameters.AddWithValue("a", RegistrationNumber.Text);
                        SqlDataReader reader1;

                        reader1 = cmd1.ExecuteReader();
                        if (reader1.Read())
                        {
                            MessageBox.Show("Student with this Registrartion Number already exists");
                            con.Close();
                        }
                        else
                        {
                            con.Close();
                            var con1 = Configuration.getInstance().getConnection();
                            con1.Open();
                            SqlCommand cmd = new SqlCommand("Insert into Student values ( @FirstName,@LastName,@Contact,@Email,@RegistrationNumber,@Status)", con1);
                            cmd.Parameters.AddWithValue("@FirstName", FirstName.Text);
                            cmd.Parameters.AddWithValue("@LastName", LastName.Text);
                            cmd.Parameters.AddWithValue("@Contact", Contact.Text);
                            cmd.Parameters.AddWithValue("@Email", Email.Text);
                            cmd.Parameters.AddWithValue("@RegistrationNumber", RegistrationNumber.Text);
                            cmd.Parameters.AddWithValue("@Status", int.Parse(Status.Text));
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Added");
                            LoadData();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Email Address");
                    }

                }
                else if (Contact.BackColor == Color.Red)
                {
                    MessageBox.Show("Invalid Phone Number");
                }
            }
            else
            {
                MessageBox.Show("Fill in all the Text Boxes");
            }
        }


        private void Edit_Click(object sender, EventArgs e)
        {
            if (Id != null && FirstName.Text != null && LastName.Text != null && Contact.Text != null && Email.Text != null && RegistrationNumber.Text != null && Status.Text != null)
            {
                if (Contact.BackColor == Color.Green)
                {
                    if (Regex.IsMatch(Email.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") == true)
                    {
                        var con = Configuration.getInstance().getConnection();
                        SqlCommand cmd = new SqlCommand("UPDATE Student SET FirstName=@FirstName,LastName=@LastName,Contact=@Contact,Email=@Email,RegistrationNumber = @RegistrationNumber,Status=@Status WHERE Id=@Id ", con);
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.Parameters.AddWithValue("@FirstName", FirstName.Text);
                        cmd.Parameters.AddWithValue("@LastName", LastName.Text);
                        cmd.Parameters.AddWithValue("@Contact", Contact.Text);
                        cmd.Parameters.AddWithValue("@Email", Email.Text);
                        cmd.Parameters.AddWithValue("@RegistrationNumber", RegistrationNumber.Text);
                        cmd.Parameters.AddWithValue("@Status", int.Parse(Status.Text));
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Successfully Updated");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("InValid Email Address");
                    }

                }
                else if (Contact.BackColor == Color.Red)
                {
                    MessageBox.Show("Invalid Phone Number");
                }
            }
            else
            {
                MessageBox.Show("Please Fill out all the Text Boxes");
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (Id != null)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("DELETE FROM Student WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Deleted");
                LoadData();
            }
            else
            {
                MessageBox.Show("Please Select the Record you want to Delete from Table");
            }
        }

        private void Show_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Student", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Search_Click_1(object sender, EventArgs e)
        {
            if (RegistrationNumber.Text != null)
            {
                DataTable dt = new DataTable();
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select * from Student WHERE RegistrationNumber LIKE '%" + RegistrationNumber.Text + "%'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Please Select the Record you want to Search from table");
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            DataGridViewRow r = dataGridView1.Rows[i];
            Id = int.Parse(r.Cells[0].Value.ToString());
            FirstName.Text = r.Cells[1].Value + "";
            LastName.Text = r.Cells[2].Value + "";
            Contact.Text = r.Cells[3].Value + "";
            Email.Text = r.Cells[4].Value + "";
            RegistrationNumber.Text = r.Cells[5].Value + "";
            Status.Text = r.Cells[6].Value + "";
        }


        private void Contact_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Clear_Click(object sender, EventArgs e)
        {
            FirstName.Text = "";
            LastName.Text = "";
            Contact.Text = "";
            Email.Text = "";
            RegistrationNumber.Text = "";
            Status.Text = "";
        }
    }
}
