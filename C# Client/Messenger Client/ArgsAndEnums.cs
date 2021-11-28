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
        MessageView = 3
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

    // Defining the arguments for a PopupEvent

    /// <param name=message">The message that will be displayed by the popup</param>
    public class PopupEventArgs
    {
        public PopupEventArgs(string message) { Message = message; }
        public string Message { get; }
    }

    // Defining the arguments for an FriendsUpdate event.
    public class UpdateFriendsEventArgs
    {
        public UpdateFriendsEventArgs(List<string> friends) { Friends = friends; }

        public List<string> Friends { get; }
    }

    // Defining the arguments for a MessageEvent.

    public class MessageEventArgs
    {
        public MessageEventArgs(string message) { Message = message; }
        public string Message { get; }
    }


}
