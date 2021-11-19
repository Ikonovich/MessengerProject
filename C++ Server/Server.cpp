#include "include/Server.h"
#include "include/MessengerSystem.h"

//#include "include/Security.h"


using namespace sql;
using namespace sql::mysql;
using namespace MessengerSystem;

MessengerSystem::Server::Server() {}

void MessengerSystem::Server::StartServer() {


    ConnectDatabase();
    SocketHandler();

    // Security = Security();

    // std::cout << Security.GenerateHash("testInput");
}




// Input packet format: 
// [Requested connection type - 2 chars]
// [User ID - 16 chars - All zeroes for Initial Registration or Reconnect connection type]
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
// SM - Sending a message to another user
// PM - Pull message records between requesting user and another user.

// Available response codes are:
// RS - Registration Successful
// RU - Registration Unsuccessful - Should be accompanied by an error message
// LS - Login Successful
// LU - Login Unsuccessful - Should be accompanied by an error message
// AM - Administrative message - A generic response message for connection requests and whatnot


void Server::HeaderHandler(std::vector<char> inputBuffer, int clientSocket) {


    std::string input(inputBuffer.begin(), inputBuffer.end()); // String constructor taking the beginning and end of a char vector.

    // Note: string.substr(index, length) gets the string starting at index, with length length.
    // If length is longer than the number of characters after index, gets as many characters as possible.
    // If index is bigger than the string size, throws out_of_bounds.

    std::string requestType = input.substr(0, 2);  // Gets the first two characters of the string as the message type "MT"

    std::string output = input.substr(2, input.length()); // Gets the remainder of the string.


    std::cout << "Request type is: " << requestType + ".\n";
    
    int requestInt = -1;

    try {
        requestInt = RequestTypeMap.at(requestType);
    }
    catch (const std::out_of_range err) {
        std::cout << "Request type not found in mapping.\n";
        std::cout << "Message is: " << input << "\n";
    }
    

    switch (requestInt) {
        case 0:
            InitialRegistration(output, clientSocket);
            break;
        case 1:
            LoginRequest(output, clientSocket);
            break;
        case 2:
            MessageReceived(output, clientSocket);
            break;
        case 3:
            PullMessages(output);
            break;
        default:
            std::cout << "An error has occurred: A request type that does not exist "
                "has been provided.\n";
            TransmissionHandler(clientSocket, "AM An error has occurred: A request type that does not exist "
                "has been provided.\n");
    }
}

// Login request receive packet format:
// [LR][32 Char UserName][Up to 128 Char password]
// Login Request Return packet format:
// [Response type - 2 chars] [Optional Error Message - 64 chars]
// LS - Login successful
// LU - Login Unsuccessful


void Server::LoginRequest(std::string input, int clientSocket) {

    std::string senderID;
    std::string password;

    ParseUser(input, senderID, password);

    std::cout << "User attempting to login: " << senderID << " on socket " << std::to_string(clientSocket) << ".\n";

    std::string session = "";

    if (IDtoSessionMap.contains(senderID) == false) {

        session = GenerateSessionID();
    }

    SessionToIDMap[session] = senderID;
    IDtoSessionMap[senderID] = session;
    SessionToSocketMap[session] = clientSocket;

    std::cout << "Session " << session << " assigned to user " << senderID << ".\n";

    std::cout << "Confirming session assignment for user " << senderID << ": " << IDtoSessionMap[senderID] << ".\n";
    std::cout << "Login approved.";
}


void Server::SendTestMessages(std::string senderID) {

    TransmissionHandler(senderID, "RM Test Message");
    sleep(1);
    TransmissionHandler(senderID, "RM Next Test Message");
    sleep(1);
    TransmissionHandler(senderID, "RM Third Test Message");
}

void Server::PullMessages(std::string input) {}

// // This method parses out the sender from a data block.

void Server::ParseUser(std::string input, std::string &sender, std::string &returnMessage) {

    std::string senderID = input.substr(0, 32);  // Gets the sender's username of length 32.
    std::string messageString = input.substr(32, input.length());

    std::string delimiter = "*";

    size_t senderIDlimit = senderID.find(delimiter);

    if (senderIDlimit != std::string::npos) {
        senderID.erase(senderIDlimit); // Erases filler characters from the User ID;
    }

    sender = senderID;
}

// // This method parses out the sender and receiver from a data block that contains both.

void Server::ParseUsers(std::string input, std::string &sender, std::string &receiver, std::string &returnMessage) {

    std::string senderID = input.substr(0, 32);  // Gets the sender's username of length 32.
    std::string receiverID = input.substr(32, 32); // Gets the receiver's username of length 32.
    std::string messageString = input.substr(64, input.length());


    std::string delimiter = "*";

    size_t senderIDlimit = senderID.find(delimiter);
    if (senderIDlimit != std::string::npos) {
        senderID.erase(senderIDlimit); // Erases filler characters from the User ID;
    }

    size_t receiverIDlimit = receiverID.find(delimiter);
    if (receiverIDlimit != std::string::npos) {
        receiverID.erase(receiverIDlimit); // Erases filler characters from the receiver ID;
    }

    sender = senderID;
    receiver = receiverID;

}

void Server::MessageReceived(std::string input, int clientSocket) {

    std::string data = input;

    std::string senderID;
    std::string receiverID;
    std::string remainder;

    ParseUsers(data, senderID, receiverID, remainder);

    std::cout << "Message received from user: " + senderID + " for user " + receiverID + ": \n" + remainder + " on socket " + std::to_string(clientSocket) + "\n";

    SendTestMessages(senderID);

}

void Server::InitialRegistration(std::string input, int clientSocket) 
{

    std::cout << "Registration request made.\n";

    std::string userName;
    std::string data;

    ParseUser(input, userName, data);

    
    if (NameToIDMap.count(userName) > 0) {

        TransmissionHandler(clientSocket, "RUThe provided username was already in use.\n");
        return;
    }


    std::string response = "RS" + userName;
    TransmissionHandler(clientSocket, response);

}

// // Returns an unused session ID string of length 16.
std::string Server::GenerateSessionID() {

    std::string sessionID = "";

    for (int i = 0; i < 16; i++) {

        char newChar = 97 + rand() % 26;

        sessionID += newChar;
    }

    return sessionID;
}


int Server::TransmissionHandler(std::string userID, std::string response) {

    try {

        std::vector<std::string> keys;
        keys.reserve(IDtoSessionMap.size());
        std::vector<std::string> vals;
        vals.reserve(IDtoSessionMap.size());

        for(auto kv : IDtoSessionMap) {
            keys.push_back(kv.first);
            vals.push_back(kv.second);  
        } 
        for (size_t size = 0; size < keys.size(); size++) {

            std::cout << keys[size] << " ";
            std::cout << vals[size] << "\n";
        }

        std::string session = IDtoSessionMap[userID];
        int responseSocket = SessionToSocketMap[session];

        std::cout << "Gathered session: " << IDtoSessionMap["TestUsername"] << " for tester TestUsername.\n";

        std::cout << "Gathered session: " << session << " for user " << userID << ".\n";
        std::cout << "Transmitting message: " << response << " on socket " << std::to_string(responseSocket) << "\n";


        auto bytes_sent = send(responseSocket, response.data(), response.length(), 0);
    }
    catch (...) {

        std::cout << "An error has occured during transmission.\n";
        return -1;
    }
    return 0;
}

// // This transmission handler can be used when a User ID has not been assigned.
// // It takes a socket identifier directly rather than a User ID. 

int Server::TransmissionHandler(int responseSocket, std::string response) 
{

    std::cout << "Transmitting message: " << response << " on socket " << std::to_string(responseSocket) << "\n";

    try {
        auto bytes_sent = send(responseSocket, response.data(), response.length(), 0);
    }
    catch (...) {

        std::cout << "An error has occured during transmission.\n";
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

    std::cout << "Detecting addresses" << std::endl;
    // Using IPV6 length ensures both IPv4 and v6 addresses can be stored
    // Defined in inet.h

    char ipString[INET6_ADDRSTRLEN];

    unsigned int addrCount = 0;

    // Arrow symbol is used to access struct members through a pointer

    for (p = res; p != NULL; p = p -> ai_next) 
    {

        void *addr;
        std::string ipVersion;

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
        std::cout << "IP (" << addrCount << ")" << ipVersion << " : " << ipString << std::endl;
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
    std::cout << "Accepting connections.\n";


    int newFD = accept(socketFD, (sockaddr*)&client_addr, &client_addr_size);
    if (newFD == -1) {

        std::cerr << "Error while accepting on socket.\n";
        return -1;
    }


    while (1) {

    // int newFD = accept(socketFD, (sockaddr *) &client_addr, &client_addr_size);
        //if (newFD == -1) {

        //  std::cerr << "Error while accepting on socket.\n";
        //   continue;
        //}


        // Recv accepts data from the client.
        std::vector<char> inputBuffer(5000); 
        auto bytes_received = recv(newFD, inputBuffer.data(), inputBuffer.size(), 0);


        // Prints all received chars.
        // std::cout << "Bytes received: ";


        // for (char i : inputBuffer) {

        //     std::cout << i;
        // }
        // std::cout << "\n";
        
        if (inputBuffer.size() > 0) {

            HeaderHandler(inputBuffer, newFD);
        }
    }

    // Close the socket on exit.
    close(socketFD);
    freeaddrinfo(res);

     return 0;
}

// // This method initializes the database variables and connects to the database.
void Server::ConnectDatabase() {


    ResultSet *resultSet; 
    //Result *result; // Represents return result for a query that does not return data.


    driver = get_mysql_driver_instance();
    connection = driver->connect("tcp://127.0.0.1:3306", "root", "giga321");

    connection->setSchema("messenger_database");

    statement = connection->createStatement();
    //statement->execute("INSERT INTO RegisteredUsers(UserName, UserStatus) VALUES (\"Ikonovich\", \"Active\");");

    //resultSet = statement->executeQuery("SELECT * FROM RegisteredUsers WHERE UserName=\"Ikonovich\"");

}



using namespace MessengerSystem;

    int main(int argc, char *argv[])
    {
        MessengerSystem::Server server = MessengerSystem::Server();
        server.StartServer();

        return 0;
    }