using IEEEsbUserManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IEEEsbUserManagement.ViewModel
{
    public class AccessManagementViewModel
    {
        public static AccessManagementViewModel Current { get; set; }
        public PropertiesFileHandler PropertiesFileHandler { get; set; }
        public DoorDatabaseConnection DoorDatabaseConnection { get; set; }
        public EmailListHelper EmailListHelper { get; set; }
        public WorkerMonitor Monitor { get; set; }
        public Thread Revoker { get; set; }
        public Thread Regenerator { get; set; }

        public AccessManagementViewModel()
        {
            Current = this;
            PropertiesFileHandler = new PropertiesFileHandler();
            DoorDatabaseConnection = new DoorDatabaseConnection(PropertiesFileHandler);
            EmailListHelper = new EmailListHelper(PropertiesFileHandler);
            Monitor = new WorkerMonitor();
            LogConnection.Initialize(PropertiesFileHandler);
        }

        public void RevokeAccessToken(string DNI)
        {
            var t = new Thread(() => revokeAccessToken(DNI));
            t.Start();
        }

        public void GenerateNewRegisterID(string DNI, string email)
        {
            var t = new Thread(() => generateNewRegisterID(DNI, email));
            t.Start();
        }

        private void revokeAccessToken(string DNI)
        {
            if(DoorDatabaseConnection.DeleteAccessToken(DNI))
                LogConnection.LogEvent(LogConnection.SUBTYPE.DOORDB, "Eliminando el token de: " + DNI);
            Monitor.Working = false;
            MessageBox.Show("Terminado", "Control de acceso");
        }

        private void generateNewRegisterID(string DNI, string email)
        {
            string newRegistrationID = RegistrationIDGenerator.Generate();
            
            if(DoorDatabaseConnection.GenerateNewRegistrationID(DNI, newRegistrationID))
                LogConnection.LogEvent(LogConnection.SUBTYPE.DOORDB, "Generando un nuevo ID de registro para: " + DNI);
            if(EmailListHelper.SendIDRegenEmail(newRegistrationID, email))
                LogConnection.LogEvent(LogConnection.SUBTYPE.DOORDB, "Enviando email de regeneración de registro a: " + DNI);
            Monitor.Working = false;
            MessageBox.Show("Terminado", "Control de acceso");
        }
    }
}
