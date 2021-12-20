$folder = "./.local"
$db_password_file = "$folder/db_password.txt"
$connection_string_file = "$folder/connection_string.txt"
$cert_file = "$folder/cert.pfx"
$cert_password_file = "$folder/cert_password.txt"

$db_password = New-Guid
$database = SELearning

$cert_password = New-Guid

# Create $folder if it doesn't exist
if (!(Test-Path $folder)) {
    New-Item -ItemType Directory -Force -Path $folder
}

# If $db_password_file exists, read the password from it
if (Test-Path $db_password_file) {
    $db_password = (Get-Content $db_password_file).Trim()
} else {
    Set-Content $db_password_file $db_password
}

# If $connection_string_file exists, delete it
if (Test-Path $connection_string_file) {
    Remove-Item $connection_string_file -Force
}

connection_string = "Server=db;Database=$database;User Id=sa;Password=$db_password;TrustServerCertificate=True"
Set-Content $connection_string_file $connection_string

# If cert_password_file exists, read the password from it
if (Test-Path $cert_password_file) {
    $cert_password = (Get-Content $cert_password_file).Trim()
} else {
    Set-Content $cert_password_file $cert_password
}

# Delete cert_file if it exists
if (Test-Path $cert_file) {
    Remove-Item $cert_file -Force
}

dotnet dev-certs https -ep $cert_file -p $cert_password
dotnet dev-certs https --trust