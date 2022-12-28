using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolManagementSystem
{
    public partial class Attendances : Form
    {
        public Attendances()
        {
            InitializeComponent();
            DisplayAttendance();
            FillStudId();
        }
        int AID = 0;

        private void Attendances_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void FillStudId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select StId from StudentTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("StId", typeof(int));
            dt.Load(rdr);
            StIdCb.ValueMember = "StId";
            StIdCb.DataSource = dt;
            Con.Close();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\abduh\Documents\SchoolDb.mdf;Integrated Security=True;Connect Timeout=30");

        private void DisplayAttendance()
        {
            Con.Open();
            string Query = "Select * from AttendanceTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            AttendanceDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void GetStudName()
        {
            Con.Open();
            SqlCommand Cmd = new SqlCommand("select * from StudentTbl where StId = @SID", Con);
            Cmd.Parameters.AddWithValue("@SID", StIdCb.SelectedValue.ToString());
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(Cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                STNameTb.Text = dr["StName"].ToString();
            }
            Con.Close();
        }

        private void Reset()
        {
            AttStatusCb.SelectedIndex = -1;
            STNameTb.Text = "";
            StIdCb.SelectedIndex = -1;
            AID = 0;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (STNameTb.Text == "" || AttStatusCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing information");
            }

            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into AttendanceTbl(AttStId, AttStName, AttDate, AttStatus) values(@StId, @StName, @AttDate, @Status)", Con);
                    cmd.Parameters.AddWithValue("@StId", StIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@StName", STNameTb.Text);
                    cmd.Parameters.AddWithValue("@AttDate", AttDatePicker.Value.Date);
                    cmd.Parameters.AddWithValue("@Status", AttStatusCb.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Attendance Taken");
                    Con.Close();
                    DisplayAttendance();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void StIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetStudName();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
           
            try
            {
                Con.Open();
                SqlCommand cmd = Con.CreateCommand();
                int id = Convert.ToInt32(AttendanceDGV.SelectedRows[0].Cells[0].Value);
                cmd.CommandText = "Delete from AttendanceTbl where AttNum='" + id + "'";
                AttendanceDGV.Rows.RemoveAt(AttendanceDGV.SelectedRows[0].Index);
                cmd.ExecuteNonQuery();
                MessageBox.Show("O'quvchi o'chirildi!");
                Con.Close();
                DisplayAttendance();
                Reset();
            }
            catch (Exception Ex)
            {

                MessageBox.Show(Ex.Message);
            }
            Reset();
        }

        private void AttendanceDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //StIdCb.SelectedValue = AttendanceDGV.SelectedRows[0].Cells[1].Value.ToString();
            //STNameTb.Text = AttendanceDGV.SelectedRows[0].Cells[2].Value.ToString();
            //AttDatePicker.Text = AttendanceDGV.SelectedRows[0].Cells[3].Value.ToString();
            //AttStatusCb.SelectedItem = AttendanceDGV.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void AttendanceDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //MessageBox.Show(AttendanceDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
           
            AID = Convert.ToInt32(AttendanceDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
            StIdCb.SelectedValue = AttendanceDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
            STNameTb.Text = AttendanceDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
            AttDatePicker.Text = AttendanceDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
            AttStatusCb.SelectedItem = AttendanceDGV.Rows[e.RowIndex].Cells[4].Value.ToString();

        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (STNameTb.Text == "" || AttStatusCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing information");
            }

            else
            {
                try
                {
                    //MessageBox.Show(StId.ToString());
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update AttendanceTbl set AttStId =@StId, AttStName = @StName, AttDate = @ADate, AttStatus = @AStatus where AttNum = @ANum ", Con);
                    cmd.Parameters.AddWithValue("@StId", StIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@StName",STNameTb.Text);
                    cmd.Parameters.AddWithValue("@ADate", AttDatePicker.Value.Date);
                    cmd.Parameters.AddWithValue("@AStatus", AttStatusCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@ANum", AID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Attendance Updated");
                    Con.Close();
                    DisplayAttendance();
                    
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            Hide();
        }

        private void StIdCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
