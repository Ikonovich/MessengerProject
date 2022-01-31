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

        // Defines the allowable sizes of certain entities.
        public const int MaxUserNameLength = 32;
        public const int MinUserNameLength = 8;
        public const int MaxUserIDLength = 32;
        public const int MaxPasswordLength = 128;
        public const int MinPasswordLength = 8;
        public const int ChatIDLength = 8;
        public const int MaxChatNameLength = 32;
        public const int MinChatNameLength = 8;
        public const int MaxMessageIDLength = 32;


        // Determines how long the controller should wait for the GUI before connecting.

        private const int GUI_WAIT = 5000;



        public static Chat ActiveChat { get; private set; } // Stores the currently active chat.

        public static List<FriendUser> FriendsList { get; private set; } // Stores the current user's friends.

        private static List<Chat> ChatList; // Stores all available chatpairs.

        private ConnectionHandler ConnectionHandler;
        private MainWindow MainWindow;

        private static Dictionary<string, string> CodeToRequestMap = new();


        public static int UserID { get; private set; } = 0;
        public static string SessionID { get; private set; } = "";


        private static string PendingUsername = "";


        // Username control bindings

        private static string username = "Not Logged In";
        public string DisplayUsername
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged("DisplayUsername");
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
            FriendsList = new List<FriendUser>();
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

            if (username.Length < MinUserNameLength)
            {
                RaiseNotificationPopupEvent("Username must be at least 8 characters.");
                return;
            }
            else if (username.Length > MaxUserNameLength)
            {
                RaiseNotificationPopupEvent("Username must be no more than 32 characters.");
                return;
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
            if (password.Length < MinPasswordLength)
            {
                RaiseNotificationPopupEvent("Password must be at least 8 characters.");
                return;
            }
            if (password.Length > MaxPasswordLength)
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


        /// <param name="friendUserName">The username of the friend to be added.</param>
        /// <param name="friendUserID">The user ID of the friend to be added.</param>
        public void ApproveRequest(int requestID)
        {
            ConnectionHandler.ApproveRequest(UserID, SessionID, requestID);
        }

        /// <summary> 
        /// Searches through friend lists for users with names containing a specific string.
        /// </summary>
        /// <param name="searchString">The string that will be looked for in usernames.</param>
        public List<FriendUser> FriendSearch(string searchString)
        {
            List<FriendUser> results = new();

            for (int i = 0; i < FriendsList.Count; i++)
            {

                FriendUser tempFriend = FriendsList[i];
                string tempName = tempFriend.UserName.ToLower();

                if (tempName.Contains(searchString))
                {
                    results.Add(tempFriend);
                }
            }

            Debugger.Record("Friend search for " + searchString + " found " + results.Count + " friends.", DebugMask);
            return results;
        }

        /// <summary> 
        /// Sends a request to search the database for usernames containing a string.
        /// </summary>
        /// <param name="searchString">The string that will be looked for in usernames.</param>

        public void UserSearch(string searchString)
        {
            ConnectionHandler.UserSearch(UserID, searchString, SessionID);
        }

        public bool CreateChat(string newChatName)
        {

            // Parameter checking //


            if (newChatName.Length < ChatIDLength)
            {
                RaiseNotificationPopupEvent("Chat title must be at least 8 characters.");
                return false;
            }
            else if (newChatName.Length > MaxChatNameLength)
            {
                RaiseNotificationPopupEvent("Chat title must be no more than 32 characters.");
                return false;
            }

            // Performs a regex match to ensure no asterisks are in the chat name.
            Regex reg = new Regex("[*]", RegexOptions.None);

            if (reg.Match(newChatName).Success == true)
            {
                RaiseNotificationPopupEvent("Chat title must contain only Alphanumeric characters or underscores.");
                return false;
            }

            ConnectionHandler.CreateChat(UserID, DisplayUsername, SessionID, newChatName);

            return true;
        }

        public void SendChatInvitation(string friendUserID, int chatID)
        {
            ConnectionHandler.SendChatInvitation(UserID, SessionID, friendUserID, chatID);
        }


        //----------BEGIN MESSAGE HANDLING CODE ------------- //


        /// <param name="input">The dictionary produced by the parser from a string received by the socket.</param>
        public void MessageHandler(Dictionary<string, string> input)
        {
            Debugger.Record("MessageHandler called with input: " + string.Join(Environment.NewLine, input), DebugMask);


            string opcode = "";
            try
            {
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
                case "RP":
                    RequestsPush(input);
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


            if (username == PendingUsername)
            {

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

        public void RequestsPush(Dictionary<string, string> input)
        {

            Debug.WriteLine("RequestsPush called with input: " + string.Join(Environment.NewLine, input));

            if (VerifyMessage(input) == true)
            {
                List<Dictionary<string, string>> requests = new();

                try
                {
                    string requestsString = input["Message"];
                    requests = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(requestsString);
                    RaiseUpdateRequestsEvent(requests);
                }
                catch (Exception e)
                {
                    Debugger.Record("JSON parsing exception in requests: " + e.Message, DebugMask);
                }
            }
        }

        public void FriendsPush(Dictionary<string, string> input)
        {
            Debugger.Record("FriendsPush called with input: " + string.Join(Environment.NewLine, input) + "\n", DebugMask);


            if (VerifyMessage(input) == true)
            {

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

                FriendsList = friends;
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
                        string chatName = chat["ChatName"];
                        int chatID = int.Parse(chat["ChatID"]);
                        bool isMember = int.Parse(chat["IsMember"]) == 1 ? true : false;

                        chatList.Add(new Chat(chatID, chatName, isMember, new List<Dictionary<string, string>>()));
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
                    ActiveChat = new Chat(chatID, "No Chat Name", false, messageList);
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

            List<Dictionary<string, string>> resultsList = new();

            try
            {
                resultsList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(messageString);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Json conversion on user search results failed: " + e.Message);
            }

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



        public void ChatSelected(int chatID)
        {

            Debug.WriteLine("ChatSelected called in controller: " + chatID);


            for (int i = 0; i < ChatList.Count; i++)
            {
                if (ChatList[i].ChatID == chatID)
                {
                    ActiveChat = ChatList[i];
                }
            }

            ConnectionHandler.PullMessagesForChat(UserID, SessionID, chatID);
            RaiseChangeViewEvent(Segment.Right, ViewType.MessageView);
            RaiseChangeChatEvent();

        }

        public void FriendSelected(int userID)
        {

            Debug.WriteLine("FriendSelected called in controller: " + userID);

            for (int i = 0; i < FriendsList.Count; i++)
            {
                FriendUser tempFriend = FriendsList[i];

                if (tempFriend.UserID == userID)
                {

                    ActiveChat = new Chat(tempFriend.ChatID, "Chat with " + tempFriend.UserName, false, new List<Dictionary<string, string>>());

                    ConnectionHandler.PullMessagesForChat(UserID, SessionID, tempFriend.ChatID);
                    RaiseChangeViewEvent(Segment.Right, ViewType.MessageView);
                    RaiseChangeChatEvent();

                    break;
                }
            }
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

        public delegate void NotificationPopupEventHandler(object sender, NotificationPopupEventArgs args);

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

        public delegate void UpdateUserSearchEventHandler(object sender, UpdateUserSearchEventArgs args);

        public event UpdateUserSearchEventHandler UpdateUserSearchEvent;
        public void RaiseUpdateUserSearchEvent(List<Dictionary<string, string>> resultsList)
        {
            UpdateUserSearchEvent?.Invoke(this, new UpdateUserSearchEventArgs(resultsList));
        }

        // Called to change the username displayed by the main window.

        public delegate void ChangeUsernameEventHandler(object sender, MessageEventArgs args);

        public event ChangeUsernameEventHandler ChangeUsernameEvent;

        public void RaiseChangeUsernameEvent(string username)
        {
            ChangeUsernameEvent?.Invoke(this, new MessageEventArgs(username));
        }

        // Called to update the list of friends in the Friends control.

        public delegate void UpdateFriendsEventHandler(object sender, UpdateFriendsEventArgs args);

        public event UpdateFriendsEventHandler UpdateFriendsEvent;

        public void RaiseUpdateFriendsEvent(List<FriendUser> friends)
        {
            Debug.WriteLine("Raise update friends called.\n");
            FriendsList = friends;
            UpdateFriendsEvent?.Invoke(this, new UpdateFriendsEventArgs(friends));
        }

        // Called to update the chat display in the Chats View control.

        public delegate void UpdateChatEventHandler(object sender, UpdateChatEventArgs args);

        public event UpdateChatEventHandler UpdateChatEvent;

        public void RaiseUpdateChatEvent(List<Chat> chatList)
        {
            ChatList = chatList;
            UpdateChatEvent?.Invoke(this, new UpdateChatEventArgs(chatList));
        }



        // Called to update the message display in the Message control.

        public delegate void ChangeChatEventHandler(object sender, ChangeChatEventArgs args);

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
