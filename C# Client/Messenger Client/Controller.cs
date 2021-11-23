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
            ConnectionHandler.Register(username, password, verification);
        }


        public void Login(string username, string password)
        {
            string verification = GenerateVerification();
            CodeToRequestMap[verification] = "LR"; // Add a login request code to the request map and link it to this code.

            Debug.WriteLine("Controller sending login request to connection handler.");

            ConnectionHandler.Login(username, password, verification);

        }


        //----------BEGIN MESSAGE HANDLING CODE ------------- //

        // After a message is received by the ConnectionHandler and parsed by the Parser, 
        // this method takes the result of the parse and uses the ReceiptCode to decide how to handle the message.
        // Available receipt codes are:

        // RS - Registration Successful - Should be accompanied by verification code and username.
        // LS - Login Successful - Should be accompanied by verification code, sessionID, and username.
        // LU - Login Unsuccessful - Should be accompanied by verification code, username, and an error message
        // PR - Pull Message Request - Sent to indicate that a new message has been sent and a pull request should be made
        // for a specific user. Accompanied by receiving username, sending username, and session ID.
        // AM - Administrative message - A generic response code that should be accompanied by a session ID, username, and message.
        public void MessageHandler(Dictionary<string, string> input)
        {
            string code = "";
            try
            {
                code = input["ReceiptCode"];
            }
            catch (ArgumentException e)
            {
                ErrorCall(e, "Receipt code not found in message dictionary.");
            }

            switch (code)
            {
                case "RS":
                    RegistrationSuccessful(input["VerificationCode"], input["Username"], input["Message"]);
                    break;
                case "LS":
                    LoginSuccessful(input["VerificationCode"], input["Username"], input["Message"]);
                    break;
                case "LU":

                    break;
                case "PR":

                    break;
                case "AM":

                    break;
                default:

                    break;
            }

        }

        public void RegistrationSuccessful(string verification, string username, string message)
        {

            if ((username == PendingUsername) && (CodeToRequestMap.ContainsKey(verification) == true) && (CodeToRequestMap[verification] == "IR"))
            {
                PendingUsername = "";
                RegistrationPending = false;
                CodeToRequestMap.Remove(verification);
                MainWindow.ShowPopup(message);
                MainWindow.LoginView();
            }
        }

        public void LoginSuccessful(string verification, string username, string message)
        {

            if ((username == PendingUsername) && (CodeToRequestMap.ContainsKey(verification) == true) && (CodeToRequestMap[verification] == "IR"))
            {
                PendingUsername = "";
                RegistrationPending = false;
                CodeToRequestMap.Remove(verification);
                MainWindow.ShowPopup(message);
                MainWindow.LoginView();
            }
        }






        // --------------- END MESSAGE HANDLING CODE --------------- //

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
