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
//need to fix save button
namespace c__project1
{
    public partial class Checkout : Sampleadd
    {

        public Checkout()
        {
            InitializeComponent();
        }


        public double amt;
        public int MainID = 0;
   
        public double rec=0;
        public double rem = 0;

        private void txtReceived_TextChanged(object sender, EventArgs e)
        {
            txtdis.Text = "0";

            double amt = 0;
            double receipt = 0;
            double change = 0;
            double discount;

            double.TryParse(txtBillAmount.Text, out amt);
            double.TryParse(txtReceived.Text, out receipt);
            double.TryParse(txtdis.Text, out discount);


            double disval = 100 - discount;

            double billval = amt * (disval / 100);

            change = Math.Abs(billval - receipt); //convert negative or positive to positive


            txtChange.Text = change.ToString();

        }
        private void txtdis_TextChanged_1(object sender, EventArgs e)
        {

            double amt = 0;
            double receipt = 0;
            double change = 0;
            double discount;

            double.TryParse(txtBillAmount.Text, out amt);
            double.TryParse(txtReceived.Text, out receipt);
            double.TryParse(txtdis.Text, out discount);

            
           double disval = 100 - discount;

            double billval = amt * (disval/100);
            
            change = Math.Abs(billval - receipt); //convert negative or positive to positive


            if (txtReceived.Text != null)
            {
                txtChange.Text = change.ToString();
            }
        }




        public override void btnsave_Click(object sender, EventArgs e)
        {
            Pos frm = new Pos();


            if (frm.rhold.Checked != false || frm.btndeliver.Checked != false || frm.btnaway.Checked != false || frm.btnin.Checked != false)
            { 
                    
            }

            string qry = @" update tblMain set  total=@total , received =@rec ,change =@change , status='Paid'
                             where MainID = @id ";
            

            Hashtable ht = new Hashtable();
            ht.Add("@id ", MainID);
            ht.Add("@total", Convert.ToDouble(txtBillAmount.Text));
            ht.Add("@rec", Convert.ToDouble(txtReceived.Text));
            ht.Add("@change", Convert.ToDouble(txtChange.Text));


            //if (txtReceived.Text!=null && txtChange.Text!=null)
            //{
            //    MessageBox.Show("Saved Successfully...", "Save Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    this.Close();
            //}


            if (Mainclass.SQL(qry, ht) > 0)
            {
                MessageBox.Show("Saved Successfully...", "Save Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            
        }

            
        private void Checkout_Load(object sender, EventArgs e)
        {
            txtBillAmount.Text = amt.ToString();
        }



     

        private void btnclose_Click_1(object sender, EventArgs e)
        {   
            
            Pos frm =new Pos();
            frm.lbl3.Text =txtdis.Text ;
            
            this.Close();
        }

        private void btnsave_Click_1(object sender, EventArgs e)
        {
            
           //
            
        }

        
    }
}
