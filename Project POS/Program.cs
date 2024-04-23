using Project_POS.Model;
using Project_POS.Report;
using Renci.SshNet;
using System;
using System.Windows.Forms;

namespace Project_POS
{
    internal static class Program
    {
        // Declare a static variable to hold the SSH tunnel
        private static ForwardedPortLocal forwardedPort;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start the SSH tunnel if it's not already started
            if (forwardedPort == null)
            {
                forwardedPort = SshTunnel.StartSshTunnel();
            }

            // If the SSH tunnel could not be established, exit the application
            if (forwardedPort == null)
            {
                MessageBox.Show("SSH Tunnel could not be established. The application will now exit.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Register an event handler to stop the SSH tunnel when the application exits
            Application.ApplicationExit += (sender, e) =>
            {
                if (forwardedPort != null && forwardedPort.IsStarted)
                {
                    forwardedPort.Stop();
                    forwardedPort.Dispose();
                }
            };

            // Run the login form
            Application.Run(new frmLogin());
        }
    }
}
