#!/bin/bash
folder="./.local"

db_password=$(uuidgen)
cert_password=$(uuidgen)
database=SELearning
connection_string=$(echo "server=localhost;database=${database};user id=sa;password=${db_password}")

mkdir $folder 
echo "$db_password" > $(echo "${folder}/db_password.txt")
echo "$connection_string" > $(echo "${folder}/connection_string.txt")

dotnet dev-certs https -ep $(echo "${folder}/cert.pfx") -p $cert_password
dotnet dev-certs https --trust
echo "$cert_password" > $(echo "${folder}/cert_password.txt")
