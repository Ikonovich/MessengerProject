//OpenSSL 
#include <openssl/bio.h> // Basic IO streams
#include <openssl/err.h> // Errors
#include <openssl/ssl.h> // Core library
#include <openssl/sha.h> // SHA functions

// LibCrypto
#include <openssl/conf.h>
#include <openssl/evp.h>
#include <openssl/err.h> // Errors

#include "include/libs.h"
#include "include/namespace.h"




SecurityController::SecurityController() {}

int SecurityController::GenerateHash(string input, string& output) 
{

    // string saltString;

    // if (GenerateSalt(saltString) != 1) {
    //     return -1;
    // }

    // unsigned char hashInput[] = strcpy(saltString + input);

    // unsigned char hashOutput[32];
    // SHA256(hashInput, strlen(hashInput), hashOutput);

    // output = hashOutput;

    return 0;

}


int SecurityController::GenerateSalt(string& saltString) {

//byte saltBuffer[256];

   //return RAND_bytes(saltBuffer, sizeof(saltBuffer));

   return 0;
}
