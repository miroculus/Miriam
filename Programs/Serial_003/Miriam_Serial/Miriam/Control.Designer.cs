using System;
using System.Windows.Forms;

namespace Miriam
{
    partial class Control
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Control));
            this.ButtonStart = new System.Windows.Forms.Button();
            this.LabelTempU = new System.Windows.Forms.Label();
            this.LabelTempM = new System.Windows.Forms.Label();
            this.LabelDuration = new System.Windows.Forms.Label();
            this.CboxTempU = new System.Windows.Forms.ComboBox();
            this.CboxTempM = new System.Windows.Forms.ComboBox();
            this.CboxDuration = new System.Windows.Forms.ComboBox();
            this.Plate = new System.Windows.Forms.DataGridView();
            this.LabelTempUC = new System.Windows.Forms.Label();
            this.LabelTempMC = new System.Windows.Forms.Label();
            this.ButtonHeat = new System.Windows.Forms.Button();
            this.AskHeat = new System.Windows.Forms.Button();
            this.Data = new System.Windows.Forms.ListBox();
            this.Results = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ButtonWrite = new System.Windows.Forms.Button();
            this.COM_label = new System.Windows.Forms.Label();
            this.COM = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.Plate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Results)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(512, 9);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(113, 98);
            this.ButtonStart.TabIndex = 12;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // LabelTempU
            // 
            this.LabelTempU.AutoSize = true;
            this.LabelTempU.Location = new System.Drawing.Point(135, 56);
            this.LabelTempU.Name = "LabelTempU";
            this.LabelTempU.Size = new System.Drawing.Size(93, 20);
            this.LabelTempU.TabIndex = 18;
            this.LabelTempU.Text = "Upper temp";
            // 
            // LabelTempM
            // 
            this.LabelTempM.AutoSize = true;
            this.LabelTempM.Location = new System.Drawing.Point(8, 56);
            this.LabelTempM.Name = "LabelTempM";
            this.LabelTempM.Size = new System.Drawing.Size(95, 20);
            this.LabelTempM.TabIndex = 17;
            this.LabelTempM.Text = "Middle temp";
            // 
            // LabelDuration
            // 
            this.LabelDuration.AutoSize = true;
            this.LabelDuration.Location = new System.Drawing.Point(266, 56);
            this.LabelDuration.Name = "LabelDuration";
            this.LabelDuration.Size = new System.Drawing.Size(70, 20);
            this.LabelDuration.TabIndex = 16;
            this.LabelDuration.Text = "Duration";
            // 
            // CboxTempU
            // 
            this.CboxTempU.FormattingEnabled = true;
            this.CboxTempU.Location = new System.Drawing.Point(139, 79);
            this.CboxTempU.Name = "CboxTempU";
            this.CboxTempU.Size = new System.Drawing.Size(121, 28);
            this.CboxTempU.TabIndex = 15;
            // 
            // CboxTempM
            // 
            this.CboxTempM.FormattingEnabled = true;
            this.CboxTempM.Location = new System.Drawing.Point(12, 79);
            this.CboxTempM.Name = "CboxTempM";
            this.CboxTempM.Size = new System.Drawing.Size(121, 28);
            this.CboxTempM.TabIndex = 14;
            // 
            // CboxDuration
            // 
            this.CboxDuration.FormattingEnabled = true;
            this.CboxDuration.Location = new System.Drawing.Point(266, 79);
            this.CboxDuration.Name = "CboxDuration";
            this.CboxDuration.Size = new System.Drawing.Size(121, 28);
            this.CboxDuration.TabIndex = 13;
            // 
            // Plate
            // 
            this.Plate.AllowDrop = true;
            this.Plate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Plate.Location = new System.Drawing.Point(12, 113);
            this.Plate.Name = "Plate";
            this.Plate.RowTemplate.Height = 28;
            this.Plate.Size = new System.Drawing.Size(805, 392);
            this.Plate.TabIndex = 19;
            // 
            // LabelTempUC
            // 
            this.LabelTempUC.AutoSize = true;
            this.LabelTempUC.Location = new System.Drawing.Point(8, 9);
            this.LabelTempUC.Name = "LabelTempUC";
            this.LabelTempUC.Size = new System.Drawing.Size(120, 20);
            this.LabelTempUC.TabIndex = 20;
            this.LabelTempUC.Text = "Temperature U:";
            // 
            // LabelTempMC
            // 
            this.LabelTempMC.AutoSize = true;
            this.LabelTempMC.Location = new System.Drawing.Point(176, 9);
            this.LabelTempMC.Name = "LabelTempMC";
            this.LabelTempMC.Size = new System.Drawing.Size(121, 20);
            this.LabelTempMC.TabIndex = 21;
            this.LabelTempMC.Text = "Temperature M:";
            // 
            // ButtonHeat
            // 
            this.ButtonHeat.Location = new System.Drawing.Point(393, 9);
            this.ButtonHeat.Name = "ButtonHeat";
            this.ButtonHeat.Size = new System.Drawing.Size(113, 46);
            this.ButtonHeat.TabIndex = 22;
            this.ButtonHeat.Text = "Heat";
            this.ButtonHeat.UseVisualStyleBackColor = true;
            this.ButtonHeat.Click += new System.EventHandler(this.ButtonHeat_Click);
            // 
            // AskHeat
            // 
            this.AskHeat.Location = new System.Drawing.Point(393, 61);
            this.AskHeat.Name = "AskHeat";
            this.AskHeat.Size = new System.Drawing.Size(113, 46);
            this.AskHeat.TabIndex = 23;
            this.AskHeat.Text = "Ask heat";
            this.AskHeat.UseVisualStyleBackColor = true;
            this.AskHeat.Click += new System.EventHandler(this.AskHeat_Click);
            // 
            // Data
            // 
            this.Data.FormattingEnabled = true;
            this.Data.ItemHeight = 20;
            this.Data.Location = new System.Drawing.Point(393, 225);
            this.Data.Name = "Data";
            this.Data.Size = new System.Drawing.Size(61, 84);
            this.Data.TabIndex = 24;
            this.Data.Visible = false;
            // 
            // Results
            // 
            chartArea1.Name = "ChartArea1";
            this.Results.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.Results.Legends.Add(legend1);
            this.Results.Location = new System.Drawing.Point(12, 511);
            this.Results.Name = "Results";
            this.Results.Size = new System.Drawing.Size(805, 484);
            this.Results.TabIndex = 25;
            this.Results.Text = "chart1";
            this.Results.Visible = false;
            // 
            // ButtonWrite
            // 
            this.ButtonWrite.Location = new System.Drawing.Point(631, 9);
            this.ButtonWrite.Name = "ButtonWrite";
            this.ButtonWrite.Size = new System.Drawing.Size(113, 46);
            this.ButtonWrite.TabIndex = 26;
            this.ButtonWrite.Text = "Write CSV";
            this.ButtonWrite.UseVisualStyleBackColor = true;
            this.ButtonWrite.Click += new System.EventHandler(this.ButtonWrite_Click);
            // 
            // COM_label
            // 
            this.COM_label.AutoSize = true;
            this.COM_label.Location = new System.Drawing.Point(627, 56);
            this.COM_label.Name = "COM_label";
            this.COM_label.Size = new System.Drawing.Size(77, 20);
            this.COM_label.TabIndex = 28;
            this.COM_label.Text = "COM port";
            // 
            // COM
            // 
            this.COM.FormattingEnabled = true;
            this.COM.Location = new System.Drawing.Point(631, 79);
            this.COM.Name = "COM";
            this.COM.Size = new System.Drawing.Size(121, 28);
            this.COM.TabIndex = 29;
            // 
            // Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(840, 519);
            this.Controls.Add(this.COM);
            this.Controls.Add(this.COM_label);
            this.Controls.Add(this.ButtonWrite);
            this.Controls.Add(this.Results);
            this.Controls.Add(this.Data);
            this.Controls.Add(this.AskHeat);
            this.Controls.Add(this.ButtonHeat);
            this.Controls.Add(this.LabelTempMC);
            this.Controls.Add(this.LabelTempUC);
            this.Controls.Add(this.Plate);
            this.Controls.Add(this.LabelTempU);
            this.Controls.Add(this.LabelTempM);
            this.Controls.Add(this.LabelDuration);
            this.Controls.Add(this.CboxTempU);
            this.Controls.Add(this.CboxTempM);
            this.Controls.Add(this.CboxDuration);
            this.Controls.Add(this.ButtonStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Control";
            this.Text = "Control_Serial";
            ((System.ComponentModel.ISupportInitialize)(this.Plate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Results)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Label LabelTempU;
        private System.Windows.Forms.Label LabelTempM;
        private System.Windows.Forms.Label LabelDuration;
        private System.Windows.Forms.ComboBox CboxTempU;
        private System.Windows.Forms.ComboBox CboxTempM;
        private System.Windows.Forms.ComboBox CboxDuration;
        private System.Windows.Forms.DataGridView Plate;
        private Label LabelTempUC;
        private Label LabelTempMC;
        private Button ButtonHeat;
        private Button AskHeat;
        private ListBox Data;
        private System.Windows.Forms.DataVisualization.Charting.Chart Results;
        private Button ButtonWrite;
        private Label COM_label;
        private ComboBox COM;
    }
}

