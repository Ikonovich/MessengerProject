using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger_Client
{
    public class Controller
    {

        private ConnectionHandler ConnectionHandler;
        private MainWindow MainWindow;

        private Dictionary<string, string> CodeToRequestMap = new();

        private bool RegistrationPending = false;
        private string PendingUsername = "";

        private string Username = "Not Logged In";
        private string SessionID = "";

        private static Controller controllerInstance { get; set; }

        public static Controller ControllerInstance
        {
            get
            {
                if (controllerInstance == null)
                {
                    controllerInstance = new Controller();
                }
                return controllerInstance;
            }
        }

        // private singleton constructor
        private Controller()
        {
            ConnectionHandler = ConnectionHandler.HandlerInstance;
            MainWindow = Application.Current.MainWindow as MainWindow;
        }


        public void Register(string username, string password)
        {

            string verification = GenerateVerification();
            CodeToRequestMap[verification] = "IR"; // Add an initial registration code to the request map and link it to this code.
            RegistrationPending = true;
            PendingUsername = username;
            ConnectionHandler.Register(username, password);
        }

        public void RegistrationMessage(string verification, string username, string message)
        {

            if ((username == PendingUsername) && (CodeToRequestMap.ContainsKey(verification) == true) && (CodeToRequestMap[verification] == "IR"))
            {
                PendingUsername = "";
                RegistrationPending = false;
                CodeToRequestMap.Remove(verification);
                MainWindow.ShowPopup(message);
            }
        }


        public void Login(string username, string password)
        {
            //ConnectionHandler.Login(username, password);
        }

        // Used to generate a unique 16 character verification code that maps to a request,
        // allowing the server to verify when a specific request has been responded to.

        private string GenerateVerification()
        {
            return Security.GenerateCode(16);
        }

        public string GetUsername()
        {
            return Username;
        }

        // Called whenever an error occurs, calls MainWindow to show an error popup.
        public void ErrorCall(Exception e, string errorMessage)
        {
            MainWindow.ShowPopup(errorMessage);
        }
    }

}
