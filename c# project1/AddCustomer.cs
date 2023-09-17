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
    public partial class AddCustomer : Form
    {
        public AddCustomer()
        {
            InitializeComponent();
        }


        public string orderType = "";
        public int driverID = 0;
        public string cusName = "";
        
        public int mainID = 0;

        private void AddCustomer_Load(object sender, EventArgs e)
        {
             if(orderType == "Take On")
            {
                 lblDriver.Visible = false;
                 cbDriver.Visible = false;
            }
            string qry = "select sid 'id', sname 'name' from staff where srole = 'Driver'";
            Mainclass.CBFILL(qry, cbDriver);

            if (mainID > 0)
            {
                cbDriver.SelectedValue = driverID;
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            driverID = Convert.ToInt32(cbDriver.SelectedValue);
        }
    }
}
