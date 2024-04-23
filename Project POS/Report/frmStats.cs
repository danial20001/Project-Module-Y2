using MySql.Data.MySqlClient;
using Project_POS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_POS.Report
{
    public partial class frmStats : Form
    {
        public frmStats()
        {
            InitializeComponent();
            PopulateComboBox(); // Ensure the combo box is populated first
            guna2ComboBox1.SelectedIndex = 0; // Optionally set a default selection
            UpdateStatistics(); // Initialize with the current selection
            LoadTopSoldItems(); // Load the data for top sold items immediately
        }

        private void PopulateComboBox()
        {
            guna2ComboBox1.Items.Add("Today");
            guna2ComboBox1.Items.Add("Yesterday");
            guna2ComboBox1.Items.Add("Last 7 Days");
            guna2ComboBox1.Items.Add("Last 30 Days");
            guna2ComboBox1.Items.Add("Last 6 Months");
            guna2ComboBox1.Items.Add("Last 12 Months");
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            if (guna2ComboBox1.SelectedItem == null) return;

            string selectedRange = guna2ComboBox1.SelectedItem.ToString();
            DateTime endDate = DateTime.Today;
            DateTime startDate = endDate;

            switch (selectedRange)
            {
                case "Today":
                    startDate = endDate;
                    break;
                case "Yesterday":
                    startDate = endDate.AddDays(-1);
                    break;
                case "Last 7 Days":
                    startDate = endDate.AddDays(-6);
                    break;
                case "Last 30 Days":
                    startDate = endDate.AddMonths(-1);
                    break;
                case "Last 6 Months":
                    startDate = endDate.AddMonths(-6);
                    break;
                case "Last 12 Months":
                    startDate = endDate.AddYears(-1);
                    break;
                default:
                    break;
            }

            lblRevenue.Text = $"{FetchTotalRevenue(startDate, endDate):C}";
            label1.Text = $"{FetchTotalOrders(startDate, endDate)}";
            label2.Text = $"{FetchTotalDishes(startDate, endDate)}";
            label3.Text = $"{FetchTotalOrdersCount(startDate, endDate)}";
        }


        private decimal FetchTotalRevenue(DateTime startDate, DateTime endDate)
        {
            string qry = "SELECT SUM(Total) FROM tbMain WHERE aDate BETWEEN @StartDate AND @EndDate";
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        private int FetchTotalOrders(DateTime startDate, DateTime endDate)
        {
            string qry = "SELECT COUNT(*) FROM tbMain WHERE aDate BETWEEN @StartDate AND @EndDate";
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();
                return Convert.ToInt32(result);
            }
        }

        private int FetchTotalDishes(DateTime startDate, DateTime endDate)
        {
            // Query to count dishes based on date range using JOIN
            string qry = @"
        SELECT COUNT(*) 
        FROM tblDetails 
        INNER JOIN tbMain ON tblDetails.MainID = tbMain.MainID
        WHERE tbMain.aDate BETWEEN @StartDate AND @EndDate";
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }

        private int FetchTotalOrdersCount(DateTime startDate, DateTime endDate)
        {
            string qry = "SELECT COUNT(*) FROM tbMain WHERE aDate BETWEEN @StartDate AND @EndDate";
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();
                return Convert.ToInt32(result);
            }
        }





        private void lblRevenue_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmStats_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        


        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            }

        public void LoadTopSoldItems()
        {
            string qry = @"
    SELECT p.pName AS dgvItemName, SUM(d.Qty) AS dgvSold
    FROM tblDetails AS d
    INNER JOIN products AS p ON d.pID = p.pID
    GROUP BY d.pID, p.pName
    ORDER BY SUM(d.Qty) DESC
    LIMIT 10"; // Adjust the limit as needed

            Dictionary<string, string> columnMappings = new Dictionary<string, string>
{
    {"dgvItemName", "dgvItemName"},
    {"dgvSold", "dgvSold"}
};

            // This assumes you have a generalized method to handle the loading of data into a DataGridView, which might look something like this:
            LoadData(qry, guna2DataGridView1, columnMappings);
        }

        // Example of a generalized LoadData method
        private void LoadData(string query, DataGridView dgv, Dictionary<string, string> mappings)
        {
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgv.DataSource = dt;
                dgv.AutoGenerateColumns = false; // Ensure this is set to false to use predefined columns

                // Assuming columns in DataGridView have been set up with DataPropertyName attributes matching keys from mappings
                foreach (KeyValuePair<string, string> map in mappings)
                {
                    dgv.Columns[map.Key].DataPropertyName = map.Value;
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmReport posForm = new frmReport(); // Create an instance of frmPos
            posForm.Show(); // Display the form
        }

    }

}

