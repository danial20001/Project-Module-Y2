using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Project_POS
{
    internal class Database
    {
        public const string ConnectionString = "server=127.0.0.1;port=3307;database=u2280965;user=u2280965;password=DS02jul01ds;";

        public static MySqlConnection Connect()
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Database connection established.");
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error connecting to the database: " + ex.Message);
                return null;
            }
        }

        public static int ExecuteSQL(string qry, Dictionary<string, object> parameters)
        {
            int result = 0;
            using (MySqlConnection connection = Connect())
            {
                if (connection == null) return result;

                using (MySqlCommand command = new MySqlCommand(qry, connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    result = command.ExecuteNonQuery();
                }

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return result;
        }

        public static void LoadData(string query, DataGridView gridView, Dictionary<string, string> columnMappings)
        {
            try
            {
                using (MySqlConnection connection = Connect())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (var mapping in columnMappings)
                    {
                        gridView.Columns[mapping.Key].DataPropertyName = mapping.Value;
                    }

                    gridView.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }








    }
}
