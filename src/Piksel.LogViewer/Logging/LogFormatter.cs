using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Piksel.LogViewer.Logging
{
    public class LogFormatter
    {

        public LogLevel EnabledLevels { get; set; } = LogLevel.All;
        public bool ShowExceptions { get; set; } = true;

        public int SourceMaxLength { get; set; } = 26;
        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";

        public LogFormatter()
        {

        }


        public string FormatLogMessage(LogMessage message)
        {
            var sb = new StringBuilder();

            var source = message.Source;

            if (source.Length > SourceMaxLength)
            {
                source = source.Substring(source.Length - SourceMaxLength, SourceMaxLength);
            }
            else
            {
                source = source.PadLeft(SourceMaxLength, ' ');
            }

            sb.Append(DateTime.Now.ToString(DateTimeFormat))
                            .Append(" | ").Append(source)
                            .Append(" | ").Append(message.LogLevel.ToString().ToUpperInvariant()).Append(": ")
                            .Append(message.Message ?? "");

            if (message.Exception != null)
            {
                sb.Append("\r\n").Append(message.Exception).Append("\r\n");
            }
            sb.Append("\r\n");

            return sb.ToString();
        }

    }
}