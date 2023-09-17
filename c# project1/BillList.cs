using c__project1.other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace c__project1
{
    public partial class BillList : Sampleadd
    {

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Rashminda\Documents\C# databases\HardwareStore.mdf"";Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();
        public BillList()
        {
            InitializeComponent();
        }

        public int MainID = 0;

        private void BillList_Load(object sender, EventArgs e)
        {
            LoadData();

        }

        private void LoadData()
        {
            string qry = @" select MainID,TableName,WaiterName,orderType,status,total from tblMain
                            where  status <> 'Pending' ";



            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvtable);
            lb.Items.Add(dgvWaiter);
            lb.Items.Add(dgvType);
            lb.Items.Add(dgvStatus);
            lb.Items.Add(dgvTotal);

            Mainclass.LoadData(qry, datagrid1, lb);
        }

        private void datagrid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //serial no


            int count = 0;
            foreach (DataGridViewRow row in datagrid1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
                row.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datagrid1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                MainID = Convert.ToInt32(datagrid1.CurrentRow.Cells["dgvid"].Value);
                this.Close();
            }
            if (datagrid1.CurrentCell.OwningColumn.Name == "dgvdel")
            {

                printPreviewDialog1.Document = printDocument1;
                printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Product Detail", 300, 300);
                printPreviewDialog1.ShowDialog();

            }



            string colname = datagrid1.Columns[e.ColumnIndex].Name;
            if (colname == "dgvadd")
            {
                tot = datagrid1.CurrentRow.Cells["dgvTotal"].Value.ToString();
                oname = datagrid1.CurrentRow.Cells["dgvtable"].Value.ToString();
                staff = datagrid1.CurrentRow.Cells["dgvWaiter"].Value.ToString();
                otype = datagrid1.CurrentRow.Cells["dgvType"].Value.ToString();
                status = datagrid1.CurrentRow.Cells["dgvStatus"].Value.ToString();
            }


        }

        public string otype;
        public string oname;
        public string staff; 
        public string status;
        public string tot;


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void datagrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            datagrid1.ScrollBars = ScrollBars.Both;
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {


            //string qry = @"select orderType,total,received,change from tblMain ";

            //SqlCommand cmd2 = new SqlCommand(qry, Mainclass.conn);
            //DataTable dt2 = new DataTable();
            //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //da2.Fill(dt2);

            string ordertype = otype ;  //dt2.Rows[1]["orderType"].ToString();
            string onam = oname;
            string total = tot;    //dt2.Rows[2]["total"].ToString();
            string staf = staff ;   //dt2.Rows[3]["received"].ToString();
            string stat = status   ;   //dt2.Rows[4]["change"].ToString();



            e.Graphics.DrawString("Hardware Store", new Font("Century", 10), Brushes.IndianRed, new Point(12, 10));
            e.Graphics.DrawString(" Managment ", new Font("Century", 10), Brushes.IndianRed, new Point(10, 30));
            e.Graphics.DrawString("---------------------------------------------------------------------------------------------------------- ", new Font("Stencil", 12), Brushes.IndianRed, new Point(10, 60));
            e.Graphics.DrawString("Order Type : " + ordertype, new Font("Century", 9), Brushes.Black, new Point(10, 80));
            e.Graphics.DrawString("---------------------------------------------------------------------------------------------------------- ", new Font("Stencil", 9), Brushes.Gray, new Point(10, 100));
            e.Graphics.DrawString("Order Name : " + onam, new Font("Century", 9), Brushes.Black, new Point(10, 120));
            e.Graphics.DrawString("Staff Name : " + staf, new Font("Century", 9), Brushes.Black, new Point(10, 140));
            e.Graphics.DrawString("Status     : " +stat , new Font("Century", 9), Brushes.Black, new Point(10, 160));
            e.Graphics.DrawString("Total     : " + tot, new Font("Century", 9), Brushes.Black, new Point(10, 180));

            e.Graphics.DrawString("----------------------------------------------------------------------------------------------------------- ", new Font("Century", 9), Brushes.IndianRed, new Point(10, 200));
            e.Graphics.DrawString("Thank you... ", new Font("Century", 10), Brushes.IndianRed, new Point(10, 220));

            otype = "";
            oname = "";
            tot = "";
            staff = "";
            status = "";


        }

       
    }
}
