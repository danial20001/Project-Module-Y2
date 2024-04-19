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
using Guna.UI2.WinForms;
using System.Net.NetworkInformation; // Make sure this namespace is included

namespace Project_POS.Model
{
    public partial class frmTableSelect : Form
    {
        public frmTableSelect()
        {
            InitializeComponent();
        }

        private void frmTableSelect_Load(object sender, EventArgs e)
        {
            LoadTables();
        }

        private void LoadTables()
        {
            string qry = "SELECT * FROM `tables`"; // Correct table name
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                flowLayoutPanel1.Controls.Clear();

                // Define color palette for buttons
                Color[] colors = new Color[]
                {
                    Color.FromArgb(246,221,225), // #ffcae0
                    Color.FromArgb(253,255,200), // #d7bfda
                    Color.FromArgb(216,255,220), // #bec7e3
                    Color.FromArgb(213,238,255), // #8ecdd9
                    Color.FromArgb(249,237,255) // #70c4c6
                };

                int index = 0;
                foreach (DataRow row in dt.Rows)
                {
                    Guna2Button btn = new Guna2Button
                    {
                        Text = row["tname"].ToString(),
                        Size = new Size(175, 52),
                        FillColor = colors[index % colors.Length],
                        ForeColor = Color.Black,
                        Font = new Font("Gill Sans Nova", 10, FontStyle.Regular),
                        Margin = new Padding(12),
                        BorderThickness = 0, // Removes the border
                        HoverState = { FillColor = Color.FromArgb(224, 224, 224) } // Changes color on hover
                    };

                    btn.Click += TableButton_Click;
                    flowLayoutPanel1.Controls.Add(btn);

                    index++;
                }
            }
        }


        public string SelectedTableName { get; private set; }

        private void TableButton_Click(object sender, EventArgs e)
        {
            Guna2Button btn = sender as Guna2Button;
            SelectedTableName = btn.Text;  // Assuming you have a property to hold the selected table name
            this.DialogResult = DialogResult.OK;  // Set the dialog result to OK
            this.Close();  // Close the form
        }




        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // This method can remain empty unless you need to perform custom painting.
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

      
    }
}
