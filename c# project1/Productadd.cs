using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c__project1
{
    public partial class Productadd : Sampleadd
    {
        public Productadd()
        {
            InitializeComponent();
        }

        public int id = 0;
        public int cid = 0;

        private void Productadd_Load(object sender, EventArgs e)
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

        string filepath;
        Byte[] imageByteArray;



        private void btnbrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images ( .jpg,.png,.jpeg )|* .jpeg;*.png;*.jpg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filepath = ofd.FileName;
                txtimage.Image = new Bitmap(filepath);
                //txtimage.BackgroundImage = null;/// edit
            }
        }

        public override void btnsave_Click(object sender, EventArgs e)
        {
            string qry = "";

            if (id == 0) // insert
            {
                qry = "INSERT INTO products VALUES (@name, @price, @cat, @image)";
            }
            else // update
            {
                qry = "UPDATE products SET pname = @name, pprice = @price, categoryID = @cat";
                if (!string.IsNullOrEmpty(filepath))
                {
                    qry += ", pimage = @image";
                }
                qry += " WHERE pid = @id";
            }

            // Check if a new image is selected
            if (!string.IsNullOrEmpty(filepath))
            {
                // Delete previous image from the database
                DeletePreviousImageFromDatabase();

                // Save the new image
                SaveNewImage(qry);
            }
            else
            {
                MessageBox.Show("Please select an image.", "Image Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // No new image selected, update other fields without changing the image
                // UpdateProductWithoutImage(qry);
            }


        }

        private void DeletePreviousImageFromDatabase()
        {
            string deleteImageQuery = "UPDATE products SET pimage = NULL WHERE pid = @id";
            Hashtable deleteImageParams = new Hashtable();
            deleteImageParams.Add("@id", id);
            Mainclass.SQL(deleteImageQuery, deleteImageParams);
        }

        private void SaveNewImage(string qry)
        {
            Image temp = new Bitmap(txtimage.Image);
            MemoryStream ms = new MemoryStream();
            temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            imageByteArray = ms.ToArray();

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@name", txtName.Text);
            ht.Add("@price", txtprice.Text);
            ht.Add("@cat", Convert.ToInt32(cbcat.SelectedValue));
            ht.Add("@image", imageByteArray);

            if (Mainclass.SQL(qry, ht) > 0)
            {
                MessageBox.Show("Data Saved successfully..", "Data Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                id = 0;
                cid = 0;
                txtName.Text = "";
                txtprice.Text = "";
                txtimage.Image = Properties.Resources.image;
                cbcat.SelectedIndex = 0;
                cbcat.SelectedIndex = -1;
                txtName.Focus();
            }
            else
            {
                // No image was selected, so insert data without the image field
                qry = "INSERT INTO products (pname, pprice, categoryID) VALUES (@name, @price, @cat)";

                ht.Remove("@image"); // Remove the image parameter from the hashtable

                if (Mainclass.SQL(qry, ht) > 0)
                {
                    MessageBox.Show("Data Saved successfully.", "Data Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    id = 0;
                    cid = 0;
                    txtName.Text = "";
                    txtprice.Text = "";
                    txtimage.Image = Properties.Resources.image;
                    cbcat.SelectedIndex = -1;
                    txtName.Focus();
                }
            }
        }

        private void UpdateProductWithoutImage(string qry)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@name", txtName.Text);
            ht.Add("@price", txtprice.Text);
            ht.Add("@cat", Convert.ToInt32(cbcat.SelectedValue));

            if (!qry.Contains("@image"))
            {
                qry += ", pimage = NULL"; // Set pimage to NULL if no new image is selected
            }

            if (Mainclass.SQL(qry, ht) > 0)
            {
                MessageBox.Show("Data Saved successfully.", "Data Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                id = 0;
                cid = 0;
                txtName.Text = "";
                txtprice.Text = "";
                txtimage.Image = Properties.Resources.image;
                cbcat.SelectedIndex = -1;
                txtName.Focus();
            }
        }

        private void ForupdateLoadData()
        {
            string qry = "SELECT * FROM products WHERE pid = " + id;
            SqlCommand cmd = new SqlCommand(qry, Mainclass.conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                txtName.Text = dt.Rows[0]["pname"].ToString();
                txtprice.Text = dt.Rows[0]["pprice"].ToString();

                if (dt.Rows[0]["pimage"] != DBNull.Value)
                {
                    byte[] imageArray = (byte[])dt.Rows[0]["pimage"];
                    txtimage.Image = Image.FromStream(new MemoryStream(imageArray));
                }
                else
                {
                    txtimage.Image = Properties.Resources.image;
                }
            }
        }
    }
}
