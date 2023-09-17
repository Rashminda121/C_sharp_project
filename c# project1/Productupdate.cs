using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace c__project1
{
    public partial class Productupdate : Form
    {

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Rashminda\Documents\C# databases\HardwareStore.mdf"";Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();
        public Productupdate()
        {
            InitializeComponent();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            string qry = "";
            try
            {
                // ...
                if (id != 0) // update
                {
                    qry = "UPDATE products SET pname=@name, pprice=@price, categoryID=@cat, pimage=@image WHERE pid=@id";
                }
                // ...

                // Convert the image to byte array
                byte[] imageByteArray = null;
                if (txtimage.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        txtimage.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imageByteArray = ms.ToArray();
                    }
                }

                // ...

                if (MessageBox.Show("Are you sure you want to update the data?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    cmd = new SqlCommand(qry, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@price", txtprice.Text);
                    cmd.Parameters.AddWithValue("@cat", Convert.ToInt32(cbcat.SelectedValue));
                    cmd.Parameters.AddWithValue("@image", imageByteArray);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    // ...
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



            this.Close();
        }


        string filepath;
        Byte[] imageByteArray;


        private void btnbrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images ( .jpg,.png,.jpeg )|*.jpeg;*.png;*.jpg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filepath = ofd.FileName;
                txtimage.Image = new Bitmap(filepath);
                //txtimage.BackgroundImage = null;/// edit
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public int id = 0;
        public int cid = 0;
        private void Productupdate_Load(object sender, EventArgs e)
        {
            //cb fill
            string qry = "select catid 'id' , catname 'name' from category";
            Mainclass.CBFILL(qry, cbcat);

            if (cid > 0)
            {
                cbcat.SelectedValue = cid;
            }
            if (id > 0)
            {
                ForupdateLoadData();
            }
        }

        private void ForupdateLoadData()
        {
            string qry = @" select * from products  where pid=" + id + "";
            SqlCommand cmd = new SqlCommand(qry, Mainclass.conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                txtid.Text = dt.Rows[0]["pid"].ToString();
                txtName.Text = dt.Rows[0]["pname"].ToString();
                txtprice.Text = dt.Rows[0]["pprice"].ToString();

                Byte[] imageArray = (byte[])(dt.Rows[0]["pimage"]);
                byte[] imageByteArray = imageArray;
                txtimage.Image = Image.FromStream(new MemoryStream(imageArray));
            }

            txtid.Enabled=false;

        }

        public void SetImageData(byte[] imageData)
        {
            if (imageData != null)
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    txtimage.Image = Image.FromStream(ms);
                }
            }
        }

    }
}
