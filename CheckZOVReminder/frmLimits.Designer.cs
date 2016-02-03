namespace CheckZOVReminder
{
    partial class frmLimits
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvLimits = new System.Windows.Forms.ListView();
            this.chFrom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chQuantity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panBottom = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvLimits
            // 
            this.lvLimits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFrom,
            this.chTo,
            this.chQuantity});
            this.lvLimits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLimits.HideSelection = false;
            this.lvLimits.Location = new System.Drawing.Point(0, 0);
            this.lvLimits.Name = "lvLimits";
            this.lvLimits.Size = new System.Drawing.Size(564, 519);
            this.lvLimits.TabIndex = 0;
            this.lvLimits.UseCompatibleStateImageBehavior = false;
            this.lvLimits.View = System.Windows.Forms.View.Details;
            // 
            // chFrom
            // 
            this.chFrom.Text = "From";
            this.chFrom.Width = 160;
            // 
            // chTo
            // 
            this.chTo.Text = "To";
            this.chTo.Width = 160;
            // 
            // chQuantity
            // 
            this.chQuantity.Text = "Quantity";
            this.chQuantity.Width = 40;
            // 
            // panBottom
            // 
            this.panBottom.Controls.Add(this.btnRefresh);
            this.panBottom.Controls.Add(this.btnOk);
            this.panBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panBottom.Location = new System.Drawing.Point(0, 479);
            this.panBottom.Name = "panBottom";
            this.panBottom.Size = new System.Drawing.Size(564, 40);
            this.panBottom.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(13, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(478, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmLimits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 519);
            this.Controls.Add(this.panBottom);
            this.Controls.Add(this.lvLimits);
            this.Name = "frmLimits";
            this.Text = "Лимиты";
            this.Load += new System.EventHandler(this.frmLimits_Load);
            this.panBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvLimits;
        private System.Windows.Forms.Panel panBottom;
        private System.Windows.Forms.ColumnHeader chFrom;
        private System.Windows.Forms.ColumnHeader chTo;
        private System.Windows.Forms.ColumnHeader chQuantity;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnRefresh;
    }
}