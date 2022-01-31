using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger_Client.SupportClasses
{
    public class Request
    {

        public int RequestID { get; private set; }

        public string SenderName { get; private set; }

        public string ReceiverName { get; private set; }

        public string RequestType { get; private set; }

        public string ChatName { get; private set; }

        public bool IsInbox { get; private set; }  = false;

        // Used to store a message to display in the UI.
        public string DisplayMessage { get; private set; }


        public Request(Dictionary<string, string> requestDict)
        {
            RequestID = int.Parse(requestDict["RequestID"]);
            int senderID = int.Parse(requestDict["UserID"]);
            SenderName = requestDict["UserName"];
            ReceiverName = requestDict["FriendUserName"];
            RequestType = requestDict["RequestType"];

            if (RequestType == "INVITE")
            {

                ChatName = requestDict["ChatName"];

                if (senderID == Controller.UserID)
                {
                    DisplayMessage = "You invited " + SenderName + " to \"" + ChatName + "\"";
                }
                else
                {
                    DisplayMessage = "You have been invited to \"" + ChatName + "\" by " + SenderName;
                    IsInbox = true;
                }

            }
            else if (RequestType == "FRIEND")
            {

                if (senderID == Controller.UserID)
                {
                    DisplayMessage = "You asked " + SenderName + " to be your friend";
                }
                else
                {
                    DisplayMessage = SenderName + " wants to be your friend";
                    IsInbox = true;
                }

            }
        }
    }
}
