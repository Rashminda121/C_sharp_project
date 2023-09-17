using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IdentityModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c__project1
{
    public partial class Productview : Sampleview
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Rashminda\Documents\C# databases\HardwareStore.mdf"";Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();

        public Productview()
        {
            InitializeComponent();
        }
        private void Productview_Load(object sender, EventArgs e)
        {
            GETDATA();
        }


        public void GETDATA()
        {
            string qry = " select pid,pname,pprice,categoryID,c.catname from products p inner join category c on c.catid =p.categoryID  where pname like '%" + txtsearch.Text + "%'";
            ListBox lb = new ListBox();
            lb.Items.Add(pid);
            lb.Items.Add(pname);
            lb.Items.Add(price);
            lb.Items.Add(categoryid);
            lb.Items.Add(category);

            Mainclass.LoadData(qry, datagrid1, lb);
        }

        public override void btnadd_Click(object sender, EventArgs e)
        {
            //Orderadd frm = new Orderadd();
            //frm.ShowDialog();

            Mainclass.blurbackground(new Productadd());


            GETDATA();
        }

        public override void txtsearch_TextChanged(object sender, EventArgs e)
        {
            GETDATA();
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //
        }

        private void datagrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = datagrid1.Columns[e.ColumnIndex].Name;
            if (colname == "pedit")
            {
                int productId = Convert.ToInt32(datagrid1.CurrentRow.Cells["pid"].Value);
                int categoryId = Convert.ToInt32(datagrid1.CurrentRow.Cells["categoryid"].Value);

                Productupdate frm = new Productupdate();
                frm.id = productId;
                frm.cid = categoryId;

                string qry = "SELECT pimage FROM products WHERE pid='" + productId + "'";
                conn.Open();
                cmd = new SqlCommand(qry, conn);
                byte[] imageData = (byte[])cmd.ExecuteScalar();
                conn.Close();

                frm.SetImageData(imageData);

                DialogResult result = frm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    GETDATA();
                }
            }
            else if (colname == "pdelete")
            {
                if (MessageBox.Show("Are you sure want to delete this record ?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    cmd = new SqlCommand("delete from products where pname like '" + datagrid1.Rows[e.RowIndex].Cells[3].Value.ToString() + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been Successfully deleted !", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            Productview_Load(null, EventArgs.Empty);
        }
       

    }
}
