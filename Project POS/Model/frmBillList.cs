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
        }

        private void LoadBillList()
        {
            string qry = "SELECT MainID, TableName, WaiterName, OrderType, Status FROM tbMain ORDER BY MainID DESC;";
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(qry, con))
                {
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);

                    dgvBillList.Rows.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        int rowIndex = dgvBillList.Rows.Add();
                        dgvBillList.Rows[rowIndex].Cells["dgvMainID"].Value = row["MainID"];
                        dgvBillList.Rows[rowIndex].Cells["dgvTable"].Value = row["TableName"];
                        dgvBillList.Rows[rowIndex].Cells["dgvWaiter"].Value = row["WaiterName"];
                        dgvBillList.Rows[rowIndex].Cells["dgvOrderType"].Value = row["OrderType"];
                        dgvBillList.Rows[rowIndex].Cells["dgvStatus"].Value = row["Status"];
                    }
                }
            }
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

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // This can remain empty unless you have specific logic for when a cell is clicked
        }
    }
}
