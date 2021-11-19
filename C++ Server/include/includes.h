#ifndef INCLUDES_H
#define INCLUDES_H

// Including the namespace
#include "MessengerSystem.h" 

#include <iostream>

// Headers for socket(), getaddrinfo(), etc

//in_port_t, in_addr_t
#include <arpa/inet.h>

// may make available the type in_port_t and the type in_addr_t
// Defines hostent, netent, protoent, and servent structures
#include <netdb.h>

// Makes available socklen_t, defines sa_family_t integral type
// Defines sockaddr, msghdr, and cmsghdr structures
#include <sys/socket.h>

// Includes definitions for tons of types
#include <sys/types.h>

#include <unistd.h> // Miscellaneous symbolic constants, types, and functions.

#include <string.h> // Memset


#include <vector>   // Vector data type

#include <unordered_map>    // Dictionary data type

//OpenSSL 
#include <openssl/bio.h> // Basic IO streams
#include <openssl/err.h> // Errors
#include <openssl/ssl.h> // Core library
#include <openssl/sha.h> // SHA functions

// LibCrypto
#include <openssl/conf.h>
#include <openssl/evp.h>
#include <openssl/err.h> // Errors

//MySQL
#include <mysql_connection.h>
#include <mysql_driver.h>

#include <mysqlx/xdevapi.h>
#include <mysqlx/devapi/result.h>


#include <cppconn/driver.h>
#include <cppconn/connection.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>
#include <cppconn/prepared_statement.h>


// Json parser
#include "nlohmann/json.hpp"
using json = nlohmann::json;


#endif