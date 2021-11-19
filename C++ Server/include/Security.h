#ifndef SECURITY_H
#define SECURITY_H


#include "includes.h"
#include <string>

using namespace MessengerSystem;

class MessengerSystem::Security
{

public:

    Security();
    void GenerateHash(string input, string& output);

private: 

    int GenerateSalt(string& saltString);

};

#endif