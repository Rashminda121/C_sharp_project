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
    public partial class OrderSelect : Form
    {
        public OrderSelect()
        {
            InitializeComponent();
        }


        public string TableName;


        private void TableSelect_Load(object sender, EventArgs e)
        {
            string qry = "select * from orders";
            SqlCommand cmd = new SqlCommand(qry, Mainclass.conn);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                Button button = new Button();
                button.Text = row["oname"].ToString();
                button.Width = 150;
                button.Height = 50;
                button.BackColor = Color.FromArgb(241, 85, 126);
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 55, 89);
                button.FlatAppearance.BorderSize = 0;
                button.FlatStyle = FlatStyle.Flat;

                //event for click
                button.Click += new EventHandler(_Click);

                flowLayoutPanel1.Controls.Add(button);
            }

        }

        private void _Click(object sender, EventArgs e)
        {

            TableName = (sender as Button).Text.ToString();
            this.Hide();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
