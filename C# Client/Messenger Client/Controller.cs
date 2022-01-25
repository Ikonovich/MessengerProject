using Messenger_Client.SupportClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Messenger_Client
{

    public class Controller : INotifyPropertyChanged
    {

        int DebugMask = 2;

        // Determines how long the controller should wait for the GUI before connecting.

        private const int GUI_WAIT = 5000; 

        public static Chat ActiveChat { get; private set; } // Stores the currently active chat.

        private ConnectionHandler ConnectionHandler;
        private MainWindow MainWindow;

        private Dictionary<string, string> CodeToRequestMap = new();


        public int UserID { get; private set; } = 0;
        public string SessionID { get; private set; } = "";

        public List<FriendUser> Friends{ get; private set; }

        private List<Chat> Chats;


        private string PendingUsername = "";


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
        }

        /// <summary> 
        /// The Controller is instantiated and activated by this function, when called by
        /// MainWindow, so that connecting happens only after the GUI has
        /// had time to spin up.
        /// </summary>
        public void Connect()
        {
            ConnectionHandler = new ConnectionHandler(this);
            ConnectionHandler.Connect();
        }

        /// <param name="username">The supplied username</param>
        /// <param name="password">The supplied password</param>
        public void Register(string username, string password)
        {
            RaiseChangeViewEvent(Segment.Right, ViewType.RegisterView);
            PendingUsername = username;
            ConnectionHandler.Register(username, password);
        }

        /// <param name="username">The supplied username</param>
        /// <param name="password">The supplied password</param>
        public void Login(string username, string password)
        {


            // Parameter checking //

            if (username.Length < 8)
            {
                RaiseNotificationPopupEvent("Username must be at least 8 characters.");
                return;
            }
            else if (username.Length > 32)
            {
                RaiseNotificationPopupEvent("Username must be no more than 32 characters.");
            }

            // Performs a regex match to ensure only underscores and alphanumeric characters are in the username.
            Regex reg = new Regex("^A - Za - z\\d_", RegexOptions.None);

            if (reg.Match(username).Success == true)
            {
                RaiseNotificationPopupEvent("Username must contain only Alphanumeric characters or underscores.");
                return;
            }
            if (password.Contains("*"))
            {
                RaiseNotificationPopupEvent("Password must not contain asterisks.");
                return;
            }
            if (password.Length < 8)
            {
                RaiseNotificationPopupEvent("Password must be at least 8 characters.");
                return;
            }
            if (password.Length > 128)
            {
                RaiseNotificationPopupEvent("Password must be 128 characters or less.");
                return;
            }

            Debugger.Record("Controller sending login request to connection handler with username " + username, DebugMask);

            RaiseNotificationPopupEvent("Attempting to login..");

            PendingUsername = username;
            ConnectionHandler.Login(username, password);

        }

        public void Logout()
        {
            if (SessionID != "")
            {
                ConnectionHandler.Logout(UserID, SessionID);
            }
        }

        public void SendMessage(string message)
        {
            ConnectionHandler.SendMessage(UserID, DisplayUsername, SessionID, ActiveChat.ChatID, message);
        }

        public void EditMessage(int messageID, string message)
        {
            ConnectionHandler.EditMessage(UserID, SessionID, messageID, message);
        }

        public void DeleteMessage(string messageID)
        {
            ConnectionHandler.DeleteMessage(UserID, SessionID, messageID);
        }



        /// <param name="friendUserName">The username of the friend to be added.</param>
        /// <param name="friendUserID">The user ID of the friend to be added.</param>
        public void SendFriendRequest(string friendUserName, string friendUserID)
        {
            ConnectionHandler.SendFriendRequest(UserID, SessionID, friendUserID);
        }


        public void UserSearch(string searchString)
        {
            ConnectionHandler.UserSearch(UserID, searchString, SessionID);
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
                RaiseNotificationPopupEvent("Opcode not found in message dictionary.\n");
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
                case "LO":
                    LogoutSuccessful(input);
                    break;
                case "FP":
                    FriendsPush(input);
                    break;
                case "FR":
                    FriendRequestsPush(input);
                    break;
                case "MP":
                    MessagePush(input);
                    break;
                case "CP":
                    ChatsPush(input);
                    break;
                case "CN":
                    ChatNotification(input);
                    break;
                case "UR":
                    UserSearchResults(input);
                    break;
                case "AM":
                    AdministrativeMessage(input);
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
        public void AdministrativeMessage(Dictionary<string, string> input)
        {
            string message = input["Message"];
            RaiseNotificationPopupEvent(message);
        }

        /// <param name="input">The dictionary produced by the parser from a string received by the socket.</param>
        public void RegistrationSuccessful(Dictionary<string, string> input)
        {
            string username = input["UserName"];
            string message = input["Message"];

            if (username == PendingUsername)
            {
                PendingUsername = "";
                RaiseNotificationPopupEvent(message);
                RaiseChangeViewEvent(Segment.Right, ViewType.LoginView);
            }
        }

        public void RegistrationUnsuccessful(Dictionary<string, string> input)
        {

            string username = input["UserName"];
            string message = input["Message"];


            if (username == PendingUsername) {

                PendingUsername = "";
                RaiseNotificationPopupEvent(message);

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
                ConnectionHandler.PullRequests(UserID, SessionID);
                ConnectionHandler.PullChats(UserID, SessionID);

                RaiseNotificationPopupEvent(message);
                RaiseChangeUsernameEvent(DisplayUsername);
                RaiseChangeViewEvent(Segment.Left, ViewType.FriendsView);
                RaiseChangeViewEvent(Segment.Right, ViewType.Blank);
                RaiseChangeBlankDisplayEvent("To begin, click the drop down and select 'Find Friends'.");
            }
        }


        public void LoginUnsuccessful(Dictionary<string, string> input)
        {

            Debugger.Record("Login unsuccessful called.", DebugMask);
            string message = input["Message"];
            
            PendingUsername = "";
            RaiseNotificationPopupEvent(message);

        }

        public void LogoutSuccessful(Dictionary<string, string> input)
        {

            Debugger.Record("Logout successful called.", DebugMask);
            string message = input["Message"];

            DisplayUsername = "Not logged in";
            SessionID = "";
            UserID = 0;

            RaiseChangeViewEvent(Segment.Right, ViewType.LoginView);
            RaiseChangeViewEvent(Segment.Left, ViewType.Blank);
            RaiseChangeUsernameEvent("NONE");
            RaiseNotificationPopupEvent(message);
            RaiseChangeBlankDisplayEvent("");

        }

        public void FriendRequestsPush(Dictionary<string, string> input)
        {

            Debugger.Record("FriendsRequestsPush called with input: " + string.Join(Environment.NewLine, input) + "\n", DebugMask);

            if (VerifyMessage(input) == true)
            {
                List<Dictionary<string, string>> friendRequests = new();

                try
                {
                    string requestsString = input["Message"];
                    friendRequests = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(requestsString);
                    RaiseUpdateRequestsEvent(friendRequests);
                }
                catch (Exception e)
                {
                    Debugger.Record("JSON parsing exception in friend requests: " + e.Message, DebugMask);
                }
            }
        }

        public void FriendsPush(Dictionary<string, string> input)
        {
            Debugger.Record("FriendsPush called with input: " + string.Join(Environment.NewLine, input) + "\n", DebugMask);


            if (VerifyMessage(input) == true) {

                List<Dictionary<string, string>> friendsList = new();
                try
                {

                    string friendsString = input["Message"];
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

        public void ChatsPush(Dictionary<string, string> input)
        {

            if (VerifyMessage(input) == true)
            {

                List<Dictionary<string, string>> tempList;
                List<Chat> chatList;

                try
                {
                    string chatsString = input["Message"];

                    tempList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(chatsString);

                    chatList = new();

                    foreach (Dictionary<string, string> chat in tempList)
                    {
                        string chatName = chat["ChatID"];
                        int chatID = int.Parse(chat["ChatID"]);

                        chatList.Add(new Chat(chatID, chatName, new List<Dictionary<string, string>>()));
                    }

                    Debug.WriteLine("Pushing chats");

                    RaiseUpdateChatEvent(chatList);
                }
                catch (Exception e)
                {
                    Debugger.Record("JSON Parsing exception in Controller: " + e.Message, DebugMask + 1);
                }
            }
        }





        public void MessagePush(Dictionary<string, string> input)
        {
            // Debugger.Record("MessagePush called with input: " + string.Join(Environment.NewLine, input) + "\n", DebugMask);
            Debugger.Record("MessagePush called with input: " + input["Message"] + "\n", DebugMask);

            if (VerifyMessage(input) == true)
            {


                List<Dictionary<string, string>> messageList;
                int chatID = int.Parse(input["ChatID"]);

                try
                {
                    string messageString = input["Message"];

                    if (messageString.Length == 0)
                    {
                        messageList = new List<Dictionary<string, string>>();
                    }
                    else
                    {
                        messageList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(messageString);
                    }
                }
                catch (Exception e)
                {
                    Debugger.Record("MessagePush failed to parse message: " + e.Message, DebugMask);
                    return;
                }
                if (ActiveChat == null || ActiveChat.ChatID != chatID)
                {
                    ActiveChat = new Chat(chatID, "No Chat Name", messageList);
                }
                else
                {
                    ActiveChat.ReplaceMessages(messageList);
                }

                RaiseChangeChatEvent();
            }
        }

        public void UserSearchResults(Dictionary<string, string> input)
        {
            Debugger.Record("User Search Results called with input: " + string.Join(Environment.NewLine, input) + "\n", DebugMask);

            string messageString = input["Message"];

            List<Dictionary<string, string>> resultsList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(messageString);
     
            RaiseUpdateUserSearchEvent(resultsList);

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

        public void ChatSelected(int chatID, string chatName)
        {

            Debug.WriteLine("ChatSelected called in controller: " + chatID);



            ActiveChat = new Chat(chatID, chatName, new List<Dictionary<string, string>>());

            ConnectionHandler.PullMessagesForChat(UserID, SessionID, chatID);
            RaiseChangeViewEvent(Segment.Right, ViewType.MessageView);
            RaiseChangeChatEvent();

        }


        // ---------- BEGIN EVENT DELEGATES --------- //

        // Called to update visible requests.
        public delegate void UpdateRequestsEventHandler(object sender, UpdateRequestsEventArgs e);

        public event UpdateRequestsEventHandler UpdateRequestsEvent;

        /// <param name="requests">A list of dictionaries containing the requests, as parsed from the incoming transmission</param>
        public void RaiseUpdateRequestsEvent(List<Dictionary<string, string>> requests)
        {
            UpdateRequestsEvent?.Invoke(this, new UpdateRequestsEventArgs(requests));
        }

        // Called to display a selection popup.

        public delegate void SelectionPopupEventHandler(object sender, SelectionPopupEventArgs e);

        public event SelectionPopupEventHandler SelectionPopupEvent;

        /// <param name="message">The message that will be displayed by the popup.</param>
        public void RaiseSelectionPopupEvent(string message, string optionOneText, string optionOneTag, string optionTwoText, string optionTwoTag, string parameterOne, string parameterTwo)
        {
            Debug.WriteLine("Raise popup called.\n");

            Debug.WriteLine("Raise Selection Pop up event sent from controller. Option one: " + optionOneText + " Option Two: " + optionTwoText);


            SelectionPopupEvent?.Invoke(this, new SelectionPopupEventArgs(message, optionOneText, optionOneTag, optionTwoText, optionTwoTag, parameterOne, parameterTwo));
        }


        // Called to display a notification popup.

        public delegate void NotificationPopupEventHandler(object sender, NotificationPopupEventArgs e);

        public event NotificationPopupEventHandler NotificationPopupEvent;

        /// <param name="message">The message that will be displayed by the popup.</param>
        public void RaiseNotificationPopupEvent(string message)
        {
            Debug.WriteLine("Raise popup called.\n");

            NotificationPopupEvent?.Invoke(this, new NotificationPopupEventArgs(message));

        }

        // Called to change the display text in the blank control.

        public delegate void ChangeBlankDisplayEventHandler(object sender, MessageEventArgs args);

        public event ChangeBlankDisplayEventHandler ChangeBlankDisplayEvent;

        /// <param name="message">The message that will be displayed
        public void RaiseChangeBlankDisplayEvent(string message)
        {
            Debug.WriteLine("Raise change view called.\n");

            ChangeBlankDisplayEvent?.Invoke(this, new MessageEventArgs(message));

        }


        // Called to change the view in the main window.

        public delegate void ChangeViewEventHandler(object sender, ChangeViewEventArgs args);

        public event ChangeViewEventHandler ChangeViewEvent;

        /// <param name="segment">Determines which segment of the main window will hold the requested view.</param>
        /// <param name="view">Determines which view will be applied to the segment</param>

        public void RaiseChangeViewEvent(Segment segment, ViewType view)
        {
            Debug.WriteLine("Raise change view called.\n");

            ChangeViewEvent?.Invoke(this, new ChangeViewEventArgs(segment, view));

        }

        // Called to update the list of user search results in FindFriends view.

        public delegate void UpdateUserSearchEventHandler(object sender, UpdateUserSearchEventArgs e);

        public event UpdateUserSearchEventHandler UpdateUserSearchEvent;
        public void RaiseUpdateUserSearchEvent(List<Dictionary<string, string>> resultsList)
        {
            UpdateUserSearchEvent?.Invoke(this, new UpdateUserSearchEventArgs(resultsList));
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

        // Called to update the chat display in the Chats View control.

        public delegate void UpdateChatEventHandler(object sender, UpdateChatEventArgs e);

        public event UpdateChatEventHandler UpdateChatEvent;

        public void RaiseUpdateChatEvent(List<Chat> chatList)
        {
            UpdateChatEvent?.Invoke(this, new UpdateChatEventArgs(chatList));
        }



        // Called to update the message display in the Message control.

        public delegate void ChangeChatEventHandler(object sender, ChangeChatEventArgs e);

        public event ChangeChatEventHandler ChangeChatEvent;

        public void RaiseChangeChatEvent()
        {
            ChangeChatEvent?.Invoke(this, new ChangeChatEventArgs());
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
