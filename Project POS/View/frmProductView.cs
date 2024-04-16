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
    public partial class frmProductView : SampleView
    {
        public frmProductView()
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
                "SELECT p.pID, p.pName, p.pPrice, c.catName, p.CategoryID FROM products p INNER JOIN category c ON p.CategoryID = c.catID" :
                $"SELECT p.pID, p.pName, p.pPrice, c.catName, p.CategoryID FROM products p INNER JOIN category c ON p.CategoryID = c.catID WHERE p.pName LIKE '%{searchTerm}%' OR p.pPrice LIKE '%{searchTerm}%' OR c.catName LIKE '%{searchTerm}%'";

            var columnMappings = new Dictionary<string, string>
    {
        {"dgvID", "pID"},
        {"dgvName", "pName"},
        {"dgvPrice", "pPrice"},
        {"dgvCat", "catName"},
        {"dgvCatID", "CategoryID"}
    };

            // Your existing Database.LoadData method might need to adjust to use these mappings correctly.
            Database.LoadData(qry, guna2DataGridView1, columnMappings);
        }




        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void frmProductView_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvedit")
            {
                frmProductAdd frm = new frmProductAdd();
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.productId = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvID"].Value);
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.OK)
                {
                    GetData(); // Refresh the data grid to show updated data
                }
            }
            else if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvdel")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this product?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int productId = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvID"].Value);
                    string qry = "DELETE FROM products WHERE pID = @ProductID;";
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        {"@ProductID", productId}
                    };

                    if (Database.ExecuteSQL(qry, parameters) > 0)
                    {
                        MessageBox.Show("Product deleted successfully.");
                        GetData(); // Refresh the data grid
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the product. Please check the data.");
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmProductAdd frm = new frmProductAdd();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                GetData();
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            frmProductAdd frm = new frmProductAdd();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                GetData();
            }
        }
    }
}
