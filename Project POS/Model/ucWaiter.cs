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
    public partial class ucWaiter : UserControl
    {
        public ucWaiter()
        {
            InitializeComponent();
            // In ucWaiter's constructor or initialization code
            

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        public int WaiterId { get; set; }
        public string WaiterName
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public Image WaiterImage
        {
            get { return guna2CirclePictureBox1.Image; }
            set { guna2CirclePictureBox1.Image = value; }
        }

        private void ucWaiter_Load(object sender, EventArgs e)
        {
            
        }
    }
}
