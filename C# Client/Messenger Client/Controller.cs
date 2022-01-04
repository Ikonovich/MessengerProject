using Messenger_Client.SupportClasses;
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

        int DebugMask = 2;

        public static Chat ActiveChat { get; private set; } // Stores the currently active chat.

        private ConnectionHandler ConnectionHandler;
        private MainWindow MainWindow;

        private Dictionary<string, string> CodeToRequestMap = new();


        private int UserID = 0;
        private string SessionID = "";

        private List<FriendUser> Friends;

        private List<Chat> Chats;


        private bool RegistrationPending = false;
        private string PendingUsername = "";



        private readonly int usernameLength = 32;
        private readonly int passwordLength = 128;

        // Username control bindings

        private string username = "Not Logged In";
        public string DisplayUsername
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
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
            RegistrationPending = true;
            PendingUsername = username;
            ConnectionHandler.Register(username, password);
        }

        /// <param name="username">The supplied username</param>
        /// <param name="password">The supplied password</param>
        public void Login(string username, string password)
        {

            PendingUsername = username;


            Debugger.Record("Controller sending login request to connection handler with username " + username, DebugMask);
            ConnectionHandler.Login(username, password);

        }

        public void SendMessage(string message)
        {
            if (ActiveChat != null)
            {

                string messageOut = "SM" + Parser.Pack(UserID, 32) + SessionID + Parser.Pack(ActiveChat.ChatID, 32) + message;

                ConnectionHandler.SendMessage(UserID, SessionID, ActiveChat.ChatID, message);
            }
        }



        //----------BEGIN MESSAGE HANDLING CODE ------------- //

        /// </summary>
        /// After a message is received by the ConnectionHandler and parsed by the Parser, 
        /// this method takes the result of the parse and uses the ReceiptCode to decide how to handle the message.
        /// Available receipt codes are:
        ///
        /// RS - Registration Successful - Should be accompanied by username.
        /// LS - Login Successful - Should be accompanied by sessionID, and username.
        /// LU - Login Unsuccessful - Should be accompanied by username, and an error message.
        /// PR - Pull Message Request - Sent to indicate that a new message has been sent and a pull request should be made
        /// for a specific user. Accompanied by receiving username, sending username, and session ID.
        /// AM - Administrative message - A generic response code that should be accompanied by a session ID, username, and message.
        /// </summary>


        /// <param name="input">The dictionary produced by the parser from a string received by the socket.</param>
        public void MessageHandler(Dictionary<string, string> input)
        {
            Debugger.Record("MessageHandler called with input: " + string.Join(Environment.NewLine, input), DebugMask);


            string opcode = "";
            try {
                opcode = input["Opcode"];
            }
            catch (Exception e)
            {
                RaisePopupEvent("Opcode not found in message dictionary.\n");
                return;
            }

            switch (opcode)
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
                case "MP":
                    MessagePush(input);
                    break;
                case "CN":
                    ChatNotification(input);
                    break;
                case "AM":

                    break;
                case "HB":
                    ReceiveHeartbeat(input);
                    break;
                default:
                    Debugger.Record("An invalid opcode of " + opcode + " has been received by the Controller.", DebugMask);
                    break; 
            }

        }

        /// <param name="input">The dictionary produced by the parser from a string received by the socket.</param>
        public void RegistrationSuccessful(Dictionary<string, string> input)
        {
            string username = input["Username"];
            string message = input["Message"];

            if (username == PendingUsername)
            {
                PendingUsername = "";
                RegistrationPending = false;
                RaisePopupEvent(message);
                RaiseChangeViewEvent(Segment.Right, ViewType.LoginView);
            }
        }

        public void RegistrationUnsuccessful(Dictionary<string, string> input)
        {

            string username = input["Username"];
            string message = input["Message"];


            if (username == PendingUsername) {

                PendingUsername = "";
                RegistrationPending = false;
                RaisePopupEvent(message);

            }

        }


        /// <param name="input">The dictionary produced by the parser from a string received by the socket.</param>

        public void LoginSuccessful(Dictionary<string, string> input)
        {

            string userID = input["UserID"];
            string session = input["SessionID"];
            string username = input["UserName"];
            string message = input["Message"];

            Debug.WriteLine("Making login checks with username " + username + " and session " + session + "\n");

            if (username == PendingUsername)
            {
                Debug.WriteLine("Login checks passed.\n");
                PendingUsername = "";



                UserID = int.Parse(userID);
                DisplayUsername = username;
                SessionID = session;

                // Request the user's friends list

                Debug.WriteLine("Controller sending friends pull request.");


                ConnectionHandler.PullFriends(UserID, SessionID);

             
                RaisePopupEvent(message);
                RaiseChangeViewEvent(Segment.Right, ViewType.MessageView);
                RaiseChangeUsernameEvent(DisplayUsername);
                RaiseChangeViewEvent(Segment.Left, ViewType.FriendsView);


            }
        }


        public void LoginUnsuccessful(Dictionary<string, string> input)
        {

            Debugger.Record("Login unsuccessful called.", DebugMask);
            string message = input["Message"];
            
            PendingUsername = "";
            RaisePopupEvent(message);

        }

        public void FriendsPush(Dictionary<string, string> input)
        {
            Debugger.Record("FriendsPush called with input: " + string.Join(Environment.NewLine, input) + "\n", DebugMask);


            if (VerifyMessage(input) == true) {

                string friendsString = input["Message"];
                List<Dictionary<string, string>> friendsList = new();
                try
                {
                    friendsList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(friendsString);
                }
                catch (Exception e)
                {
                    Debugger.Record("JSON Parsing exception in Controller: " + e.Message, DebugMask + 1);
                    return;
                }

                List<FriendUser> friends = new();

                foreach (Dictionary<string, string> dict in friendsList)
                {
                    FriendUser newFriend = new FriendUser(dict["FriendUserName"], dict["FriendUserID"], dict["PrivateChatID"]);
                    friends.Add(newFriend);
                    Debug.WriteLine("Added friend with ID: " + dict["FriendUserID"]);
                }

                Friends = friends;
                RaiseUpdateFriendsEvent(friends);
            }
        }

        public void ChatPush(Dictionary<string, string> input)
        {

            if (VerifyMessage(input) == true)
            {
                List<Dictionary<string, string>> chatsList;

                try
                {

                    string chatsString = input["Message"];
                    int chatID = int.Parse(input["Message"]);

                    chatsList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(chatsString);


                }
                catch (Exception e)
                {
                    Debugger.Record("JSON Parsing exception in Controller: " + e.Message, DebugMask + 1);
                }
            }
        }



        public void MessagePush(Dictionary<string, string> input)
        {
            Debugger.Record("MessagePush called with input: " + string.Join(Environment.NewLine, input) + "\n", DebugMask);


            if (VerifyMessage(input) == true)
            {
                string messageString = input["Message"];
                int chatID = int.Parse(input["ChatID"]);
                List<Dictionary<string, string>> messageList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(messageString);

                if (ActiveChat == null || ActiveChat.ChatID != chatID)
                {
                    ActiveChat = new Chat(chatID, messageList);
                }

                RaiseUpdateChatEvent();
            }
        }

        public void ChatNotification(Dictionary<string, string> input)
        {

            Debugger.Record("ChatNotification called with input: " + string.Join(Environment.NewLine, input) + "\n", DebugMask);

            if (VerifyMessage(input) == true)
            {
                int chatID = 0;

                try
                {
                    chatID = int.Parse(input["ChatID"]);
                }
                catch (Exception e)
                {
                    Debugger.Record("ChatNotification failed to parse chatID from input: " + e.Message, DebugMask + 1);
                }

                if (chatID == ActiveChat.ChatID && chatID != 0)
                {
                    ConnectionHandler.PullMessagesForChat(UserID, SessionID, chatID);
                }

            }

        }

        public void ReceiveHeartbeat(Dictionary<string, string> input)
        {
            ConnectionHandler.TransmissionHandler("HB");
        }

        private bool VerifyMessage(Dictionary<string, string> input)
        {
            string userID = input["UserID"];
            string session = input["SessionID"];

            if ((userID == UserID.ToString()) && (session == SessionID))
            {
                Debugger.Record("Message verified.", DebugMask);
                return true;
            }

            return false;
        }


        public string GetUsername()
        {
            return DisplayUsername;
        }

        public void ChatSelected(int chatID)
        {

            Debug.WriteLine("ChatSelected called in controller.");

            ConnectionHandler.PullMessagesForChat(UserID, SessionID, chatID);
            RaiseChangeViewEvent(Segment.Right, ViewType.MessageView);

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

        // Called to update the list of friends in the Friends control.

        public delegate void UpdateFriendsEventHandler(object sender, UpdateFriendsEventArgs e);

        public event UpdateFriendsEventHandler UpdateFriendsEvent;

        public void RaiseUpdateFriendsEvent(List<FriendUser> friends)
        {
            Debug.WriteLine("Raise update friends called.\n");
            UpdateFriendsEvent?.Invoke(this, new UpdateFriendsEventArgs(friends));
        }


        // Called to update the message display in the Message control.

        public delegate void UpdateChatEventHandler(object sender, UpdateChatEventArgs e);

        public event UpdateChatEventHandler UpdateChatEvent;

        public void RaiseUpdateChatEvent()
        {
            UpdateChatEvent?.Invoke(this, new UpdateChatEventArgs());
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
