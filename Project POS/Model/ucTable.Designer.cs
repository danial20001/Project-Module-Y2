namespace Project_POS.Model
{
    partial class ucTable
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label lblMainID;
            System.Windows.Forms.Label lblWaiter;
            System.Windows.Forms.Label lblStatus;
            this.lblTableName = new System.Windows.Forms.Label();
            lblMainID = new System.Windows.Forms.Label();
            lblWaiter = new System.Windows.Forms.Label();
            lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMainID
            // 
            lblMainID.AutoSize = true;
            lblMainID.Font = new System.Drawing.Font("Gill Sans MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblMainID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            lblMainID.Location = new System.Drawing.Point(137, 15);
            lblMainID.Name = "lblMainID";
            lblMainID.Size = new System.Drawing.Size(62, 23);
            lblMainID.TabIndex = 10;
            lblMainID.Text = "Main ID";
            lblMainID.Click += new System.EventHandler(this.lblMainID_Click);
            // 
            // lblTableName
            // 
            this.lblTableName.Font = new System.Drawing.Font("Gill Sans MT", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTableName.ForeColor = System.Drawing.Color.Black;
            this.lblTableName.Location = new System.Drawing.Point(12, 5);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.Size = new System.Drawing.Size(137, 38);
            this.lblTableName.TabIndex = 11;
            this.lblTableName.Text = "Table 5";
            this.lblTableName.Click += new System.EventHandler(this.lblTableName_Click);
            // 
            // lblWaiter
            // 
            lblWaiter.AutoSize = true;
            lblWaiter.Font = new System.Drawing.Font("Gill Sans MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblWaiter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            lblWaiter.Location = new System.Drawing.Point(15, 43);
            lblWaiter.Name = "lblWaiter";
            lblWaiter.Size = new System.Drawing.Size(57, 23);
            lblWaiter.TabIndex = 12;
            lblWaiter.Text = "Waiter";
            lblWaiter.Click += new System.EventHandler(this.lblWaiter_Click);
            // 
            // lblStatus
            // 
            lblStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            lblStatus.Font = new System.Drawing.Font("Gill Sans MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            lblStatus.Location = new System.Drawing.Point(132, 43);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(79, 32);
            lblStatus.TabIndex = 13;
            lblStatus.Text = "Main ID";
            lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // ucTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Controls.Add(lblStatus);
            this.Controls.Add(lblWaiter);
            this.Controls.Add(this.lblTableName);
            this.Controls.Add(lblMainID);
            this.Name = "ucTable";
            this.Size = new System.Drawing.Size(244, 84);
            this.Load += new System.EventHandler(this.ucTable_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTableName;
    }
}
