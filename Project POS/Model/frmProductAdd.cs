using MySql.Data.MySqlClient;
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
    public partial class frmProductAdd : Form
    {

        public int productId = 0;
        public frmProductAdd()
        {
            InitializeComponent();
            LoadCategories();
        }


        private void LoadCategories()
        {
            comboxCategory.Items.Clear();
            string qry = "SELECT catID, catName FROM category"; // Corrected table name here
            using (MySqlConnection connection = Database.Connect())
            {
                MySqlCommand cmd = new MySqlCommand(qry, connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboxCategory.Items.Add(new ComboBoxItem { Value = reader["catID"].ToString(), Text = reader["catName"].ToString() });
                    }
                }
            }
        }


        private class ComboBoxItem
        {
            public string Value { get; set; }
            public string Text { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }


        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || comboxCategory.SelectedItem == null || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // Parse price text
            if (!decimal.TryParse(txtPhone.Text, out decimal price))
            {
                MessageBox.Show("Please enter a valid price.");
                txtPhone.Focus(); // Assuming txtPrice is the TextBox for price input
                return;
            }

            string qry = productId == 0 ?
                "INSERT INTO products (pName, pPrice, CategoryID) VALUES (@Name, @Price, @CatID);" :
                "UPDATE products SET pName = @Name, pPrice = @Price, CategoryID = @CatID WHERE pID = @ProductID;";

            var parameters = new Dictionary<string, object>
    {
        {"@Name", txtName.Text.Trim()},
        {"@Price", price},
        {"@CatID", ((ComboBoxItem)comboxCategory.SelectedItem).Value},
        {"@ProductID", productId}
    };

            if (Database.ExecuteSQL(qry, parameters) > 0)
            {
                MessageBox.Show("Product saved successfully.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to save the product. Please check the data.");
            }
        }




        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // Simply closes the form
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmProductAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
