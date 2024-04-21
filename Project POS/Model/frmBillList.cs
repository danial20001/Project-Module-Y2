using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using System.Data;


namespace Project_POS.Model
{
    public partial class frmBillList : Form
    {
        public frmBillList()
        {
            InitializeComponent();
            LoadBillList();
            dgvBillList.CellContentClick += dgvBillList_CellContentClick;  // Ensure this is hooked up either here or in the designer
        }

        private frmPOS _existingPOSForm;

        public frmBillList(frmPOS existingPOSForm)
        {
            InitializeComponent();
            _existingPOSForm = existingPOSForm;
            LoadBillList();
        }




        private void LoadBillList()
        {
            // SQL query to fetch required fields including Total directly from tbMain
            // Filter to show only entries where Status is 'Pending'
            string qry = @"
SELECT 
    MainID, 
    TableName, 
    WaiterName, 
    OrderType, 
    Status, 
    Total  -- Assuming Total is also a column in tbMain
FROM tbMain
WHERE Status = 'Pending'  -- Filter to include only pending statuses
ORDER BY MainID DESC;";

            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(qry, con))
                {
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);

                    dgvBillList.Rows.Clear();

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No pending records found.");
                        return; // Exit the function if no records are found
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        int rowIndex = dgvBillList.Rows.Add();
                        dgvBillList.Rows[rowIndex].Cells["dgvMainID"].Value = row["MainID"];
                        dgvBillList.Rows[rowIndex].Cells["dgvTable"].Value = row["TableName"];
                        dgvBillList.Rows[rowIndex].Cells["dgvWaiter"].Value = row["WaiterName"];
                        dgvBillList.Rows[rowIndex].Cells["dgvOrderType"].Value = row["OrderType"];
                        dgvBillList.Rows[rowIndex].Cells["dgvStatus"].Value = row["Status"];
                        dgvBillList.Rows[rowIndex].Cells["dgvTotal"].Value = row["Total"]; // Ensure you have a dgvTotal column in your DataGridView
                    }
                }
            }
        }

        private void ShowPayForm(int mainID, decimal totalAmount)
        {
            Pay payForm = new Pay(mainID, totalAmount);
            payForm.UpdateEvent += PayForm_UpdateEvent;  // Subscribe to the event
            payForm.ShowDialog();
        }

        private void PayForm_UpdateEvent(object sender, EventArgs e)
        {
            LoadBillList();  // Refresh the DataGridView
        }

      


        private void dgvBillList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle the event when a row is double-clicked
            if (e.RowIndex >= 0)
            {
                int mainID = Convert.ToInt32(dgvBillList.Rows[e.RowIndex].Cells["dgvMainID"].Value);
                // You can call another form or method here and pass 'mainID' to load the specific order
            }
        }
        


        private void dgvBillList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvBillList.Columns[e.ColumnIndex].Name == "dgvedit")
            {
                int mainID = Convert.ToInt32(dgvBillList.Rows[e.RowIndex].Cells["dgvMainID"].Value);
                frmPOS posForm = new frmPOS(mainID);
                posForm.ShowDialog();  // This opens the frmPOS form for editing the selected record
            }
            if (e.RowIndex >= 0 && dgvBillList.Columns[e.ColumnIndex].Name == "dgvBill")
            {
                int mainID = Convert.ToInt32(dgvBillList.Rows[e.RowIndex].Cells["dgvMainID"].Value);
                decimal totalAmount = Convert.ToDecimal(dgvBillList.Rows[e.RowIndex].Cells["dgvTotal"].Value); // Make sure you have a column dgvTotal
                Pay payForm = new Pay(mainID, totalAmount);
                payForm.ShowDialog();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvBillList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
