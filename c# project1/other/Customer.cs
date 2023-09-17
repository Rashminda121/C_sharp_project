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
using System.Xml.Linq;

namespace c__project1
{
    public partial class Customer : Form
    {

        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-RN86JLR\SQLEXPRESS;Initial Catalog=hardwarestore;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Customer()
        {
            InitializeComponent();
            Loaddata();
        }

        public void Loaddata()
        {
            conn.Open();
            
            SqlDataReader dr;
            int i = 0;
            dgcustomer.Rows.Clear();

            cmd = new SqlCommand("SELECT * FROM customers", conn);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                i++;
                dgcustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            conn.Close();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            
            Customeradd customeradd = new Customeradd();

            customeradd.btnsave.Enabled = true;
            customeradd.btnupdate.Enabled = false;
            customeradd.ShowDialog();
            Loaddata();
        }

        private void dgcustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dgcustomer.Columns[e.ColumnIndex].Name;
            if (colname == "Edit")
            {
                Customeradd cadd = new Customeradd();
               // cadd.lblcid.Text = dgcustomer.Rows[e.RowIndex].Cells[0].Value.ToString();
                cadd.txtname.Text = dgcustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                cadd.txtphone.Text = dgcustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
                cadd.txtnic.Text = dgcustomer.Rows[e.RowIndex].Cells[4].Value.ToString();
                cadd.txtaddress.Text = dgcustomer.Rows[e.RowIndex].Cells[5].Value.ToString();
                cadd.txtemail.Text = dgcustomer.Rows[e.RowIndex].Cells[6].Value.ToString();

                cadd.btnsave.Enabled = false;
                cadd.btnupdate.Enabled = true;
                cadd.txtphone.Enabled = false;
                cadd.ShowDialog();
            }
            else if (colname == "Delete")
            {
                if (MessageBox.Show("Are you sure want to delete this record ?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    cmd = new SqlCommand("delete from customers where id like '" + dgcustomer.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been Successfully deleted !");
                }
            }
            Loaddata();
        }
    }
}
