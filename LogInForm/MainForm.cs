using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SD = System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using LogInForm.Models;

namespace LogInForm
{
    public partial class MainForm : Form
    {
        User CurrentUser = LogInForm.getCurrentUser();
        Point lastPoint;
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void closeBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        public void setDGV()
        {
            if (groupBox.Text != null && predmetBox.Text != null)
            {
                DB db = new DB();
                db.openConnection();
                MySqlCommand command = new MySqlCommand("SELECT users.id,fio,count_lab,ocenka,exam_ocenka FROM queue.users " +
                    "left join queue.uspevaemost on uspevaemost.users_id=users.id " +
                    "left join queue.predmets on uspevaemost.predmets_id = predmets.id " +
                    "where users.group_name = @GN AND predmets.name = @PN;", db.getConnection());
                command.Parameters.Add("@GN", MySqlDbType.VarChar).Value = groupBox.Text;
                command.Parameters.Add("@PN", MySqlDbType.VarChar).Value = predmetBox.Text;
                MySqlDataReader reader = command.ExecuteReader();
                dgv.Rows.Clear();
                while (reader.Read())
                {
                    string[] temp = new string[5];
                    for (int i = 0; i < 5; i++)
                        temp[i] = reader[i].ToString();
                    dgv.Rows.Add(temp);
                }
                reader.Close();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label1.Text = $"Здравствуй, {CurrentUser.Fio1}\n" +
                          $"Ваш логин --> {CurrentUser.Username1}\n" +
                          $"Ваш пароль --> {CurrentUser.Password1}\n" +
                          $"Ваша группа --> {CurrentUser.Group_name1}\n" +
                          $"Ваша роль --> {CurrentUser.Role1}\n";
            DB db = new DB();
            db.openConnection();
            if (CurrentUser.Role1 == "admin" || CurrentUser.Role1 == "teacher")
            {
                MySqlCommand command = new MySqlCommand("select group_name from queue.users where group_name is NOT null group by group_name", db.getConnection());
                MySqlDataReader reader = command.ExecuteReader();
                groupBox.Items.Clear();
                while (reader.Read())
                    groupBox.Items.Add(reader[0]);
                reader.Close();
                command = new MySqlCommand("select name from queue.predmets group by name", db.getConnection());
                reader = command.ExecuteReader();
                predmetBox.Items.Clear();
                while (reader.Read())
                    predmetBox.Items.Add(reader[0]);
                reader.Close();
            }
            else
            {
                groupBox.Items.Clear();
                groupBox.DropDownStyle = ComboBoxStyle.DropDown;
                groupBox.Text = CurrentUser.Group_name1;
                groupBox.Enabled = false;
                MySqlCommand command = new MySqlCommand("select name from queue.predmets" +
                    " left join queue.users_predmets on predmets.id = users_predmets.predmets_id" +
                    " where users_predmets.users_id = @CU group by name", db.getConnection());
                command.Parameters.Add("@CU", MySqlDbType.VarChar).Value = CurrentUser.Id1;
                MySqlDataReader reader = command.ExecuteReader();
                predmetBox.Items.Clear();
                while (reader.Read())
                    predmetBox.Items.Add(reader[0]);
                reader.Close();

            }

            db.closeConnection();
           
        }
        private void SetBorderAndGridlineStyles()
        {
            /*dgv.GridColor = Color.BlueViolet;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            


        }

        private void button2_Click_1(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            setDGV();
        }

        private void dgv_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                DB db = new DB();
                db.openConnection();
                MySqlCommand command = new MySqlCommand("select id from queue.predmets where name = @CP",db.getConnection());
                command.Parameters.Add("@CP", MySqlDbType.VarChar).Value = predmetBox.Text;
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int CurGroupId = int.Parse(reader[0].ToString()); 
                reader.Close();
                command = new MySqlCommand("update queue.uspevaemost set count_lab = @CL, ocenka = @O, exam_ocenka = @EO where users_id = @CU and predmets_id = @CP", db.getConnection());
                command.Parameters.Add("@CL", MySqlDbType.Int32).Value = int.Parse(dgv[2,e.RowIndex].Value.ToString());
                command.Parameters.Add("@O", MySqlDbType.Int32).Value = int.Parse(dgv[3, e.RowIndex].Value.ToString());
                command.Parameters.Add("@EO", MySqlDbType.Int32).Value = int.Parse(dgv[4, e.RowIndex].Value.ToString());
                command.Parameters.Add("@CU", MySqlDbType.Int32).Value = int.Parse(dgv[0, e.RowIndex].Value.ToString());
                command.Parameters.Add("@CP", MySqlDbType.Int32).Value = CurGroupId;
                command.ExecuteNonQuery();
                db.closeConnection();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }
    }
}
