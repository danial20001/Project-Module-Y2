using Renci.SshNet;
using System;
using System.Threading.Tasks;

namespace Project_POS
{
    public static class SshTunnel
    {
        private const string sshHostname = "selene.hud.ac.uk"; // SSH server hostname
        private const string sshUsername = "u2280965"; // SSH username
        private const string sshPassword = "DS02jul01ds"; // SSH password
        private const int localPort = 3307; // Local port for forwarding
        private const string remoteMySqlServer = "127.0.0.1"; // Remote MySQL server (usually localhost as seen from the server)
        private const int remoteMySqlPort = 3306; // Remote MySQL port

        private static bool isTunnelStarted = false; // Flag to track if the tunnel is already started

        private static ForwardedPortLocal portForwarded;
        public static bool IsTunnelActive()
        {
            return isTunnelStarted && (portForwarded != null && portForwarded.IsStarted);
        }

        public static ForwardedPortLocal StartSshTunnel()
        {
            if (isTunnelStarted)
            {
                Console.WriteLine("SSH tunnel is already started.");
                return portForwarded; // Return the existing tunnel
            }

            



            var client = new SshClient(sshHostname, sshUsername, sshPassword);
            client.Connect();
            if (client.IsConnected)
            {
                Console.WriteLine("SSH connection established.");
                portForwarded = new ForwardedPortLocal("127.0.0.1", (uint)localPort, remoteMySqlServer, (uint)remoteMySqlPort);
                client.AddForwardedPort(portForwarded);
                portForwarded.Start();
                isTunnelStarted = true;
                return portForwarded;
            }
            else
            {
                Console.WriteLine("SSH connection failed.");
                return null;
            }
        }
    }
}
