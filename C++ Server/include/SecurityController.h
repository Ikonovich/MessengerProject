#ifndef SECURITYCONTROLLER_H
#define SECURITYCONTROLLER_H

#include <string>

#include "libs.h"
#include "namespace.h"


class MessengerSystem::SecurityController
{

public:

    SecurityController();
    int GenerateHash(string input, string& output);

private: 

    int GenerateSalt(string& saltString);

};

#endif