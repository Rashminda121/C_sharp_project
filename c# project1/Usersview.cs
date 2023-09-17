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

namespace c__project1.other
{
    public partial class Usersview : Sampleview
    {

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Rashminda\Documents\C# databases\HardwareStore.mdf"";Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();


        public Usersview()
        {
            InitializeComponent();
        }


        private void Usersview_Load(object sender, EventArgs e)
        {
            GETDATA();
        }

        public void GETDATA()
        {
            string qry = " select * from users where username like '%" + txtsearch.Text + "%'";
            ListBox lb = new ListBox();
            lb.Items.Add(uid);
            lb.Items.Add(username);
            lb.Items.Add(upass);
            lb.Items.Add(uphone);
            lb.Items.Add(uname);

            Mainclass.LoadData(qry, datagrid1, lb);
        }
        public override void btnadd_Click(object sender, EventArgs e)
        {
            Mainclass.blurbackground(new UserAdd());
            GETDATA();
        }

        
        public override void txtsearch_TextChanged(object sender, EventArgs e)
        {
            GETDATA();
        }

        

        private void datagrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                            UserUp userup = new UserUp();
                            userup.txtid.Text = datagrid1.Rows[e.RowIndex].Cells[2].Value.ToString();
                            userup.txtCat.Text = datagrid1.Rows[e.RowIndex].Cells[3].Value.ToString();
                            userup.txtPass.Text = datagrid1.Rows[e.RowIndex].Cells[4].Value.ToString();
                            userup.txtPhone.Text = datagrid1.Rows[e.RowIndex].Cells[5].Value.ToString();
                            userup.txtName.Text = datagrid1.Rows[e.RowIndex].Cells[6].Value.ToString();

                            userup.btnupdate.Enabled = true;
                            userup.txtid.Enabled = false; ///edit
                            //odrupdate.ShowDialog();
                            Mainclass.blurbackground(userup);
                        }
                    }
                    else if (colname == "odelete")
                    {
                        if (e.RowIndex < datagrid1.Rows.Count)
                        {
                            if (MessageBox.Show("Are you sure want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                conn.Open();
                                cmd = new SqlCommand("delete from users where username like '" + datagrid1.Rows[e.RowIndex].Cells[3].Value.ToString() + "'", conn);
                                cmd.ExecuteNonQuery();
                                conn.Close();
                                MessageBox.Show("Record has been successfully deleted!", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }

            Usersview_Load(null, EventArgs.Empty);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
