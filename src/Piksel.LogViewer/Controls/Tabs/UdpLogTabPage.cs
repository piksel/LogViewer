using Piksel.LogViewer.Logging;
using Piksel.LogViewer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Piksel.LogViewer.Controls.Tabs
{
    class UdpLogTabPage: LogTabPage
    {
        private LogListener logListener;

        public UdpLogTabPage(FormMain parentForm, ILogParser logParser) : base(parentForm, logParser)
        {
            logListener = new LogListener();
            logListener.Connect += LogListener_Connect;
            logListener.LogDataRecieved += LogListener_LogDataRecieved;

        }

        private void LogListener_LogDataRecieved(object sender, LogDataRecievedEventArgs e)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new EventHandler<LogDataRecievedEventArgs>(LogListener_LogDataRecieved), sender, e);
                return;
            }

            ProcessLogData(e.Data);
        }

        private void LogListener_Connect(object sender, ListenerConnectEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler<ListenerConnectEventArgs>(LogListener_Connect), sender, e);
                return;
            }

            if (e.Failed)
            {
                Log($"Could not start listening on debug port {e.Port}.", LogLevel.Error, e.Exception.Message + "\r\n"  + e.Exception.StackTrace);
            }
            else
            {
                Log($"Started listening on debug port {e.Port}.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                logListener.Dispose();
            }

            base.Dispose(disposing);
            
        }
    }
}
