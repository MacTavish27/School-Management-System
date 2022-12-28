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
    public partial class Events : Form
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
        public Events()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            DisplayEvents();
        }

        int EID = 0;
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\abduh\Documents\SchoolDb.mdf;Integrated Security=True;Connect Timeout=30");

        private void DisplayEvents()
        {
            Con.Open();
            string Query = "Select * from EventsTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            EventsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Reset()
        {
            EdurationTb.Text = "";
            EDescTb.Text = "";
            EID = 0;
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (EDescTb.Text == "" || EdurationTb.Text == "")
            {
                MessageBox.Show("Ma'lumot yetishmayapti!");
            }

            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into EventsTbl(EDesc, EDate,EDuration) values (@EvDesc, @EvDate, @EvDur )", Con);
                    cmd.Parameters.AddWithValue("@EvDesc", EDescTb.Text);
                    cmd.Parameters.AddWithValue("@EvDate", EDate.Value.Date);
                    cmd.Parameters.AddWithValue("@EvDur", EdurationTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Qo'shildi!");
                    Con.Close();
                    DisplayEvents();
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

        private void BackBtn_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            Hide();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = Con.CreateCommand();
                int id = Convert.ToInt32(EventsDGV.SelectedRows[0].Cells[0].Value);
                cmd.CommandText = "Delete from EventsTbl where EId='" + id + "'";
                EventsDGV.Rows.RemoveAt(EventsDGV.SelectedRows[0].Index);
                cmd.ExecuteNonQuery();
                MessageBox.Show("O'chirildi!");
                Con.Close();
                DisplayEvents();
                Reset();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

        }

        private void EventsDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EID = Convert.ToInt32(EventsDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
            EDescTb.Text = EventsDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
            EDate.Text = EventsDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
            EdurationTb.Text = EventsDGV.Rows[e.RowIndex].Cells[3].Value.ToString();         
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (EDescTb.Text == "" || EdurationTb.Text == "")
            {
                MessageBox.Show("Ma'lumot yetishmayapti!");
            }

            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update EventsTbl set EDesc = @EvDesc, EDate = @EvDate, Eduration = @EvDuration where EId = @EvID", Con);
                    cmd.Parameters.AddWithValue("@EvID", EID);
                    cmd.Parameters.AddWithValue("@EvDesc", EDescTb.Text);
                    cmd.Parameters.AddWithValue("@EvDate", EDate.Value.Date);
                    cmd.Parameters.AddWithValue("@EvDuration", EdurationTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Yangilandi!");
                    Con.Close();
                    DisplayEvents();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }
    }
}
