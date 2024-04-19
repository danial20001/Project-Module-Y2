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
            this.guna2Button5.Click += new System.EventHandler(this.guna2Button5_Click);
            
        }







        private void guna2Button7_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Takeout";
        }

        public void label1_Click(object sender, EventArgs e)
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

        //I made a change here and added String proID , which is a colum in both 
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
                // Make sure you are using the correct column name here for pID
                if (Convert.ToInt32(row.Cells["dgvpID"].Value) == product.id)
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
                int rowIndex = guna2DataGridView1.Rows.Add(new object[] {
            guna2DataGridView1.Rows.Count + 1, // Assuming first column is a row number which auto-increments
            product.id, // Make sure this is the pID from the products table
            product.PName,
            1, // Quantity
            product.PPrice,
            product.PPrice // Initial amount is just the price as quantity is 1
        });

                // Explicitly set the pID in case the above isn't working
                guna2DataGridView1.Rows[rowIndex].Cells["dgvpID"].Value = product.id;
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
            frmTableSelect tableSelectForm = new frmTableSelect();
            if (tableSelectForm.ShowDialog(this) == DialogResult.OK) // Ensures that the form was closed with OK
            {
                string selectedTable = tableSelectForm.SelectedTableName;
                if (!string.IsNullOrEmpty(selectedTable))
                {
                    lblTable.Text = selectedTable;
                    lblTable.Visible = true; // Make the label visible
                    OpenWaiterSelect(); // Opens the waiter selection form
                }
                else
                {
                    MessageBox.Show("No table was selected.");
                }
            }
        }



        public void OpenWaiterSelect()
        {
            using (frmWaiterSelect waiterSelectForm = new frmWaiterSelect())
            {
                if (waiterSelectForm.ShowDialog(this) == DialogResult.OK)
                {
                    lblWaiter.Text = waiterSelectForm.SelectedWaiterName;
                    lblWaiter.Visible = true;

                    if (waiterSelectForm.SelectedWaiterImage != null)
                    {
                        guna2CirclePictureBox1.Image = waiterSelectForm.SelectedWaiterImage;
                        guna2CirclePictureBox1.Visible = true;
                    }
                    
                }
            }
        }





        private void lblWaiter_Click(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            string tableName = lblTable.Text;
            string waiterName = lblWaiter.Text;
            string status = "Pending"; // Default status, can be adjusted based on your application logic
            string orderType = OrderType;

            // Remove the "Total: £" part to isolate the number and try parsing it
            string totalString = lblTotal.Text.Replace("Total: £", "").Trim();
            if (!float.TryParse(totalString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float total))
            {
                MessageBox.Show("Invalid total format.");
                return; // Exit if parsing fails
            }

            float received = 0.0f; // Placeholder, modify as necessary
            float change = 0.0f; // Placeholder, modify as necessary

            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                // Start transaction
                MySqlTransaction trans = con.BeginTransaction();
                cmd.Transaction = trans;

                try
                {
                    // Insert into tbMain
                    cmd.CommandText = "INSERT INTO `tbMain` (`aDate`, `aTime`, `TableName`, `WaiterName`, `Status`, `OrderType`, `Total`, `Received`, `Change`) VALUES (CURDATE(), CURTIME(), @TableName, @WaiterName, @Status, @OrderType, @Total, @Received, @Change);";

                    cmd.Parameters.AddWithValue("@TableName", tableName);
                    cmd.Parameters.AddWithValue("@WaiterName", waiterName);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@OrderType", orderType);
                    cmd.Parameters.AddWithValue("@Total", total);
                    cmd.Parameters.AddWithValue("@Received", received);
                    cmd.Parameters.AddWithValue("@Change", change);
                    cmd.ExecuteNonQuery();

                    // Get the last inserted id
                    long mainID = cmd.LastInsertedId;

                    // Insert into tblDetails
                    foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            int proID = Convert.ToInt32(row.Cells["dgvpID"].Value);
                            int qty = Convert.ToInt32(row.Cells["dgvQty"].Value);
                            float price = Convert.ToSingle(row.Cells["dgvPrice"].Value);
                            float amount = Convert.ToSingle(row.Cells["dgvAmount"].Value);

                            cmd.CommandText = "INSERT INTO tblDetails (MainID, pID, Qty, Price, Amount) VALUES (@MainID, @pID, @Qty, @Price, @Amount);";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@MainID", mainID);
                            cmd.Parameters.AddWithValue("@pID", proID);
                            cmd.Parameters.AddWithValue("@Qty", qty);
                            cmd.Parameters.AddWithValue("@Price", price);
                            cmd.Parameters.AddWithValue("@Amount", amount);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Commit transaction
                    trans.Commit();
                    MessageBox.Show("Order saved successfully!");
                }
                catch (Exception ex)
                {
                    // Something went wrong, rollback
                    trans.Rollback();
                    MessageBox.Show("Failed to save the order. Error: " + ex.Message);
                }
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            frmBillList billListForm = new frmBillList();  // Create an instance of frmBillList
            billListForm.ShowDialog(this);  // Open it as a modal dialog relative to the current form
        }


    }
}



