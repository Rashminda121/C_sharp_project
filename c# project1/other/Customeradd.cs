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

namespace c__project1
{
    public partial class Customeradd : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-RN86JLR\SQLEXPRESS;Initial Catalog=hardwarestore;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Customeradd()
        {
            InitializeComponent();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            { 

                if (MessageBox.Show("Are you sure want to save data ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();


                    //string query= "INSERT INTO suppliers(id, fullname, phone, nic, address, email) VALUES('"txtid.Text ","+txtname.Text+ ","+txtphone.Text+","+txtnic.Text+","+ txtaddress.Text + ","+ txtemail.Text + "')";
                    //cmd= new SqlCommand(query,conn);

                    cmd = new SqlCommand("INSERT INTO customers(name,phone,nic,address,email)VALUES(@name,@phone,@nic,@address,@email)", conn);

                    cmd.Parameters.AddWithValue("@name", txtname.Text);
                    cmd.Parameters.AddWithValue("@phone", int.Parse(txtphone.Text));
                    cmd.Parameters.AddWithValue("@nic", txtnic.Text);
                    cmd.Parameters.AddWithValue("@address", txtaddress.Text);
                    cmd.Parameters.AddWithValue("@email", txtemail.Text);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                    MessageBox.Show("Data Successfully Saved.");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Clear()
        {
            txtname.Clear();
            txtphone.Clear();
            txtnic.Clear();
            txtaddress.Clear();
            txtemail.Clear();
            txtsearch.Clear();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            Clear();
            btnsave.Enabled = true;
            btnupdate.Enabled = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            conn.Open();

            string query2 = "select name,phone,nic,address,email from customers where phone=@phone";
            SqlCommand cmd2 = new SqlCommand(query2, conn);

            if (txtsearch.Text != "")
            {
                cmd2.Parameters.AddWithValue("@phone", int.Parse(txtsearch.Text));

                SqlDataReader da = cmd2.ExecuteReader();

                while (da.Read())
                {
                    
                    txtname.Text = da.GetValue(0).ToString();
                    txtphone.Text = da.GetValue(1).ToString();
                    txtnic.Text = da.GetValue(2).ToString();
                    txtaddress.Text = da.GetValue(3).ToString();
                    txtemail.Text = da.GetValue(4).ToString();
                }
            }
            btnsave.Enabled = false;
            conn.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure want to update data ?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();

                    cmd = new SqlCommand("update customers set name=@name,phone=@phone,nic=@nic,address=@address,email=@email where id like '" + lblcid.Text + "'", conn);

                    cmd.Parameters.AddWithValue("@name", txtname.Text);
                    cmd.Parameters.AddWithValue("@phone", txtphone.Text);
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
    }
}
