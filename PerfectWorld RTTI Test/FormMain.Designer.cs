namespace PerfectWorld_RTTI_Test
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            this.textBoxLog = new System.Windows.Forms.RichTextBox();
            this.contextLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextLogClear = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewClassTree = new System.Windows.Forms.TreeView();
            this.buttonStartStop = new System.Windows.Forms.Button();
            this.bWorkerMain = new System.ComponentModel.BackgroundWorker();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.contextLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.ContextMenuStrip = this.contextLog;
            this.textBoxLog.Location = new System.Drawing.Point(366, 177);
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(345, 240);
            this.textBoxLog.TabIndex = 0;
            this.textBoxLog.Text = "";
            // 
            // contextLog
            // 
            this.contextLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextLogClear});
            this.contextLog.Name = "contextLog";
            this.contextLog.Size = new System.Drawing.Size(102, 26);
            // 
            // contextLogClear
            // 
            this.contextLogClear.Name = "contextLogClear";
            this.contextLogClear.Size = new System.Drawing.Size(101, 22);
            this.contextLogClear.Text = "Clear";
            // 
            // treeViewClassTree
            // 
            this.treeViewClassTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeViewClassTree.Location = new System.Drawing.Point(0, 0);
            this.treeViewClassTree.Name = "treeViewClassTree";
            this.treeViewClassTree.Size = new System.Drawing.Size(360, 417);
            this.treeViewClassTree.TabIndex = 1;
            this.treeViewClassTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewClassTree_BeforeExpand);
            // 
            // buttonStartStop
            // 
            this.buttonStartStop.Location = new System.Drawing.Point(625, 12);
            this.buttonStartStop.Name = "buttonStartStop";
            this.buttonStartStop.Size = new System.Drawing.Size(75, 40);
            this.buttonStartStop.TabIndex = 2;
            this.buttonStartStop.Text = "Start";
            this.buttonStartStop.UseVisualStyleBackColor = true;
            this.buttonStartStop.Click += new System.EventHandler(this.buttonStartStop_Click);
            // 
            // bWorkerMain
            // 
            this.bWorkerMain.WorkerReportsProgress = true;
            this.bWorkerMain.WorkerSupportsCancellation = true;
            this.bWorkerMain.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bWorkerMain_DoWork);
            this.bWorkerMain.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bWorkerMain_RunWorkerCompleted);
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(447, 23);
            this.textBoxAddress.MaxLength = 8;
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(124, 20);
            this.textBoxAddress.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(366, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Base Address:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 417);
            this.Controls.Add(this.textBoxAddress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonStartStop);
            this.Controls.Add(this.treeViewClassTree);
            this.Controls.Add(this.textBoxLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PerfectWorld RTTI Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.contextLog.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBoxLog;
        private System.Windows.Forms.TreeView treeViewClassTree;
        private System.Windows.Forms.Button buttonStartStop;
        private System.ComponentModel.BackgroundWorker bWorkerMain;
        private System.Windows.Forms.ContextMenuStrip contextLog;
        private System.Windows.Forms.ToolStripMenuItem contextLogClear;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.Label label2;
    }
}

