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
using System.Runtime.InteropServices;

namespace SchoolManagementSystem
{
    public partial class Teachers : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
         (
           int nLeftRect,     // x-coordinate of upper-left corner
           int nTopRect,      // y-coordinate of upper-left corner
           int nRightRect,    // x-coordinate of lower-right corner
           int nBottomRect,   // y-coordinate of lower-right corner
           int nWidthEllipse, // width of ellipse
           int nHeightEllipse // height of ellipse
         );
        public Teachers()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            DisplayTeachers();
        }
        int StId = 0;
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\abduh\Documents\SchoolDb.mdf;Integrated Security=True;Connect Timeout=30");

        private void DisplayTeachers()
        {
            Con.Open();
            string Query = "Select * from TeacherTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TeachersDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Reset()
        {
            TnameTb.Text = "";
            TGenCb.SelectedIndex = -1;
            SubCb.SelectedIndex = -1;
            TPhoneTb.Text = "";
            TAddTb.Text = "";
            StId = 0;

        }

        private void Teachers_Load(object sender, EventArgs e)
        {

        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            if (TnameTb.Text == "" || TPhoneTb.Text == "" || TAddTb.Text == "" || TGenCb.SelectedIndex == -1 || SubCb.SelectedIndex == -1)
            {
                MessageBox.Show("Ma'lumot yetishmayapti!");
            }

            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into TeacherTbl(TName, TGen,TPhone, TSub, TAdd, TDOB) values(@Tname, @TGen,@TPhone, @TSub, @TAdd, @TDOB)", Con);
                    cmd.Parameters.AddWithValue("@Tname", TnameTb.Text);
                    cmd.Parameters.AddWithValue("@TGen", TGenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TPhone", TPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@TSub", SubCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TAdd", TAddTb.Text);
                    cmd.Parameters.AddWithValue("@TDOB", TDOB.Value.Date);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("O'qituvchi qo'shildi!");
                    Con.Close();
                    DisplayTeachers();
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

        private void TeachersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TnameTb.Text = TeachersDGV.SelectedRows[0].Cells[1].Value.ToString();
            TGenCb.SelectedItem = TeachersDGV.SelectedRows[0].Cells[2].Value.ToString();
            TPhoneTb.Text = TeachersDGV.SelectedRows[0].Cells[3].Value.ToString();
            SubCb.SelectedItem = TeachersDGV.SelectedRows[0].Cells[4].Value.ToString();
            TAddTb.Text = TeachersDGV.SelectedRows[0].Cells[5].Value.ToString();
            TDOB.Text = TeachersDGV.SelectedRows[0].Cells[6].Value.ToString();

        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = Con.CreateCommand();
                int id = Convert.ToInt32(TeachersDGV.SelectedRows[0].Cells[0].Value);
                cmd.CommandText = "Delete from TeacherTbl where TId='" + id + "'";
                TeachersDGV.Rows.RemoveAt(TeachersDGV.SelectedRows[0].Index);
                cmd.ExecuteNonQuery();
                MessageBox.Show("O'qituvchi o'chirildi!");
                Con.Close();
                DisplayTeachers();
                Reset();
            }
            catch (Exception Ex)
            {

                MessageBox.Show(Ex.Message);
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (TnameTb.Text == "" || TPhoneTb.Text == "" || TAddTb.Text == "" || TGenCb.SelectedIndex == -1 || SubCb.SelectedIndex == -1)
            {
                MessageBox.Show("Ma'lumot yetishmayapti!");
            }

            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update TeacherTbl set TName =@Tname, TGen = @TGen, TPhone = @TPhone, TSub = @TSub, TAdd = @TAdd,TDOB = @TDOB where TId = @StId", Con);
                    cmd.Parameters.AddWithValue("@StId", StId);
                    cmd.Parameters.AddWithValue("@Tname", TnameTb.Text);
                    cmd.Parameters.AddWithValue("@TGen", TGenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TPhone", TPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@TSub", SubCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TAdd", TAddTb.Text);
                    cmd.Parameters.AddWithValue("@TDOB", TDOB.Value.Date);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Yangilandi!");
                    Con.Close();
                    DisplayTeachers();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void TeachersDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            StId = Convert.ToInt32(TeachersDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
            TnameTb.Text = TeachersDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
            TGenCb.SelectedItem = TeachersDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
            TPhoneTb.Text = TeachersDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
            SubCb.SelectedItem = TeachersDGV.Rows[e.RowIndex].Cells[4].Value.ToString();
            TAddTb.Text = TeachersDGV.Rows[e.RowIndex].Cells[5].Value.ToString();
            TDOB.Text = TeachersDGV.Rows[e.RowIndex].Cells[6].Value.ToString();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            Hide();
        }
    }
}
