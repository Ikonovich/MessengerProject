// #include "include/includes.h"
// #include "include/MessengerSystem.h"


// #include "include/Server.h"
// #include "include/Security.h"

// public:

//     Security::Security() {}

//     int Security::GenerateHash(string input, string& output) 
//     {

//         string saltString;

//         if (GenerateSalt(saltString) != 1) {
//             return -1;
//         }

//         char hashInput[] = strcpy(saltString + input);

//         char hashOutput[32];
//         SHA256(hashInput, strlen(hashInput), hashOutput);

//         output = hashOutput;

//         return 0;

//     }

// private:

        

//     int Security::GenerateSalt(string& saltString) {

//         byte saltBuffer[256];

//         return RAND_bytes(saltBuffer, sizeof(buffer));
//     }
