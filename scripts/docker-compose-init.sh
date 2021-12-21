#!/bin/bash
folder="./.local"
db_password_file="$folder/db_password.txt"
connection_string_file="$folder/connection_string.txt"
cert_file="$folder/cert.pfx"
cert_password_file="$folder/cert_password.txt"

db_password=$(uuidgen)
database=SELearning

cert_password=$(uuidgen)


# Create $folder folder if it doesn't exist
if [ ! -d "$folder" ]; then
  mkdir $folder
fi

# If password exists, read it from file
# Otherwise, write the generated password to file
if [ -f "$db_password_file" ]; then
    db_password=$(cat "$db_password_file")
else
    echo "$db_password" > $(echo "$db_password_file")
fi

# If connection string file exists, delete it
if [ -f "$connection_string_file" ]; then
    rm $connection_string_file
fi

# Write connection string to file
connection_string=$(echo "Server=db;Database=${database};User Id=sa;Password=${db_password};TrustServerCertificate=true")
echo "$connection_string" > $(echo "${folder}/connection_string.txt")

# If password exists, read it from file
# Otherwise, write the generated password to file
if [ -f "$cert_password_file" ]; then
    cert_password=$(cat "$cert_password_file")
else
    echo "$cert_password" > $(echo "$cert_password_file")
fi

# Delete cert file if it exists
if [ -f "$cert_file" ]; then
    rm $cert_file
fi

dotnet dev-certs https -ep $(echo "${folder}/cert.pfx") -p $cert_password
dotnet dev-certs https --trust
