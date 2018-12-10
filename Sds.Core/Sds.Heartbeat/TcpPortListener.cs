using Sds.Reflection;
using Serilog;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sds.Heartbeat
{
    public static class TcpPortListener
    {
        public static void Start(int port)
        {
            Task.Run(() =>
            {
                try
                {
                    // Start an asynchronous socket to listen for connections.  
                    var listener = new TcpListener(IPAddress.Any, port);
                    listener.Start();

                    Log.Information($"Waiting for a connection on port {port} ...");
                    while (true)
                    {
                        var clientTask = listener.AcceptTcpClientAsync();
                        if (clientTask.Result != null)
                        {
                            var client = clientTask.Result;
                            byte[] data = Encoding.ASCII.GetBytes(Assembly.GetEntryAssembly().GetTitle());
                            client.GetStream().Write(data, 0, data.Length);

                            client.GetStream().Dispose();
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Error opening socket {e.ToString()}");
                }
            });
        }
    }
}
