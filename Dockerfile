# A MySQL server equipped Ubuntu distro
# This container will be used to host a C++ server for communicating with a Unity video game.

FROM ubuntu:latest

RUN echo "Hopefully this build goes well."

# Adding -y after install will cause all prompts to be answered with yes

RUN apt-get update && apt-get install -y mysql-server 
RUN apt-get update && apt-get install -y build-essential
RUN apt-get update && apt-get install -y manpages-dev

#RUN mysql_secure_installation

RUN mkdir -p /home/Docker

WORKDIR /home/Docker

RUN service mysql start

