using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piksel.LogViewer.Logging
{
    public interface ILogParser
    {
        LogMessage ParseLogData(byte[] buffer);
    }
}
