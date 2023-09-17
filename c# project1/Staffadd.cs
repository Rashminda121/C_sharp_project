using System;
using System.Collections;
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
    public partial class Staffadd : Sampleadd
    {
        public Staffadd()
        {
            InitializeComponent();
        }

        public int id = 0;

        private void Staffadd_Load(object sender, EventArgs e)
        {
            //
        }

        public override void btnsave_Click(object sender, EventArgs e)
        {
            string qry = "";

            if (id == 0)//insert
            {
                qry = "insert into staff values (@name,@phone,@nic,@role)";
            }
            else//update
            {
                qry = " update staff set sname = @name, sphone=@phone, snic=@nic,srole=@role  where sid=@id";

            }
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@name", txtName.Text);
            ht.Add("@phone", txtphone.Text);
            ht.Add("@nic", txtnic.Text);
            ht.Add("@role", cbrole.Text);

            if (Mainclass.SQL(qry, ht) > 0)
            {
                MessageBox.Show("Data Saved successfully..", "Data Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                id = 0;
                txtName.Text = "";
                txtphone.Text = "";
                txtnic.Text = "";
                cbrole.SelectedIndex = -1;
                txtName.Focus();
            }

        }

        private void btnsave_Click_1(object sender, EventArgs e)
        {
           //
        }
    }
}
