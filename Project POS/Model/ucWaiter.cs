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
        // Declare an event that can be subscribed to by the form that hosts this UserControl
        public event EventHandler WaiterSelected;

        public ucWaiter()
        {
            InitializeComponent();
            SetupClickEvents();  // Setup clickable events for the UserControl and its components
        }

        private void SetupClickEvents()
        {
            // Make the entire UserControl respond to clicks
            this.Click += (s, e) => OnWaiterSelected();
            // Additionally, ensure that clicks on any child controls (like labels, pictures) also trigger the event
            foreach (Control control in this.Controls)
            {
                control.Click += (s, e) => OnWaiterSelected();
            }
        }

        // Method to invoke the WaiterSelected event
        protected void OnWaiterSelected()
        {
            WaiterSelected?.Invoke(this, EventArgs.Empty);
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

        private void label1_Click(object sender, EventArgs e)
        {
            OnWaiterSelected();  // Trigger selection when the label is clicked
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            OnWaiterSelected();  // Trigger selection when the picture box is clicked
        }

        private void ucWaiter_Load(object sender, EventArgs e)
        {

        }
    }
}
