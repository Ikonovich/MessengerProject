using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;

namespace Messenger_Client
{

    /// <summary> 
    /// 
    /// This class is a singleton that handles the server connection and stores 
    /// information about the currently active user.
    ///
    /// The core server opcodes (What this client transmits) with their bitmasks are outlined in the
    /// Message Definitions documentation. 
    /// </summary>


    public class ConnectionHandler
    {

        Controller Controller;
        int DebugMask = 4;

        // The TransmissionBuffer storess partial transmissions to be concatenated.
        // The StateBuffer stores the state of each transmission type. 
        // Together they can handle one transmission from each of the various types simultaneously. 

        Dictionary<string, string> TransmissionBuffer = new Dictionary<string, string>();

        private int ByteCount = 0; // Stores the number of bytes received by the most recent transmission.

        // Determines the wait time between reconnect attempts. Should be slightly longer than the server automatic disconnect
        // time. You should update the Connect(), Receive() and MessageReceived() error messages if you change this (They say 5 seconds).
        private const int ReconnectWait = 5100;

        // Connection-related variables
        Socket ServerSocket = null;
        Int32 Port = 4269;

        // Stores the server address
        private const string ServerAddress = "18.216.118.7";

        // The constructor initializes the connection handler listening to the server.
        // It also takes and sets the Controller variable for this class. 



        /// <param name="controller">The Controller singleton</param>
        public ConnectionHandler(Controller controller)
        {
            Controller = controller;
        }


        Byte[] BytesReceived = new byte[1048];

        private static string Username { get; set; } = "Not Logged In";

        /// <param name="username">The supplied username</param>
        /// <param name="password">The supplied password</param>

        public void Register(string username, string password)
        {
            username = Parser.Pack(username, Controller.MaxUserNameLength);
            password = Parser.Pack(password, Controller.MaxPasswordLength);

            TransmissionHandler("IR" + username + password);
        }




        /// <param name="username">The supplied username</param>
        /// <param name="password">The supplied password</param>
        /// 
        public void Login(string username, string password)
        {
            TransmissionHandler("LR" + Parser.Pack(username, Controller.MaxUserNameLength) + Parser.Pack(password, Controller.MaxPasswordLength));
        }


        public void Logout(int userID, string sessionID)
        {
            TransmissionHandler("LO" + Parser.Pack(userID, Controller.MaxUserIDLength) + sessionID);
        }

        /// <summary> 
        /// Sends a server request to push requests for this user, if any.
        /// </summary>
        /// <param name="param">description</param>
        /// <param name="param">description</param>

        public void PullRequests(int userID, string sessionID)
        {

            Debugger.Record("Sending pull requests request.", DebugMask);

            string transmitString = "PR" + Parser.Pack(userID.ToString(), Controller.MaxUserIDLength) + sessionID;

            TransmissionHandler(transmitString);
        }


        /// <param name="userID">The ID of the user whose friends will be pulled</param>
        /// <param name="sessionID">The sessionID used to verify the transmission.</param>					   
        public void PullFriends(int userID, string sessionID)
        {
            Debugger.Record("Sending pull friends request.", DebugMask);

            string transmitString = "PF" + Parser.Pack(userID.ToString(), Controller.MaxUserIDLength) + sessionID;

            TransmissionHandler(transmitString);
        }




        /// <summary> 
        /// Sends a request to create a new chat. The server will assign this user as the owner and with full permissions.
        /// </summary>
        /// <param name="param">description</param>
        /// <param name="param">description</param>

        public void CreateChat(int userID, string userName, string sessionID, string chatName)
        {

            Debugger.Record("Sending create chats request.", DebugMask);

            string transmitString = "CC" + Parser.Pack(userID, Controller.MaxUserIDLength) + Parser.Pack(userName, Controller.MaxUserNameLength) + sessionID + chatName;

            TransmissionHandler(transmitString);
        }


        // This message pulls all of the chats the user is a member of, in the form of User-Chat Pairs.

        public void PullChats(int userID, string sessionID)
        {
            Debugger.Record("Sending pull chats request.", DebugMask);

            string transmitString = "PC" + Parser.Pack(userID, Controller.MaxUserIDLength) + sessionID;

            TransmissionHandler(transmitString);

        }

        public void PullMessagesForChat(int userID, string sessionID, int chatID)
        {
            Debugger.Record("Sending pull messages for chat request.", DebugMask);

            string transmitString = "PM" + Parser.Pack(userID, Controller.MaxUserIDLength) + sessionID + Parser.Pack(chatID, Controller.ChatIDLength);

            TransmissionHandler(transmitString);

        }

        public void SendMessage(int userID, string username, string sessionID, int chatID, string message)
        {

            string transmitString = "SM" + Parser.Pack(userID, Controller.MaxUserIDLength) + Parser.Pack(username, Controller.MaxUserNameLength) + sessionID + Parser.Pack(chatID, Controller.ChatIDLength) + message;

            TransmissionHandler(transmitString);
        }

        public void EditMessage(int userID, string sessionID, int messageID, string message)
        {
            string transmitString = "EM" + Parser.Pack(userID, Controller.MaxUserIDLength) + sessionID + Parser.Pack(messageID, Controller.MaxMessageIDLength) + message;
            TransmissionHandler(transmitString);
        }


        public void DeleteMessage(int userID, string sessionID, string messageID)
        {
            string transmitString = "DM" + Parser.Pack(userID, Controller.MaxUserIDLength) + sessionID + Parser.Pack(messageID, Controller.MaxMessageIDLength);
            TransmissionHandler(transmitString);
        }

        /// <summary> 
        /// This method sends an add friend request. 
        /// It is unique in that the server will create a pending request and sends a request to the friended user.
        /// When the friended user sends an add friend request for this user, the pending request will be approved.
        /// 
        /// </summary>  
        /// <param name="userID">The ID of the user requesting the friend add.</param>
        /// <param name="friendID">The ID of the user to be added. </param>
        /// <param name="sessionID">The sessionID used to verify the transmission.</param>

        public void SendFriendRequest(int userID, string sessionID, string friendUserID)
        {
            Debugger.Record("Sending friend request.", DebugMask);

            Dictionary<String, String> requestDict = new();

            requestDict.Add("RequestType", "FRIEND");
            requestDict.Add("Parameter", "NONE");

            string requestJson = JsonConvert.SerializeObject(requestDict);

            string transmitString = "SR" + Parser.Pack(userID, Controller.MaxUserIDLength) + Parser.Pack(friendUserID, Controller.MaxUserIDLength) + sessionID + requestJson;

            TransmissionHandler(transmitString);
        }


        public void ApproveRequest(int userID, string sessionID, int requestID)
        {
            string transmitString = "AR" + Parser.Pack(userID, Controller.MaxUserIDLength) + sessionID + requestID;
            TransmissionHandler(transmitString);
        }

        /// <summary> 
        /// This method sends a chat invitation to another user.
        /// </summary>  
        /// <param name="userID">The ID of the user requesting the friend invite.</param>
        /// <param name="friendID">The ID of the user to be invited. </param>
        /// <param name="sessionID">The sessionID used to verify the transmission.</param>
        ///  /// <param name="chatID">The ID of the chat to invite them to.</param>


        public void SendChatInvitation(int userID, string sessionID, string friendUserID, int chatID)
        {
            Debugger.Record("Sending chat invite.", DebugMask);

            string transmitString = "CI" + Parser.Pack(userID, Controller.MaxUserIDLength) + Parser.Pack(friendUserID, Controller.MaxUserIDLength) + sessionID + chatID;

            TransmissionHandler(transmitString);
        }



        public void UserSearch(int userID, string searchString, string sessionID)
        {
            string transmitString = "US" + Parser.Pack(userID, 32) + Parser.Pack(searchString, 32) + sessionID;

            TransmissionHandler(transmitString);
        }



        public string GetUsername()
        {
            return Username;
        }

        public void Connect()
        {

            bool successful = false;
            int attempt = 0;


            while (successful == false)
            {
                try
                {
                    IPHostEntry hostEntry = null; // Container for host address info.
                    //IPAddress hostAddress = IPAddress.Parse(ServerAddress);
                    IPAddress hostAddress = IPAddress.Loopback;

                    hostEntry = Dns.GetHostEntry(hostAddress); // Resolves the hostAddress to a HostEntry;

                    foreach (IPAddress address in hostEntry.AddressList)
                    {
                        IPEndPoint endPoint = new IPEndPoint(address, Port);

                        Socket tempSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        tempSocket.Connect(endPoint);

                        // Breaks out of the for loop when the socket is able to connect to the server.

                        if (tempSocket.Connected)
                        {
                            ServerSocket = tempSocket;
                            break;
                        }
                    }
                    successful = true;
                }
                catch (Exception e)
                {
                    Debugger.Record("Failed to connect to server: " + e.Message, DebugMask);

                    Controller.RaiseNotificationPopupEvent("Failed to connect to server. Reattempting in 5 seconds.");
                }

                if (successful == false)
                {
                    attempt++;

                    if (attempt == 1)
                    {
                        Controller.RaiseNotificationPopupEvent("Failed to connect to server. Reattempting in 5 seconds.");
                    }
                    else
                    {
                        Controller.RaiseNotificationPopupEvent("Connect attempt #" + attempt + " failed. Reattempting in 5 seconds.");
                    }
                    Thread.Sleep(ReconnectWait);
                }
            }
            Receive();
        }

        private void Receive()
        {
            try
            {
                do
                {
                    string message = "Message";
                    ServerSocket.BeginReceive(BytesReceived, 0, BytesReceived.Length, 0, new AsyncCallback(MessageReceived), message);

                    string stringReceived = Encoding.ASCII.GetString(BytesReceived, 0, ByteCount);


                    if (stringReceived.Length > 0)
                    {
                        HeaderParser(stringReceived);
                    }

                } while (ServerSocket.Available > 0);
            }
            catch (Exception e)
            {
                Debugger.Record("An error has occurred in Receive(): " + e.Message, DebugMask);
                Controller.RaiseNotificationPopupEvent("An undefined connection failure has occurred. Attempting to reconnect in 5 seconds.");
                Thread.Sleep(ReconnectWait);
                Connect();
            }


        }


        /// <summary> 
        /// This method attempts to end receive on the socket after being triggered,
        /// getting the number of bytes currently waiting.
        /// The number of bytes is then used in the Receive method to read the correct number of bytes from the socket.
        /// 
        /// If it fails a read, it will loop continuously, presenting an initial error message followed by a looping error message.
        /// </summary>

        private void MessageReceived(IAsyncResult result)
        {

            try
            {
                ByteCount = ServerSocket.EndReceive(result);
            }
            catch (Exception e)
            {
                Debugger.Record("Connection to server lost: " + e.Message, DebugMask);

                Controller.RaiseNotificationPopupEvent("Connection with the server was lost. Will attempt to reconnect after 5 seconds.");
                Thread.Sleep(ReconnectWait);
                Connect();

            }


            //string stringReceived = Encoding.ASCII.GetString(BytesReceived, 0, byteCount);

            Receive();
        }


        /// <param name="received">The string that was received by the socket</param>

        private void HeaderParser(string received)
        {
            string message = "";

            try
            {

                Debug.WriteLine("\nConnection received: " + received);
                ////// Begin Transmission Buffer handling //////
               ///
               /// If the PartialIndicator is F, the function looks for a mapping to the opcode in the TransmissionBuffer.
               /// If there is such a mapping and the value isn't an empty string, it adds the new message to the buffer,
               /// sends it to the parser, and clears the buffer value for that transmission type.

               string partialIndicator = received.Substring(0, 1);
                string opcode = received.Substring(1, 2);

                string transmitMessage = "";

                if (partialIndicator == "F")
                {

                    if (TransmissionBuffer.ContainsKey(opcode) && (TransmissionBuffer[opcode] != ""))
                    {
                        Debug.WriteLine("\nAdding to TransmissionBuffer: " + received.Substring(67).Replace("\0", ""));

                        TransmissionBuffer[opcode] = TransmissionBuffer[opcode] + received.Substring(67).Replace("\0", "");
                        Debug.WriteLine("\nWith results:" + TransmissionBuffer[opcode]);


                        transmitMessage = TransmissionBuffer[opcode];
                        TransmissionBuffer[opcode] = "";

                        Debug.WriteLine("\nEmptying TransmissionBuffer. Result: " + transmitMessage + "\n");
                    }
                    else
                    {
                        transmitMessage = received.Substring(1).Replace("\0", "");
                    }
                }
                else if (partialIndicator == "T")
                {

                    if (TransmissionBuffer.ContainsKey(opcode) && TransmissionBuffer[opcode] != "")
                    {
                        Debugger.Record("\nAdding to TransmissionBuffer: " + received.Substring(75).Replace("\0", ""), DebugMask);

                        TransmissionBuffer[opcode] = TransmissionBuffer[opcode] + received.Substring(75).Replace("\0", "");


                        Debugger.Record("\nWith results:" + TransmissionBuffer[opcode], DebugMask);

                    }
                    else
                    {
                        Debugger.Record("\nInitializing TransmissionBuffer: " + received.Substring(1), DebugMask);
                        TransmissionBuffer[opcode] = received.Substring(1).Replace("\0", "");
                    }
                    // Do not proceed to parsing at this time, because more messages are to follow to combine with this one.
                    return;
                }
                else
                {
                    Debugger.Record("Header parser encountered incorrect Partial Message Indicator.", DebugMask);
                    return;
                }

                ////// End Transmission Buffer handling //////


                if (Controller == null)
                {
                    Controller = Controller.ControllerInstance;
                }

                Dictionary<string, string> results = new();

                if (Parser.Parse(transmitMessage, out results) == true)
                {
                    Controller.MessageHandler(results);
                }
                else
                {
                    Controller.RaiseNotificationPopupEvent("An impossible-to-parse message has been received from the server: \n" + message);
                }
            }
            catch (Exception e)
            {
                Debugger.Record("HeaderParser failed to handle an incoming message: " + e.Message, DebugMask);
            }
        }

        /// <param name="messageOut">The string that will be transmitted.</param>

        public void TransmissionHandler(string messageOut)
        {


            if (ServerSocket != null)
            {
                try
                {
                    Debugger.Record("Transmitting: " + messageOut, DebugMask);
                    Byte[] sent = Encoding.ASCII.GetBytes("F" + messageOut + "\n");
                    ServerSocket.Send(sent);
                }
                catch (Exception e)
                {
                    Controller.RaiseNotificationPopupEvent("There is a problem with the connection to the server.");
                }
            }
            else
            {
                Controller.RaiseNotificationPopupEvent("There is a problem with the connection to the server.");
            }
        }

        private void MessageSent(IAsyncResult result)
        {
            int byteCount = ServerSocket.EndSend(result);
            Receive();
        }

    }
}
