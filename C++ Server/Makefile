CC=g++
CPPFLAGS=-I -L
INC=-I /usr/include/mysql-cppconn-8/
LDFLAGS=-L /lib/x86_64-linux-gnu/
LDLIBS+=-lmysqlclient
LDLIBS+=-lmysqlcppconn8
DEPS=includes.h

%.o: %.c ${DEPS}
	${CC} -c -o $@ $< ${CFLAGS}

servermake: Server.cpp
	${CC} Server.cpp -o Server -lmysqlcppconn -lmysqlcppconn8 -I/usr/include/mysql-cppconn-8/ -I/usr/include/mysql-cppconn-8/jdbc -std=c++2a