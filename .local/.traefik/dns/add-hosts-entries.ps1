# Define the entries to add as an array of strings
$entries = @(
    "127.0.0.1 aspire.docker.internal",
    "127.0.0.1 keycloak.docker.internal",
    "127.0.0.1 traefik.docker.internal",
    "127.0.0.1 weather.docker.internal"
)

# Define the path to the hosts file
$hostsPath = "$env:SystemRoot\System32\drivers\etc\hosts"

# Read the contents of the hosts file
$hostsContent = Get-Content -Path $hostsPath

# Loop through each entry
foreach ($entry in $entries) {
    # Check if the entry already exists
    if ($hostsContent -notcontains $entry) {
        # Add the entry to the hosts file
        Add-Content -Path $hostsPath -Value "`r`n$entry" -NoNewline
        Write-Output "Added entry: $entry"
    } else {
        Write-Output "Entry already exists: $entry"
    }
}