using Piksel.LogViewer.Logging;
using System.Text;

namespace Piksel.LogViewer.Controls.Logging
{
    internal class GenericLogParser : ILogParser
    {
        private readonly Encoding encoding = new UTF8Encoding(false);

        public LogMessage ParseLogData(byte[] buffer)
        {
            return new LogMessage.Builder()
              .WithLevel(LogLevel.Info)
              .WithSource("")
              .WithMessageLine(encoding.GetString(buffer))
              .Build();
        }
    }
}