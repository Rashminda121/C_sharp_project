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

namespace c__project1
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (Mainclass.IsValidUser(txtusername.Text, txtpassword.Text) == false)
            {
                MessageBox.Show("Invalid username or password", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                this.Hide();
                Main mainfrm = new Main();
                mainfrm.Show();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            info.Visible = true;
            lblinfo.Visible = true;
            pictureBox3.Visible = true;
        }



        private void btnclear_Click(object sender, EventArgs e)
        {
            txtusername.Text = "";
            txtpassword.Text = "";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            info.Visible = false;
            lblinfo.Visible = false;
            pictureBox3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //UserAdd frm = new UserAdd();
            //frm.Show();
            blurbackground(new UserAdd());
        }
        public static void blurbackground(Form Model)
        {
            Form background = new Form();
            using (Model)
            {
                background.StartPosition = FormStartPosition.CenterScreen;
                background.FormBorderStyle = FormBorderStyle.None;
                background.Opacity = 0.5d;
                background.BackColor = System.Drawing.Color.Black;
                background.Size = Main.Instance.Size;
                background.Location = Main.Instance.Location;
                background.ShowInTaskbar = false;
                background.Show();
                Model.Owner = background;
                Model.ShowDialog(background);
                background.Dispose();
            }
        }

    }
}
