#ifndef SERVER_H
#define SERVER_H

#include "libs.h"
#include "namespace.h"

using namespace std;
using namespace MessengerSystem;



class MessengerSystem::Server
{

private: 

// Controllers
MessengerSystem::DatabaseController *DatabaseCon;
MessengerSystem::UserController *UserCon;
MessengerSystem::SecurityController *SecurityCon;

public:

    Server();

    void StartServer();

private: 

// Socket variables
const char *portNum = "3000";
const int maxConnections = 50;

// Maps sessions to sockets:
unordered_map<string, int> SessionToSocketMap;

// Mapping header information to integers
const unordered_map<string, int> RequestTypeMap{
                                        {"IR", 0},
                                        {"LR", 1},
                                        {"SM", 2},
                                        {"PM", 3}
                                        };

    void HeaderHandler(string input, int clientSocket);

    void SendTestMessages(string senderID);

    int SocketHandler();

    int TransmissionHandler(string userID, string response);

    int TransmissionHandler(int responseSocket, string response);

    void PullMessages(string input);

    string GenerateSessionID();

};

#endif