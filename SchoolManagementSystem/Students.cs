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
using BrbVideoManager.Controls;
using System.Runtime.InteropServices;

namespace SchoolManagementSystem
{
    public partial class Students : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
    (
      int nLeftRect,     // x-coordinate of upper-left corner
      int nTopRect,      // y-coordinate of upper-left corner
      int nRightRect,    // x-coordinate of lower-right corner
      int nBottomRect,   // y-coordinate of lower-right corner
      int nWidthEllipse, // height of ellipse
      int nHeightEllipse // width of ellipse
    );
        public Students()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            DisplayStudent();
     
        }
        int StId = 0;
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\abduh\Documents\SchoolDb.mdf;Integrated Security=True;Connect Timeout=30");

        private void DisplayStudent()
        {
            Con.Open();
            string Query = "Select * from StudentTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            StudentsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if(StNameTb.Text == "" || FeesTb.Text == "" || AddressTb.Text == "" || StGenCb.SelectedIndex == -1 || ClassCb.SelectedIndex == -1)
            {
                MessageBox.Show("Ma'lumot yetishmayapti!");
            }

            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into StudentTbl(StName, StGen,StDOB, StClass, StFees, StAdd) values(@SName, @SGen,@SDob, @SClass, @SFees, @SAdd)", Con);
                    cmd.Parameters.AddWithValue("@SName", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@SGen", StGenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SDob", DOBPicker.Value.Date);
                    cmd.Parameters.AddWithValue("@SClass", ClassCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SFees", FeesTb.Text);
                    cmd.Parameters.AddWithValue("@SAdd", AddressTb.Text); 
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("O'quvchi qo'shildi!");
                    Con.Close();
                    DisplayStudent();
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

        private void Reset()
        {
            StNameTb.Text = "";
            StGenCb.SelectedIndex = -1;
            DOBPicker.Text = "";
            ClassCb.SelectedIndex = -1;
            FeesTb.Text = "";
            AddressTb.Text = "";
            StId = 0;
        }
       
       
        private void StudentsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //StNameTb.Text = StudentsDGV.SelectedRows[0].Cells[1].Value.ToString();
            //StGenCb.SelectedItem = StudentsDGV.SelectedRows[0].Cells[2].Value.ToString();
            //DOBPicker.Text = StudentsDGV.SelectedRows[0].Cells[3].Value.ToString();
            //ClassCb.SelectedItem = StudentsDGV.SelectedRows[0].Cells[4].Value.ToString();
            //FeesTb.Text = StudentsDGV.SelectedRows[0].Cells[5].Value.ToString();
            //AddressTb.Text = StudentsDGV.SelectedRows[0].Cells[6].Value.ToString();

        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
                try
                {
                    Con.Open();
                    SqlCommand cmd = Con.CreateCommand();
                    int id = Convert.ToInt32(StudentsDGV.SelectedRows[0].Cells[0].Value);
                    cmd.CommandText = "Delete from StudentTbl where StId='" + id + "'";
                    StudentsDGV.Rows.RemoveAt(StudentsDGV.SelectedRows[0].Index);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("O'quvchi o'chirildi!");
                    Con.Close();
                    DisplayStudent();
                    Reset();
                }
                catch (Exception Ex)
                {

                    MessageBox.Show(Ex.Message);
                }
                          
        }



        private void StudentsDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            StId = Convert.ToInt32(StudentsDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
            StNameTb.Text = StudentsDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
            StGenCb.SelectedItem = StudentsDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
            DOBPicker.Text = StudentsDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
            ClassCb.SelectedItem = StudentsDGV.Rows[e.RowIndex].Cells[4].Value.ToString();
            FeesTb.Text = StudentsDGV.Rows[e.RowIndex].Cells[5].Value.ToString();
            AddressTb.Text = StudentsDGV.Rows[e.RowIndex].Cells[6].Value.ToString();

        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || FeesTb.Text == "" || AddressTb.Text == "" || StGenCb.SelectedIndex == -1 || ClassCb.SelectedIndex == -1)
            {
                MessageBox.Show("Ma'lumot yetishmayapti!");
            }

            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update StudentTbl set StName = @SName, StGen = @SGen, StDOB = @SDob, StClass = @SClass, StFees = @SFees,StAdd = @SAdd where StId = @StId", Con);
                    cmd.Parameters.AddWithValue("@StId", StId);
                    cmd.Parameters.AddWithValue("@SName", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@SGen", StGenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SDob", DOBPicker.Value.Date);
                    cmd.Parameters.AddWithValue("@SClass", ClassCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SFees", FeesTb.Text);
                    cmd.Parameters.AddWithValue("@SAdd", AddressTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Yangilandi!");
                    Con.Close();
                    DisplayStudent();
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

    
    }
}
