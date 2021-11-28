using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Messenger_Client
{




    public class Controller : INotifyPropertyChanged
    {

        private ConnectionHandler ConnectionHandler;
        private MainWindow MainWindow;

        private Dictionary<string, string> CodeToRequestMap = new();

        private bool RegistrationPending = false;
        private string PendingUsername = "";

        private string Session = "";

        // Username control bindings

        private string displayUsername = "Not Logged In";
        public string DisplayUsername
        {
            get
            {
                return displayUsername;
            }
            set
            {
                displayUsername = value;
                OnPropertyChanged("Username");
            }
        }

        // Singleton stuff
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
            MainWindow = Application.Current.MainWindow as MainWindow;
            ConnectionHandler = new ConnectionHandler(this);
        }


        /// <param name="username">The supplied username</param>
        /// <param name="password">The supplied password</param>
        public void Register(string username, string password)
        {
            RaiseChangeViewEvent(Segment.Right, ViewType.RegisterView);
            RaiseChangeUsernameEvent("testyname");
            string verification = GenerateVerification();
            CodeToRequestMap[verification] = "IR"; // Add an initial registration code to the request map and link it to this code.
            RegistrationPending = true;
            PendingUsername = username;
            ConnectionHandler.Register(username, password, verification);
        }

        /// <param name="username">The supplied username</param>
        /// <param name="password">The supplied password</param>
        public void Login(string username, string password)
        {
            string verification = GenerateVerification();
            PendingUsername = username;
            CodeToRequestMap[verification] = "LR"; // Add a login request code to the request map and link it to this code.

            Debug.WriteLine("Controller sending login request to connection handler with username " + username + " and verification " + verification);

            ConnectionHandler.Login(username, password, verification);

        }


        //----------BEGIN MESSAGE HANDLING CODE ------------- //

        /// </summary>
        /// After a message is received by the ConnectionHandler and parsed by the Parser, 
        /// this method takes the result of the parse and uses the ReceiptCode to decide how to handle the message.
        /// Available receipt codes are:
        ///
        /// RS - Registration Successful - Should be accompanied by verification code and username.
        /// LS - Login Successful - Should be accompanied by verification code, sessionID, and username.
        /// LU - Login Unsuccessful - Should be accompanied by verification code, username, and an error message
        /// PR - Pull Message Request - Sent to indicate that a new message has been sent and a pull request should be made
        /// for a specific user. Accompanied by receiving username, sending username, and session ID.
        /// AM - Administrative message - A generic response code that should be accompanied by a session ID, username, and message.
        /// </summary>


        /// <param name="input">The dictionary produced by the parser from a string received by the socket.</param>
        public void MessageHandler(Dictionary<string, string> input)
        {

            string code = "";
            try {
                code = input["ReceiptCode"];
            }
            catch (ArgumentException e)
            {
                RaisePopupEvent("Receipt code not found in message dictionary.\n");
            }

            switch (code)
            {
                case "RS":
                    RegistrationSuccessful(input);
                    break;
                case "RU":
                    RegistrationUnsuccessful(input);
                    break;
                case "LS":
                    LoginSuccessful(input);
                    break;
                case "LU":
                    LoginUnsuccessful(input);
                    break;
                case "FP":
                    FriendsPush(input);
                    break;
                case "FM":

                    break;
                case "AM":

                    break;
                default:

                    break;
            }

        }

        /// <param name="input">The dictionary produced by the parser from a string received by the socket.</param>
        public void RegistrationSuccessful(Dictionary<string, string> input)
        {
            string username = input["Username"];
            string verification = input["VerificationCode"];
            string message = input["Message"];

            if ((username == PendingUsername) && (CodeToRequestMap.ContainsKey(verification) == true) && (CodeToRequestMap[verification] == "IR"))
            {
                PendingUsername = "";
                RegistrationPending = false;
                CodeToRequestMap.Remove(verification);
                RaisePopupEvent(message);
                RaiseChangeViewEvent(Segment.Right, ViewType.LoginView);
            }
        }

        public void RegistrationUnsuccessful(Dictionary<string, string> input)
        {


            string username = input["Username"];
            string verification = input["VerificationCode"];
            string message = input["Message"];


            if ((username == PendingUsername) && (CodeToRequestMap.ContainsKey(verification) == true) && (CodeToRequestMap[verification] == "IR")) {

                PendingUsername = "";
                RegistrationPending = false;
                RaisePopupEvent(message);

            }

        }


        /// <param name="input">The dictionary produced by the parser from a string received by the socket.</param>

        public void LoginSuccessful(Dictionary<string, string> input)
        {
            string username = input["Username"];
            string verification = input["VerificationCode"];
            string session = input["SessionID"];
            string message = input["Message"];

            Debug.WriteLine("Making login checks with username " + username + " and session " + session + " and verification " + verification + "\n");

            if ((username == PendingUsername) && (CodeToRequestMap.ContainsKey(verification) == true) && (CodeToRequestMap[verification] == "LR"))
            {
                Debug.WriteLine("Login checks passed.\n");
                PendingUsername = "";
                RegistrationPending = false;

                DisplayUsername = username;
                Session = session;

                // Request the user's friends list
                string friendsVerification = GenerateVerification();
                CodeToRequestMap[friendsVerification] = "FP";

                Debug.WriteLine("Controller sending friends pull request with verification " + verification);


                ConnectionHandler.TransmissionHandler("PF" + friendsVerification + Parser.Pack(username, 32) + session);





                CodeToRequestMap.Remove(verification);
                RaisePopupEvent(message);
                RaiseChangeViewEvent(Segment.Right, ViewType.MessageView);
                RaiseChangeUsernameEvent(DisplayUsername);
                RaiseChangeViewEvent(Segment.Left, ViewType.FriendsView);


            }
        }


        public void LoginUnsuccessful(Dictionary<string, string> input)
        {
            string username = input["Username"];
            string verification = input["VerificationCode"];
            string message = input["Message"];

            if ((username == PendingUsername) && (CodeToRequestMap.ContainsKey(verification) == true) && (CodeToRequestMap[verification] == "LR"))
            {

                PendingUsername = "";
                RegistrationPending = false;
                RaisePopupEvent(message);

            }
        }

        public void FriendsPush(Dictionary<string, string> input)
        {

            Debug.WriteLine("FriendsPush called with input: " + string.Join(Environment.NewLine, input) + "\n");
            Debug.WriteLine("Current session: " + Session);
            Debug.WriteLine("Current username: " + DisplayUsername);


            if (VerifyMessage(input, "FP") == true) {

                string friends = input["Friends"];

                List<string> friendsList = JsonConvert.DeserializeObject<List<string>>(friends);

                Debug.WriteLine("Friends returned by request: " + friends);
                RaiseUpdateFriendsEvent(friendsList);
            }
        }

        private bool VerifyMessage(Dictionary<string, string> input, string messageType)
        {
            string username = input["Username"];
            string verification = input["VerificationCode"];
            string session = input["SessionID"];

            if ((username == DisplayUsername) && (session == Session) && (CodeToRequestMap.ContainsKey(verification)) && (CodeToRequestMap[verification] == "FP"))
            {
                return true;
            }

            return false;
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
            return DisplayUsername;
        }

        // ---------- BEGIN EVENT DELEGATES --------- //

        // Called to display a popup.

        public delegate void PopupEventHandler(object sender, PopupEventArgs e);

        public event PopupEventHandler PopupEvent;


        /// <param name="message">The message that will be displayed by the popup.</param>

        public void RaisePopupEvent(string message)
        {
            Debug.WriteLine("Raise popup called.\n");

            PopupEvent?.Invoke(this, new PopupEventArgs(message));

        }


        // Called to change the view in the main window.

        public delegate void ChangeViewEventHandler(object sender, ChangeViewEventArgs e);

        public event ChangeViewEventHandler ChangeViewEvent;

        /// <param name="segment">Determines which segment of the main window will hold the requested view.</param>
        /// <param name="view">Determines which view will be applied to the segment</param>

        public void RaiseChangeViewEvent(Segment segment, ViewType view)
        {
            Debug.WriteLine("Raise change view called.\n");

            ChangeViewEvent?.Invoke(this, new ChangeViewEventArgs(segment, view));

        }

        // Called to change the username displayed by the main window.

        public delegate void ChangeUsernameEventHandler(object sender, MessageEventArgs e);

        public event ChangeUsernameEventHandler ChangeUsernameEvent;

        public void RaiseChangeUsernameEvent(string username)
        {
            ChangeUsernameEvent?.Invoke(this, new MessageEventArgs(username));
        }

        // Called to update the list of friends in the Message control.

        public delegate void UpdateFriendsEventHandler(object sender, UpdateFriendsEventArgs e);

        public event UpdateFriendsEventHandler UpdateFriendsEvent;

        public void RaiseUpdateFriendsEvent(List<string> friends)
        {

            Debug.WriteLine("Raise update friends called.\n");

            UpdateFriendsEvent?.Invoke(this, new UpdateFriendsEventArgs(friends));
        }
   

        //INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        /// <param name="propertyName">The bound property that has been modified.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
