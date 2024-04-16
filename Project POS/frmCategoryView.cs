using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
    public partial class frmCategoryView : SampleView
    {
        public frmCategoryView()
        {
            InitializeComponent();
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            guna2DataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);
        }

        private void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Assuming the first column of DataGridView is for the serial number.
            // Update this index if the serial number column is at a different index.
            if (e.ColumnIndex == 0)
            {
                e.Value = e.RowIndex + 1;
            }
        }


        public void GetData()
        {
            string searchTerm = txtSearch.Text.Trim();
            string qry = string.IsNullOrEmpty(searchTerm) ?
                "SELECT * FROM category" :
                $"SELECT * FROM category WHERE catName LIKE '%{searchTerm}%' OR catID LIKE '%{searchTerm}%'";

            Dictionary<string, string> categoryMappings = new Dictionary<string, string>
    {
        {"dgvID", "catID"},
        {"dgvName", "catName"}
    };

            Database.LoadData(qry, guna2DataGridView1, categoryMappings);
        }



        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }






        private void frmCategoryView_Load(object sender, EventArgs e)
        {
            GetData();
        }

        public override void guna2ImageButton1_Click(object sender, EventArgs e)
        {

        }

        

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle editing operation
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                frmCategoryAdd frm = new frmCategoryAdd();
                frm.StartPosition = FormStartPosition.CenterParent; // Center the form relative to its parent
                                                                    // Here, ensure 'dgvID' is the actual name of the DataGridView column holding the ID.
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvID"].Value);
                frm.txtName.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                frm.ShowDialog();
                GetData();
            }

            // Handle deletion operation
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                DialogResult result = MessageBox.Show(this, "Are you sure you want to delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Here as well, use 'dgvID' if that's the DataGridView column for IDs.
                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvID"].Value);
                    string qry = "DELETE FROM category WHERE catID = @id";
                    Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@id", id }
            };

                    if (Database.ExecuteSQL(qry, parameters) > 0)
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


        private void btnAdd_Click(object sender, EventArgs e)
        {
           
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            frmCategoryAdd frm = new frmCategoryAdd();
            frm.ShowDialog();
            GetData();
        }

       



    }
}
