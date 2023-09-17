using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace c__project1
{
    public partial class Staffview : Sampleview
    {

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Rashminda\Documents\C# databases\HardwareStore.mdf"";Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();


        public Staffview()
        {
            InitializeComponent();
        }

        private void Staffview_Load(object sender, EventArgs e)
        {
            GETDATA();
        }


        public void GETDATA()
        {
            string qry = " select * from staff where sname like '%" + txtsearch.Text + "%'";
            ListBox lb = new ListBox();
            lb.Items.Add(sid);
            lb.Items.Add(sname);
            lb.Items.Add(sphone);
            lb.Items.Add(snic);
            lb.Items.Add(srole);    

            Mainclass.LoadData(qry, datagrid1, lb);
        }

        public override void btnadd_Click(object sender, EventArgs e)
        {
            //Orderadd frm = new Orderadd();
            //frm.ShowDialog();

            Mainclass.blurbackground(new Staffadd());


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
            if (colname == "sedit")
            {
                Staffupdate staffupdate = new Staffupdate();
                staffupdate.txtid.Text = datagrid1.Rows[e.RowIndex].Cells[2].Value.ToString();
                staffupdate.txtname.Text = datagrid1.Rows[e.RowIndex].Cells[3].Value.ToString();
                staffupdate.txtphone.Text = datagrid1.Rows[e.RowIndex].Cells[4].Value.ToString();
                staffupdate.txtnic.Text = datagrid1.Rows[e.RowIndex].Cells[5].Value.ToString();
                staffupdate.cbrole.Text = datagrid1.Rows[e.RowIndex].Cells[6].Value.ToString();

                staffupdate.btnupdate.Enabled = true;
                staffupdate.txtid.Enabled = false;

                staffupdate.ShowDialog();
                //Mainclass.blurbackground(staffupdate);
            }
            else if (colname == "sdelete")
            {
                if (MessageBox.Show("Are you sure want to delete this record ?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    cmd = new SqlCommand("delete from staff where sname like '" + datagrid1.Rows[e.RowIndex].Cells[3].Value.ToString() + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been Successfully deleted !", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            Staffview_Load(null, EventArgs.Empty);
        }
    }
}
