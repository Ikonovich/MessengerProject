#ifndef SERVER_H
#define SERVER_H

#include "includes.h"

using namespace std;
using namespace MessengerSystem;
using namespace sql;
using namespace sql::mysql;



class MessengerSystem::Server
{
public:

    Server();

    void StartServer();

private: 

//Security security = MessengerSystem::Security();

// Database variables.
MySQL_Driver *driver;
Connection *connection;
Statement *statement;

// Socket variables
const char *portNum = "3000";
const int maxConnections = 50;

// Maps sessions to sockets:
std::unordered_map<std::string, int> SessionToSocketMap;

// Maps client usernames to UserIDs: 
std::unordered_map<std::string, std::string> NameToIDMap;

// Maps client User IDs to UserNames: 
std::unordered_map<std::string, std::string> IDtoNameMap;

// Maps logged in users to session IDs.
std::unordered_map<std::string, std::string> IDtoSessionMap;


// Maps session IDs to logged in users.
std::unordered_map<std::string, std::string> SessionToIDMap;

// Maps connected clients to each other. 
std::unordered_map<std::string, std::string> ActiveConnectionMap;

// Maps pending requested from clients to requested clients.
std::unordered_map<std::string, std::string> PendingConnectionMap;


// Mapping header information to integers
const std::unordered_map<std::string, int> RequestTypeMap{
                                        {"IR", 0},
                                        {"LR", 1},
                                        {"SM", 2},
                                        {"PM", 3}
                                        };

    void HeaderHandler(std::vector<char> inputBuffer, int clientSocket);

    void ParseUser(std::string input, std::string &sender, std::string &returnMessage);
    
    void ParseUsers(std::string input, std::string &sender, std::string &receiver, std::string &returnMessage);

    void LoginRequest(std::string input, int clientSocket);

    void SendTestMessages(std::string senderID);

    void MessageReceived(string input, int clientSocket);

    void InitialRegistration(string input, int clientSocket);

    void ConnectDatabase();

    int SocketHandler();

    int TransmissionHandler(string userID, string response);

    int TransmissionHandler(int responseSocket, string response);

    void PullMessages(std::string input);


    string GenerateSessionID();

};

#endif