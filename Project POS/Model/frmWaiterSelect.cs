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

        public string SelectedWaiterName { get; private set; }

        private void ucWaiter_Click(object sender, EventArgs e)
        {
            ucWaiter selectedWaiter = sender as ucWaiter;
            if (selectedWaiter != null)
            {
                SelectedWaiterName = selectedWaiter.WaiterName;
                SelectedWaiterImage = selectedWaiter.WaiterImage; // Make sure ucWaiter has a public property for the image
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        private void frmWaiterSelect_Load(object sender, EventArgs e)
        {
            LoadWaiters();
        }

        private void LoadWaiters()
        {
            string qry = "SELECT staffID, sName, imagePath FROM staff";
            using (MySqlConnection con = new MySqlConnection(Database.ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(qry, con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                WaiterPanel.Controls.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    ucWaiter waiter = new ucWaiter
                    {
                        WaiterId = Convert.ToInt32(row["staffID"]),
                        WaiterName = row["sName"].ToString(),
                        WaiterImage = Image.FromFile(row["imagePath"].ToString())
                    };
                    waiter.WaiterSelected += Waiter_Selected;
                    WaiterPanel.Controls.Add(waiter);
                }
            }
        }

        private void Waiter_Selected(object sender, EventArgs e)
        {
            ucWaiter selectedWaiter = sender as ucWaiter;
            if (selectedWaiter != null)
            {
                SelectedWaiterName = selectedWaiter.WaiterName;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public Image SelectedWaiterImage { get; private set; }




        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmWaiterSelect_Load_1(object sender, EventArgs e)
        {
            LoadWaiters();
        }
    }


}

