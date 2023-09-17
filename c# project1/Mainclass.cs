using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace c__project1
{
    internal class Mainclass
    {
        public static SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Rashminda\Documents\C# databases\HardwareStore.mdf"";Integrated Security=True;Connect Timeout=30");
        

        public static bool IsValidUser(string user,string pass)
        {
            bool isVAlid = false;

            string qry = @"select * from users where username= '" + user + "' and upassword='"+pass+"'  " ;
            SqlCommand cmd = new SqlCommand(qry, conn);
            DataTable dt=new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                isVAlid=true;
                USER = dt.Rows[0]["uName"].ToString();
            }

            return isVAlid;
        }
        public static string user;
        public static string USER
        {
            get { return user; }
            private set { user = value; }
        }

        // crud operations

        public static int SQL(string qry,Hashtable ht)
        {
            int res = 0;
            try
            {
                SqlCommand cmd= new SqlCommand(qry, conn);
                cmd.CommandType = CommandType.Text;

                foreach (DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(),item.Value  );
                }
                if (conn.State== ConnectionState.Closed) { conn.Open(); }
                res = cmd.ExecuteNonQuery();
                if (conn.State == ConnectionState.Open) { conn.Close(); }

            }
            catch ( Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conn.Close();
            }

            return res;
        }

        //load data from database
        
        public static void LoadData(string qry,DataGridView gv,ListBox lb)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(qry, conn);
                cmd.CommandType = CommandType.Text; 
                SqlDataAdapter da=new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for(int i = 0; i < lb.Items.Count; i++)
                {
                    string colnam1= ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colnam1].DataPropertyName = dt.Columns[i].ToString();
                }
                gv.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conn.Close();
            }
        }



        /*
        private static void cellformatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            datagridview gv = (datagridview)sender;
            int count = 0;
            foreach(DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }

        }*/

        

        public static void blurbackground(Form Model)
        {
            Form background= new Form();
            using (Model)
            {
                background.StartPosition=FormStartPosition.Manual;
                background.FormBorderStyle = FormBorderStyle.None;
                background.Opacity = 0.5d;
                background.BackColor = System.Drawing.Color.Black;
                background.Size = Main.Instance.Size;
                background.Location = Main.Instance.Location;
                background.ShowInTaskbar = false;
                background.Show();
                Model.Owner=background;
                Model.ShowDialog(background);
                background.Dispose();
            }
        }

        public static void blurfill(Form Model)
        {
            Form background = new Form();
            using (Model)
            {
                background.WindowState= FormWindowState.Maximized;
                background.Opacity = 0.5d;
                background.BackColor = System.Drawing.Color.Black;
                background.Size = Main.Instance.Size;
                background.Location = Main.Instance.Location;
                background.ShowInTaskbar = false;
                Model.Owner = background;
                Model.ShowDialog(background);
                background.Dispose();
            }
        }

        //cb fill

        public static void CBFILL(string qry,ComboBox cb)
        {
            SqlCommand cmd= new SqlCommand(qry,conn);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cb.DisplayMember = "name";
            cb.ValueMember = "id";
            cb.DataSource = dt;
            cb.SelectedIndex = -1;
        }

    }
}
