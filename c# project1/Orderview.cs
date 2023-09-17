using c__project1.other;
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
    public partial class Orderview : Sampleview
    {

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Rashminda\Documents\C# databases\HardwareStore.mdf"";Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();


        public Orderview()
        {
            InitializeComponent();
        }

        private void Orderview_Load(object sender, EventArgs e)
        {
            GETDATA();
        }


        public void GETDATA()
        {
            string qry = " select * from orders where oname like '%" + txtsearch.Text + "%'";
            ListBox lb = new ListBox();
            lb.Items.Add(oid);
            lb.Items.Add(oname);

            Mainclass.LoadData(qry, datagrid1, lb);
        }


        public override void btnadd_Click(object sender, EventArgs e)
        {
            //Orderadd frm = new Orderadd();
            //frm.ShowDialog();

            Mainclass.blurbackground(new Orderadd());


            GETDATA();
        }

        public override void txtsearch_TextChanged(object sender, EventArgs e)
        {
            GETDATA();
        }

        private void btnadd_Click_1(object sender, EventArgs e)
        {
            //
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Validate row and column indices
            {
                DataGridViewColumn clickedColumn = datagrid1.Columns[e.ColumnIndex];

                if (clickedColumn != null)
                {
                    string colname = clickedColumn.Name;

                    if (colname == "oedit")
                    {
                        if (e.RowIndex < datagrid1.Rows.Count)
                        {
                            Orderupdate odrupdate = new Orderupdate();
                            odrupdate.txtid.Text = datagrid1.Rows[e.RowIndex].Cells[2].Value.ToString();
                            odrupdate.txtname.Text = datagrid1.Rows[e.RowIndex].Cells[3].Value.ToString();

                            odrupdate.btnupdate.Enabled = true;
                            odrupdate.txtid.Enabled = false; ///edit
                            //odrupdate.ShowDialog();
                            Mainclass.blurbackground(odrupdate);
                        }
                    }
                    else if (colname == "odelete")
                    {
                        if (e.RowIndex < datagrid1.Rows.Count)
                        {
                            if (MessageBox.Show("Are you sure want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                conn.Open();
                                cmd = new SqlCommand("delete from orders where oname like '" + datagrid1.Rows[e.RowIndex].Cells[3].Value.ToString() + "'", conn);
                                cmd.ExecuteNonQuery();
                                conn.Close();
                                MessageBox.Show("Record has been successfully deleted!", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }

            Orderview_Load(null, EventArgs.Empty);
        }
    }
}
