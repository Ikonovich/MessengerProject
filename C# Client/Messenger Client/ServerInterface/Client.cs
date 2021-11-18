using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Messenger_Client
{

    // Available request codes are:
    // SR - Stats Request
    // SU - Stats Update
    // CR - Connection request
    // CA - Connection approval 
    // CT - Connection Termination Request
    // CC - Continued communication
    // CS - Continued communication successful
    // IR - Initial registration
    // RC - Reconnect for an already registered user

    // Available receive codes are:
    // RS - Registration Successful
    // RC - Reconnect Successful
    // ST - Stats Transmit - Receipt of stats from the server
    // CR - Connection Request Incoming
    // CA - Connection Request Approved
    // CT - Connection Terminated
    // RD - Receiving data on connection
    // AM - Administrative message - A generic server response message for connection requests and whatnot


    class Client
    {
        string UserID = "0000000000000000";
        Socket ServerSocket; 

        // This method handles incoming packets, taking their request type to decide what to do with them.
        protected void HeaderHandler(string input)
        {
            Console.WriteLine("Header input: " + input + "\n");


            string requestType = input.Substring(0, 2);
            string data = input.Substring(2);
    

            switch(requestType)
            {
                case "RS":
                    RegistrationSuccessful(data);
                    break;
                case "RC":
                    ReconnectSuccessful(data);
                    break;
                case "ST":
                    StatsTransmitted(data);
                    break;
                case "CR":
                    ConnectionRequest(data);
                    break;
                case "CA":
                    ConnectionApproved(data);
                    break;
                case "CT":
                    ConnectionTerminated(data);
                    break;
                case "RD":
                    ReceiveCommunication(data);
                    break;
                case "AM":
                    AdministrativeMessage(data);
                    break;
                default:
                    Console.WriteLine("Error: A nonexistent request type was received.");
                    break;

            }

        }

        private void RegistrationSuccessful(string data)
        {

            UserID = data.Substring(0, 16);
            Console.WriteLine("Registration has been successful. New User ID is: " + UserID + "\n");

            StatsRequest();
        }

        private void ReconnectSuccessful(string data)
        {
            Console.WriteLine("Reconnection has been successful.\n");

        }

        private void StatsTransmitted(string data)
        {
            Console.WriteLine("You have received stats from the server.\n");

        }

        private void ConnectionRequest(string data)
        {
            Console.WriteLine("You have received a connection request.\n");

        }

        private void ConnectionApproved(string data)
        {
            Console.WriteLine("Your connection request has been approved.\n");
        }

        private void ReceiveCommunication(string data)
        {
            Console.WriteLine("You have received a communication on an open connection.\n");
        }

        private void AdministrativeMessage(string data)
        {
            Console.WriteLine("You have received an administrative message: " + data + "\n");
        }

        private void ConnectionTerminated(string data)
        {
            Console.WriteLine("An open connection has been terminated by the other client.\n");

        }

        private void StatsRequest()
        {

            Console.WriteLine("Requesting stats.");
            string request = "SR" + UserID;
            Byte[] outbound = Encoding.ASCII.GetBytes(request);
            ServerSocket.Send(outbound, outbound.Length, 0);

        }

        // Creates a socket for the loopback address and socket provided in Port.
        public Socket GetSocket()
        {

            Int32 Port = 3000;
            ServerSocket = null;
            IPHostEntry host = null; // Container for host address info
            IPAddress hostAddress = IPAddress.Loopback;

            host = Dns.GetHostEntry(hostAddress); //GetHostEntry takes a System.Net.IPAddress parameter

            foreach(IPAddress address in host.AddressList)
            {
                IPEndPoint endPoint = new IPEndPoint(address, Port); // Represents a network end point as an IP address and port number

                // AddressFamily returns InterNetwork for IPv4 or InterNetworkV6 for IPv6
                // SocketType.Stream uses the TCP protocol.
                // ProtocolType is used to inform the Windows Socket API of the requested protocol.

                Socket tempSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                tempSocket.Connect(endPoint); // Establishes a connection to a remote host

                if(tempSocket.Connected)
                {

                    ServerSocket = tempSocket;
                    break;

                }
                else
                {
                    continue;
                }
            }
            return ServerSocket;
        }

        // This method requests the content from the specified server.
        private string SendReceive(string server, int port)
        {
            string request = "IR" + UserID + "TestUserName%";

            Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
            Byte[] bytesReceived = new byte[256];
            string page = "";

            // Create a socket connection using the socket provided by GetSocket.
            using (Socket sock = GetSocket())
            {
                if (sock == null)
                {
                    return ("Unable to acquire socket.\n");
                }

                // Send request to the server.
                sock.Send(bytesSent, bytesSent.Length, 0);


                // Receive the response.
                int bytes = 0;


                // This will block until the message is completely transmitted.
                do
                {
                    bytes = sock.Receive(bytesReceived, bytesReceived.Length, 0);

                    string receivedString = Encoding.ASCII.GetString(bytesReceived, 0, bytes);

                    page += receivedString;

                    if (receivedString.Length > 0)
                    {
                        HeaderHandler(receivedString);
                    }

                } while (bytes > 0);
            }

       
            return page;
        }

    }


}


