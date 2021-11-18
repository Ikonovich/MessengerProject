-- Messenger Project --

Description:
---- V0.1 ----
This project implements a client/server messaging system. It is intended to allow clients to communicate with friended users, whether offline or online.
Messages will be stored in a MySQL database on the client side. When logging in, clients will pull messages from the server to populate their friend
and group chats and provide new message notifications. 

The client side is implemented in C# using Windows Presentation Foundation and is designed to be integrated with Unity after the core protocol has
completed development. The server side is implemented using C++ and a MySQL database using the MySQL Connector/C++ library.