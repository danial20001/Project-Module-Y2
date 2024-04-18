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
    public partial class frmWaiterSelect : Form
    {
        public frmWaiterSelect()
        {
            InitializeComponent();
        }

        private void WaiterPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmWaiterSelect_Load(object sender, EventArgs e)
        {
            LoadWaiters();
        }

        private void LoadWaiters()
        {
            string qry = "SELECT staffID, sName, imagePath FROM staff";  // Corrected table and column names
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                try
                {
                    con.Open();  // Explicitly open the connection
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No staff found.");
                        return;
                    }

                    WaiterPanel.Controls.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            ucWaiter waiter = new ucWaiter
                            {
                                WaiterId = Convert.ToInt32(row["staffID"]),
                                WaiterName = row["sName"].ToString(),
                                WaiterImage = Image.FromFile(row["imagePath"].ToString()) // Make sure the image path is accessible
                            };
                            WaiterPanel.Controls.Add(waiter);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error loading image or adding waiter to panel: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database connection or query error: {ex.Message}");
                }
            }
        }




        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmWaiterSelect_Load_1(object sender, EventArgs e)
        {
            LoadWaiters();
        }
    }


}

