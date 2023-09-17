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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;

namespace c__project1
{
    public partial class Supplieradd : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-RN86JLR\SQLEXPRESS;Initial Catalog=hardwarestore;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Supplieradd()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtpass.Text != txtrepass){ Messagebox.Show("password did not match !","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);return; } 

                if (MessageBox.Show("Are you sure want to save data ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    

                    //string query= "INSERT INTO suppliers(id, fullname, phone, nic, address, email) VALUES('"txtid.Text ","+txtname.Text+ ","+txtphone.Text+","+txtnic.Text+","+ txtaddress.Text + ","+ txtemail.Text + "')";
                    //cmd= new SqlCommand(query,conn);

                     cmd =new SqlCommand("INSERT INTO suppliers(id,fullname,phone,nic,address,email)VALUES(@id,@fullname,@phone,@nic,@address,@email)", conn);

                     cmd.Parameters.AddWithValue("@id",txtid.Text);
                     cmd.Parameters.AddWithValue("@fullname", txtname.Text);
                     cmd.Parameters.AddWithValue("@phone",int.Parse( txtphone.Text));
                     cmd.Parameters.AddWithValue("@nic", txtnic.Text);
                     cmd.Parameters.AddWithValue("@address", txtaddress.Text);
                     cmd.Parameters.AddWithValue("@email", txtemail.Text);

                     cmd.ExecuteNonQuery();
                     
                     conn.Close();
                     MessageBox.Show("Data Successfully Saved.");
                     Clear();
                } 
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clear();
            btnsave.Enabled = true;
            btnupdate.Enabled = false;
        }
        public void Clear()
        {
            txtid.Clear();
            txtname.Clear();
            txtphone.Clear();
            txtnic.Clear();
            txtaddress.Clear();
            txtemail.Clear();
            txtsearch.Clear();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            conn.Open();

            string query2 = "select Id,fullname,phone,nic,address,email from suppliers where phone=@phone";
            SqlCommand cmd2 = new SqlCommand(query2, conn);

            if (txtsearch.Text != "")
            {
                cmd2.Parameters.AddWithValue("@phone", int.Parse(txtsearch.Text));

                SqlDataReader da = cmd2.ExecuteReader();

                while (da.Read())
                {
                    txtid.Text = da.GetValue(0).ToString();
                    txtname.Text = da.GetValue(1).ToString();
                    txtphone.Text = da.GetValue(2).ToString();
                    txtnic.Text = da.GetValue(3).ToString();
                    txtaddress.Text = da.GetValue(4).ToString();
                    txtemail.Text = da.GetValue(5).ToString();
                }
            }
            btnsave.Enabled = false;
            conn.Close();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure want to update data ?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();

                    cmd = new SqlCommand("update suppliers set id=@id,fullname=@fullname,nic=@nic,address=@address,email=@email where phone like '"+ int.Parse(txtphone.Text) +"'", conn);

                    cmd.Parameters.AddWithValue("@id", txtid.Text);
                    cmd.Parameters.AddWithValue("@fullname", txtname.Text);
                    cmd.Parameters.AddWithValue("@nic", txtnic.Text);
                    cmd.Parameters.AddWithValue("@address", txtaddress.Text);
                    cmd.Parameters.AddWithValue("@email", txtemail.Text);

                    cmd.ExecuteNonQuery();
                    
                    conn.Close();
                    MessageBox.Show("Data Successfully Saved.");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Supplieradd_Load(object sender, EventArgs e)
        {

        }
    }
}
