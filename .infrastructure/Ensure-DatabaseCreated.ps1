[CmdletBinding()]
param (
    [Parameter(Mandatory = $true)]
    [String]
    $Server,

    [Parameter(Mandatory = $true)]
    [String]
    $Database
)

begin {}

process {
    $accessToken = (Get-AzAccessToken -ResourceUrl "https://database.windows.net").Token

    $query = @"
        IF NOT EXISTS (
            SELECT * FROM sys.databases
            WHERE [name] = '$Database'
        )
        BEGIN
            CREATE DATABASE [$Database]
        END
"@
    Invoke-Sqlcmd -ServerInstance "$Server.database.windows.net" `
        -Database $Database `
        -Query $query `
        -AccessToken $accessToken
}

end {}