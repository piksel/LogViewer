using System;

namespace Piksel.LogViewer.Logging
{
    [Flags]
    public enum LogLevel
    {
        None = 0,
        Trace = 1,
        Debug = 2,
        Info = 4,
        Warning = 8,
        Error = 16,

        All = Trace | Debug | Info | Warning | Error
    }
}