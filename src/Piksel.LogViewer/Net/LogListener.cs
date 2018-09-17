using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Webvind3.Shared.Helpers;

namespace Piksel.LogViewer.Net
{
    public class LogDataRecievedEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
        public IPEndPoint Source { get; internal set; }
    }

    public class ListenerConnectEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public bool Failed { get; private set; }
        public Exception Exception { get; private set; }
        public int Port { get; }

        public ListenerConnectEventArgs(int port)
            => Port = port;

        public static ListenerConnectEventArgs Success(int port)
            => new ListenerConnectEventArgs(port) { Failed = false };

        public static ListenerConnectEventArgs Failure(int port, Exception x, string m = null)
            => new ListenerConnectEventArgs(port) { Exception = x, Message = m ?? x.Message, Failed = true };
    }

    internal struct LogListenerThreadArgs
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public CancellationToken Token { get; set; }
    }

    internal class LogListener: IDisposable
    {
        public const int DefaultLogPort = 9999;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly CancellationToken token;

        private List<Task> listeners = new List<Task>();

        public event EventHandler<LogDataRecievedEventArgs> LogDataRecieved;
        public event EventHandler<ListenerConnectEventArgs> Connect;

        public int Port { get; }

        public LogListener(int? port = null, bool ipv4 = true, bool ipv6 = false)
        {
            Port = port ?? DefaultLogPort;
            token = cts.Token;
            if (ipv4)
            {
                listeners.Add(Task.Run(() => UdpListener(IPAddress.Any, Port), token));
            }
            if (ipv6)
            {
                listeners.Add(Task.Run(() => UdpListener(IPAddress.IPv6Any, Port), token));
            }
        }

        private async Task UdpListener(IPAddress bindIp, int port)
        {
            using (var uc = new UdpClient(bindIp.AddressFamily))
            {

                try
                {
                    uc.Client.Bind(new IPEndPoint(bindIp, port));
                }
                catch (SocketException x)
                {
                    Connect?.Invoke(this, ListenerConnectEventArgs.Failure(port, x));
                    return;
                }

                Connect?.Invoke(this, ListenerConnectEventArgs.Success(Port));

                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var udpResult = await uc.ReceiveAsync()
                            .WithCancellation(token)
                            .ConfigureAwait(false);

                        LogDataRecieved?.Invoke(this, new LogDataRecievedEventArgs()
                        {
                            Data = udpResult.Buffer,
                            Source = udpResult.RemoteEndPoint
                        });

                    }
                    catch (OperationCanceledException) { }
                    catch (Exception x)
                    {
                        Debug.Write($"LogListener {x.GetType().Name}: {x.Message}\n\n{x.StackTrace}");
                    }
                }

            }
        }

        public static void CloseThreads()
        {
            /*
            lock (Program.DebugLogLock)
            {
                Program.DebugLogExit = true;
                Program.DebugLogCts.Cancel();
            }

            var te = threads.GetEnumerator();

            while (te.MoveNext()&& !te.Current.Join(1000))
            {
                te.Current.Abort();
            }
            */
        }

        #region IDisposable Support

        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    cts.Cancel();
                    Task.WaitAll(listeners.ToArray());
                }

                isDisposed = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}