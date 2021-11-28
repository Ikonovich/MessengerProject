#ifndef DATABASECONTROLLER_H
#define DATABASECONTROLLER_H


#include "namespace.h"
#include <string>

using namespace MessengerSystem;
using namespace sql;
using namespace sql::mysql;

class MessengerSystem::DatabaseController
{

public:

    DatabaseController(SecurityController *secCon);
    
    bool AddUser(string username, string password);

    ResultSet * QueryField(string tableName, string fieldName, string searchString);

    bool CheckField(string tableName, string fieldName, string searchString);

    bool VerifyPassword(string username, string password);

    string GetFriends(string username);


private:

    MessengerSystem::SecurityController *SecurityCon;
    // Database variables.
    MySQL_Driver *driver;
    Connection *connection;

    int ConnectDatabase();

};


#endif
