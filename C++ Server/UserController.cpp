#include "include/libs.h"
#include "include/namespace.h"


using namespace MessengerSystem;


//UserController::UserController() {}

// The constructor takes the database controller object.
UserController::UserController(const dataCon) : DatabaseController(dataCon) {

}


// This message takes an input consisting of a concatenated string of username + requested password and a string reference in which to
// store the response.
// It then asks the DatabaseController to see if the username is already occupied and verifies the password parameters.
// If these checks pass it returns 0 and transmits a Registration Successful[RS] message.
// If the checks fail, it returns -1 and transmits a Registration Unsuccessful[RU] message with an appropriate error message.

int UserController::InitialRegistration(string input, string& responseOut) 
{

    cout << "Registration request made.\n";

    string userName;
    string data;

    ParseUser(input, userName, data);

    
    // if (NameToIDMap.count(userName) > 0) {

    //     response = "RUThe provided username was already in use.";
    //     return -1;
    // }


   responseOut = "RS" + userName;
   return 0;

}

// Login request receive packet format:
// [LR][32 Char UserName][Up to 128 Char password]
// Login Request Return packet format:
// [Response type - 2 chars] [Optional Error Message - 64 chars]
// LS - Login successful
// LU - Login Unsuccessful


void UserController::LoginRequest(string input, string& responseOut) {

    // string senderID;
    // string password;

    // ParseUser(input, senderID, password);

    // cout << "User attempting to login: " << senderID << " on socket " << std::to_string(clientSocket) << ".\n";

    // string session = "";

    // if (IDtoSessionMap.contains(senderID) == false) {

    //     session = GenerateSessionID();
    // }

    // SessionToIDMap[session] = senderID;
    // IDtoSessionMap[senderID] = session;
    // SessionToSocketMap[session] = clientSocket;

    // cout << "Session " << session << " assigned to user " << senderID << ".\n";

    // cout << "Confirming session assignment for user " << senderID << ": " << IDtoSessionMap[senderID] << ".\n";
    // cout << "Login approved.";
}

void UserController::MessageReceived(string input, string& responseOut) {

    string data = input;

    string senderID;
    string receiverID;
    string remainder;

    ParseUsers(data, senderID, receiverID, remainder);

    cout << "Message received from user: " + senderID + " for user " + receiverID + ": \n" + remainder + "\n";

}


void UserController::PullMessages(string input, vector<string>& messages, string& response) {}

// // This method parses out the sender from a data block.

void UserController::ParseUser(string input, string &sender, string &returnMessage) {

    string senderID = input.substr(0, 32);  // Gets the sender's username of length 32.
    string messageString = input.substr(32, input.length());

    string delimiter = "*";

    size_t senderIDlimit = senderID.find(delimiter);

    if (senderIDlimit != string::npos) {
        senderID.erase(senderIDlimit); // Erases filler characters from the User ID;
    }

    sender = senderID;
}

// // This method parses out the sender and receiver from a data block that contains both.

void UserController::ParseUsers(string input, string &sender, string &receiver, string &returnMessage) {

    string senderID = input.substr(0, 32);  // Gets the sender's username of length 32.
    string receiverID = input.substr(32, 32); // Gets the receiver's username of length 32.
    string messageString = input.substr(64, input.length());


    string delimiter = "*";

    size_t senderIDlimit = senderID.find(delimiter);
    if (senderIDlimit != string::npos) {
        senderID.erase(senderIDlimit); // Erases filler characters from the User ID;
    }

    size_t receiverIDlimit = receiverID.find(delimiter);
    if (receiverIDlimit != string::npos) {
        receiverID.erase(receiverIDlimit); // Erases filler characters from the receiver ID;
    }

    sender = senderID;
    receiver = receiverID;

}
