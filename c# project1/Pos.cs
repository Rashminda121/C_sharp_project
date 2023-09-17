using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace c__project1
{
    public partial class Pos : Form
    {

        public Pos()
        {
            InitializeComponent();
            this.Load += Pos_Load;
        }

        public int MainId = 0;
        public string OrderType = "";
        public int driverID = 0;
        public string customerName = "";
        public string customerPhone = "";


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Pos_Load(object sender, EventArgs e)
        {
            datagrid1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            ProductPanel.Controls.Clear();
            ProductPanel.FlowDirection = FlowDirection.LeftToRight; // Set flow direction

            LoadEntries();
            Loadlist();
        }



        private void AddCategory()
        {
            string qry = "SELECT * FROM category";
            SqlCommand cmd = new SqlCommand(qry, Mainclass.conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            CatergoryPanel.Controls.Clear();

            if (dt.Rows.Count > 0)
            {
                int buttonHeight = 40;
                int spacing = 10;
                int currentY = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    string categoryName = dr["catname"].ToString();

                    Button button = new Button();
                    button.Text = categoryName;
                    button.BackColor = Color.FromArgb(50, 55, 89);
                    button.ForeColor = Color.White;
                    button.FlatStyle = FlatStyle.Flat;
                    button.Size = new Size(110, buttonHeight);
                    button.Location = new Point(0, currentY);

                    //event for click
                    button.Click += new EventHandler(_Click);


                    button.Click += category_Click; // Attach the event handler

                    CatergoryPanel.Controls.Add(button);

                    currentY += buttonHeight + spacing;
                }
            }
        }

        private void _Click(object sender, EventArgs e)
        {

            Button button = (Button)sender;

            if (button.Text == "All Categories")
            {
                txtsearch.Text = "1";
                txtsearch.Text = "";
                return;
            }

            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains(button.Text.Trim().ToLower());
            }

            lcount = 1;
        }



        private void category_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string categoryName = button.Text;
        }




        private void AddItems(string id, string proID, string name, string cat, string price, Image pimage)
        {
            var w = new ucProduct()
            {
                PName = name,
                PPrice = price,
                PCategory = cat,
                PImage = pimage,
                id = Convert.ToInt32(proID)

            };
            ProductPanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;
                bool productExists = false;

                foreach (DataGridViewRow item in datagrid1.Rows)
                {
                    if (Convert.ToInt32(item.Cells["dgvproID"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) * double.Parse(item.Cells["dgvprice"].Value.ToString());

                        productExists = true;
                        break;
                    }
                }

                if (!productExists)
                {
                    datagrid1.Rows.Add(new object[] { "-", 0, wdg.id, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                }
                GetTotal();
            };
        }

        //getting product from database

        private void LoadEntries()
        {
            string qry = "select * from products inner join category on catid =categoryID";
            SqlCommand cmd = new SqlCommand(qry, Mainclass.conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                Byte[] imagearray = (byte[])item["pImage"];
                byte[] imagebytearray = imagearray;

                AddItems("0", item["pid"].ToString(), item["pname"].ToString(), item["catname"].ToString(), item["pprice"].ToString(), Image.FromStream(new MemoryStream(imagearray)));
            }
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtsearch.Text.Trim().ToLower());
            }
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

        private void GetTotal()
        {
            double tot = 0;
            lblTotal.Text = "";

            foreach (DataGridViewRow item in datagrid1.Rows)
            {
                tot += double.Parse(item.Cells["dgvAmount"].Value.ToString());
            }
            lblTotal.Text = tot.ToString("N2");
        }

        int lcount = 0;
        private void button1_Click(object sender, EventArgs e)
        {

            if (lcount == 1)
            {
                ProductPanel.Controls.Clear();
                LoadEntries();
                lcount = 0;
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblStaff.Text = "";
            lblTable.Visible = false;
            lblStaff.Visible = false;
            datagrid1.Rows.Clear();
            MainId = 0;
            lblTotal.Text = "00";

        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblStaff.Text = "";
            lblTable.Visible = false;
            lblStaff.Visible = false;
            OrderType = "Delivery";



            AddCustomer frm = new AddCustomer();
            frm.mainID = MainId;
            frm.orderType = OrderType;
            Mainclass.blurbackground(frm);

            if (frm.txtName.Text != "") //as take away did not have driver info
            {
                driverID = frm.driverID;
                lblDriverName.Text = "Customer Name: " + frm.txtName.Text + " Phone: " + frm.txtPhone.Text + " Driver : " + frm.cbDriver.Text;
                lblDriverName.Visible = true;
                customerName = frm.txtName.Text;
                customerPhone = frm.txtPhone.Text;
            }

            btndeliver.Checked = true; //////////
        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            btnaway.Checked = true;

            lblTable.Text = "";
            lblStaff.Text = "";
            lblTable.Visible = false;
            lblStaff.Visible = false;
            OrderType = "Take On";

            AddCustomer frm = new AddCustomer();
            frm.mainID = MainId;
            frm.orderType = OrderType;
            Mainclass.blurbackground(frm);
            frm.cbDriver.Visible = false;
            frm.lblDriver.Visible = false;

            if (frm.txtName.Text != "") //as take away did not have driver info
            {
                frm.cbDriver.Visible = false;
                frm.lblDriver.Visible = false;
                //driverID = frm.driverID;
                lblDriverName.Text = "Customer Name: " + frm.txtName.Text + " Phone: " + frm.txtPhone.Text;
                lblDriverName.Visible = false;
                customerName = frm.txtName.Text;
                customerPhone = frm.txtPhone.Text;
            }

            btnaway.Checked = true; ///////////
        }

        private void btnDin_Click(object sender, EventArgs e)
        {
            btnin.Checked = true; ////////////


            OrderType = "Order ";
            lblDriverName.Visible = false;

            OrderSelect frm = new OrderSelect();
            frm.ShowDialog();

            if (!string.IsNullOrEmpty(frm.TableName))
            {
                lblTable.Text = frm.TableName;
                lblTable.Visible = true;

                StaffSelect frm2 = new StaffSelect();
                frm2.ShowDialog();

                if (!string.IsNullOrEmpty(frm2.staffName))
                {
                    lblStaff.Text = frm2.staffName;
                    lblStaff.Visible = true;

                    
                    this.Show();
                }
                else
                {
                    lblStaff.Text = "";
                    lblStaff.Visible = false;
                }
            }
            else
            {
                lblTable.Text = "";
                lblTable.Visible = false;
            }
        }


        private void btnOther_Click(object sender, EventArgs e)
        {

            lblTable.Text = "";
            lblStaff.Text = "";
            lblTable.Visible = false;
            lblStaff.Visible = false;
            OrderType = "Other";

            if (datagrid1 != null)
            {


                // save the data in database

                //Main table
                string qry1 = "";

                //Detail table
                string qry2 = "";


                int detailID = 0;

                //if (OrderType == "")
                //{
                //    MessageBox.Show("Please Select a Method...", "Select", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}



                if (MainId == 0) //insert
                {
                    qry1 = @" Insert into tblMain Values(@aDate, @aTime, @TableName, @WaiterName, @status, @orderType, @total, @received, @change,@driverID,@CustName,@CustPhone);
                                                            Select SCOPE_IDENTITY()";

                    // get recent add id value
                }
                else //update
                {
                    qry1 = @" update tblMain set status=@status, total=@total, received=@received, change=@change  where MainID=@ID";
                }


                SqlCommand cmd = new SqlCommand(qry1, Mainclass.conn);

                cmd.Parameters.AddWithValue("@ID", MainId);
                cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
                cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@TableName", lblTable.Text);
                cmd.Parameters.AddWithValue("@WaiterName", lblStaff.Text);
                cmd.Parameters.AddWithValue("@status", "Complete");
                cmd.Parameters.AddWithValue("@orderType", OrderType);
                cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lblTotal.Text)); // value will update when payment received 
                cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
                cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
                cmd.Parameters.AddWithValue("@driverID", driverID);
                cmd.Parameters.AddWithValue("@CustName", customerName);
                cmd.Parameters.AddWithValue("@CustPhone", customerPhone);



                if (Mainclass.conn.State == ConnectionState.Closed) { Mainclass.conn.Open(); }

                if (MainId == 0) { MainId = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }

                if (Mainclass.conn.State == ConnectionState.Open) { Mainclass.conn.Close(); }



                foreach (DataGridViewRow row in datagrid1.Rows)
                {
                    detailID = Convert.ToInt32(row.Cells[1].Value); // Replace 1 with the index of the 'dgvid' column
                    int mainID = MainId; // Use the existing MainId value

                    int productID = Convert.ToInt32(row.Cells[2].Value); // Replace 2 with the index of the 'dgvproID' column
                    int quantity = Convert.ToInt32(row.Cells[4].Value); // Replace 4 with the index of the 'dgvQty' column
                    double price = Convert.ToDouble(row.Cells[5].Value); // Replace 5 with the index of the 'dgvPrice' column
                    double amount = Convert.ToDouble(row.Cells[6].Value); // Replace 6 with the index of the 'dgvAmount' column

                    string query;
                    SqlCommand command;

                    if (detailID == 0) // Insert
                    {
                        query = "INSERT INTO tblDetails (MainID, proID, qty, price, amount) VALUES (@MainID, @proID, @qty, @price, @amount)";
                        command = new SqlCommand(query, Mainclass.conn);
                    }
                    else // Update
                    {
                        query = "UPDATE tblDetails SET proID = @proID, qty = @qty, price = @price, amount = @amount WHERE DetailID = @ID";
                        command = new SqlCommand(query, Mainclass.conn);
                        command.Parameters.AddWithValue("@ID", detailID);
                    }

                    command.Parameters.AddWithValue("@MainID", mainID);
                    command.Parameters.AddWithValue("@proID", productID);
                    command.Parameters.AddWithValue("@qty", quantity);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@amount", amount);

                    if (Mainclass.conn.State == ConnectionState.Closed)
                        Mainclass.conn.Open();

                    command.ExecuteNonQuery();

                    if (Mainclass.conn.State == ConnectionState.Open)
                        Mainclass.conn.Close();



                }

                MessageBox.Show("Data Saved successfully..", "Data Save", MessageBoxButtons.OK, MessageBoxIcon.Information);


                MainId = 0;
                detailID = 0;
                datagrid1.Rows.Clear();
                lblTable.Text = "";
                lblStaff.Text = "";
                lblTable.Visible = false;
                lblStaff.Visible = false;
                lblTotal.Text = "00";
                OrderType = "";/////////////////////////////////////
            }
            else
            {
                MessageBox.Show("Please Select Products...", "Select", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

        }

        private void btnKot_Click(object sender, EventArgs e)
        {

            if (OrderType != "" && datagrid1 != null)
            {

                // save the data in database

                //Main table
                string qry1 = "";

                //Detail table
                string qry2 = "";


                int detailID = 0;

                if (MainId == 0) //insert
                {
                    qry1 = @" Insert into tblMain Values(@aDate, @aTime, @TableName, @WaiterName, @status, @orderType, @total, @received, @change,@driverID,@CustName,@CustPhone);
                                                            Select SCOPE_IDENTITY()";

                    // get recent add id value
                }
                else //update
                {
                    qry1 = @" update tblMain set status=@status, total=@total, received=@received, change=@change  where MainID=@ID";
                }


                SqlCommand cmd = new SqlCommand(qry1, Mainclass.conn);

                cmd.Parameters.AddWithValue("@ID", MainId);
                cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
                cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@TableName", lblTable.Text);
                cmd.Parameters.AddWithValue("@WaiterName", lblStaff.Text);
                cmd.Parameters.AddWithValue("@status", "Pending");
                cmd.Parameters.AddWithValue("@orderType", OrderType);
                cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lblTotal.Text)); // value will update when payment received 
                cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
                cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
                cmd.Parameters.AddWithValue("@driverID", driverID);
                cmd.Parameters.AddWithValue("@CustName", customerName);
                cmd.Parameters.AddWithValue("@CustPhone", customerPhone);


                if (Mainclass.conn.State == ConnectionState.Closed) { Mainclass.conn.Open(); }

                if (MainId == 0) { MainId = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }

                if (Mainclass.conn.State == ConnectionState.Open) { Mainclass.conn.Close(); }



                foreach (DataGridViewRow row in datagrid1.Rows)
                {
                    detailID = Convert.ToInt32(row.Cells[1].Value); // Replace 1 with the index of the 'dgvid' column
                    int mainID = MainId; // Use the existing MainId value

                    int productID = Convert.ToInt32(row.Cells[2].Value); // Replace 2 with the index of the 'dgvproID' column
                    int quantity = Convert.ToInt32(row.Cells[4].Value); // Replace 4 with the index of the 'dgvQty' column
                    double price = Convert.ToDouble(row.Cells[5].Value); // Replace 5 with the index of the 'dgvPrice' column
                    double amount = Convert.ToDouble(row.Cells[6].Value); // Replace 6 with the index of the 'dgvAmount' column

                    string query;
                    SqlCommand command;

                    if (detailID == 0) // Insert
                    {
                        query = "INSERT INTO tblDetails (MainID, proID, qty, price, amount) VALUES (@MainID, @proID, @qty, @price, @amount)";
                        command = new SqlCommand(query, Mainclass.conn);
                    }
                    else // Update
                    {
                        query = "UPDATE tblDetails SET proID = @proID, qty = @qty, price = @price, amount = @amount WHERE DetailID = @ID";
                        command = new SqlCommand(query, Mainclass.conn);
                        command.Parameters.AddWithValue("@ID", detailID);
                    }

                    command.Parameters.AddWithValue("@MainID", mainID);
                    command.Parameters.AddWithValue("@proID", productID);
                    command.Parameters.AddWithValue("@qty", quantity);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@amount", amount);

                    if (Mainclass.conn.State == ConnectionState.Closed)
                        Mainclass.conn.Open();

                    command.ExecuteNonQuery();

                    if (Mainclass.conn.State == ConnectionState.Open)
                        Mainclass.conn.Close();



                }

                MessageBox.Show("Data Saved successfully...", "Data Save", MessageBoxButtons.OK, MessageBoxIcon.Information);

                MainId = 0;
                detailID = 0;
                datagrid1.Rows.Clear();
                lblTable.Text = "";
                lblStaff.Text = "";
                lblTable.Visible = false;
                lblStaff.Visible = false;
                lblTotal.Text = "00";
                lblDriverName.Text = "";
                OrderType = "";////////////////////////////////////////
            }
            else
            {
                MessageBox.Show("Please Select a Method...", "Select", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        public int id = 0;

        private void btnBill_Click(object sender, EventArgs e)
        {
            BillList frm = new BillList();
            Mainclass.blurbackground(frm);


            if (frm.MainID > 0)
            {

                id = frm.MainID;
                MainId = frm.MainID;
                Loadlist();
            }
        }

        private void Loadlist()
        {
            string qry = @"select * from tblMain m
                                 inner join tblDetails d on m.MainID = d.MainID
                                 inner join products p on p.pid = d.proID
                                 where m.MainID = " + id + "";


            SqlCommand cmd2 = new SqlCommand(qry, Mainclass.conn);
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);

            try
            {
                if (dt2.Rows[0]["orderType"].ToString() == "Delivery")
                {
                    btndeliver.Checked = true;
                    lblStaff.Visible = false;
                    lblTable.Visible = false;

                }
                else if (dt2.Rows[0]["orderType"].ToString() == "Take On")
                {
                    btnaway.Checked = true;
                    lblStaff.Visible = false;
                    lblTable.Visible = false;
                }
                else
                {
                    btnin.Checked = true;
                    lblStaff.Visible = true;
                    lblTable.Visible = true;
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }


            datagrid1.Rows.Clear();


            foreach (DataRow item in dt2.Rows)
            {
                lblTable.Text = item["TableName"].ToString();
                lblStaff.Text = item["WaiterName"].ToString();

                string detailid = item["DetailID"].ToString();
                string proName = item["pname"].ToString();
                string proid = item["proID"].ToString();
                string qty = item["qty"].ToString();
                string price = item["price"].ToString();
                string amount = item["amount"].ToString();

                object[] obj = { 0, detailid, proid, proName, qty, price, amount };
                datagrid1.Rows.Add(obj);

            }
            GetTotal();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            Checkout frm = new Checkout();
            frm.MainID = id;
            frm.amt = Convert.ToDouble(lblTotal.Text);

            Mainclass.blurbackground(frm);

            if (rhold.Checked != false || btndeliver.Checked != false || btnaway.Checked != false || btnin.Checked != false || rbOther.Checked != false)
            {
                datagrid1.Rows.Clear();
                lblTotal.Text = "0.00";
            }


            MainId = 0;
            //datagrid1.Rows.Clear();
            lblTable.Text = "";
            lblStaff.Text = "";
            lblTable.Visible = false;
            lblStaff.Visible = false;
            //lblTotal.Text = "00";

            lbl1.Text = frm.txtReceived.Text.ToString();
            lbl2.Text = frm.txtChange.Text.ToString();
            lbl3.Text = frm.txtdis.Text.ToString();


        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            rhold.Checked = true;
            if (OrderType != "" && datagrid1 != null)
            {
                // save the data in database

                //Main table
                string qry1 = "";

                //Detail table
                string qry2 = "";


                int detailID = 0;


                if (MainId == 0) //insert
                {
                    qry1 = @" Insert into tblMain Values(@aDate, @aTime, @TableName, @WaiterName, @status, @orderType, @total, @received, @change,@driverID,@CustName,@CustPhone);
                                                            Select SCOPE_IDENTITY()";

                    // get recent add id value
                }
                else //update
                {
                    qry1 = @" update tblMain set status=@status, total=@total, received=@received, change=@change  where MainID=@ID";
                }


                SqlCommand cmd = new SqlCommand(qry1, Mainclass.conn);

                cmd.Parameters.AddWithValue("@ID", MainId);
                cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
                cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@TableName", lblTable.Text);
                cmd.Parameters.AddWithValue("@WaiterName", lblStaff.Text);
                cmd.Parameters.AddWithValue("@status", "Hold");
                cmd.Parameters.AddWithValue("@orderType", OrderType);
                cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lblTotal.Text)); // value will update when payment received 
                cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
                cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
                cmd.Parameters.AddWithValue("@driverID", driverID);
                cmd.Parameters.AddWithValue("@CustName", customerName);
                cmd.Parameters.AddWithValue("@CustPhone", customerPhone);



                if (Mainclass.conn.State == ConnectionState.Closed) { Mainclass.conn.Open(); }

                if (MainId == 0) { MainId = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }

                if (Mainclass.conn.State == ConnectionState.Open) { Mainclass.conn.Close(); }



                foreach (DataGridViewRow row in datagrid1.Rows)
                {
                    detailID = Convert.ToInt32(row.Cells[1].Value); // Replace 1 with the index of the 'dgvid' column
                    int mainID = MainId; // Use the existing MainId value

                    int productID = Convert.ToInt32(row.Cells[2].Value); // Replace 2 with the index of the 'dgvproID' column
                    int quantity = Convert.ToInt32(row.Cells[4].Value); // Replace 4 with the index of the 'dgvQty' column
                    double price = Convert.ToDouble(row.Cells[5].Value); // Replace 5 with the index of the 'dgvPrice' column
                    double amount = Convert.ToDouble(row.Cells[6].Value); // Replace 6 with the index of the 'dgvAmount' column

                    string query;
                    SqlCommand command;

                    if (detailID == 0) // Insert
                    {
                        query = "INSERT INTO tblDetails (MainID, proID, qty, price, amount) VALUES (@MainID, @proID, @qty, @price, @amount)";
                        command = new SqlCommand(query, Mainclass.conn);
                    }
                    else // Update
                    {
                        query = "UPDATE tblDetails SET proID = @proID, qty = @qty, price = @price, amount = @amount WHERE DetailID = @ID";
                        command = new SqlCommand(query, Mainclass.conn);
                        command.Parameters.AddWithValue("@ID", detailID);
                    }

                    command.Parameters.AddWithValue("@MainID", mainID);
                    command.Parameters.AddWithValue("@proID", productID);
                    command.Parameters.AddWithValue("@qty", quantity);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@amount", amount);

                    if (Mainclass.conn.State == ConnectionState.Closed)
                        Mainclass.conn.Open();

                    command.ExecuteNonQuery();

                    if (Mainclass.conn.State == ConnectionState.Open)
                        Mainclass.conn.Close();



                }

                MessageBox.Show("Data Saved successfully..", "Data Save", MessageBoxButtons.OK, MessageBoxIcon.Information);

                MainId = 0;
                detailID = 0;
                datagrid1.Rows.Clear();
                lblTable.Text = "";
                lblStaff.Text = "";
                lblTable.Visible = false;
                lblStaff.Visible = false;
                lblTotal.Text = "00";
                lblDriverName.Text = "";
                OrderType = ""; ////////////////////////////////////////////
            }
            else
            {
                MessageBox.Show("Please Select a Method...", "Select", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }




        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Checkout frm = new Checkout();

            string receive = lbl1.Text;
            string remain = lbl2.Text;
            string dis= lbl3.Text;

            Font printFont = new Font("Century", 10);
            float yPos = 10; // Adjust the initial vertical position
            float leftMargin = 10; // Adjust the left margin
            float topMargin = 10; // Adjust the top margin

            e.Graphics.DrawString("Hardware Store ", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("Galagedara, Padukka. " + "\n", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);

            e.Graphics.DrawString("Date & Time: " + DateTime.Now.ToString() + "\n", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("================================================================ ", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("Item Name                                               Quantity                     Amount  ", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("================================================================ ", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);

            foreach (DataGridViewRow row in datagrid1.Rows)
            {
                string itemName = row.Cells[3].Value.ToString().PadRight(30);
                string quantity = row.Cells[4].Value.ToString().PadRight(10);
                string amount = row.Cells[6].Value.ToString().PadRight(10);
                string line = itemName + quantity + amount;
                float lineHeight = printFont.GetHeight(e.Graphics);

                // Split the line into individual characters and print them one by one
                for (int i = 0; i < line.Length; i++)
                {
                    char c = line[i];
                    e.Graphics.DrawString(c.ToString(), printFont, Brushes.Black, leftMargin + (i * 10), yPos, new StringFormat());

                }

                yPos += lineHeight;
            }

            e.Graphics.DrawString("================================================================ " + "\n", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("Total:           " + lblTotal.Text, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("Received:        " + receive, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("Discount:        " + dis +"%", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("Change:          " + remain, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("================================================================ " + "\n", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            yPos += printFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("Thank You ...!         ", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (datagrid1.Rows.Count > 0)
            {
                PrintDocument doc = new PrintDocument();
                doc.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
                System.Windows.Forms.PrintDialog dlg = new System.Windows.Forms.PrintDialog();
                dlg.Document = doc;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    doc.Print();
                }
            }
            else
            {
                MessageBox.Show("Select Products...", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void datagrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = datagrid1.Columns[e.ColumnIndex].Name;
            if (colname == "dgdelete" && e.RowIndex >= 0)
            {
                if (MessageBox.Show("Are you sure you want to remove this record?", "Remove Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    double product = 0.0;
                    if (datagrid1.Rows[e.RowIndex].Cells["dgvAmount"].Value != null && double.TryParse(datagrid1.Rows[e.RowIndex].Cells["dgvAmount"].Value.ToString(), out product))
                    {
                        double lbl = Convert.ToDouble(lblTotal.Text);
                        lblTotal.Text = (lbl - product).ToString();
                    }

                    datagrid1.Rows.RemoveAt(e.RowIndex);

                }
            }
        }


    }

    public class pos
    {
        private int btnBill;
        private int btnCheckout;
        private int btndeliver;
        private int btnHold;
        private int btnKot;
        private int btnOther;
        private int btnTake;
        private int customerName;
        private int customerPhone;
        private int datagrid1;
        private int dgdelete;
        private int dgName;
        private int dgvAmount;
        private int dgvid;
        private int dgvprice;
        private int dgvproID;
        private int dgvQty;
        private int lblTotal;

        public void btnBill_Click()
        {
            throw new System.NotImplementedException();
        }

        public void btnCheckout_Click()
        {
            throw new System.NotImplementedException();
        }

        public void GetTotal()
        {
            throw new System.NotImplementedException();
        }

        public void LoadEntries()
        {
            throw new System.NotImplementedException();
        }

        public void Loadlist()
        {
            throw new System.NotImplementedException();
        }

        public void pictureBox3_Click()
        {
            throw new System.NotImplementedException();
        }

        public void Pos()
        {
            throw new System.NotImplementedException();
        }

        public void printDocument1_PrintPage()
        {
            throw new System.NotImplementedException();
        }
    }
}




