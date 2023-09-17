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
    public partial class Orderadd : Sampleadd
    {
        public Orderadd()
        {
            InitializeComponent();
        }

        public int id = 0;


        public override void btnsave_Click(object sender, EventArgs e)
        {
            string qry = "";

            if (id == 0)//insert
            {
                qry = "insert into orders values (@Name)";
            }
            else//update
            {
                qry = " update orders set oname = @Name where oid=@id";

            }
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);

            if (Mainclass.SQL(qry, ht) > 0)
            {
                MessageBox.Show("Data Saved successfully..","Data Save", MessageBoxButtons.OK,MessageBoxIcon.Information);
                id = 0;
                txtName.Text = "";
                txtName.Focus();
            }

        }

        private void btnsave_Click_1(object sender, EventArgs e)
        {
            txtName.Text = "";
        }
    }
}
