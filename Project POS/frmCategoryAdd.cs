using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_POS
{
    public partial class frmCategoryAdd : Form
    {
        public int id;

        public frmCategoryAdd()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string qry = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Name", txtName.Text.Trim()); // Trim to remove whitespace

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("The name cannot be empty.");
                return;
            }

            if (id == 0) // insert
            {
                qry = "INSERT INTO category (catName) VALUES (@Name);";
            }
            else // update
            {
                qry = "UPDATE category SET catName = @Name WHERE catID = @id;";
                parameters.Add("@id", id);
            }


            if (Database.ExecuteSQL(qry, parameters) > 0)
            {
                MessageBox.Show("Saved Successfully.");
                id = 0; // Reset id if needed
                txtName.Text = ""; // Clear the textbox
                txtName.Focus(); // Set the focus back to the textbox
                this.DialogResult = DialogResult.OK; // If you're using ShowDialog, this will indicate success
            }
            else
            {
                MessageBox.Show("Save Failed. Please check your data and try again.");
            }
        }







        public virtual void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmCategoryAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
