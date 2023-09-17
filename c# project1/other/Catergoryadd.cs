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

   
    public partial class Catergoryadd : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-RN86JLR\SQLEXPRESS;Initial Catalog=hardwarestore;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();

        public Catergoryadd()
        {
            InitializeComponent();
        }

        public int ID { get; internal set; }
    }
}
