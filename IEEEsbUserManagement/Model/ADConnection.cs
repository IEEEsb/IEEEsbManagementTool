using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IEEEsbUserManagement.Model
{
    public class ADConnection
    {
        public string Script = "";
        private string scriptPath;

        public ADConnection(PropertiesFileHandler properties)
        {
            scriptPath = properties.PropertiesDirectory + "\\NewUser2";
        }

        public bool Insert(User user, string password)
        {
            bool success = true;
            try
            {
                RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
                Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);

                runspace.Open();
                RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);
                Pipeline pipeline = runspace.CreatePipeline();
                Command scriptCommand = new Command(scriptPath);
                CommandParameter UserName = new CommandParameter(null, user.UserName);
                CommandParameter Name = new CommandParameter(null, user.Name);
                CommandParameter Surnames = new CommandParameter(null, user.Surnames);
                CommandParameter Email = new CommandParameter(null, user.Email);
                CommandParameter Phone = new CommandParameter(null, user.Phone);
                CommandParameter DNI = new CommandParameter(null, user.DNI);
                CommandParameter IEEEn = new CommandParameter(null, user.IEEEn);
                CommandParameter Password = new CommandParameter(null, password);
                scriptCommand.Parameters.Add(UserName);
                scriptCommand.Parameters.Add(Name);
                scriptCommand.Parameters.Add(Surnames);
                scriptCommand.Parameters.Add(Email);
                scriptCommand.Parameters.Add(Phone);
                scriptCommand.Parameters.Add(DNI);
                scriptCommand.Parameters.Add(IEEEn);
                scriptCommand.Parameters.Add(Password);
                pipeline.Commands.Add(scriptCommand);
                Collection<PSObject> psObjects;
                psObjects = pipeline.Invoke();
            }
            catch(Exception e)
            {
                success = false;
                LogConnection.LogError(LogConnection.SUBTYPE.ACTIVE_DIRECTORY, "Error conectando con Active Directory: " + e.Message);
            }
            return success;
        }


    }
}
