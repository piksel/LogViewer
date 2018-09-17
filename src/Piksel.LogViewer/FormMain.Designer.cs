namespace Piksel.LogViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbSettings = new System.Windows.Forms.ToolStripDropDownButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.useThemedBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.showLogFormatOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.tsbAutoScroll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbTrace = new System.Windows.Forms.ToolStripButton();
            this.tsbDebug = new System.Windows.Forms.ToolStripButton();
            this.tsbInfo = new System.Windows.Forms.ToolStripButton();
            this.tsbWarning = new System.Windows.Forms.ToolStripButton();
            this.tsbError = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExceptions = new System.Windows.Forms.ToolStripButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new Piksel.LogViewer.Controls.CustomTabControl();
            this.tpNew = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxExtender = new Piksel.LogViewer.Controls.TextBoxExtender();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpNew.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSettings,
            this.toolStripSeparator3,
            this.tsbClear,
            this.tsbAutoScroll,
            this.toolStripSeparator1,
            this.tsbTrace,
            this.tsbDebug,
            this.tsbInfo,
            this.tsbWarning,
            this.tsbError,
            this.toolStripSeparator2,
            this.tsbExceptions});
            this.toolStrip1.Location = new System.Drawing.Point(535, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(1);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(328, 36);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbSettings
            // 
            this.tsbSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbSettings.AutoSize = false;
            this.tsbSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSettings.DropDown = this.contextMenuStrip1;
            this.tsbSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsbSettings.Image")));
            this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSettings.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tsbSettings.Name = "tsbSettings";
            this.tsbSettings.Size = new System.Drawing.Size(42, 32);
            this.tsbSettings.Text = "Clear";
            this.tsbSettings.ToolTipText = "Settings";
            this.tsbSettings.Click += new System.EventHandler(this.tsbSettings_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.toolStripMenuItem2,
            this.useThemedBackgroundToolStripMenuItem,
            this.toolStripMenuItem1,
            this.showLogFormatOptionsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(232, 82);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.aboutToolStripMenuItem.Text = "Piksel Log Viewer";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(228, 6);
            // 
            // useThemedBackgroundToolStripMenuItem
            // 
            this.useThemedBackgroundToolStripMenuItem.Name = "useThemedBackgroundToolStripMenuItem";
            this.useThemedBackgroundToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.useThemedBackgroundToolStripMenuItem.Text = "Use Themed Background";
            this.useThemedBackgroundToolStripMenuItem.Click += new System.EventHandler(this.useThemedBackgroundToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(228, 6);
            // 
            // showLogFormatOptionsToolStripMenuItem
            // 
            this.showLogFormatOptionsToolStripMenuItem.Name = "showLogFormatOptionsToolStripMenuItem";
            this.showLogFormatOptionsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.showLogFormatOptionsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.showLogFormatOptionsToolStripMenuItem.Text = "Show Log Format Options";
            this.showLogFormatOptionsToolStripMenuItem.Click += new System.EventHandler(this.showLogFormatOptionsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 34);
            // 
            // tsbClear
            // 
            this.tsbClear.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbClear.AutoSize = false;
            this.tsbClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClear.Image = ((System.Drawing.Image)(resources.GetObject("tsbClear.Image")));
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(32, 32);
            this.tsbClear.Text = "Clear";
            this.tsbClear.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // tsbAutoScroll
            // 
            this.tsbAutoScroll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbAutoScroll.AutoSize = false;
            this.tsbAutoScroll.Checked = true;
            this.tsbAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbAutoScroll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAutoScroll.Image = ((System.Drawing.Image)(resources.GetObject("tsbAutoScroll.Image")));
            this.tsbAutoScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAutoScroll.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tsbAutoScroll.Name = "tsbAutoScroll";
            this.tsbAutoScroll.Size = new System.Drawing.Size(32, 32);
            this.tsbAutoScroll.Text = "Autoscroll";
            this.tsbAutoScroll.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.tsbAutoScroll.Click += new System.EventHandler(this.ToggleToolstripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 34);
            // 
            // tsbTrace
            // 
            this.tsbTrace.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbTrace.AutoSize = false;
            this.tsbTrace.Checked = true;
            this.tsbTrace.CheckOnClick = true;
            this.tsbTrace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbTrace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTrace.Image = ((System.Drawing.Image)(resources.GetObject("tsbTrace.Image")));
            this.tsbTrace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTrace.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tsbTrace.Name = "tsbTrace";
            this.tsbTrace.Size = new System.Drawing.Size(32, 32);
            this.tsbTrace.Text = "Trace";
            this.tsbTrace.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.tsbTrace.Click += new System.EventHandler(this.ToggleToolstripButton_Click);
            // 
            // tsbDebug
            // 
            this.tsbDebug.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbDebug.AutoSize = false;
            this.tsbDebug.Checked = true;
            this.tsbDebug.CheckOnClick = true;
            this.tsbDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDebug.Image = ((System.Drawing.Image)(resources.GetObject("tsbDebug.Image")));
            this.tsbDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDebug.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tsbDebug.Name = "tsbDebug";
            this.tsbDebug.Size = new System.Drawing.Size(32, 32);
            this.tsbDebug.Text = "Debug";
            this.tsbDebug.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.tsbDebug.Click += new System.EventHandler(this.ToggleToolstripButton_Click);
            // 
            // tsbInfo
            // 
            this.tsbInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbInfo.AutoSize = false;
            this.tsbInfo.Checked = true;
            this.tsbInfo.CheckOnClick = true;
            this.tsbInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbInfo.Image = ((System.Drawing.Image)(resources.GetObject("tsbInfo.Image")));
            this.tsbInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbInfo.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tsbInfo.Name = "tsbInfo";
            this.tsbInfo.Size = new System.Drawing.Size(32, 32);
            this.tsbInfo.Text = "Info";
            this.tsbInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.tsbInfo.Click += new System.EventHandler(this.ToggleToolstripButton_Click);
            // 
            // tsbWarning
            // 
            this.tsbWarning.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbWarning.AutoSize = false;
            this.tsbWarning.Checked = true;
            this.tsbWarning.CheckOnClick = true;
            this.tsbWarning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbWarning.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbWarning.Image = ((System.Drawing.Image)(resources.GetObject("tsbWarning.Image")));
            this.tsbWarning.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbWarning.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tsbWarning.Name = "tsbWarning";
            this.tsbWarning.Size = new System.Drawing.Size(32, 32);
            this.tsbWarning.Text = "Warnings";
            this.tsbWarning.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.tsbWarning.Click += new System.EventHandler(this.ToggleToolstripButton_Click);
            // 
            // tsbError
            // 
            this.tsbError.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbError.AutoSize = false;
            this.tsbError.Checked = true;
            this.tsbError.CheckOnClick = true;
            this.tsbError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbError.Image = ((System.Drawing.Image)(resources.GetObject("tsbError.Image")));
            this.tsbError.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbError.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tsbError.Name = "tsbError";
            this.tsbError.Size = new System.Drawing.Size(32, 32);
            this.tsbError.Text = "Errors";
            this.tsbError.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.tsbError.Click += new System.EventHandler(this.ToggleToolstripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 34);
            // 
            // tsbExceptions
            // 
            this.tsbExceptions.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbExceptions.AutoSize = false;
            this.tsbExceptions.Checked = true;
            this.tsbExceptions.CheckOnClick = true;
            this.tsbExceptions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbExceptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExceptions.Image = ((System.Drawing.Image)(resources.GetObject("tsbExceptions.Image")));
            this.tsbExceptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExceptions.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tsbExceptions.Name = "tsbExceptions";
            this.tsbExceptions.Size = new System.Drawing.Size(32, 32);
            this.tsbExceptions.Text = "Exceptions";
            this.tsbExceptions.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.tsbExceptions.ToolTipText = "Show Exceptions";
            this.tsbExceptions.Click += new System.EventHandler(this.ToggleToolstripButton_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "add");
            this.imageList1.Images.SetKeyName(1, "file");
            this.imageList1.Images.SetKeyName(2, "local");
            this.imageList1.Images.SetKeyName(3, "remote");
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpNew);
            this.tabControl1.DisplayStyle = Piksel.LogViewer.Controls.TabStyle.Chrome;
            // 
            // 
            // 
            this.tabControl1.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.tabControl1.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
            this.tabControl1.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.tabControl1.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
            this.tabControl1.DisplayStyleProvider.CloserColorActive = System.Drawing.Color.White;
            this.tabControl1.DisplayStyleProvider.FocusColor = System.Drawing.SystemColors.Highlight;
            this.tabControl1.DisplayStyleProvider.FocusTrack = false;
            this.tabControl1.DisplayStyleProvider.HotTrack = true;
            this.tabControl1.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tabControl1.DisplayStyleProvider.Opacity = 1F;
            this.tabControl1.DisplayStyleProvider.Overlap = 12;
            this.tabControl1.DisplayStyleProvider.Padding = new System.Drawing.Point(7, 5);
            this.tabControl1.DisplayStyleProvider.Radius = 12;
            this.tabControl1.DisplayStyleProvider.ShowTabCloser = true;
            this.tabControl1.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
            this.tabControl1.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.tabControl1.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.HotTrack = true;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.ItemSize = new System.Drawing.Size(160, 24);
            this.tabControl1.Location = new System.Drawing.Point(3, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(865, 412);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.TabStop = false;
            this.tabControl1.TabClosing += new System.EventHandler<System.Windows.Forms.TabControlCancelEventArgs>(this.tabControl1_TabClosing);
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpNew
            // 
            this.tpNew.BackColor = System.Drawing.Color.White;
            this.tpNew.Controls.Add(this.groupBox2);
            this.tpNew.Controls.Add(this.groupBox1);
            this.tpNew.ImageKey = "add";
            this.tpNew.Location = new System.Drawing.Point(4, 29);
            this.tpNew.Name = "tpNew";
            this.tpNew.Padding = new System.Windows.Forms.Padding(3);
            this.tabControl1.SetPersistant(this.tpNew, true);
            this.tpNew.Size = new System.Drawing.Size(857, 379);
            this.tpNew.TabIndex = 1;
            this.tpNew.Text = "New";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(442, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(308, 203);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "New remote logging: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Host:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(15, 75);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(278, 20);
            this.textBox1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(15, 115);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(279, 69);
            this.button2.TabIndex = 1;
            this.button2.Text = "Listen for remote log messages";
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Location = new System.Drawing.Point(103, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(308, 203);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New local logging: ";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(15, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(279, 69);
            this.button1.TabIndex = 0;
            this.button1.Text = "Listen for local log messages";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(15, 115);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(279, 69);
            this.button3.TabIndex = 2;
            this.button3.Text = "Open log file";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 426);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(740, 400);
            this.Name = "FormMain";
            this.Text = "Log Viewer";
            this.Activated += new System.EventHandler(this.FormMain_Activated);
            this.Deactivate += new System.EventHandler(this.FormMain_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDebug_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDebug_FormClosed);
            this.Load += new System.EventHandler(this.FormDebug_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpNew.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.ToolStripButton tsbAutoScroll;
        private System.Windows.Forms.ToolStripButton tsbTrace;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbInfo;
        private System.Windows.Forms.ToolStripButton tsbDebug;
        private System.Windows.Forms.ToolStripButton tsbWarning;
        private System.Windows.Forms.ToolStripButton tsbError;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbExceptions;
        private Piksel.LogViewer.Controls.CustomTabControl tabControl1;
        private System.Windows.Forms.Timer timer1;
        private Controls.TextBoxExtender textBoxExtender;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tpNew;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton tsbSettings;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem useThemedBackgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem showLogFormatOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}