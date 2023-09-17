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
    public partial class Inventory : Form
    {
        public Inventory()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit(); 
            }
        }

        private Form activeForm=null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
            
                activeForm.Close();
                activeForm = childForm;
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                panelmain.Controls.Add(childForm); 
                panelmain.Tag = childForm;
                childForm.BringToFront();
                childForm.Show();

        }

        private void btnsupplier_Click(object sender, EventArgs e)
        {
            openChildForm(new Supplier());
        }

        private void btncustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new Customer());
        }
    }
}
