using Piksel.LogViewer.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Piksel.LogViewer.Controls.Tabs
{
    public delegate void ProcessLogDataDelegate(byte[] data);

    [DesignerCategory("code")]
    public abstract class LogTabPage: TabPage
    {
        protected FormMain parentForm;
        //protected readonly RichTextBox rtf;
        protected readonly LogFormatter fmt;
        protected const string LogViewerSourceName = "LogViewer";
        protected ILogParser logParser;
        protected readonly LogListView logListView;

        public bool ConfigBarVisible
        {
            get => logListView.ConfigBarVisible;
            set {
                logListView.ConfigBarVisible = value;
                parentForm.UpdateTabActions();
            }
        }

        public bool AutoScrollLog { get; set; } = true;

        LogMessage.Builder internalMessageBuilder = new LogMessage.Builder()
            .WithSource(LogViewerSourceName);

        public LogLevel EnabledLevels
        {
            get => fmt.EnabledLevels;
            set => fmt.EnabledLevels = value;
        }

        public bool ShowExceptions
        {
            get => fmt.ShowExceptions;
            set => fmt.ShowExceptions = value;
        }

        public string TypeName { get; internal set; }

        public LogTabPage(FormMain parentForm, ILogParser logParser)
        {
            this.parentForm = parentForm;
            this.logParser = logParser;


            //rtf = new RichTextBox();
            logListView = new LogListView();

            fmt = new LogFormatter();

            Padding = new Padding(0, 4, 0, 0);
            BackColor = SystemColors.Window;

            Controls.Add(logListView);
            logListView.Dock = DockStyle.Fill;


            /*
            rtf.HideCaret();
            rtf.BackColor = SystemColors.Window;
            rtf.BorderStyle = BorderStyle.None;
            rtf.Font = new Font("Consolas", 9f);
            */
        }

        public void ProcessLogData(byte[] data)
        {
            var logFmt = logParser.ParseLogData(data);

            if(logFmt != null) AddToLog(logFmt);
        }

        protected void AddToLog(LogMessage logMessage)
            => AddToLog(new [] { logMessage });

        protected void AddToLog(IEnumerable<LogMessage> logMessages)
        {
            logListView.BeginUpdate();
            foreach (var lm in logMessages)
            {
                if (lm.LogLevel != LogLevel.None)
                {
                    logListView.Items.Add(lm);
                }
                else
                {
                    Debug.WriteLine("Loglevel is None");
                }
            }
            logListView.EndUpdate();
        }

        protected void Log(string message, LogLevel logLevel = LogLevel.Info, string exception = null)
            => AddToLog(internalMessageBuilder
                .WithLevel(logLevel)
                .WithMessageLine(message)
                .WithExceptionLine(exception)
                .WithUniversalTime(DateTime.UtcNow)
                .Build());

        internal void Clear()
        {
            logListView.Items.Clear();
        }

        public virtual void UpdateText(int i)
        {
            Text = $"{i}. {TypeName}";
        }

        public virtual void EnableLogLevel(LogLevel level, bool enabled = true)
        {
            if (enabled)
            {
                EnabledLevels |= level;
            }
            else
            {
                EnabledLevels ^= level;
            }

            logListView.UpdateVisibleLogLevels(EnabledLevels);
        }
    }
}