using IEEEsbUserManagement.Model;
using LatchSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IEEEsbUserManagement.ViewModel
{
    public class LatchManagementViewModel
    {
        public static LatchManagementViewModel Current { get; set; }
        public WorkerMonitor Monitor { get; set; }
        public Thread Worker { get; set; }
        public DoorDatabaseConnection DoorDatabaseConnection { get; set; }
        public PropertiesFileHandler PropertiesFileHandler { get; set; }

        public const string LATCH_APP_ID = "asdfghagsdassgd";
        public const string LATCH_SECRET = "aasdgasdadsglhasdlghañsld";


        public LatchManagementViewModel()
        {
            Current = this;
            PropertiesFileHandler = new PropertiesFileHandler();
            DoorDatabaseConnection = new DoorDatabaseConnection(PropertiesFileHandler);
            Monitor = new WorkerMonitor();
            LogConnection.Initialize(PropertiesFileHandler);
        }

        public void Pair(string ID, string PairingToken)
        {
            var t = new Thread(() => pair(ID, PairingToken));
            t.Start();
        }

        private void pair(string ID, string PairingToken)
        {
            LogConnection.LogEvent(LogConnection.SUBTYPE.LATCH, "Emparejando al usuario: " + ID + " con Latch");
            Latch latchHandler = new Latch(LATCH_APP_ID, LATCH_SECRET);
            LatchResponse response = latchHandler.Pair(PairingToken);
            object accountIdObj = string.Empty;
            if(response.Data.TryGetValue("accountId", out accountIdObj) && response.Error == null)
            {
                string accountId = (string)accountIdObj;
                DoorDatabaseConnection.InsertAccId(accountId, ID);
                LogConnection.LogEvent(LogConnection.SUBTYPE.LATCH, "Usuario " + ID + " emparejado correctamente");
            }
            else
            {
                LogConnection.LogEvent(LogConnection.SUBTYPE.LATCH, "No se pudo emparejar al usuario " + ID + " correctamente");
            }
            Monitor.Working = false;
            MessageBox.Show("Terminado", "Control de Latch");
        }

        public void Unpair(string ID)
        {
            var t = new Thread(() => unpair(ID));
            t.Start();
        }

        public void unpair(string ID)
        {
            LogConnection.LogEvent(LogConnection.SUBTYPE.LATCH, "Desemparejando al usuario: " + ID + " con Latch");
            Latch latchHandler = new Latch(LATCH_APP_ID, LATCH_SECRET);
            string accId = DoorDatabaseConnection.GetAccId(ID);
            LatchResponse response = latchHandler.Unpair(accId);
            if(response.Error == null)
            {
                DoorDatabaseConnection.DeleteAccId(ID);
                LogConnection.LogEvent(LogConnection.SUBTYPE.LATCH, "Usuario " + ID + " desemparejado correctamente");
            }
            else
            {
                LogConnection.LogEvent(LogConnection.SUBTYPE.LATCH, "No se pudo desemparejar al usuario " + ID + " correctamente");
            }
            
            Monitor.Working = false;
            MessageBox.Show("Terminado", "Control de Latch");
        }

    }
}
