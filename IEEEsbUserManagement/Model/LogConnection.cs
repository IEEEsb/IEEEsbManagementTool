using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IEEEsbUserManagement.Model
{
    public sealed class LogConnection
    {
        private static readonly LogConnection current = new LogConnection();
   
        private LogConnection(){}

        private static MySqlConnection connection;
        private static string server;
        private static string database;
        private static string uid;
        private static string password;

        public enum TYPE {ERROR, EVENT, WARNING, HIDDEN };
        public enum SUBTYPE 
        {
            LOGDB, DOORDB, FRIDGEDB, ACTIVE_DIRECTORY, EMAIL, LATCH, ACCESS_MANAGEMENT, SYSTEM, PRINTER, HTTP, HTTP_VERSION, HTTP_DOOR, HTTP_FRIDGE, HTTP_LATCH, HTTP_SLIC3R, SLIC3R, DOOR, DNI, NFC, HTTP_IEEENUMBER, PROPERTIES, OTHER
        };


        public static void Initialize(PropertiesFileHandler properties)
        {
            DatabaseConnectionInfo connectionInfo = properties.RetrieveConnectionInfo("LogDatabaseInfo");
            server = connectionInfo.Server;
            database = connectionInfo.Database;
            uid = connectionInfo.User;
            password = connectionInfo.Password;
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        private static bool OpenConnection()
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
                        MessageBox.Show("Cannot connect to server.  Contact administrator", "Door database error");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again", "Door database error");
                        break;
                    default:
                        MessageBox.Show("Cannot connect to database", "Door database error");
                        break;
                }
                return false;
            }
        }

        private static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        public static void LogEvent(SUBTYPE subtype, string message)
        {
            InsertLog(TYPE.EVENT, subtype, message);
        }

        public static void LogWarning(SUBTYPE subtype, string message)
        {
            MessageBox.Show(message, subtype + " " + TYPE.WARNING);
            InsertLog(TYPE.WARNING, subtype, message);
        }

        public static void LogError(SUBTYPE subtype, string message)
        {
            MessageBox.Show(message, subtype + " " + TYPE.ERROR);
            InsertLog(TYPE.ERROR, subtype, message);
        }

        public static ObservableCollection<LogEntry> RetrieveFullLog()
        {
            string query = "SELECT * FROM doorLog";
            ObservableCollection<LogEntry> FullLog = new ObservableCollection<LogEntry>();
            //open connection
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader; 
                //Execute command
                try
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        LogEntry logEntry = new LogEntry();
                        logEntry.Timestamp = reader.GetDateTime("Timestamp");
                        logEntry.Type = (LogConnection.TYPE)Enum.Parse(typeof(LogConnection.TYPE), reader.GetString("Type"), true);
                        logEntry.Subtype = (LogConnection.SUBTYPE)Enum.Parse(typeof(LogConnection.SUBTYPE), reader.GetString("Subtype"), true);
                        logEntry.Message = reader.GetString("Message");
                        FullLog.Add(logEntry);
                    }
                    reader.Close();
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, "Log database error");
                }

                //close connection
                CloseConnection();
            }
            return FullLog;
        }

        private static void InsertLog(TYPE type, SUBTYPE subtype, string message)
        {
            message = message.Replace("'", "");
            string query = "INSERT INTO doorLog (TimeStamp, Type, Subtype, Message) VALUES(NOW(), '"+type+"', '"+subtype+"', '"+message+"')";
            //open connection
            if (OpenConnection() == true)
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
                    MessageBox.Show(e.Message, "Log database error");
                }

                //close connection
                CloseConnection();
            }
        }
    }
}
