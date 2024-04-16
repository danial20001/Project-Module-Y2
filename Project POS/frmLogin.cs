using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_POS
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string username = usernamebox.Text;  // Assuming this is the TextBox for the username
            string password = textPassword.Text; // Assuming this is the TextBox for the password

            // Start the SSH tunnel
            var portForwarded = SshTunnel.StartSshTunnel();
            if (portForwarded == null)
            {
                MessageBox.Show("SSH tunnel could not be established.");
                return;
            }

            using (MySqlConnection connection = Database.Connect())
            {
                if (connection == null)
                {
                    MessageBox.Show("Database connection failed. Please check the connection details.");
                    portForwarded.Stop(); // Close the SSH tunnel if the connection fails
                    return;
                }

                if (connection.State != ConnectionState.Open)
                {
                    MessageBox.Show("Connection must be open before executing a command.");
                    portForwarded.Stop(); // Close the SSH tunnel if the connection is not open
                    return;
                }

                // Updated query to select the name
                string query = "SELECT name FROM users WHERE username = @username AND password = @password";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);  // Consider using hashed passwords

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Login successful
                            MessageBox.Show("Login successful!");

                            // Retrieve the name from the reader
                            string name = reader["name"].ToString();

                            // Create an instance of frmMain
                            frmMain form1 = new frmMain();

                            // Pass the name to frmMain
                            form1.UserName = name;

                            // Show frmMain and hide the login form
                            form1.Show();
                            this.Hide();
                        }
                        else
                        {
                            // Login failed
                            MessageBox.Show("Invalid username or password.");
                        }
                    }
                }
               
            }
        }






        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void usernamebox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
