using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StudentRegistrationForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MaximizeBox = false;
            MinimizeBox = false;
            load();
        }

        private static readonly string ConnectionString = ("Data Source=.\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True;Encrypt=False");
        private readonly SqlConnection con = new SqlConnection(ConnectionString);
        static SqlCommand cmd;
        static string id;
        static bool mode = true;
        static string query;
        static SqlDataAdapter da;
        static SqlDataReader dr;

        public void load()
        {
            try
            {
                query = "SELECT * FROM STUD";
                cmd = new SqlCommand(query, con);
                con.Open();

                dr = cmd.ExecuteReader();
                da = new SqlDataAdapter(query, con);
                dataGridView1.Rows.Clear();

                while (dr.Read())
                {
                    dataGridView1.Rows.Add(dr[0], dr[1], dr[2], dr[3]);

                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getId(String id)
        {
            query = "SELECT * FROM STUD WHERE ID = '" + id + "'";

            cmd = new SqlCommand(query, con);
            con.Open();
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                txtName.Text = dr["NAME"].ToString();
                txtCourse.Text = dr["COURSE"].ToString();
                txtFees.Text = dr["FEES"].ToString();
            }
            con.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string course = txtCourse.Text;
            string fees = txtFees.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(course) || string.IsNullOrWhiteSpace(fees))
            {
                MessageBox.Show("Please fill all fields before saving the record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (mode == true)
                {
                    query = "INSERT INTO STUD (NAME,COURSE,FEES) VALUES (@SNAME,@COURSE,@FEES)";
                    con.Open();
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SNAME", name);
                    cmd.Parameters.AddWithValue("@COURSE", course);
                    cmd.Parameters.AddWithValue("@FEES", fees);
                    MessageBox.Show("Record added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmd.ExecuteNonQuery();

                    txtName.Clear();
                    txtCourse.Clear();
                    txtFees.Clear();
                    txtName.Focus();
                }
                else
                {
                    id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    query = "UPDATE STUD SET NAME = @NAME, COURSE = @COURSE, FEES = @FEES WHERE ID = @ID";
                    con.Open();
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SNAME", name);
                    cmd.Parameters.AddWithValue("@COURSE", course);
                    cmd.Parameters.AddWithValue("@FEES", fees);
                    cmd.Parameters.AddWithValue("@ID", id);
                    MessageBox.Show("Record successfully edited!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmd.ExecuteNonQuery();

                    txtName.Clear();
                    txtCourse.Clear();
                    txtFees.Clear();
                    txtName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter only numbers for the fees field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["EDIT"].Index && e.RowIndex > 0)
            {
                mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getId(id);
                load();
            }
            else if (e.ColumnIndex == dataGridView1.Columns["DELETE"].Index && e.RowIndex >= 0)
            {
                mode = true;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                query = "DELETE FROM STUD WHERE ID = @ID";
                con.Open();
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Are You sure want delete this recoed", "Comfimation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                MessageBox.Show("Record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                load();
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            load();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            txtCourse.Text = string.Empty;
            txtFees.Text = string.Empty;
            txtName.Focus();
        }
    }
}
