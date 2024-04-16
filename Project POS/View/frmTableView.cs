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
    public partial class frmTableView : SampleView
    {
        public frmTableView()
        {
            InitializeComponent();
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            guna2DataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);
        }



        private void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Value = e.RowIndex + 1;
            }
        }


        public void GetData()
        {
            string searchTerm = txtSearch.Text.Trim();
            string qry = string.IsNullOrEmpty(searchTerm) ?
                "SELECT * FROM `tables`" :
                $"SELECT * FROM `tables` WHERE tname LIKE '%{searchTerm}%' OR tid LIKE '%{searchTerm}%'";

            Dictionary<string, string> tableMappings = new Dictionary<string, string>
    {
        {"dgvID", "tid"},
        {"dgvName", "tname"}
    };

            Database.LoadData(qry, guna2DataGridView1, tableMappings);
        }
        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void frmTableView_Load(object sender, EventArgs e)
        {
            GetData();
        }




        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                frmTableAdd frm = new frmTableAdd();
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvID"].Value);
                frm.txtName.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    GetData();
                }
            }
            else if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvID"].Value);
                    string qry = "DELETE FROM tables WHERE tid = @id";
                    Dictionary<string, object> parameters = new Dictionary<string, object> { { "@id", id } };

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




        private void btnAdd_Click_2(object sender, EventArgs e)
        {
            frmTableAdd frm = new frmTableAdd();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                GetData();
            }
        }
    }
}
