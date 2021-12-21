#include "include/libs.h"
#include "include/namespace.h"
#include <bitset>
#include <pthread.h>



using namespace MessengerSystem;

// The server constructor initializes all the relevant objects for the system and passes them as necessary.
// It initializes the usercontroller, databasecontroller, and securitycontroller.
Server::Server() {

    
    SecurityCon = new MessengerSystem::SecurityController();
    DatabaseCon = new MessengerSystem::DatabaseController(SecurityCon);

    string test = "Test";
    UserCon = new MessengerSystem::UserController(DatabaseCon);

}

void Server::StartServer() {

    SocketHandler();

    // Security = Security();

    // cout << Security.GenerateHash("testInput");
}




// Input packet format: 
// [Requested connection type - 2 chars]
// [VerificationCode - 32 chars]
// [User ID - 32 chars]
// [Additional information based on request type, as defined above each function handling
// that request type]

// Return packet format:
// [Response type - 2 chars]
// [Receiving ID - 16 chars - identifies which user the return packet should go to.]
// [Requesting ID, if any - 16 Chars - identifies which user the data or request originated with]
// [Additional information based on request type, as defined above each function handling
// that request type]

// This process uses the first two characters of the header of the packet to determine where it should be sent.
// It strips these two characters before sending the string on elsewhere.
// Available receive codes are:

// IR - Initial registration
// lR - Login request for an already registered user
// PF - Pull friends list
// AF - Add to friends list
// SM - Sending a message to another user
// PM - Pull message records between requesting user and another user.

// Available response codes are:
// RS - Registration Successful
// RU - Registration Unsuccessful - Should be accompanied by an error message
// LS - Login Successful
// LU - Login Unsuccessful - Should be accompanied by an error message
// FP - Friend Push - Friends list follows in the form of a JSON string. 
// AM - Administrative message - A generic response message for connection requests and whatnot

void Server::ServerThread(void *socket) {

    cout << "Starting server thread";

    int sockID = *(int*)socket;

    while (1) {
            
            // Recv accepts data from the client.
            std::vector<char> inputBuffer(5000); 
            auto bytes_received = recv(sockID, inputBuffer.data(), inputBuffer.size(), 0);


            string input(inputBuffer.begin(), inputBuffer.end()); // String constructor taking the beginning and end of a char vector.


            if (input.at(0) != 0) {
                printf("Value of char at 0 is %i", (int)input.at(0));
                HeaderHandler(input, sockID);
            }
        }

    close(sockID);
    //freeaddrinfo(res);

    exit(0);
}

void Server::HeaderHandler(string input, int clientSocket) {

    // Note: string.substr(index, length) gets the string starting at index, with length length.
    // If length is longer than the number of characters after index, gets as many characters as possible.
    // If index is bigger than the string size, throws out_of_bounds
    
    cout << "Input to header handler: " << input << "\n";

    string requestType = input.substr(0, 2);  // Gets the first two characters of the string as the message type "MT"
    string verifyCode = input.substr(2, 16); // Gets byte 2-18 as the verification response code from the client.
    string output = input.substr(18, input.length()); // Gets the remainder of the string.


    cout << "Request type is: " << requestType + ".\n";
    
    int requestInt = -1;

    try {
        requestInt = RequestTypeMap.at(requestType);
    }
    catch (const std::out_of_range err) {
        cout << "Request type not found in mapping.\n";
        cout << "Message is: " << input << " with length: " << input.length() << "\n";
    }
    
    string response; // Stores the response message from the procedure call
    string session; // Stores the session ID created by a successful login request.
    string pullString; // Stores json string from a pull response
    

    switch (requestInt) {
        case 0:
            UserCon->InitialRegistration(output, verifyCode, response);
            TransmissionHandler(clientSocket, response);
            break;
        case 1:

            if (UserCon->LoginRequest(output, verifyCode, session, response) == true) {
                SessionToSocketMap[session] = clientSocket;

                cout << "Assigned session " << session << " to socket " << clientSocket;
                TransmissionHandler(session, response);
            }
            else {
                TransmissionHandler(clientSocket, response);
            }
            break;
        case 2:
            UserCon->MessageReceived(output, verifyCode, session, response);
            break;
        case 3:
            UserCon->PullMessages(output, verifyCode, session, response);

            TransmissionHandler(session, response);
            break;
        case 4:
            UserCon->PullFriends(output, verifyCode, session, response);

            
            cout << "Validated session: " << session << "\n";
            TransmissionHandler(session, response);
            break;
        default:
            cout << "An error has occurred: A request type that does not exist "
                "has been provided.\n";
            TransmissionHandler(clientSocket, "AM An error has occurred: A request type that does not exist "
                "has been provided.\n");
    }
}


void Server::SendTestMessages(string senderID) {

    TransmissionHandler(senderID, "RM Test Message");
    sleep(1);
    TransmissionHandler(senderID, "RM Next Test Message");
    sleep(1);
    TransmissionHandler(senderID, "RM Third Test Message");
}

int Server::TransmissionHandler(string session, string response) {

    try {

        int responseSocket = SessionToSocketMap[session];

        cout << "Gathered session: " << session << ".\n";
        cout << "Transmitting message: " << response << " on socket " << std::to_string(responseSocket) << "\n";


        auto bytes_sent = send(responseSocket, response.data(), response.length(), 0);
    }
    catch (...) {

        cout << "An error has occured during transmission.\n";
        return -1;
    }
    return 0;
}

// // This transmission handler can be used when a User ID has not been assigned.
// // It takes a socket identifier directly rather than a User ID. 

int Server::TransmissionHandler(int responseSocket, string response) 
{
    cout << "Transmitting message: " << response << " on socket " << std::to_string(responseSocket) << "\n";

    try {
        auto bytes_sent = send(responseSocket, response.data(), response.length(), 0);
    }
    catch (...) {

        cout << "An error has occured during transmission.\n";
        return -1;
    }
     return 0;
}

int Server::SocketHandler() 
{

    addrinfo hints;
    addrinfo *res; // Holds pointer to addrinfo returned by getaddrinfo()
    addrinfo *p; // Used to iterate over same space res points to

    memset((void*)&hints, 0, sizeof(hints)); // Fills hints with 0 to the size of the variable


    hints.ai_family = AF_UNSPEC; // Don't specify IP version as 4 or 6 
    hints.ai_socktype = SOCK_STREAM; //SOCK_STREAM refers to TCP, SOCK_DGRAM refers to UDP

    // Returned socket address will be suitable for finding a socket that will accept connections.
    // Returned socket address will contain "wildcard address" - INADDR_ANY for IPV4, IN6ADDR_ANY for IPv6.
    // The wildcard address is used by applications that intend to accept connections on any network address.
    // If node is not null, AI_PASSIVE flag is ignored.
    hints.ai_flags = AI_PASSIVE; 

    // Gets addresses matching the hints addrinfo and places them in &res
    int addrRes = getaddrinfo(NULL, portNum, &hints, &res);

    if (addrRes != 0) {
        // Prints getaddrinfo error data
        // Using std:: prevents namespace conflicts brought on by "using namespace std"

        std::cerr << gai_strerror(addrRes) << "\n";
        return -2;
    }

    // std::endl inserts a newline character and flushes the stream.

    cout << "Detecting addresses" << std::endl;
    // Using IPV6 length ensures both IPv4 and v6 addresses can be stored
    // Defined in inet.h

    char ipString[INET6_ADDRSTRLEN];

    unsigned int addrCount = 0;

    // Arrow symbol is used to access struct members through a pointer

    for (p = res; p != NULL; p = p -> ai_next) 
    {

        void *addr;
        string ipVersion;

        // Handles the address if it's IPv4
        if (p->ai_family == AF_INET) 
        {
            ipVersion = "IPv4";

            // Reinterpret casting the ai_addr variable of p* to sockaddr_in type
            sockaddr_in *ipv4 = reinterpret_cast<sockaddr_in *>(p->ai_addr);

            // Set addr variable to the sin_addr member of the ipv4 struct
            addr = &(ipv4->sin_addr);

            ++addrCount;
        }
        // Handles the address if it is IPv6
        else 
        {
            ipVersion = "IPv6";
            sockaddr_in6 *ipv6 = reinterpret_cast<sockaddr_in6 *>(p->ai_addr);
            addr = &(ipv6->sin6_addr);
            ++addrCount;
        }

        // Converts the addr acquired this loop to a char array, stored in ipString

        inet_ntop(p->ai_family, addr, ipString, sizeof(ipString));

        // Prints information about this iteration
        cout << "IP (" << addrCount << ")" << ipVersion << " : " << ipString << std::endl;
    }
    // Checks to ensure at least one address has been located
    if (addrCount == 0) 
    {
        std::cerr << "No host addresses were found.\n";
        return -3;
    }

    // Grabs and uses the first address located.

    p = res; // Sets the iterator pointer back to the starting point

    // Creating a new socket. SocketFD is returned as socket descriptor.
    // Returns -1 if there is an error, probably.
    while (true) {
        // Parameters:
        // ai_family - Int, 2 for IPv4, 23 for IPv6
        //ai_socktype - Int, 1 for SOCK_STREAM(TCP), 2 for SOCK_DGRAM(UDP)
        //ai_protocol - Int, 6 for TCP, 17 for UDP
        int socketFD = socket(p->ai_family, p->ai_socktype, p->ai_protocol);

        if (socketFD == -1) {

            std::cerr << "An error has occurred during socket creation. \n";
            freeaddrinfo(res); //Deallocates the addrinfo structs.
            return -1;
        }

        // Binding the socket.

        int bindSock = bind(socketFD, p->ai_addr, p->ai_addrlen);  

        if (bindSock == -1) {

            std::cerr << "An error has occurred while binding the socket.\n";
            printf("Value of errno: %d\n", errno);
            // Closing the socket if there is an error.
            close(socketFD);
            freeaddrinfo(res);
            return -1;
        }

        // Start listening for connections on the socket.

        int listenSock = listen(socketFD, maxConnections);

        if (listenSock == -1) {

            std::cerr << "Error while listening on socket. \n";

            // Close the socket if there is an error.
            close(socketFD);
            freeaddrinfo(res);
            return -1;
        }

        // Structure to hold the client's address
        sockaddr_storage client_addr;
        socklen_t client_addr_size = sizeof(client_addr);

        // This infinite loop handles incoming connections
        cout << "Accepting connections.\n";


        int newFD = accept(socketFD, (sockaddr*)&client_addr, &client_addr_size);
        if (newFD == -1) {

            std::cerr << "Error while accepting on socket.\n";
            return -1;
        }


        // while (1) {
            
        //     // Recv accepts data from the client.
        //     std::vector<char> inputBuffer(5000); 
        //     auto bytes_received = recv(newFD, inputBuffer.data(), inputBuffer.size(), 0);


        //     string input(inputBuffer.begin(), inputBuffer.end()); // String constructor taking the beginning and end of a char vector.


        //     if (input.at(0) != 0) {
        //         printf("Value of char at 0 is %i", (int)input.at(0));
        //         HeaderHandler(input, newFD);
        //     }
        // }
    }


    // Close the socket on exit.
 
    return 0;
}



using namespace MessengerSystem;

    int main(int argc, char *argv[])
    {
        MessengerSystem::Server server = MessengerSystem::Server();
        server.StartServer();

        return 0;
    }