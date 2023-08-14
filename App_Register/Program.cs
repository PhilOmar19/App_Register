using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App_Register
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public class DatabaseConnection
        {
            private SqlConnection connection;

            public DatabaseConnection(string connectionString)
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }

            public SqlConnection GetConnection()
            {
                return connection;
            }
        }
    }
}
