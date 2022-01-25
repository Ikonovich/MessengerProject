using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger_Client.SupportClasses
{


    /// <summary> 
    /// This class contains the relevant information for a single chat that has been pulled by the client.
    /// This includes the ChatID and Messages, which are automatically sorted by date from oldest to newest.
    /// 
    /// Raises an UpdateMessagesEvent when new messages are provided.
    /// </summary>
    /// 
    public class Chat
    {
        public int ChatID { get; private set; } // Stores the chat's unique identifier.

        public int PermissionMask { get; set; } = 0; // Stores the permissions the currently logged in user has in the particular chat.

        public string ChatName { get; private set; }

        public List<Message> Messages { get; private set; } // Stores all messages currently on-hand for this chat.

        private int LastRetrieved; // Stores the index of the last message that was gotten via Retrieve();

        private int DebugMask = 32; // Debug mask for support classes.

        /// <param name="chatID">The ID of the chat, provided by the server.</param>
        /// <param name="messages">The initializing set of messages of the chat, provided by the server.</param>
        public Chat(int chatID, string chatName, List<Dictionary<string, string>> messages)
        {
            ChatName = chatName;
            ChatID = chatID;
            messages.Sort(CompareByDate);
            Messages = new List<Message>();

            for (int i = 0; i < messages.Count; i++)
            {
                Messages.Add(new Message(messages[i]));
            }
        }

        public void ReplaceMessages(List<Dictionary<string, string>> newMessages)
        {
            Messages = new List<Message>();
           
            for (int i = 0; i < newMessages.Count; i++)
            {
                Messages.Add(new Message(newMessages[i]));
            }
        }
        public void UpdateMessages(List<Dictionary<string, string>> newMessages)
        {

            newMessages.Sort(CompareByDate);

            for (int i = 0; i < newMessages.Count; i++)
            {
                Messages.Add(new Message(newMessages[i]));
            }

        }

        public List<Message> RetrieveAll()
        {
            return Messages;
        }

        public List<Message> RetrieveNew()
        {
            List<Message> freshMessages = new();

            Debugger.Record("Retrieving messages from chat.", DebugMask);

            if (LastRetrieved == 0)
            {
                Debugger.Record("Retrieving messages from chat. Message count:" + Messages.Count, DebugMask);
                LastRetrieved = Messages.Count;
                return Messages;
            }
            else
            {
                for (int i = LastRetrieved; i < Messages.Count; i++)
                {
                    freshMessages.Add(Messages[i]);
                    LastRetrieved++;
                }
            }
            Debugger.Record("Retrieving messages from chat. Message count:" + freshMessages.Count, DebugMask);

            return freshMessages;

        }

        /// <summary> 
        /// A comparator used for sorting messages by date. Returns DateTime.Compare() on the timestamps in the maps of each  message.
        /// Both inputs must have a "CreateTimestamp" key.
        /// </summary>
        /// <param name="messageOne">The first message dictionary to compare.</param>
        /// <param name="messageTwo">The second message dictionary to compare.</param>

        private int CompareByDate(Dictionary<string, string> messageOne, Dictionary<string, string> messageTwo)
        {
            try
            {
                string dateOneString = messageOne["CreateTimestamp"];
                string dateTwoString = messageTwo["CreateTimestamp"];

                DateTime dateOne = DateTime.ParseExact(dateOneString, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dateTwo = DateTime.ParseExact(dateTwoString, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                return DateTime.Compare(dateOne, dateTwo);

            }
            catch (Exception e)
            {
                Debugger.Record("CompareByDate has failed: " + e.Message, DebugMask);
                return 0;
            }

        }


        // Begin event handling //

        public delegate void UpdateMessagesEventHandler(object sender, MessageEventArgs e);

        public event UpdateMessagesEventHandler UpdateMessagesEvent;
        private void RaiseUpdateMessagesEvent()
        {
            UpdateMessagesEvent?.Invoke(this, new MessageEventArgs(""));
        }

    }
}
