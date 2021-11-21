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

    DatabaseController();

private:

    // // Database variables.
    // MySQL_Driver *driver;
    // Connection *connection;
    // Statement *statement;

    int ConnectDatabase();

};


#endif
