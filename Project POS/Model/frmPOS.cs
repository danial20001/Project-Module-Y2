using MySql.Data.MySqlClient;
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

namespace Project_POS.Model
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
            guna2DataGridView1.CellClick += guna2DataGridView1_CellClick;

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Takeout";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            AddCategory();
            LoadProducts();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {

        }

        private void AddCategory()
        {
            string qry = "SELECT * FROM category";
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                CategoryPanel.Controls.Clear();

                // Color palette
                Color[] colors = new Color[]
                {
            Color.FromArgb(255, 202, 224), // #ffcae0
            Color.FromArgb(215, 191, 218), // #d7bfda
            Color.FromArgb(190, 199, 227), // #bec7e3
            Color.FromArgb(142, 205, 217), // #8ecdd9
            Color.FromArgb(112, 196, 198)  // #70c4c6
                };

                int buttonHeight = 52;
                int verticalSpacing = 12;
                int buttonWidth = 175;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dt.Rows[i];
                        Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button
                        {
                            Text = row["catName"].ToString(),
                            Size = new Size(buttonWidth, buttonHeight),
                            FillColor = colors[i % colors.Length],
                            ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton,
                            ForeColor = Color.Black,
                            Font = new Font("Gill Sans Nova", 10, FontStyle.Regular),
                            Location = new Point(0, i * (buttonHeight + verticalSpacing) + verticalSpacing) // Set location with vertical spacing
                        };
                        b.Click += CategoryButton_Click;
                        CategoryPanel.Controls.Add(b);
                    }
                }
            }
        }


        private void AddItems(string id, string name, string cat, string price)
        {
            double productPrice;
            if (!double.TryParse(price, out productPrice))
            {
                MessageBox.Show("Invalid price format");
                return;
            }

            var productControl = new ucProduct()
            {
                PName = name,
                PPrice = productPrice, // Ensure the price is parsed correctly
                PCategory = cat,
                id = int.Parse(id) // Safe parsing should be implemented
            };

            // Attach the onSelect event handler
            productControl.onSelect += Product_OnSelect;

            // Add the UserControl to the ProductPanel
            ProductPanel.Controls.Add(productControl);
        }


        private void Product_OnSelect(object sender, EventArgs e)
        {
            var product = sender as ucProduct;
            if (product == null) return;

            bool found = false;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (Convert.ToInt32(row.Cells["dgvid"].Value) == product.id)
                {
                    // Product exists, increase quantity
                    int qty = Convert.ToInt32(row.Cells["dgvQty"].Value) + 1;
                    row.Cells["dgvQty"].Value = qty;
                    row.Cells["dgvAmount"].Value = qty * product.PPrice;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                // Product does not exist, add a new row
                guna2DataGridView1.Rows.Add(new object[] {
            guna2DataGridView1.Rows.Count + 1, // Assuming first column is a row number which auto-increments
            product.id,
            product.PName,
            1, // Quantity
            product.PPrice,
            product.PPrice // Initial amount is just the price as quantity is 1
        });
            }

            UpdateTotal();  // Update the total whenever products are added or quantities are changed
        }

        //getting product from database

        private void LoadProducts()
        {

            string qry = "SELECT * FROM products inner join category on catID = CategoryID";
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ProductPanel.Controls.Clear();
                foreach (DataRow item in dt.Rows)
                {

                    AddItems(item["pID"].ToString(), item["pName"].ToString(), item["catName"].ToString(),
                        item["pPrice"].ToString());


                }
            }
        }

        private void CategoryPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            var button = sender as Guna.UI2.WinForms.Guna2Button;
            if (button == null) return;

            // Display the clicked category
            // MessageBox.Show("You clicked: " + button.Text);

            // Load products filtered by the category name associated with the button
            LoadProductsFilteredByCategory(button.Text);
        }


        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Retrieve the search text from the textbox
            string searchText = txtSearch.Text.ToLower().Trim();

            // Clear existing controls in the ProductPanel
            ProductPanel.Controls.Clear();

            // Reload and filter products based on the search text
            LoadProductsFiltered(searchText);
        }


        private void LoadProductsFiltered(string searchText)
        {
            string qry = "SELECT * FROM products INNER JOIN category ON catID = CategoryID";
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow item in dt.Rows)
                {
                    string productName = item["pName"].ToString();
                    string productCategory = item["catName"].ToString();
                    // Filter products based on name or category containing the search text
                    if (productName.ToLower().Contains(searchText) || productCategory.ToLower().Contains(searchText))
                    {
                        AddItems(item["pID"].ToString(), productName, productCategory, item["pPrice"].ToString());
                    }
                }
            }
        }


        private void UpdateTotal()
        {
            double total = 0;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                double amount = 0;
                // Safely try to parse the Amount column values
                if (double.TryParse(row.Cells["dgvAmount"].Value?.ToString(), out amount))
                {
                    total += amount;
                }
            }

            // Update the lblTotal text
            lblTotal.Text = $"Total: £{total:N2}";  // N2 formats the number as a currency with 2 decimal places
        }


        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is on the delete image column
            if (e.RowIndex >= 0 && e.ColumnIndex == guna2DataGridView1.Columns["dgvdel"].Index)
            {
                // Confirm deletion
                if (MessageBox.Show("Are you sure you want to delete this item?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    guna2DataGridView1.Rows.RemoveAt(e.RowIndex);

                    // Update the total after deletion
                    UpdateTotal();
                }
            }
        }

        private void ProductPanel_Paint(object sender, PaintEventArgs e)
        {

        }


        private void LoadProductsFilteredByCategory(string categoryName)
        {
            string qry = "SELECT products.pID, products.pName, category.catName, products.pPrice " +
                         "FROM products " +
                         "INNER JOIN category ON products.CategoryID = category.catID " +
                         "WHERE category.catName = @CategoryName;";

            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ProductPanel.Controls.Clear();
                foreach (DataRow item in dt.Rows)
                {
                    AddItems(item["pID"].ToString(), item["pName"].ToString(), item["catName"].ToString(), item["pPrice"].ToString());
                }
            }
        }
        public int MainID = 0;
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            MainID = 0;
            lblTotal.Text = "Total: £0";
        }


        public string OrderType;
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Delivery";
        }

        private void kitchenbutton_Click(object sender, EventArgs e)
        {

        }

        private void lblWaiter_Click(object sender, EventArgs e)
        {

        }
    }
}