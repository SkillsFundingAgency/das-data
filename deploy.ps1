$username = "[USERNAME]"
$password = "[PASSWORD]"
$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $username,$password)))

$apiUrl = "https://datapipe.scm.azurewebsites.net/api/zip/site/wwwroot"
$filePath = "C:\path\to\ase-dev-fn-demo.zip"
Invoke-RestMethod -Uri $apiUrl -Headers @{Authorization=("Basic {0}" -f $base64AuthInfo)} -Method PUT -InFile $filePath -ContentType "multipart/form-data"