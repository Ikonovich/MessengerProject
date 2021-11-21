#include "include/libs.h"
#include "include/namespace.h"


using namespace MessengerSystem;


DatabaseController::DatabaseController() {

    
}

// This method initializes the database variables and connects to the database.
// Returns 0 if successful, returns -1 if it fails.

int DatabaseController::ConnectDatabase() {

    try {

        // ResultSet *resultSet; 
        // //Result *result; // Represents return result for a query that does not return data.


        // driver = get_mysql_driver_instance();
        // connection = driver->connect("tcp://127.0.0.1:3306", "root", "giga321");

        // connection->setSchema("messenger_database");

        // statement = connection->createStatement();

        //statement->execute("INSERT INTO RegisteredUsers(UserName, UserStatus) VALUES (\"Ikonovich\", \"Active\");");

        //resultSet = statement->executeQuery("SELECT * FROM RegisteredUsers WHERE UserName=\"Ikonovich\"");

        return 0;
    }
    catch (...) {

        std::cout << "The database has failed to connect properly.\n";

        return -1;
    }


}
