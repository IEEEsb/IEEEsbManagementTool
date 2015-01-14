using IEEEsbUserManagement.Model;
using IEEEsbUserManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace IEEEsbUserManagement.ViewModel
{
    public class NewUserViewModel
    {
        public static NewUserViewModel Current { get; set; }
        public User CurrentUser { get; set; }
        public FridgeDatabaseConnection FridgeDatabaseConnection { get; set; }
        public DoorDatabaseConnection DoorDatabaseConnection { get; set; }
        public ADConnection ADConnection { get; set; }
        public PropertiesFileHandler PropertiesFileHandler { get; set; }
        public EmailListHelper EmailListHelper { get; set; }
        public WorkerMonitor Monitor { get; set; }
        private string userPassword;

        public NewUserViewModel()
        {
            Current = this;
            CurrentUser = new User();
            PropertiesFileHandler = new PropertiesFileHandler();
            FridgeDatabaseConnection = new FridgeDatabaseConnection(PropertiesFileHandler);
            DoorDatabaseConnection = new DoorDatabaseConnection(PropertiesFileHandler);
            ADConnection = new ADConnection(PropertiesFileHandler);
            EmailListHelper = new EmailListHelper(PropertiesFileHandler);
            Monitor = new WorkerMonitor();
            LogConnection.Initialize(PropertiesFileHandler);
        }

        public void Register(string password)
        {
            userPassword = password;
            var t = new Thread(() => register(userPassword));
            t.Start();
        }

        private void register(string password)
        {
            CurrentUser.RegistrationID = RegistrationIDGenerator.Generate();
            InsertUserIntoDoor();
            InsertUserIntoFridge();
            InsertUserIntoAD(password);
            SendEmail();
            CurrentUser.Clear();
            Monitor.Working = false;
            MessageBox.Show("Terminado", "Registro de nuevo usuario");
        }

        public void InsertUserIntoAD(string password)
        {
            if(ADConnection.Insert(CurrentUser, password))
                LogConnection.LogEvent(LogConnection.SUBTYPE.ACTIVE_DIRECTORY, "Registrando nuevo usuario en el active directory: " + CurrentUser.ToString());
        }

        public void InsertUserIntoFridge()
        {
            if(FridgeDatabaseConnection.Insert(CurrentUser))
                LogConnection.LogEvent(LogConnection.SUBTYPE.FRIDGEDB, "Registrado nuevo usuario en la nevera: " + CurrentUser.ToString());
        }

        public void InsertUserIntoDoor()
        {
            bool success = DoorDatabaseConnection.Insert(CurrentUser);
            if (!success)
                success = DoorDatabaseConnection.Update(CurrentUser);
            DoorDatabaseConnection.DeleteAccessToken(CurrentUser.DNI);

            if(success)
                LogConnection.LogEvent(LogConnection.SUBTYPE.DOORDB, "Registrado nuevo usuario en la puerta: " + CurrentUser.ToString());
        }

        public void SendEmail()
        {
            if(EmailListHelper.SendWelcomeEmail(CurrentUser))
                LogConnection.LogEvent(LogConnection.SUBTYPE.EMAIL, "Enviando email de registro a: " + CurrentUser.ToString());
        }

    }
}
