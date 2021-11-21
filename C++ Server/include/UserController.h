#ifndef USERCONTROLLER_H
#define USERCONTROLLER_H

#include "libs.h"
#include "namespace.h"

using namespace MessengerSystem;

class MessengerSystem::UserController
{

public:
    
    UserController(const MessengerSystem::DatabaseController dataCon);

    int InitialRegistration(string input, string& responseOut);

    void LoginRequest(string input, string& responseOut);

    void MessageReceived(string input, string& responseOut);

    void PullMessages(string input, vector<string>& messages, string& response);

private:

    // Variables
    MessengerSystem::DatabaseController DatabaseCon;

    void ParseUser(string input, string &sender, string& remainder);
    
    void ParseUsers(string input, string &sender, string& receiver, string& remainder);

};

#endif