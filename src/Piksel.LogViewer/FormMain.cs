using Microsoft.Win32;
using Nett;
using Nett.Coma;
using Piksel.LogViewer.Controls;
using Piksel.LogViewer.Controls.Tabs;
using Piksel.LogViewer.Helpers;
using Piksel.LogViewer.Logging;
using Piksel.LogViewer.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Piksel.LogViewer
{
    public partial class FormMain : Form
    {
        private static FormMain instance = null;
        private Color accentColor;
        private bool dynamicBgColor;
        private const string rtfData = @"{\rtf1\ansi\deff0{\fonttbl{\f0 Consolas;}}\fs16 }";

        public static Config<Configuration> Config;
        private bool themedEnabled;

        public FormMain()
        {
            InitializeComponent();

            var confDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "piksel", "LogViewer");
            var confFile = Path.Combine(confDir, "logviewer.conf");

            if(!File.Exists(confFile))
            {
                Toml.WriteFile(Configuration.Default, confFile);
            }

            Config = Nett.Coma.Config.CreateAs()
                .MappedToType(() => new Configuration())
                .StoredAs(s => s.File(confFile))
                /*
                .UseTomlConfiguration(TomlSettings.Create(c => c
                    .ConfigureType<char>(ts => ts
                        .WithConversionFor<TomlString>(conv => conv
                            .ToToml(cv => cv.ToString())
                            .FromToml(tv => tv.Value[0])
                        )
                    )
                ))
                */
                .Initialize();

            
        }

        public static bool ShowWindow(Rectangle? relativeBounds = null)
        {
            bool created = false;
            if(instance == null)
            {
                instance = new FormMain();
                instance.Show();
                created = true;
            }
            else
            {
                instance.Activate();
                created = false;
            }

            if (relativeBounds.HasValue)
            {
                var b = relativeBounds.Value;
                instance.Top = (b.Y + b.Height);
                instance.Left = b.X;
                instance.Width = b.Width;
            }

            return created;
        }

        private void FormDebug_Load(object sender, EventArgs e)
        {
            //var ep = new IPEndPoint(ip, this.port);

            toolStrip1.Renderer = new LogViewerToolStripRenderer()
            {
                NoBackground = true,
                NoBorder = true
            };

            UpdateTabActions();

            try
            {
                themedEnabled = Config.Get(c => c.Application.UseThemedBackground);

                var winX = Config.Get(c => c.Application.WindowX) ?? Left;
                var winY = Config.Get(c => c.Application.WindowY) ?? Top;
                Location = new Point(winX, winY);

                var winW = Config.Get(c => c.Application.WindowWidth) ?? Width;
                var winH = Config.Get(c => c.Application.WindowHeight) ?? Height;
                Size = new Size(winW, winH);

                if (Config.Get(c => c.Application.Maximized))
                {
                    WindowState = FormWindowState.Maximized;
                }
            }
            catch (Exception) { } // Ignore missing configuration exceptions

            UpdateThemed();

            var args = Environment.GetCommandLineArgs();
            if(args.Length > 1)
            {
                if (File.Exists(args[1]))
                {
                    CreateLogPage(() => new FileLogTabPage(this, args[1]), Path.GetFileName(args[1]), "file");
                }
            }
        }

        private void FormDebug_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void FormDebug_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogListener.CloseThreads();
            if (WindowState == FormWindowState.Normal)
            {
                Config.Set(c => c.Application.WindowY, Top);
                Config.Set(c => c.Application.WindowX, Left);
                if (Height > 20) Config.Set(c => c.Application.WindowHeight, Height);
                if (Width > 100) Config.Set(c => c.Application.WindowWidth, Width);
            }
            Config.Set(c => c.Application.Maximized, WindowState == FormWindowState.Maximized);
            
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab is LogTabPage tab)
            {
                tab.Clear();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CreateLogPage(() => new UdpLogTabPage(this, new Log4jParser()), "Local", "local");
        }

        private void CreateLogPage(Func<LogTabPage> constrFunc, string name, string imageKey)
        {
            var tab = constrFunc();
            tab.TypeName = name;
            tabControl1.TabPages.Add(tab);
            tab.ImageKey = imageKey;
            UpdateTabNames();
            tabControl1.SelectedTab = tab;
        }

        private void UpdateTabNames()
        {
            for (int i = 1; i < tabControl1.TabCount; i++)
            {
                if (tabControl1.TabPages[i] is LogTabPage tab)
                {
                    tab.UpdateText(i);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTabActions();
        }

        public void UpdateTabActions()
        {
            if (tabControl1.SelectedIndex < 0) return;

            bool isLogTab = false;
            if (tabControl1.SelectedTab is LogTabPage tab)
            {
                isLogTab = true;

                tsbAutoScroll.Checked = tab.AutoScrollLog;
                tsbExceptions.Checked = tab.ShowExceptions;

                tsbTrace.Checked = tab.EnabledLevels.HasFlag(LogLevel.Trace);
                tsbDebug.Checked = tab.EnabledLevels.HasFlag(LogLevel.Debug);
                tsbInfo.Checked = tab.EnabledLevels.HasFlag(LogLevel.Info);
                tsbWarning.Checked = tab.EnabledLevels.HasFlag(LogLevel.Warning);
                tsbError.Checked = tab.EnabledLevels.HasFlag(LogLevel.Error);

                showLogFormatOptionsToolStripMenuItem.Checked = tab.ConfigBarVisible;
            }

            tsbAutoScroll.Enabled = isLogTab;
            tsbClear.Enabled = isLogTab;
            tsbDebug.Enabled = isLogTab;
            tsbError.Enabled = isLogTab;
            tsbExceptions.Enabled = isLogTab;
            tsbInfo.Enabled = isLogTab;
            tsbTrace.Enabled = isLogTab;
            tsbWarning.Enabled = isLogTab;

            showLogFormatOptionsToolStripMenuItem.Enabled = isLogTab;
        }

        private void ToggleToolstripButton_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab is LogTabPage tab)
            {
                if (sender == tsbAutoScroll) tab.AutoScrollLog = tsbAutoScroll.Checked;
                if (sender == tsbExceptions) tab.ShowExceptions = tsbExceptions.Checked;

                if (sender == tsbTrace) tab.EnableLogLevel(LogLevel.Trace, tsbTrace.Checked);
                if (sender == tsbDebug) tab.EnableLogLevel(LogLevel.Debug, tsbDebug.Checked);
                if (sender == tsbInfo) tab.EnableLogLevel(LogLevel.Info, tsbInfo.Checked);
                if (sender == tsbWarning) tab.EnableLogLevel(LogLevel.Warning, tsbWarning.Checked);
                if (sender == tsbError) tab.EnableLogLevel(LogLevel.Error, tsbError.Checked);

            }
        }

        private void tabControl1_TabClosing(object sender, TabControlCancelEventArgs e)
        {
            
        }

        private void tsbSettings_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CreateLogPage(() => new FileLogTabPage(this, ofd.FileName), Path.GetFileName(ofd.FileName), "file");
            }
        }

        private void FormMain_Activated(object sender, EventArgs e)
        {
            if (dynamicBgColor) BackColor = accentColor;
        }

        private void FormMain_Deactivate(object sender, EventArgs e)
        {
            if (dynamicBgColor) BackColor = Color.White;
        }

        private void useThemedBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            themedEnabled = Config.Toggle(c => c.Application.UseThemedBackground);
            UpdateThemed();
        }

        private void UpdateThemed()
        {
            var renderer = (toolStrip1.Renderer as LogViewerToolStripRenderer);
            if (themedEnabled)
            {
                if (Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\DWM")?.GetValue("AccentColor") is int c)
                {
                    accentColor = Color.FromArgb(c & 0xff, (c >> 8) & 0xff, (c >> 16) & 0xff);
                    BackColor = accentColor;
                    dynamicBgColor = true;
                }
            }
            else
            {
                BackColor = SystemColors.Control;
            }

            renderer.SeparatorColor = themedEnabled ? Color.FromArgb(128, Color.White) : SystemColors.ControlDarkDark;

            useThemedBackgroundToolStripMenuItem.Checked = themedEnabled;
        }

        private void showLogFormatOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab is LogTabPage tab)
            {
                tab.ConfigBarVisible = !tab.ConfigBarVisible;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
            => FormAbout.WithVersionFrom(this)
                .UsingSourceUrl("piksel", "logviewer")
                .ShowDialog(this);
    }
}
