using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Piksel.LogViewer.Controls.Tabs.ToolStrips
{
    class FileLogTopToolStrips : UserControl, ITabPageToolStripContainer
    {
        private ToolStrip toolStripTop;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStrip toolStripBottom;

        public ToolStrip ToolStripTop => toolStripTop;
        public bool TopVisible => true;

        public ToolStrip ToolStripBottom => toolStripBottom;
        public bool BottomVisible => true;

        private void InitializeComponent()
        {
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripBottom = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripTop
            // 
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(670, 25);
            this.toolStripTop.TabIndex = 0;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // toolStripBottom
            // 
            this.toolStripBottom.CanOverflow = false;
            this.toolStripBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripTextBox1});
            this.toolStripBottom.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripBottom.Location = new System.Drawing.Point(0, 278);
            this.toolStripBottom.Name = "toolStripBottom";
            this.toolStripBottom.Size = new System.Drawing.Size(670, 25);
            this.toolStripBottom.Stretch = true;
            this.toolStripBottom.TabIndex = 1;
            this.toolStripBottom.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(86, 22);
            this.toolStripLabel1.Text = "toolStripLabel1";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.AutoSize = false;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripTextBox1.Size = new System.Drawing.Size(200, 25);
            // 
            // FileLogTopToolStrips
            // 
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Controls.Add(this.toolStripBottom);
            this.Controls.Add(this.toolStripTop);
            this.Name = "FileLogTopToolStrips";
            this.Size = new System.Drawing.Size(670, 303);
            this.toolStripBottom.ResumeLayout(false);
            this.toolStripBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
