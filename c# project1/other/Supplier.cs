using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c__project1
{
    public partial class Supplier : Form
    {

        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-RN86JLR\SQLEXPRESS;Initial Catalog=hardwarestore;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();

        public Supplier()
        {
            InitializeComponent();
            loaddata();
        }

        public void loaddata()
        {
            conn.Open();
            
            SqlDataReader dr;
            int i = 0;
            datagrid.Rows.Clear();
            
            cmd = new SqlCommand("SELECT * FROM suppliers", conn);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                i++;
                datagrid.Rows.Add(i, dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            conn.Close();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            Supplieradd supplieradd = new Supplieradd();
            supplieradd.btnsave.Enabled = true;
            supplieradd.btnupdate.Enabled = true;
            supplieradd.ShowDialog();
            loaddata();
        }

        private void datagrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = datagrid.Columns[e.ColumnIndex].Name;
            if(colname == "Edit")
            {
                Supplieradd supplieradd = new Supplieradd();
                supplieradd.txtid.Text = datagrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                supplieradd.txtname.Text = datagrid.Rows[e.RowIndex].Cells[1].Value.ToString();
                supplieradd.txtphone.Text = datagrid.Rows[e.RowIndex].Cells[2].Value.ToString();
                supplieradd.txtnic.Text = datagrid.Rows[e.RowIndex].Cells[3].Value.ToString();
                supplieradd.txtaddress.Text = datagrid.Rows[e.RowIndex].Cells[4].Value.ToString();
                supplieradd.txtemail.Text = datagrid.Rows[e.RowIndex].Cells[5].Value.ToString();

                supplieradd.btnsave.Enabled = false;
                supplieradd.btnupdate.Enabled = true;
                supplieradd.txtphone.Enabled = false;
                supplieradd.ShowDialog();
            }
            else if (colname == "Delete")
            {
                if (MessageBox.Show("Are you sure want to delete this record ?","Delete Record",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    conn.Open();
                    cmd = new SqlCommand("delete from suppliers where phone like '" + datagrid.Rows[e.RowIndex].Cells[2].Value.ToString() + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been Successfully deleted !");
                }
            }
            loaddata();
        }
    }
}
