using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger_Client.SupportClasses
{
    /// <summary> 
    /// This chat contains all of the information relevant to a single Friend.
    /// This includes the username, userID, activity, and the associated private chat. 
    /// </summary>
    public class FriendUser
    {



        private int DebugMask = 32;

        public string UserName { get; private set; }
        public int UserID { get; private set; }

        // Refers to the automatically-generated private chat this user shares with the logged in user.

        public int ChatID { get; private set; }

        public int FriendRanking { get; private set; }

        public Activity Activity { get; private set; }

        public FriendUser(string username, string userID, string chatID)
        {
            UserName = username;
            Activity = Activity.Active;
            FriendRanking = 1;

            try
            {
                UserID = int.Parse(userID);
                ChatID = int.Parse(chatID);
            }
            catch (Exception e)
            {
                Debugger.Record("Unable to parse user or chat ID in friend constructor.", DebugMask);
            }
        }

    }
}
