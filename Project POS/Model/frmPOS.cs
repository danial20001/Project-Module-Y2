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
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {

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
            // Convert price string to double
            double productPrice = double.Parse(price);

            var w = new ucProduct()
            {
                PName = name,
                PPrice = productPrice, // Assign converted price
                PCategory = cat,
                id = Convert.ToInt32(id)
            };

            ProductPanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;
                bool found = false;
                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    // Check if product already exists
                    if (Convert.ToInt32(item.Cells["dgvid"].Value) == wdg.id)
                    {
                        // Update quantity and amount
                        item.Cells["dgvQty"].Value = Convert.ToInt32(item.Cells["dgvQty"].Value) + 1;
                        item.Cells["dgvAmount"].Value = Convert.ToDouble(item.Cells["dgvQty"].Value) * Convert.ToDouble(item.Cells["dgvPrice"].Value);
                        found = true;
                        break;
                    }
                }

                // If product not found, add a new row
                if (!found)
                {
                    guna2DataGridView1.Rows.Add(new object[] { 0, wdg.id, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                }
            };
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

                    AddItems(item["pID"].ToString(),item["pName"].ToString(), item["catName"].ToString(),
                        item["pPrice"].ToString() );


                }


            }



        }



            private void CategoryPanel_Paint(object sender, PaintEventArgs e)
            {

            }

            private void CategoryButton_Click(object sender, EventArgs e)
            {
                // Handle category button click
                var button = sender as Guna.UI2.WinForms.Guna2Button;
                MessageBox.Show("You clicked: " + button.Text);
            }

            private void guna2Panel3_Paint(object sender, PaintEventArgs e)
            {

            }
        }
    }
