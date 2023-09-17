using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c__project1
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }


        //accessing form main

        static Main _obj;
        
        public static Main Instance
        {
            get
            {
                if (_obj == null)
                {
                    _obj = new Main();
                }
                return _obj;
            }
        }





        //add controls to main form
        
        public void AddControls(Form f)
        {
            centerpanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            f.TopMost = true;
            centerpanel.Controls.Add(f);
            f.Show();
        }

       

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            lbluser.Text= Mainclass.USER.ToString(); ////////////////////
            _obj = this;
            AddControls(new formhome());
        }

        private void btnhome_Click(object sender, EventArgs e)
        {
            AddControls(new formhome());
        }

        private void btncatergory_Click(object sender, EventArgs e)
        {
            AddControls(new Catergeryview()); 
        }

        private void btnorders_Click(object sender, EventArgs e)
        {
            AddControls(new Orderview());
        }

        private void btnstaff_Click(object sender, EventArgs e)
        {
            AddControls(new Staffview());
        }

        private void btnproducts_Click(object sender, EventArgs e)
        {
            AddControls(new Productview());
        }

        private void btnpos_Click(object sender, EventArgs e)
        {
            Pos frm= new Pos();
            frm.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void btncheckout_Click(object sender, EventArgs e)
        {
            AddControls(new PendingView());
        }

        private void btnsettings_Click(object sender, EventArgs e)
        {
            AddControls(new Setting());
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
