#include "include/libs.h"
#include "include/namespace.h"


using namespace MessengerSystem;
using namespace sql;
using namespace sql::mysql;


DatabaseController::DatabaseController(SecurityController *secCon) : SecurityCon(secCon) {

    ConnectDatabase();
}

// This method initializes the database variables and connects to the database.
// Returns 0 if successful, returns -1 if it fails.

int DatabaseController::ConnectDatabase() {

    try {

     
        // Result *result; // Represents return result for a query that does not return data.


        driver = get_mysql_driver_instance();
        connection = driver->connect("tcp://127.0.0.1:3306", "root", "giga321");

        connection->setSchema("messenger_database");

        //statement = connection->createStatement();

        //statement->execute("INSERT INTO RegisteredUsers(UserName, UserStatus) VALUES (\"Ikonovich\", \"Active\");");

        //resultSet = statement->executeQuery("SELECT * FROM RegisteredUsers WHERE UserName=\"Ikonovich\"");

        return 0;
    }
    catch (...) {

        std::cout << "The database has failed to connect properly.\n";

        return -1;
    }


}

// Adds a new user to the database, used for registration.
// The password is salted and hashed before being added to the database.
bool DatabaseController::AddUser(string username, string password) {

    cout << "Adding user to database.";


    Statement *statement = connection->createStatement();

    string salt = SecurityCon->GenerateSalt();
    string saltedPassword = salt + password;
    string hash = SecurityCon->GenerateHash(saltedPassword);


    string query = "INSERT INTO RegisteredUsers(UserName, UserStatus, PasswordHash, PasswordSalt) VALUES (\"" + username + "\", " + "\"Inactive\"," + "\"" + hash + "\"" + ", \"" + salt + "\");";
    statement->execute(query);

    return true;
}

// This method checks a provided field in a provided table for a particular item, true if 
// an instance was found, false otherwise.

bool DatabaseController::CheckField(string tableName, string fieldName, string searchString) {

    Statement *statement = connection->createStatement();
    ResultSet *resultSet; 

    string query = "SELECT * FROM " + tableName + " WHERE " + fieldName + "=\"" + searchString + "\";";
    
    statement->execute("DELETE FROM RegisteredUsers WHERE Username=\"testname\";");
    //statement->execute("INSERT INTO RegisteredUsers(UserName, UserStatus, PasswordHash, PasswordSalt) VALUES (\"Ikonovich\", \"Active\", \"TempHash\", \"TastySalt\");");


    resultSet = statement->executeQuery(query);

    if (resultSet -> next() == 0) {
        return false;
    }
    return true;
}

//This method checks a provided field in a provided table for a particular item and returns the results.

ResultSet * DatabaseController::QueryField(string tableName, string fieldName, string searchString) {

    Statement *statement = connection->createStatement();
    ResultSet *resultSet; 

    string query = "SELECT * FROM " + tableName + " WHERE " + fieldName + "=\"" + searchString + "\";";
    
    resultSet = statement->executeQuery(query);
    return resultSet;
}

bool DatabaseController::VerifyPassword(string username, string password) {

    Statement *statement = connection->createStatement();
    ResultSet *resultSet; 

    string query = "SELECT Username, PasswordHash, PasswordSalt FROM RegisteredUsers WHERE Username=\"" + username + "\";";
    
    resultSet = statement->executeQuery(query);

    resultSet->next();
    string name = resultSet->getString(1);
    string hash = resultSet->getString(2);
    string salt = resultSet->getString(3);

    string newHash = SecurityCon->GenerateHash(salt + password);

    cout << "Username: " << name << " OldHash: " << hash << " NewHash: " << newHash << "\n";
    return true;
}


