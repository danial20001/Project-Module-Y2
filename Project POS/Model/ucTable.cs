using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_POS.Model
{
    public partial class ucTable : UserControl
    {
        public ucTable()
        {
            InitializeComponent();
            
        }


        public int MainID
        {
            get
            {
                // Attempt to parse the label's text back to an integer. If it fails, return a default value, such as 0.
                return int.TryParse(label4.Text, out int id) ? id : 0;
            }
            set
            {
                // Convert the integer to a string and set it as the label's text.
                label4.Text = value.ToString();
            }
        }


        public string TableName
        {
            get { return label3.Text; }
            set { label3.Text = value ?? "Not Defined"; }
        }

        public string Waiter
        {
            get { return label1.Text; }
            set { label1.Text = value ?? "Not Defined"; }
        }

        public string Status
        {
            get { return label2.Text; }
            set { label2.Text = value ?? "Not Defined"; }
        }




        private void ucTable_Load(object sender, EventArgs e)
        {
           
        }

        private void lblTableName_Click(object sender, EventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void lblWaiter_Click(object sender, EventArgs e)
        {

        }

        private void lblMainID_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
