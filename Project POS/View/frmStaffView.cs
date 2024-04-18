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

namespace Project_POS.View
{
    public partial class frmStaffView : SampleView
    {
        public frmStaffView()
        {
            InitializeComponent();
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            guna2DataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);
        }

        private void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Assuming the first column of DataGridView is for the serial number.
            if (e.ColumnIndex == 0)
            {
                e.Value = e.RowIndex + 1;
            }
        }


        public void GetData()
        {
            string searchTerm = txtSearch.Text.Trim();
            string qry = string.IsNullOrEmpty(searchTerm) ?
                "SELECT * FROM staff" :
                $"SELECT * FROM staff WHERE sName LIKE '%{searchTerm}%' OR sPhone LIKE '%{searchTerm}%' OR sRole LIKE '%{searchTerm}%'";

            Dictionary<string, string> staffMappings = new Dictionary<string, string>
            {
                {"dgvID", "staffID"},
                {"dgvName", "sName"},
                {"dgvPhone", "sPhone"},
                {"dgvRole", "sRole"}
            };

            Database.LoadData(qry, guna2DataGridView1, staffMappings);
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void frmStaffView_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvedit")
            {
                int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvID"].Value);
                // Define the query to fetch all necessary details including the image path
                string qry = "SELECT sName, sPhone, sRole, imagePath FROM staff WHERE staffID = @id";

                using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(qry, con);
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            frmStaffAdd frm = new frmStaffAdd();
                            frm.StartPosition = FormStartPosition.CenterParent;
                            frm.id = id;
                            frm.txtName.Text = row["sName"].ToString();
                            frm.txtPhone.Text = row["sPhone"].ToString();
                            frm.txtRole.Text = row["sRole"].ToString();
                            frm.SetImagePath(row["imagePath"].ToString());  // Assuming SetImagePath is a method in frmStaffAdd
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                GetData();
                            }
                        }
                    }
                }
            }
            else if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvdel")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this staff member?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvID"].Value);
                    string qry = "DELETE FROM staff WHERE staffID = @id";
                    using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand(qry, con);
                        cmd.Parameters.AddWithValue("@id", id);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Deleted successfully");
                        }
                        else
                        {
                            MessageBox.Show("Deletion failed");
                        }
                        GetData();
                    }
                }
            }
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmStaffAdd frm = new frmStaffAdd();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                GetData();
            }
        }
    }
 }

