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
    
    public partial class ucProduct : UserControl
    {
        public ucProduct()
        {
            InitializeComponent();
        }

        private void lblProcut_Click(object sender, EventArgs e)
        {
            // Raise the onSelect event when the label is clicked
            onSelect?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler onSelect = null;

        public int id { get; set; }
        public double PPrice { get; set; }

        public string PCategory { get; set; }

        public string PName
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }
    }
}
