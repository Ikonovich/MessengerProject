using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Messenger_Client
{

    // Defines args for a message event
    public class MessageEventArgs
    {
        public MessageEventArgs(string message) { Message = message; }
        public string Message { get; } // Read-only
    }

    // This class is a singleton that handles the server connection and stores 
    // information about the currently active user.
    // It connects to the same server as the Unity client, but has a reduced set of useable 
    // request/receive codes. 

    // Available request codes are:
    // IR - Initial registration
    // LR - Login request
    // SM - Send message

    // Available receive codes are:
    // RS - Registration Successful
    // LS - Login Successful
    // LF - Login Failed
    // RM - Receive message
    // AM - Administrative message - A generic server response message for connection requests and whatnot

    public class ConnectionHandler
    {

        Controller Controller;

        // The constructor initializes the connection handler listening to the server.
        // It also takes and sets the Controller variable for this class. 


        public ConnectionHandler(Controller controller) 
        {
            Controller = controller;
            Connect();
            Receive();
        }



        // Connection-related variables
        Socket ServerSocket = null;
        Int32 Port = 3000;

        Byte[] BytesReceived = new byte[256];



        // Declaring the message event delegate
        public delegate void MessageEventHandler(object sender, MessageEventArgs e);

        // Declaring the event itself.
        public event MessageEventHandler MessageEvent;

        private static string Username { get; set; } = "Not Logged In";

        private bool LoginPending = false;
        private string PendingUsername = "None";



        public void Register(string username, string password, string verification)
        {
            username = PackString(username, 32);


            Debug.WriteLine("Registering with username: " + username);
            TransmissionHandler("IR" + verification + username + password);
        }

        public void Login(string username, string password, string verification)
        {

            LoginPending = true;
            PendingUsername = username;
            TransmissionHandler("LR" + verification + PackString(username, 32) + password);
        }

        public string GetUsername()
        {
            return Username;
        }

        private void Connect()
        {
            IPHostEntry hostEntry = null; // Container for host address info.
            IPAddress hostAddress = IPAddress.Loopback;

           // IPAddress.TryParse("24.179.8.54", out hostAddress);

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
        }

        private void Receive()
        {

            int receivedSize = 0;
            do
            {
                string message = "Message";
                ServerSocket.BeginReceive(BytesReceived, 0, BytesReceived.Length, 0, new AsyncCallback(MessageReceived), message);

                string stringReceived = Encoding.ASCII.GetString(BytesReceived);


                if (stringReceived.Length > 0)
                {
                    HeaderParser(stringReceived);
                }
            } while (receivedSize > 0);

        }

        private void MessageReceived(IAsyncResult result) 
        {
            int byteCount = ServerSocket.EndReceive(result);

            //string stringReceived = Encoding.ASCII.GetString(BytesReceived, 0, byteCount);

            Debug.WriteLine("Message received");
            Receive();
        }

        private void HeaderParser(string received)
        {
            Debug.WriteLine("Header parser activated\n");
            Debug.WriteLine(received);
            RaiseMessageEvent(received);

            if (Controller == null)
            {
                Controller = Controller.ControllerInstance;
            }
        }

        public void TransmissionHandler(string messageOut)
        {
            Byte[] sent = Encoding.ASCII.GetBytes(messageOut);
            ServerSocket.Send(sent);
        }


        private void RaiseMessageEvent(string message)
        {
            Debug.WriteLine("Raise event called");
            MessageEvent?.Invoke(this, new MessageEventArgs(message));
        }

        // This method packs a string in a special null character (currently asterisks) to make it fit the size
        // provided, allowing it to be parsed more easily.
        private string PackString(string input, int size)
        {
            for (int i = input.Length; i < size; i++)
            {
                input += "*";
            }
            return input;
        }
    }
}
