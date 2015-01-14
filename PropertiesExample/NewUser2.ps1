
$rutaAcceso="\\FURIA\Perfiles\%username%"

$password=$args[7] | ConvertTo-SecureString -AsPlainText -Force

New-ADUser $args[0] -GivenName $args[1] -Surname $args[2] -DisplayName ($args[1] + " " +$args[2]) -EmailAddres $args[3] -MobilePhone ("+34" + $args[4]) -EmployeeNumber $args[5] -EmployeeID $args[6] -AccountPassword $password -ProfilePath $rutaAcceso -HomeDirectory $rutaAcceso -Enabled 1 -PasswordNeverExpires 1 
