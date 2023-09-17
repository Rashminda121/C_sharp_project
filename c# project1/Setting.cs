using c__project1.other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c__project1
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

     

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //Usersview usersview = new Usersview();    
            //usersview.ShowDialog();

            blurbackground(new Usersview());
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            //About about = new About();
            //about.ShowDialog();

            blurbackground(new About());
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
