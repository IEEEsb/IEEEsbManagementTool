using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IEEEsbUserManagement.Model
{
    public class FridgeDatabaseConnection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public FridgeDatabaseConnection(PropertiesFileHandler properties)
        {
            DatabaseConnectionInfo connectionInfo = properties.RetrieveConnectionInfo("FridgeDatabaseInfo");
            server = connectionInfo.Server;
            database = connectionInfo.Database;
            uid = connectionInfo.User;
            password = connectionInfo.Password;
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        LogConnection.LogError(LogConnection.SUBTYPE.FRIDGEDB, "Error conectando con el servidor");
                        break;
                    case 1045:
                        LogConnection.LogError(LogConnection.SUBTYPE.FRIDGEDB, "Usuario/contraseña inválido");
                        break;
                    default:
                        LogConnection.LogError(LogConnection.SUBTYPE.FRIDGEDB, "Error conectando con la base de datos");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                LogConnection.LogError(LogConnection.SUBTYPE.FRIDGEDB, "Error en la base de datos: " + ex.Message);
                return false;
            }
        }

        //Insert statement
        public bool Insert(User user)
        {
            bool success = true;
            string dni = user.DNI.Remove(user.DNI.Length - 1);
            while(dni.Length < 9)
            {
                dni = "0" + dni;
            }
            string date = String.Format("{0:u}", DateTime.Now.Date);
            date = date.Substring(0, 10);
            string query = "INSERT INTO users (name, l_name, dni, credit, last_update, last_deposit, autorized) VALUES('" + user.Name + "', '" + user.Surnames + "', '" + dni + "', '-3', '" + date + "', '0', '0')";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch(MySqlException e)
                {
                    success = false;
                    LogConnection.LogError(LogConnection.SUBTYPE.FRIDGEDB, "Error en la base de datos: " + e.Message);
                }

                //close connection
                this.CloseConnection();
            }
            return success;
        }

    }
}
