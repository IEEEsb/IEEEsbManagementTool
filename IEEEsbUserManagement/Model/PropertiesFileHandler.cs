using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEEEsbUserManagement.Model
{
    public class PropertiesFileHandler
    {
        private string appDirectory = Directory.GetCurrentDirectory();
        public string PropertiesDirectory;

        public PropertiesFileHandler()
        {
            if (!Directory.Exists(appDirectory+"\\Properties"))
            {
                Directory.CreateDirectory(appDirectory + "\\Properties");
            }
            PropertiesDirectory = appDirectory + "\\Properties";
        }

        public DatabaseConnectionInfo RetrieveConnectionInfo(string database)
        {
            StreamReader configFile = new StreamReader(PropertiesDirectory + "\\" + database + ".properties");
            DatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfo();
            while (!configFile.EndOfStream)
            {
                string line = configFile.ReadLine();
                if(line.StartsWith("SERVER="))
                {
                    databaseConnectionInfo.Server = line.Substring(line.IndexOf("=")+1);
                }
                if (line.StartsWith("USER="))
                {
                    databaseConnectionInfo.User = line.Substring(line.IndexOf("=")+1);
                }
                if (line.StartsWith("PASSWORD="))
                {
                    databaseConnectionInfo.Password = line.Substring(line.IndexOf("=")+1);
                }
                if (line.StartsWith("DATABASE="))
                {
                    databaseConnectionInfo.Database = line.Substring(line.IndexOf("=")+1);
                }
            }
            configFile.Close();
            return databaseConnectionInfo;
        }

        public Email RetrieveEmailInfo(string database)
        {
            StreamReader configFile = new StreamReader(PropertiesDirectory + "\\" + database + ".properties");
            Email email = new Email();
            while (!configFile.EndOfStream)
            {
                string line = configFile.ReadLine();
                if (line.StartsWith("SENDER="))
                {
                    email.Sender = line.Substring(line.IndexOf("=") + 1);
                }
                if (line.StartsWith("USERNAME="))
                {
                    email.Username = line.Substring(line.IndexOf("=") + 1);
                }
                if (line.StartsWith("SMTPSERVER="))
                {
                    email.SmtpServer = line.Substring(line.IndexOf("=") + 1);
                }
                if (line.StartsWith("PORT="))
                {
                    email.Port = line.Substring(line.IndexOf("=") + 1);
                }
                if (line.StartsWith("PASSWORD="))
                {
                    email.Password = line.Substring(line.IndexOf("=") + 1);
                }
                if (line.StartsWith("ABOUT="))
                {
                    email.About = line.Substring(line.IndexOf("=") + 1);
                }
            }
            configFile.Close();
            StreamReader emailContent = new StreamReader(PropertiesDirectory + "\\" + "Email.properties");
            email.Content = emailContent.ReadToEnd();
            emailContent.Close();
            return email;
        }
    }
}
