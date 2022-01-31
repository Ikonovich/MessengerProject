using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger_Client.SupportClasses
{

    // A class that stores the data about individual messages for display.

    public class Message
    {
        public string MessageID { get; private set; }
        public string SenderID { get; private set; }
        public string SenderName { get; private set; }
        public string CreateTimestamp { get; private set; }

        private string body;
        public string Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
                OnPropertyChanged(nameof(Body));
            }
        }


        // Determines whether or not the 'Delete' button shows up
        // in the drop down for this message.
        public Visibility DeleteVisibility { get; private set; }

        // Controls whether or not this message can be deleted by the current user. 

        public Message(Dictionary<string, string> messageDict)
        {

            MessageID = messageDict["MessageID"];
            SenderID = messageDict["SenderID"];
            SenderName = messageDict["SenderName"];
            CreateTimestamp = messageDict["CreateTimestamp"];
            Body = messageDict["Message"];

            Controller controller = Controller.ControllerInstance;
            int permissions = Controller.ActiveChat.PermissionMask;

            int intSenderID = 0;

            try
            {
                intSenderID = int.Parse(SenderID);
            }
            catch
            {
                Debugger.Record("A message contains an unparseable sender ID.", 1);
            }

            if (((permissions & (int)Permissions.Delete) == (int)Permissions.Delete) || (intSenderID == Controller.UserID))
            {
                DeleteVisibility = Visibility.Visible;
            }
            else
            {
                DeleteVisibility = Visibility.Collapsed;
            }
        }

        //INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            Debugger.Record("Property change called for: " + propertyName, 2);
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
