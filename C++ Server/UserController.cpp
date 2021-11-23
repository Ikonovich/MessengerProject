#include "include/libs.h"
#include "include/namespace.h"


using namespace MessengerSystem;


//UserController::UserController() {}

// The constructor takes and sets the database controller object.
UserController::UserController(DatabaseController *dataCon) : DatabaseCon(dataCon) {

}


// This message takes an input consisting of a concatenated string of username + requested password and a string reference in which to
// store the response.
// It then asks the DatabaseController to see if the username is already occupied and verifies the password parameters.
// If these checks pass it returns 0 and transmits a Registration Successful[RS] message.
// If the checks fail, it returns -1 and transmits a Registration Unsuccessful[RU] message with an appropriate error message.

int UserController::InitialRegistration(string input, string verifyCode, string& responseOut) 
{

    cout << "Registration request made with input: " << input << "\n";

    string username;
    string password;

    ParseUser(input, username, password);
    cout << "Checking username: " << username << "\n";
    int nameCheck = ValidateUsername(username);

    cout << "Validation check complete with value " << nameCheck << "\n";

    if (nameCheck == 0) {
        // All checks passed, adding new user to database.
        cout << "Username validated successfully \n";
        DatabaseCon->AddUser(username, password);
        responseOut = "RS" + verifyCode + username;
        return 0;
    }
    else if (nameCheck == -1) {

        cout << "Username has already been taken.";
        responseOut = "RU" + verifyCode + "Provided Username is already in use.";
        return -1;

    }
    else if (nameCheck == -2) {

        cout << "Username invalid characters.";

        responseOut = "RU" + verifyCode + "Provided Username is invalid.";
        return -1;

    }
    else if (password.length() > 128) {

        cout << "Password is too long.";

        responseOut = "RU" + verifyCode + "Provided password is too long.";
        return -1;

    }
    else if (password.length() < 8) {

        cout << "Password is too short.";


        responseOut = "RU" + verifyCode + "Provided password is too short.";
        return -1;
    }
    return 0;

}

// Login request receive packet format:
// [LR][32 Char UserName][Up to 128 Char password]
// Login Request Return packet format:
// [Response type - 2 chars] [Optional Error Message - 64 chars]
// LS - Login successful
// LU - Login Unsuccessful


bool UserController::LoginRequest(string input, string verifyCode, string& sessionOut, string& responseOut) {

    string senderID;
    string password;

    ParseUser(input, senderID, password);

    cout << "User attempting to login: " << senderID << " with password " << password << ".\n";

    if (DatabaseCon->VerifyPassword(senderID, password) == false) {

        responseOut = "LU" + verifyCode + "Information for user " + senderID + " could not be verified.";
        return false;
    }

    string session = "";

    session = GenerateSessionID();

    SessionToNameMap[session] = senderID;
    NameToSessionMap[senderID] = session;
    
    cout << "Session " << session << " assigned to user " << senderID << ".\n";
    cout << "Confirming session assignment for user " << senderID << ": " << NameToSessionMap[senderID] << ".\n";
    cout << "Login approved.";

    sessionOut = session;
    responseOut = "LS" + verifyCode + senderID + "User " + senderID + " has logged in successfully.";

    return true;
}

void UserController::MessageReceived(string input, string verifyCode, string& responseOut) {

    string senderID;
    string receiverID;
    string remainder;

    ParseUsers(input, senderID, receiverID, remainder);

    cout << "Message received from user: " + senderID + " for user " + receiverID + ": \n" + remainder + "\n";

}


void UserController::PullMessages(string input, string verifyCode, vector<string>& messages, string& response) {}

// // This method parses out the sender from a data block.

void UserController::ParseUser(string input, string &sender, string &returnMessage) {

    try {

        string senderID = input.substr(0, 32);  // Gets the sender's username of length 32.
        string messageString = input.substr(32, input.length());

        string delimiter = "*";

        size_t senderIDlimit = senderID.find(delimiter);

        if (senderIDlimit != string::npos) {
            senderID.erase(senderIDlimit); // Erases filler characters from the User ID;
        }

        sender = senderID;
    }
    catch(const std::out_of_range err) {

        cout << "Error: The input provided to the parse users function was of insufficient length.";
    }
}

// This method parses out the sender and receiver from a data block that contains both.

void UserController::ParseUsers(string input, string &sender, string &receiver, string &returnMessage) {

    try {
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
    catch(const std::out_of_range err) {

        cout << "Error: The input provided to the parse users function was of insufficient length.";
    }

}

// This method parses the session ID from a string, outputting the session and the remaining string.
// It also compares the session against the user provided, validating that they are connected. 
// If the validation check passes, it returns 0. Else, it returns -1.
int UserController::ValidateSession(string input, string username, string &session, string &remainder) {

    try {
        session = input.substr(0, 32);  // Gets the length 32 session ID.
        remainder = input.substr(32, input.length());
    }
    catch(out_of_range err) {
        cout << "Error: The string provided to validate session was of insufficient length.";
        return -2;
    }

    if (SessionToNameMap.contains(session)) {
        return 0;
    }
    return -1;

}

// This method determines if the username provided by a new registration request is valid.
// It first makes sure the username doesn't contain any special characters and is of a valid length,
// returning -1 if the check fails.
// It then asks the database controller to find a user with that name, returning -2 if one is found, 
// indicating that the name is already in use.
// Otherwise, it returns 0.

int UserController::ValidateUsername(string username) {

    // Length validation
    
    cout << "Validating name length\n";
    if ((username.length() > 32) || (username.length() < 8)) {
        return -2;
    }

    // Character validation

    char nameArray[32];

    strcpy(nameArray, username.c_str());
    cout << "NOTValidating name characters\n";

    // for (int i = 0; i < username.length(); i++) {
    //     int let = nameArray[i]; // Casting the char to an int for boundary checking

    //     //Alphanumeric check 
    //     if (!(((let > 64) && (let < 90)) || ((let > 96) && (let < 123)) || ((let > 47) && (let < 58)) || (let == 95)))  {
           
    //         // Character is invalid
    //         cout << "Invalid character found\n";

    //         return -2;
    //     }
    //     cout << "Iterating\n";
    // }
    
    cout << "NOTValidating name characters\n";
    cout << "Checking username against database\n";

    // Checking to see if the username already exists in the database.
    bool dbRet = DatabaseCon->CheckField("RegisteredUsers", "UserName", username);

    if (dbRet == true) {
        // Username has been found in the database
        cout << "Username is NOT unique.\n";

        return -2;
    }
    cout << "Username is unique.\n";

    return 0;

}

// Returns an unused session ID string of length 32.
string UserController::GenerateSessionID() {

    string sessionID = "";

    for (int i = 0; i < 32; i++) {

        char newChar = 97 + rand() % 26;

        sessionID += newChar;
    }

    return sessionID;
}