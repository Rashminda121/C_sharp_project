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
using System.Xml.Linq;

namespace c__project1
{
    public partial class UserAdd : Sampleadd
    {
        public UserAdd()
        {
            InitializeComponent();
        }

        private void btnclose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        public int id = 0;


        public override void btnsave_Click(object sender, EventArgs e)
        {
            
            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Rashminda\Documents\C# databases\HardwareStore.mdf"";Integrated Security=True;Connect Timeout=30");
            SqlCommand cmd = new SqlCommand();

            conn.Open();
            if (id == 0)
            {
                cmd = new SqlCommand("INSERT INTO users(uid,username,upassword,uphone,uName)VALUES(@uid,@username,@upassword,@uphone,@uName)", conn);
            }
            else
            {
                cmd = new SqlCommand("update users set username = @username,upassword=@upassword, uphone=@uphone,uName=@uName  where uid=@id)", conn);
            }
            int newid = ++id;
            cmd.Parameters.AddWithValue("@uid", newid++);
            cmd.Parameters.AddWithValue("@username", txtCat.Text); 
            cmd.Parameters.AddWithValue("@upassword", txtPass.Text);
            cmd.Parameters.AddWithValue("@uphone", txtPhone.Text);
            cmd.Parameters.AddWithValue("@uName", txtName.Text);

            

            cmd.ExecuteNonQuery();

            conn.Close();
            MessageBox.Show("Data Saved successfully..", "Data Save", MessageBoxButtons.OK, MessageBoxIcon.Information);


            id = 0;
            txtName.Text = "";
            txtPass.Text= "";
            txtPhone.Text = "";
            txtCat.SelectedIndex = -1;
            txtName.Focus();
            

        }
    }
}
