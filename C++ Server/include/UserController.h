#ifndef USERCONTROLLER_H
#define USERCONTROLLER_H

#include "libs.h"
#include "namespace.h"

using namespace MessengerSystem;

class MessengerSystem::UserController
{

public:
    
    UserController(MessengerSystem::DatabaseController *dataCon);

    int InitialRegistration(string input, string verifyCode, string& responseOut);

    bool LoginRequest(string input, string verifyCode, string& sessionOut, string& responseOut);

    void PullFriends(string input, string verifyCode, string& session, string& response);

    void MessageReceived(string input, string verifyCode, string& session, string& responseOut);

    void PullMessages(string input, string verifyCode, string& session, string& response);

    int ValidateSession(string input, string username, string& session, string &remainder);

private:

    // Variables
    MessengerSystem::DatabaseController *DatabaseCon;

    // Maps logged in users to session IDs.
    unordered_map<string, string> NameToSessionMap;

    // Maps session IDs to logged in users.
    unordered_map<string, string> SessionToNameMap;

    void ParseUser(string input, string &sender, string& remainder);
    
    void ParseUsers(string input, string &sender, string& receiver, string& remainder);

    int ValidateUsername(string username);

    string GenerateSessionID();

    string Pack(string username);


};

#endif