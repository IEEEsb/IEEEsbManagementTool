using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IEEEsbUserManagement.Model
{
    public class DoorDatabaseConnection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;


        //Constructor
        public DoorDatabaseConnection(PropertiesFileHandler properties)
        {
            DatabaseConnectionInfo connectionInfo = properties.RetrieveConnectionInfo("DoorDatabaseInfo");
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
                        LogConnection.LogError(LogConnection.SUBTYPE.DOORDB, "No se puede conectar al servidor");
                        break;
                    case 1045:
                        LogConnection.LogError(LogConnection.SUBTYPE.DOORDB, "Usuario/contraseña incorrecto");
                        break;
                    default:
                        LogConnection.LogError(LogConnection.SUBTYPE.DOORDB, "No se puede conectar a la base de datos");
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
                LogConnection.LogError(LogConnection.SUBTYPE.DOORDB, "Error en la base de datos: " + ex.Message);
                return false;
            }
        }


        public string GetAccId(string DNI)
        {
            string query = "SELECT * FROM doorUsers WHERE dni='" + DNI + "';";
            string result = string.Empty;
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader; 

                //Execute command
                try
                {
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    result = reader.GetString("LatchAccountId");
                    reader.Close();
                }
                catch (MySqlException e)
                {
                    LogConnection.LogError(LogConnection.SUBTYPE.DOORDB, "Error en la base de datos: " + e.Message);
                    return null;
                }
                //close connection
                this.CloseConnection();
            }
            return result;
        }

        public bool DeleteAccId(string DNI)
        {
            bool success = true;
            string query = "UPDATE doorUsers SET LatchAccountId=NULL, UsingLatch='0' WHERE dni='" + DNI + "';";
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
                catch (MySqlException e)
                {
                    success = false;
                    LogConnection.LogError(LogConnection.SUBTYPE.DOORDB, "Error en la base de datos: " + e.Message);
                }

                //close connection
                this.CloseConnection();
            }
            return success;
        }

        public bool InsertAccId(string accountId, string DNI)
        {
            bool success = true;
            string query = "UPDATE doorUsers SET LatchAccountId='" + accountId + "', UsingLatch='1' WHERE dni='" + DNI + "';";
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
                catch (MySqlException e)
                {
                    success = false;
                    LogConnection.LogError(LogConnection.SUBTYPE.DOORDB, "Error en la base de datos: " + e.Message);
                }

                //close connection
                this.CloseConnection();
            }
            return success;
        }

        public bool DeleteAccessToken(string DNI)
        {
            bool success = true;
            string query = "DELETE FROM doorTokens WHERE DNI='" + DNI + "';";
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
                catch (MySqlException e)
                {
                    success = false;
                    LogConnection.LogError(LogConnection.SUBTYPE.DOORDB, "Error en la base de datos: " + e.Message);
                }

                //close connection
                this.CloseConnection();
            }
            return success;
        }

        public bool GenerateNewRegistrationID(string DNI, string newRegistrationID)
        {
            bool success = true;
            string query = "UPDATE doorUsers SET RegistrationID='" + newRegistrationID + "', IDUsed='0' WHERE dni='" + DNI + "';";
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
                catch (MySqlException e)
                {
                    LogConnection.LogError(LogConnection.SUBTYPE.DOORDB, "Error en la base de datos: " + e.Message);
                    success = false;
                }

                //close connection
                this.CloseConnection();
            }
            return success;
        }

        //Insert statement
        public bool Insert(User user)
        {
            string query = "INSERT INTO doorUsers (name, dni, RegistrationID, IDUsed) VALUES('"+ user.Name + " " + user.Surnames +"', '"+ user.DNI +"','"+ user.RegistrationID +"', '0')";
            bool success = true; 
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
                    LogConnection.LogError(LogConnection.SUBTYPE.DOORDB, "Error en la base de datos: " + e.Message);
                    success = false;
                }
                

                //close connection
                this.CloseConnection();
            }
            return success;
        }

        public bool Update(User user)
        {
            string query = "UPDATE doorUsers SET name='" + user.Name + " " + user.Surnames + "', dni='" + user.DNI + "', RegistrationID='" + user.RegistrationID + "', IDUsed='0' WHERE dni='" + user.DNI + "';";
            bool success = true;
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
                catch (MySqlException e)
                {
                    success = false;
                    LogConnection.LogError(LogConnection.SUBTYPE.DOORDB, "Error en la base de datos: " + e.Message);
                }


                //close connection
                this.CloseConnection();
            }
            return success;
        }
    }
}
