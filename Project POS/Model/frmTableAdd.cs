using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Project_POS.Model
{
    public partial class frmTableAdd : Form
    {
        public frmTableAdd()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmTableAdd_Load(object sender, EventArgs e)
        {

        }

        public int id = 0;

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Simple validation to ensure that the name is not empty
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name.");
                return;
            }

            string qry = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@Name", txtName.Text.Trim() } // Trimming to remove any leading/trailing spaces
    };

            if (id == 0) // insert
            {
                qry = "INSERT INTO tables (tname) VALUES (@Name);"; // Assuming 'tname' is the correct column name
            }
            else // update
            {
                qry = "UPDATE tables SET tname = @Name WHERE tid = @id;";
                parameters.Add("@id", id); // Ensure 'id' matches the column name for ID in your table
            }

            try
            {
                if (Database.ExecuteSQL(qry, parameters) > 0)
                {
                    MessageBox.Show("Saved Successfully.");
                    this.DialogResult = DialogResult.OK; // Set the dialog result if you're using ShowDialog
                }
                else
                {
                    MessageBox.Show("No changes were made. Please check your input and try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
