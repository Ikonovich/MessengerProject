#ifndef SECURITYCONTROLLER_H
#define SECURITYCONTROLLER_H

#include <string>

#include "libs.h"
#include "namespace.h"


class MessengerSystem::SecurityController
{

public:

    SecurityController();

    string GenerateHash(string input);

    string GenerateSalt();

};

#endif