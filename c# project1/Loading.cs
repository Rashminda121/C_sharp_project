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
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            systemprogressbar.Increment(8);
            if (systemprogressbar.Value >= 100)
            {
                systemtimer.Enabled = false;

                this.Hide();
                Login log=new Login();
                log.Show();
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
           
            systemtimer.Enabled = true;
           
        }
        private void systemprogressbar_Click(object sender, EventArgs e)
        {
           
        }

       
    }
}
