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
