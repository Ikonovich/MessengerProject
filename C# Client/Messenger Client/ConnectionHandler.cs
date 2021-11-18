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

    class ConnectionHandler
    {

        //
        // Begin singleton data
        //
        // Backing instance for singleton
        private static ConnectionHandler handlerInstance { get; set; }

        // Private singleton constructor
        private ConnectionHandler() {
            Connect();
            Receive();
            TransmissionHandler("LRTestUsername********************");

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

        Byte[] BytesReceived = new byte[256];



        // Declaring the message event delegate
        public delegate void MessageEventHandler(object sender, MessageEventArgs e);

        // Declaring the event itself.
        public event MessageEventHandler MessageEvent;

        private static string Username { get; set; } = "Not Logged In";

        public bool Login(string username, string password)
        {
            Username = username;
            return true;

        }

        public string GetUsername()
        {
            return Username;
        }

        public void Connect()
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
                    Debug.WriteLine("Chars received count: " + stringReceived.Length + "\n");
                    Debug.WriteLine("Received string: " + stringReceived + "\n");
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
            Debug.WriteLine("Header parser activated");
            Debug.WriteLine(received);
            RaiseMessageEvent(received);
        }

        public void TransmissionHandler(string messageOut)
        {
            Byte[] sent = Encoding.ASCII.GetBytes(messageOut);
            ServerSocket.Send(sent);
        }


        private void RaiseMessageEvent(string message)
        {

            //Thread thread = new Thread(() => 
            //{    
            //    Debug.WriteLine("Raise event called");
            //    MessageEvent?.Invoke(this, new MessageEventArgs(message));
            //});

            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();
            Debug.WriteLine("Raise event called");
            MessageEvent?.Invoke(this, new MessageEventArgs(message));

        }
    }
}
