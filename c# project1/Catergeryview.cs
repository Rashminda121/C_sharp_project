using c__project1.other;
using System;
using System.Collections;
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
    public partial class Catergeryview : Sampleview
    {

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Rashminda\Documents\C# databases\HardwareStore.mdf"";Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();
        public Catergeryview()
        {
            InitializeComponent();
        }

        public void GETDATA()
        {
            string qry = " select * from category where catname like '%" + txtsearch.Text + "%'";
            ListBox lb=new ListBox();
            lb.Items.Add(dgid);
            lb.Items.Add(dgname);

            Mainclass.LoadData(qry, datagrid1, lb);
        }

        private void Catergeryview_Load(object sender, EventArgs e)
        {
            GETDATA();
        }
        public override void btnadd_Click(object sender, EventArgs e)
        {
            //CatergeryAdd frm=new CatergeryAdd();
            //frm.ShowDialog();

            Mainclass.blurbackground(new CatergeryAdd());
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

                    if (colname == "dgedit")
                    {
                        if (e.RowIndex < datagrid1.Rows.Count)
                        {
                            Categoryupdate categoryupdate = new Categoryupdate();
                            categoryupdate.txtid.Text = datagrid1.Rows[e.RowIndex].Cells[2].Value.ToString();
                            categoryupdate.txtname.Text = datagrid1.Rows[e.RowIndex].Cells[3].Value.ToString();

                            categoryupdate.btnupdate.Enabled = true;
                            categoryupdate.txtid.Enabled = false; ///edit
                            //categoryupdate.ShowDialog();
                            Mainclass.blurbackground(categoryupdate);
                        }
                    }
                    else if (colname == "dgdelete")
                    {
                        if (e.RowIndex < datagrid1.Rows.Count)
                        {
                            if (MessageBox.Show("Are you sure want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                conn.Open();
                                cmd = new SqlCommand("delete from category where catname like '" + datagrid1.Rows[e.RowIndex].Cells[3].Value.ToString() + "'", conn);
                                cmd.ExecuteNonQuery();
                                conn.Close();
                                MessageBox.Show("Record has been successfully deleted!", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }

            Catergeryview_Load(null, EventArgs.Empty);
        }


        
    }
}
