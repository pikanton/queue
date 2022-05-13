using LogInForm.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogInForm
{
    public partial class LogInForm : Form
    {
        
        static User CurrentUser = new User();
        static public User getCurrentUser()
        {
            return CurrentUser;
        }
        public LogInForm()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        Point lastPoint;
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        private void loginBtn_Click(object sender, EventArgs e)
        {
            string loginSELECT = "SELECT * FROM queue.users WHERE `username` = @lU AND `password` = @pU";
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand(loginSELECT, db.getConnection());
            command.Parameters.Add("@lU",MySqlDbType.VarChar).Value = loginEdit.Text;
            command.Parameters.Add("@pU",MySqlDbType.VarChar).Value = passEdit.Text;
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                CurrentUser.SetAll(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(),
                                  reader[3].ToString(), reader[4].ToString(), reader[5].ToString());
                MainForm mainForm = new MainForm();
                Hide();
                mainForm.Show();
            }
            else
            {
                LogInErr dialog = new LogInErr();
                dialog.ShowDialog();
            }
            reader.Close();
            db.closeConnection();
        }
    }
}
