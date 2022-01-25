using Messenger_Client.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger_Client
{

    /// <summary> 
    /// This class is used to contain definitions for the various args and enums utilized by the rest of the
    /// program.
    /// 
    /// </summary>

    // Defines the opcode components for parsing messages.

    public enum Opcodes
    {
        UserID = 1,
        UserName = 2,
        Password = 4,
        SessionID = 8,
        ChatID = 16
    }


    // Enumerates the permissions granted a user in a particular chatroom.
    public enum Permissions
    {
        Read = 1,
        Talk = 2,
        Upload = 4,
        Delete = 8,
        Restrict = 16,
        Mute = 32,
        Ban = 64,
        Owner = 128
    }


    // Enumerates the activity levels available to a user.
    public enum Activity
    {
        Active = 0,
        Busy = 1,
        Offline = 2
    }



    // Enumerates the various segments where content can be displayed. Currently there are only left and right segments.
    public enum Segment
    {
        Left = 0,
        Right = 1
    }

    // Enumerates which View to display inside the selected segment.
    // Current options are LoginView, RegisterView, MessageView, and FriendsView. These correspond to the similarly named Controls.

    public enum ViewType
    {
        LoginView = 0,
        RegisterView = 1,
        FriendsView = 2,
        MessageView = 3,
        FindFriendsView = 4,
        ChatsView = 5,
        RequestsView = 6,
        Blank = 7
    }

    // Defining the events for an UpdateRequests event
    /// <param name="Requests">A list of dictionaries containing friend requests reported from the server.</param>    

    public class UpdateRequestsEventArgs
    {
        public UpdateRequestsEventArgs(List<Dictionary<string, string>> requests) { Requests = requests; }
        public List<Dictionary<string, string>> Requests { get; }
    }

    // Defining the arguments for a ChangeView event

    /// <param name="segment">The segment of the main window that will be updated. Options are left or right.</param>
    /// <param name="viewType">The view type to be utilized. Options are login, register, friends, and messages.</param>
    public class ChangeViewEventArgs
    {
        public ChangeViewEventArgs(Segment segment, ViewType viewType)
        {
            Segment = segment; 
            ViewType = viewType; 
        }
        public Segment Segment { get; }
        public ViewType ViewType { get; }
    }

    // Defining the arguments for a SelectionPopupEvent

    /// <param name=message">The message that will be displayed by the popup</param>
    /// <param name="optionOneText">The text displayed for option one</param>
    /// <param name="optionTwoText">The text displayed for option two</param>
    /// <param name="OptionOneTag">The function to be called if option one is selected.</param>
    /// <param name="optionTwoTag">The function to be called if option two is selected.</param>
    /// <param name="Parameter">The parameter provided to the functions called.</param>
    public class SelectionPopupEventArgs
    {
        public SelectionPopupEventArgs(string message, string optionOneText, string optionOneTag, string optionTwoText, string optionTwoTag, string parameterOne, string parameterTwo) 
        { 
            Message = message;
            OptionOneText = optionOneText;
            OptionOneTag = optionOneTag;
            OptionTwoText = optionTwoText;
            OptionTwoTag = optionTwoTag;
            ParameterOne = parameterOne;
            ParameterTwo = parameterTwo;


        }
        public string Message { get; }
        public string OptionOneText { get; }
        public string OptionTwoText { get; }
        public string OptionOneTag { get; }
        public string OptionTwoTag { get; }
        public string ParameterOne { get; }
        public string ParameterTwo { get; }

    }

    // Defining the arguments for a NotificationPopupEvent

    /// <param name=message">The message that will be displayed by the popup</param>
    public class NotificationPopupEventArgs
    {
        public NotificationPopupEventArgs(string message) { Message = message; }
        public string Message { get; }
    }

    // Defining the arguments for a UpdateUserSearch event.
    public class UpdateUserSearchEventArgs
    {
        public UpdateUserSearchEventArgs(List<Dictionary<string, string>> results) { Results = results; }

        public List<Dictionary<string, string>> Results { get; }
    }


    // Defining the arguments for a FriendsUpdate event.
    public class UpdateFriendsEventArgs
    {
        public UpdateFriendsEventArgs(List<FriendUser> friends) { Friends = friends; }
        public List<FriendUser> Friends { get; }
    }


    /// <summary> 
    /// Defining the arguments for an UpdateChat event, which sends the selection
    /// of received user-chat pairs to the Chats View.
    /// </summary>
    public class UpdateChatEventArgs
    {
        public UpdateChatEventArgs(List<Chat> chatList) { ChatList = chatList; }

        public List<Chat> ChatList { get; }
    }


    /// <summary> 
    /// Defining the arguments for a ChangeChat event, which signals the 
    /// MessageControl to update its display by getting the ActiveChat from the
    /// Controller.
    /// </summary>
    /// 
    public class ChangeChatEventArgs
    {
        public ChangeChatEventArgs() { }

    }


    // Defining the arguments for a MessageEvent.

    public class MessageEventArgs
    {
        public MessageEventArgs(string message) { Message = message; }
        public string Message { get; }
    }


}
