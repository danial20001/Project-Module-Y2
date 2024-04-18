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
    public partial class frmStaffAdd : Form
    {
        public frmStaffAdd()
        {
            InitializeComponent();
        }

        private void frmStaffAdd_Load(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRole_TextChanged(object sender, EventArgs e)
        {

        }

        public int id = 0; // To keep track if this is a new entry (id = 0) or an update (id != 0)

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Simple validation to ensure that the fields are not empty
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name.");
                txtName.Focus(); // Focus the Name text box to prompt the user
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter a phone number.");
                txtPhone.Focus(); // Focus the Phone text box to prompt the user
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRole.Text))
            {
                MessageBox.Show("Please enter a role.");
                txtRole.Focus(); // Focus the Role text box to prompt the user
                return;
            }

            // Prepare the SQL query and parameters for the operation
            string qry = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@Name", txtName.Text.Trim() },
        { "@Phone", txtPhone.Text.Trim() },
        { "@Role", txtRole.Text.Trim() },
        { "@ImagePath", lblImagePath.Text } // Add the image path to the parameters
    };

            if (id == 0) // Insert operation
            {
                qry = "INSERT INTO staff (sName, sPhone, sRole, imagePath) VALUES (@Name, @Phone, @Role, @ImagePath);";
            }
            else // Update operation
            {
                qry = "UPDATE staff SET sName = @Name, sPhone = @Phone, sRole = @Role, imagePath = @ImagePath WHERE staffID = @id;";
                parameters.Add("@id", id);
            }

            // Execute the query and check the result
            if (Database.ExecuteSQL(qry, parameters) > 0)
            {
                MessageBox.Show("Staff information saved successfully.");
                this.DialogResult = DialogResult.OK; // Close the form with a DialogResult indicating success
            }
            else
            {
                MessageBox.Show("An error occurred while saving. Please check your data and try again.");
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // Simply closes the form
        }

        private void pictureBUTTON_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

            
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "Select Image";
                dlg.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    lblImagePath.Text = dlg.FileName; // Display the selected image path
                }
            


        }


        public void SetImagePath(string imagePath)
        {
            lblImagePath.Text = imagePath;  // Assuming lblImagePath is a label for displaying the image path
        }


        private void lblImagePath_Click(object sender, EventArgs e)
        {

        }
    }
}
