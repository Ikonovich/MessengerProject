//OpenSSL 
#include <openssl/bio.h> // Basic IO streams
#include <openssl/err.h> // Errors
#include <openssl/ssl.h> // Core library
#include <openssl/sha.h> // SHA functions


// LibCrypto
#include <openssl/conf.h>
#include <openssl/evp.h>
#include <openssl/err.h> // Errors
 #include <openssl/rand.h> // Random generator

#include "include/libs.h"
#include "include/namespace.h"




SecurityController::SecurityController() {}

string SecurityController::GenerateHash(string saltedPass) 
{
    // --------------- Hash Method 1: ---------------
    const unsigned char* hashInput = (const unsigned char *)saltedPass.c_str();

    unsigned char* hashPtr = (unsigned char *)malloc(sizeof(char) * 32);
    SHA256(hashInput, 32, hashPtr);

    // -----------Hash method 2: ---------------
    // unsigned char hash[SHA256_DIGEST_LENGTH];
    // SHA256_CTX sha256;
    // SHA256_Init(&sha256);

    // unsigned char* hashBuffer = (unsigned char *)malloc(256);

    // SHA256_Update(&sha256, hashBuffer, saltedPass.length());

    // SHA256_Final(hash, &sha256);


    // Converts the hash result to a hexadecimal string.
    string output = "";
    char* charPtr = (char *)malloc(256);
    for (int i = 0; i < SHA256_DIGEST_LENGTH; i++) {

        sprintf(charPtr, "%02x", hashPtr[i]);
        string charString(charPtr);
        output += charString;
    }

    cout << "\nHash result: " << output << "\n";
    return output;

}


string SecurityController::GenerateSalt() {

    cout << "Generating salt.\n";
    unsigned char saltBuffer[32];

    RAND_bytes(saltBuffer, sizeof(saltBuffer));
        
    char* charPtr = (char *)malloc(32);
    string output = "";
    for (int i = 0; i < 32; i++) {

        sprintf(charPtr, "%02x", saltBuffer[i]);
        string charString(charPtr);
        output += charString;
    }
    cout << "Salt generated: " << output << "\n";

    return output;
}
