using Piksel.LogViewer.Controls.Logging;
using Piksel.LogViewer.Helpers;
using Piksel.LogViewer.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Piksel.LogViewer.Controls.Tabs
{
    public partial class FileLogTabPage : LogTabPage
    {
        static LogMessage.Builder unknownFormatBuilder;

        private LogMessage.Builder messageBuilder = new LogMessage.Builder();
        private string fileName;
        private FileSystemWatcher watcher;
        private long lastFileSize = 0;
        private Regex formatRe;

        public int FindFormatMaxLines { get; set; } = 10;
        public int ConfigMaxLines { get; set; } = 30;
        public int MaxLines { get; set; } = 1000; 
        string[] configRawLines;

        private Delimiter[] delimiters = Delimiter.GetDefaults();
        private Delimiter _delimiter;

        public FieldOrder FieldOrder { get; } = new FieldOrder();

        public List<LogField> ParseErrorFields = new List<LogField>();
        private ToolStripTextBox textBoxRawLine;

        public Delimiter Delimiter {
            get => _delimiter;
            set {
                _delimiter = value;
                ReparseLines();
            }
        }

        private void ReparseLines()
        {
            lastFileSize = 0;
            logListView.Items.Clear();
            LoadLogLines(true);
        }

        private string GetGlobString(string fileName)
        {
            var dir = Path.GetDirectoryName(fileName);
            var extIndex = fileName.LastIndexOf('.');
            var ext = extIndex >= 0 ? fileName.Substring(extIndex) : "";
            return Path.Combine(dir, '*' + ext);
        }

        public FileLogTabPage(FormMain parentForm, string fileName) : base(parentForm, new GenericLogParser())
        {
            
            this.fileName = fileName;
            _delimiter = delimiters[0];

            unknownFormatBuilder = new LogMessage.Builder()
                .WithLevel(LogLevel.Info)
                .WithoutTime()
                .WithSource(Path.GetFileName(fileName));

            var glob = GetGlobString(fileName);
            var pathOpts = FormMain.Config.Get(c => c.FileLog.PathParserOptions);

            if (pathOpts.ContainsKey(glob))
            {
                var fs = pathOpts[glob];
                var secondDelim = fs.SecondaryDelimiter?.ToCharArray()?.FirstOrNull();
                var fsDelim = delimiters.FirstOrDefault(d => 
                    d.Start == fs.PrimaryDelimiter[0] && d.End == secondDelim);
                if(fsDelim == null)
                {
                    fsDelim = new Delimiter(fs.PrimaryDelimiter[0], "Custom", secondDelim);
                    delimiters = delimiters.Append(fsDelim).ToArray();
                }

                FieldOrder.TrySetFromString(fs.FieldOrder);

                _delimiter = fsDelim;
                ConfigBarVisible = false;

            }
            else
            {
                ConfigBarVisible = true;
            }

            LoadLogLines(true);

            watcher = new FileSystemWatcher()
            {
                Path = Path.GetDirectoryName(fileName),
                Filter = Path.GetFileName(fileName),
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.Size,
                EnableRaisingEvents = true,
            };

            watcher.Changed += Watcher_Changed;

            //logListView.ScrollToBottom();

            Click += (e, ea) =>
            {
                logListView.ScrollToBottom();
            };

            InitializeToolStrips();

            logListView.SetToolStrips(topToolStrip, bottomToolStrip);

            logListView.SelectionChanged += (s, e) =>
            {
                if (ConfigBarVisible && e.Index >= 0)
                {
                    if (configRawLines != null && configRawLines.Length > e.Index)
                    {
                        textBoxRawLine.Text = configRawLines[e.Index];
                    }
                    else
                    {
                        textBoxRawLine.Text = "";
                    }
                }
            };
        }

        private void LoadLogLines(bool findLogFormat)
        {
            var lines = ReadLogFile(lastFileSize, out lastFileSize);

            if (findLogFormat)
            {
                for (int i = 0; i < FindFormatMaxLines; i++)
                {
                    if (TryFindLogFormat(lines[i]))
                    {
                        // Format found, no need to keep looking
                        break;
                    }
                }
            }

            if(true ||ConfigBarVisible)
            {
                configRawLines = lines;
            }
            else
            {
                configRawLines = new string[0];
            }

            var messages = ParseLogLines(lines);
            if (InvokeRequired)
            {
                Invoke((Action)(() =>
                {
                    AddToLog(messages);
                }));
            }
            else
            {
                AddToLog(messages);
            }

            logListView.SetErrorFields(ConfigBarVisible ? ParseErrorFields.ToArray() : new LogField[0]);

        }

        private bool TryFindLogFormat(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                var re = Delimiter.GetRegex(line);

                if (re.IsMatch(line))
                {
                    formatRe = re;
                    return true;
                }
            }
            return false;
        }

        private IEnumerable<LogMessage> ParseLogLines(IEnumerable<string> lines)
        {
            ParseErrorFields.Clear();
            bool dateTimeFailed = false;

            foreach (var line in lines)
            {
                if (formatRe != null)
                {
                    var match = formatRe.Match(line);

                    if (match.Success)
                    {

                        if (!FieldOrder.HasIndex(LogField.Level) 
                            || !Enum.TryParse(match.Groups[FieldOrder.GroupIndex(LogField.Level)].Value, true, out LogLevel level))
                        {
                            level = LogLevel.Info;
                            ParseErrorFields.Set(LogField.Level);
                        }

                        if (!FieldOrder.HasIndex(LogField.Time))
                        {
                            messageBuilder.WithRawTime("");
                            ParseErrorFields.Set(LogField.Time);
                        }
                        else
                        {
                            var rawTime = match.Groups[FieldOrder.GroupIndex(LogField.Time)].Value;
                            if (dateTimeFailed || !DateTime.TryParse(rawTime, out DateTime dateTime))
                            {
                                dateTimeFailed = true;
                                ParseErrorFields.Set(LogField.Time);
                                messageBuilder.WithRawTime(rawTime);
                            }
                            else
                            {
                                messageBuilder.WithTime(dateTime);
                            }
                        }

                        var source = "";
                        if(FieldOrder.HasIndex(LogField.Source))
                        {
                            source = match.Groups[FieldOrder.GroupIndex(LogField.Source)].Value;
                        }
                        else
                        {
                            ParseErrorFields.Set(LogField.Source);
                        }

                        messageBuilder.WithLevel(level)
                            .WithSource(source)
                            .WithMessageLine(match.Groups[FieldOrder.GroupIndex(LogField.Message)].Value);

                        yield return messageBuilder.Build();
                    }
                    else
                    {
                        yield return messageBuilder.WithMessageLine(line)
                            .WithRawTime("")
                            .WithSource("")
                            .Build();
                    }
                }
                else
                {
                    yield return unknownFormatBuilder.WithMessageLine(line).Build();
                }
            }

            // Add last line
            if (!messageBuilder.IsEmpty)
            {
                yield return messageBuilder.Build();
            }
        }

        private string[] ReadLogFile(long offset, out long fileSize)
        {
            int lineCount = 0;
            var configLimit = ConfigBarVisible;
            using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs))
            {
                fs.Seek(offset, SeekOrigin.Begin);
                fileSize = fs.Length;
                string line;
                var lines = new List<string>();
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                    ++lineCount;
                    if (configLimit && lineCount > ConfigMaxLines) break;
                }

                if(lines.Count > MaxLines)
                {
                    lines.RemoveRange(0, lines.Count - MaxLines);
                }

                return lines.ToArray();
            }
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            LoadLogLines(false);
        }

    }
}
