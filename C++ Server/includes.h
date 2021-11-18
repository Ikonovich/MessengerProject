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

// Miscellaneous symbolic constants, types, and functions.
#include <unistd.h>

// Memset
#include <string.h>

// Vector data type
#include <vector>

// Dictionary data type
#include <unordered_map>

//MySQL
#include <mysql_connection.h>
#include <mysql_driver.h>

#include <mysqlx/xdevapi.h>


#include <cppconn/driver.h>
#include <cppconn/connection.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>
#include <cppconn/prepared_statement.h>


// Json parser
#include "nlohmann/json.hpp"
using json = nlohmann::json;