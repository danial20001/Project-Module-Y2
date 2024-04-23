using Guna.Charts.WinForms;
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

namespace Project_POS.Report
{


   

    public partial class frmReport : Form
    {
        

        private GunaChart gunaChart;
        
        private GunaBarDataset gunaBarDataset;
        private ComboBox cmbTimePeriod;

        public frmReport()
        {
            InitializeComponent();
            InitializeGunaChart();
            InitializeComboBox();

        }

        private void InitializeGunaChart()
        {
            gunaChart = new GunaChart();

            // Adjusting size and location relative to the form's size
            this.ClientSize = new Size(1920, 1080); // Example form size, adjust as needed
            int chartWidth = (int)(this.ClientSize.Width * 0.8); // 80% of the form's width
            int chartHeight = (int)(this.ClientSize.Height * 0.8); // 80% of the form's height
            gunaChart.Size = new Size(chartWidth, chartHeight);
            gunaChart.Location = new Point(
                (this.ClientSize.Width - chartWidth) / 2,
                (this.ClientSize.Height - chartHeight) / 2
            );

            // Setup datasets and appearance
           
            gunaBarDataset = new GunaBarDataset
            {
                Label = "Daily Sales",
                LegendBoxFillColor = Color.MediumSlateBlue,
                YFormat = "{0:C}"
            };

            
            gunaChart.Datasets.Add(gunaBarDataset);

            this.Controls.Add(gunaChart); // This sets the chart's Parent property implicitly
        }

        private void InitializeComboBox()
        {
            cmbTimePeriod = new ComboBox();
            cmbTimePeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTimePeriod.Items.Add("Last 7 days");
            cmbTimePeriod.Items.Add("Last month");
            cmbTimePeriod.Items.Add("Last 6 months");
            cmbTimePeriod.SelectedIndex = 0; // Default selection
            cmbTimePeriod.Location = new Point(10, 10); // Adjust location as needed
            cmbTimePeriod.Width = 200; // Adjust width as needed
            cmbTimePeriod.SelectedIndexChanged += CmbTimePeriod_SelectedIndexChanged;
            this.Controls.Add(cmbTimePeriod);
        }

        private void CmbTimePeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selection = cmbTimePeriod.SelectedItem.ToString();
            LoadSalesData(selection.ToLower());
        }





        private void LoadSalesData(string timePeriod)
        {
            string query = @"
SELECT DATE(aDate) AS SaleDate, SUM(Total) AS DailyTotal
FROM tbMain
WHERE aDate >= CURDATE() - INTERVAL ";

            // Adjusting the query based on time period selection
            switch (timePeriod)
            {
                case "last 7 days":
                    query += "7 DAY ";
                    break;
                case "last month":
                    query += "1 MONTH ";
                    break;
                case "last 6 months":
                    query += "6 MONTH ";
                    break;
            }

            query += "GROUP BY DATE(aDate) ORDER BY DATE(aDate);";

            gunaBarDataset.DataPoints.Clear(); // Clear previous data points

            using (var connection = Database.Connect()) // Assuming Database.Connect() is a method returning a MySqlConnection
            {
                if (connection == null)
                {
                    MessageBox.Show("Unable to connect to the database.");
                    return;
                }

                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var saleDate = reader.GetDateTime("SaleDate");
                            var dailyTotal = reader.GetDouble("DailyTotal");

                            // Adding data points for each day to the bar dataset
                            gunaBarDataset.DataPoints.Add(new LPoint()
                            {
                                Label = saleDate.ToString("MMM dd"),
                                Y = dailyTotal
                            });
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Failed to load sales data: " + ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close(); // Ensure the connection is closed after use
                }
            }
        }



        private void frmReport_Load(object sender, EventArgs e)
        {

        }
    }
}
