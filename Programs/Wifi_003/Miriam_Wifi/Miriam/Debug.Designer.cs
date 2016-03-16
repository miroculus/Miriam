namespace Miriam
{
    partial class Debug
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
            this.Read = new System.Windows.Forms.Button();
            this.info = new System.Windows.Forms.Button();
            this.SetHeat = new System.Windows.Forms.Button();
            this.SetU = new System.Windows.Forms.Button();
            this.SetL = new System.Windows.Forms.Button();
            this.temp = new System.Windows.Forms.TextBox();
            this.Data = new System.Windows.Forms.ListBox();
            this.Heat = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Read
            // 
            this.Read.Location = new System.Drawing.Point(38, 45);
            this.Read.Name = "Read";
            this.Read.Size = new System.Drawing.Size(214, 93);
            this.Read.TabIndex = 0;
            this.Read.Text = "Read";
            this.Read.UseVisualStyleBackColor = true;
            this.Read.Click += new System.EventHandler(this.Read_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(258, 45);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(213, 93);
            this.info.TabIndex = 1;
            this.info.Text = "info";
            this.info.UseVisualStyleBackColor = true;
            this.info.Click += new System.EventHandler(this.info_Click);
            // 
            // SetHeat
            // 
            this.SetHeat.Location = new System.Drawing.Point(477, 45);
            this.SetHeat.Name = "SetHeat";
            this.SetHeat.Size = new System.Drawing.Size(213, 93);
            this.SetHeat.TabIndex = 2;
            this.SetHeat.Text = "Set heat";
            this.SetHeat.UseVisualStyleBackColor = true;
            this.SetHeat.Click += new System.EventHandler(this.SetHeat_Click);
            // 
            // SetU
            // 
            this.SetU.Location = new System.Drawing.Point(38, 144);
            this.SetU.Name = "SetU";
            this.SetU.Size = new System.Drawing.Size(214, 93);
            this.SetU.TabIndex = 3;
            this.SetU.Text = "Set upper temperature";
            this.SetU.UseVisualStyleBackColor = true;
            this.SetU.Click += new System.EventHandler(this.SetU_Click);
            // 
            // SetL
            // 
            this.SetL.Location = new System.Drawing.Point(258, 144);
            this.SetL.Name = "SetL";
            this.SetL.Size = new System.Drawing.Size(214, 93);
            this.SetL.TabIndex = 4;
            this.SetL.Text = "Set lower temperature";
            this.SetL.UseVisualStyleBackColor = true;
            this.SetL.Click += new System.EventHandler(this.SetL_Click);
            // 
            // temp
            // 
            this.temp.Location = new System.Drawing.Point(477, 177);
            this.temp.Name = "temp";
            this.temp.Size = new System.Drawing.Size(213, 26);
            this.temp.TabIndex = 5;
            // 
            // Data
            // 
            this.Data.FormattingEnabled = true;
            this.Data.ItemHeight = 20;
            this.Data.Location = new System.Drawing.Point(38, 359);
            this.Data.Name = "Data";
            this.Data.Size = new System.Drawing.Size(661, 244);
            this.Data.TabIndex = 6;
            // 
            // Heat
            // 
            this.Heat.Location = new System.Drawing.Point(38, 243);
            this.Heat.Name = "Heat";
            this.Heat.Size = new System.Drawing.Size(213, 93);
            this.Heat.TabIndex = 7;
            this.Heat.Text = "Heat";
            this.Heat.UseVisualStyleBackColor = true;
            this.Heat.Click += new System.EventHandler(this.Heat_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(258, 243);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(213, 93);
            this.Cancel.TabIndex = 8;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Debug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 647);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Heat);
            this.Controls.Add(this.Data);
            this.Controls.Add(this.temp);
            this.Controls.Add(this.SetL);
            this.Controls.Add(this.SetU);
            this.Controls.Add(this.SetHeat);
            this.Controls.Add(this.info);
            this.Controls.Add(this.Read);
            this.Name = "Debug";
            this.Text = "Debug";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Read;
        private System.Windows.Forms.Button info;
        private System.Windows.Forms.Button SetHeat;
        private System.Windows.Forms.Button SetU;
        private System.Windows.Forms.Button SetL;
        private System.Windows.Forms.TextBox temp;
        private System.Windows.Forms.ListBox Data;
        private System.Windows.Forms.Button Heat;
        private System.Windows.Forms.Button Cancel;
    }
}