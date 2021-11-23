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

        // Controller
        Controller Controller;

        //
        // Begin singleton data
        //
        // Backing instance for singleton
        private static ConnectionHandler handlerInstance { get; set; }

        // Private singleton constructor
        private ConnectionHandler() 
        {

            Connect();
            Receive();
        }

        // Public singleton variable
        public static ConnectionHandler HandlerInstance
        {
            get
            {
                if (handlerInstance == null)
                {
                    handlerInstance = new ConnectionHandler();
                }
                return handlerInstance;
            }
        }

        //
        // End Singleton data
        //

        // Connection-related variables
        Socket ServerSocket = null;
        Int32 Port = 3000;




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

            Byte[] BytesReceived = new byte[256];

            int receivedSize = 0;
            do
            {
                string message = "Message";
                string stringReceived = "";
                ServerSocket.BeginReceive(BytesReceived, 0, BytesReceived.Length, 0, new AsyncCallback(MessageReceived), message);

                stringReceived += Encoding.ASCII.GetString(BytesReceived);

                Debug.WriteLine("Received string is: " + stringReceived);
                if (stringReceived.Contains("%END%"))
                {
                    Debug.WriteLine("Received string:" + stringReceived + "\n");
                    string resultString = stringReceived.Remove(stringReceived.Length - 6);

                    Debug.WriteLine("Result string:" + resultString + "\n");

                    ReceiveHandler(resultString);
                    stringReceived = "";
                }
            } while (receivedSize > 0);

        }

        private void MessageReceived(IAsyncResult result) 
        {
            int byteCount = ServerSocket.EndReceive(result);

            Debug.WriteLine("Message received of size: " + byteCount);
            Receive();
        }

        // This method takes received transmissions, sends them to be parsed, 
        // and then sends them to the controller to be handled. 
        // It then 
        // If the parse fails, it calls the error handler. 
        private void ReceiveHandler(string received)
        {

            if (Controller == null)
            {
                Controller = Controller.ControllerInstance;
            }

            Debug.WriteLine("Header parser activated");
            Debug.WriteLine(received);
            RaiseMessageEvent(received);

            Dictionary<string, string> output = new();
            if (Parser.Parse(received, out output) == false)
            {
                Controller.ErrorCall(new ArgumentException(), "An uninterpretable server transmission has been received.");
            }
            else
            {
                Controller.MessageHandler(output);
            }

        }

        public void TransmissionHandler(string messageOut)
        {
            Debug.WriteLine("Transmitting: " + messageOut);

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
