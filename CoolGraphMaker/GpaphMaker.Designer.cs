namespace CoolGraphMaker
{
    partial class GpaphMaker
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDataFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSettingFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGraphAsImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redrawGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTextOnGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphSelectionComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.graphArea = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.graphArea)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolStripMenuItem1,
            this.graphSelectionComboBox});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(531, 27);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDataFileToolStripMenuItem,
            this.loadSettingFileToolStripMenuItem,
            this.saveGraphAsImageToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 23);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadDataFileToolStripMenuItem
            // 
            this.loadDataFileToolStripMenuItem.Name = "loadDataFileToolStripMenuItem";
            this.loadDataFileToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.loadDataFileToolStripMenuItem.Text = "Load data file";
            // 
            // loadSettingFileToolStripMenuItem
            // 
            this.loadSettingFileToolStripMenuItem.Name = "loadSettingFileToolStripMenuItem";
            this.loadSettingFileToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.loadSettingFileToolStripMenuItem.Text = "Load setting file";
            // 
            // saveGraphAsImageToolStripMenuItem
            // 
            this.saveGraphAsImageToolStripMenuItem.Name = "saveGraphAsImageToolStripMenuItem";
            this.saveGraphAsImageToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.saveGraphAsImageToolStripMenuItem.Text = "Save graph as image";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.redrawGraphToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // redrawGraphToolStripMenuItem
            // 
            this.redrawGraphToolStripMenuItem.Name = "redrawGraphToolStripMenuItem";
            this.redrawGraphToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.redrawGraphToolStripMenuItem.Text = "Redraw graph";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editTextOnGraphToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 23);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // editTextOnGraphToolStripMenuItem
            // 
            this.editTextOnGraphToolStripMenuItem.Name = "editTextOnGraphToolStripMenuItem";
            this.editTextOnGraphToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.editTextOnGraphToolStripMenuItem.Text = "Edit text on graph";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(24, 23);
            this.toolStripMenuItem1.Text = "?";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // graphSelectionComboBox
            // 
            this.graphSelectionComboBox.Name = "graphSelectionComboBox";
            this.graphSelectionComboBox.Size = new System.Drawing.Size(121, 23);
            // 
            // graphArea
            // 
            this.graphArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graphArea.BackColor = System.Drawing.SystemColors.Window;
            this.graphArea.Location = new System.Drawing.Point(0, 27);
            this.graphArea.Name = "graphArea";
            this.graphArea.Size = new System.Drawing.Size(531, 321);
            this.graphArea.TabIndex = 1;
            this.graphArea.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 351);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(531, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // GpaphMaker
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 373);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.graphArea);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "GpaphMaker";
            this.Text = "Graph maker";
            this.ResizeEnd += new System.EventHandler(this.GpaphMaker_ResizeEnd);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.GpaphMaker_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.GpaphMaker_DragEnter);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.graphArea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadDataFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSettingFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveGraphAsImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redrawGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editTextOnGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.PictureBox graphArea;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripComboBox graphSelectionComboBox;
    }
}

