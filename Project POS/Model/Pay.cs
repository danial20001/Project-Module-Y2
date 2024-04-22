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
    public partial class Pay : Form
    {
        private int _mainId;
        private decimal _totalAmount;

        public Pay(int mainId, decimal totalAmount)
        {
            InitializeComponent();
            _mainId = mainId;
            _totalAmount = totalAmount;
            lblTotal.Text = $"Total: ${totalAmount:F2}";
        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amountPaid))
            {
                decimal change = amountPaid - _totalAmount;
                lblChange.Text = $"Change: ${change:F2}";
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            decimal amountPaid;
            if (decimal.TryParse(txtAmount.Text, out amountPaid))
            {
                UpdateDatabase(amountPaid);
                UpdateEvent?.Invoke(this, EventArgs.Empty);  // Raise the event
                this.Close();
            }
        }

        public event EventHandler UpdateEvent;

        private void UpdateDatabase(decimal amountPaid)
        {
            // Prepare the SQL query using SQL comment style and backticks around `Change`
            string query = @"
        UPDATE tbMain 
        SET 
            Received = @Received, 
            `Change` = @Change,  -- Enclosed `Change` in backticks
            Status = 'Completed' 
        WHERE MainID = @MainID;";

            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    decimal change = amountPaid - _totalAmount; // Calculate the change

                    // Add parameters to the command to prevent SQL injection
                    cmd.Parameters.AddWithValue("@Received", amountPaid);
                    cmd.Parameters.AddWithValue("@Change", change);
                    cmd.Parameters.AddWithValue("@MainID", _mainId);

                    cmd.ExecuteNonQuery();  // Execute the update command
                }
            }
        }



        private void lblChange_Click(object sender, EventArgs e)
        {

        }

        private void Pay_Load(object sender, EventArgs e)
        {

        }
    }
}



