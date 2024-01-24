namespace IslandSanctuarySolver
{
    partial class MainForm
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
            this.stratProgressLabel = new System.Windows.Forms.Label();
            this.findStratsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.workshopTextBox = new System.Windows.Forms.TextBox();
            this.textBoxStocks = new System.Windows.Forms.TextBox();
            this.strategyTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // stratProgressLabel
            // 
            this.stratProgressLabel.AutoSize = true;
            this.stratProgressLabel.Location = new System.Drawing.Point(756, 63);
            this.stratProgressLabel.Name = "stratProgressLabel";
            this.stratProgressLabel.Size = new System.Drawing.Size(0, 25);
            this.stratProgressLabel.TabIndex = 0;
            // 
            // findStratsButton
            // 
            this.findStratsButton.Location = new System.Drawing.Point(382, 6);
            this.findStratsButton.Name = "findStratsButton";
            this.findStratsButton.Size = new System.Drawing.Size(178, 44);
            this.findStratsButton.TabIndex = 1;
            this.findStratsButton.Text = "Find Strategies";
            this.findStratsButton.UseVisualStyleBackColor = true;
            this.findStratsButton.Click += new System.EventHandler(this.findStratsButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Workshops:";
            // 
            // workshopTextBox
            // 
            this.workshopTextBox.Location = new System.Drawing.Point(142, 13);
            this.workshopTextBox.Name = "workshopTextBox";
            this.workshopTextBox.Size = new System.Drawing.Size(100, 29);
            this.workshopTextBox.TabIndex = 3;
            this.workshopTextBox.TextChanged += new System.EventHandler(this.workshopTextBox_TextChanged);
            // 
            // textBoxStocks
            // 
            this.textBoxStocks.Location = new System.Drawing.Point(13, 63);
            this.textBoxStocks.Multiline = true;
            this.textBoxStocks.Name = "textBoxStocks";
            this.textBoxStocks.ReadOnly = true;
            this.textBoxStocks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStocks.Size = new System.Drawing.Size(363, 823);
            this.textBoxStocks.TabIndex = 4;
            // 
            // strategyTextBox
            // 
            this.strategyTextBox.Location = new System.Drawing.Point(382, 63);
            this.strategyTextBox.Multiline = true;
            this.strategyTextBox.Name = "strategyTextBox";
            this.strategyTextBox.ReadOnly = true;
            this.strategyTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.strategyTextBox.Size = new System.Drawing.Size(368, 823);
            this.strategyTextBox.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1323, 899);
            this.Controls.Add(this.strategyTextBox);
            this.Controls.Add(this.textBoxStocks);
            this.Controls.Add(this.workshopTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.findStratsButton);
            this.Controls.Add(this.stratProgressLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label stratProgressLabel;
        private System.Windows.Forms.Button findStratsButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox workshopTextBox;
        private System.Windows.Forms.TextBox textBoxStocks;
        private System.Windows.Forms.TextBox strategyTextBox;
    }
}

