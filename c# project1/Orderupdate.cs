﻿using System;
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
    public partial class Orderupdate : Form
    {
        public Orderupdate()
        {
            InitializeComponent();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Rashminda\Documents\C# databases\HardwareStore.mdf"";Integrated Security=True;Connect Timeout=30");
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (MessageBox.Show("Are you sure want to update data ?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    Catergeryview cw = new Catergeryview();

                    cmd = new SqlCommand("update orders set oname=@oname where oid like '" + int.Parse(txtid.Text) + "'", conn);

                    cmd.Parameters.AddWithValue("@oname", txtname.Text);


                    cmd.ExecuteNonQuery();

                    conn.Close();
                    MessageBox.Show("Data Successfully updated...","Data Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
