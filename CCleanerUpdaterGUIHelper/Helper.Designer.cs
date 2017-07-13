namespace CCleanerUpdaterGUIHelper
{
    partial class Helper
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Helper));
            this.CCPathInfo = new System.Windows.Forms.Label();
            this.CCPath = new System.Windows.Forms.TextBox();
            this.CCPathSelect = new System.Windows.Forms.Button();
            this.Selector = new System.Windows.Forms.FolderBrowserDialog();
            this.LangInfo = new System.Windows.Forms.Label();
            this.Lang = new System.Windows.Forms.ComboBox();
            this.WinApp2 = new System.Windows.Forms.ComboBox();
            this.WinApp2Info = new System.Windows.Forms.Label();
            this.Service = new System.Windows.Forms.ComboBox();
            this.ServiceInfo = new System.Windows.Forms.Label();
            this.HelpMe = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CCPathInfo
            // 
            this.CCPathInfo.AutoSize = true;
            this.CCPathInfo.BackColor = System.Drawing.SystemColors.Control;
            this.CCPathInfo.Location = new System.Drawing.Point(12, 36);
            this.CCPathInfo.Name = "CCPathInfo";
            this.CCPathInfo.Size = new System.Drawing.Size(85, 13);
            this.CCPathInfo.TabIndex = 0;
            this.CCPathInfo.Text = "CCleaner\'s Path:";
            // 
            // CCPath
            // 
            this.CCPath.Location = new System.Drawing.Point(103, 33);
            this.CCPath.Name = "CCPath";
            this.CCPath.ReadOnly = true;
            this.CCPath.Size = new System.Drawing.Size(265, 20);
            this.CCPath.TabIndex = 1;
            // 
            // CCPathSelect
            // 
            this.CCPathSelect.Location = new System.Drawing.Point(374, 31);
            this.CCPathSelect.Name = "CCPathSelect";
            this.CCPathSelect.Size = new System.Drawing.Size(55, 23);
            this.CCPathSelect.TabIndex = 2;
            this.CCPathSelect.Text = "Select";
            this.CCPathSelect.UseVisualStyleBackColor = true;
            this.CCPathSelect.Click += new System.EventHandler(this.CCPathSelect_Click);
            // 
            // LangInfo
            // 
            this.LangInfo.AutoSize = true;
            this.LangInfo.BackColor = System.Drawing.SystemColors.Control;
            this.LangInfo.Location = new System.Drawing.Point(12, 91);
            this.LangInfo.Name = "LangInfo";
            this.LangInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LangInfo.Size = new System.Drawing.Size(58, 13);
            this.LangInfo.TabIndex = 3;
            this.LangInfo.Text = "Language:";
            // 
            // Lang
            // 
            this.Lang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Lang.FormattingEnabled = true;
            this.Lang.Location = new System.Drawing.Point(171, 88);
            this.Lang.Name = "Lang";
            this.Lang.Size = new System.Drawing.Size(197, 21);
            this.Lang.TabIndex = 4;
            // 
            // WinApp2
            // 
            this.WinApp2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WinApp2.FormattingEnabled = true;
            this.WinApp2.Location = new System.Drawing.Point(171, 115);
            this.WinApp2.Name = "WinApp2";
            this.WinApp2.Size = new System.Drawing.Size(197, 21);
            this.WinApp2.TabIndex = 6;
            // 
            // WinApp2Info
            // 
            this.WinApp2Info.AutoSize = true;
            this.WinApp2Info.BackColor = System.Drawing.SystemColors.Control;
            this.WinApp2Info.Location = new System.Drawing.Point(12, 118);
            this.WinApp2Info.Name = "WinApp2Info";
            this.WinApp2Info.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.WinApp2Info.Size = new System.Drawing.Size(141, 13);
            this.WinApp2Info.TabIndex = 5;
            this.WinApp2Info.Text = "Do you want add WinApp2?";
            // 
            // Service
            // 
            this.Service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Service.FormattingEnabled = true;
            this.Service.Location = new System.Drawing.Point(171, 142);
            this.Service.Name = "Service";
            this.Service.Size = new System.Drawing.Size(197, 21);
            this.Service.TabIndex = 8;
            // 
            // ServiceInfo
            // 
            this.ServiceInfo.AutoSize = true;
            this.ServiceInfo.BackColor = System.Drawing.SystemColors.Control;
            this.ServiceInfo.Location = new System.Drawing.Point(12, 145);
            this.ServiceInfo.Name = "ServiceInfo";
            this.ServiceInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ServiceInfo.Size = new System.Drawing.Size(153, 13);
            this.ServiceInfo.TabIndex = 7;
            this.ServiceInfo.Text = "Do you want set-up a Service?";
            // 
            // HelpMe
            // 
            this.HelpMe.Location = new System.Drawing.Point(12, 175);
            this.HelpMe.Name = "HelpMe";
            this.HelpMe.Size = new System.Drawing.Size(416, 23);
            this.HelpMe.TabIndex = 9;
            this.HelpMe.Text = "Run Tool";
            this.HelpMe.UseVisualStyleBackColor = true;
            this.HelpMe.Click += new System.EventHandler(this.HelpMe_Click);
            // 
            // Helper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(440, 210);
            this.Controls.Add(this.HelpMe);
            this.Controls.Add(this.Service);
            this.Controls.Add(this.ServiceInfo);
            this.Controls.Add(this.WinApp2);
            this.Controls.Add(this.WinApp2Info);
            this.Controls.Add(this.Lang);
            this.Controls.Add(this.LangInfo);
            this.Controls.Add(this.CCPathSelect);
            this.Controls.Add(this.CCPath);
            this.Controls.Add(this.CCPathInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Helper";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CCleaner Updater GUI Helper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CCPathInfo;
        private System.Windows.Forms.TextBox CCPath;
        private System.Windows.Forms.Button CCPathSelect;
        private System.Windows.Forms.FolderBrowserDialog Selector;
        private System.Windows.Forms.Label LangInfo;
        private System.Windows.Forms.ComboBox Lang;
        private System.Windows.Forms.ComboBox WinApp2;
        private System.Windows.Forms.Label WinApp2Info;
        private System.Windows.Forms.ComboBox Service;
        private System.Windows.Forms.Label ServiceInfo;
        private System.Windows.Forms.Button HelpMe;
    }
}

