mkdir .\LockScreen\bin\publish\arm64\LogonService
mkdir .\LockScreen\bin\publish\x64\LogonService
mkdir .\LockScreen\bin\publish\x86\LogonService

copy /B /Y ..\..\Windows-logon-service\LogonService\LogonService_4.8.1\bin\ARM64\Release\LogonService.exe  .\LockScreen\bin\publish\arm64\LogonService
copy /B /Y ..\..\Windows-logon-service\LogonService\LogonService_4.8.1\bin\x64\Release\LogonService.exe  .\LockScreen\bin\publish\x64\LogonService
copy /B /Y ..\..\Windows-logon-service\LogonService\LogonService_4.8.1\bin\x86\Release\LogonService.exe  .\LockScreen\bin\publish\x86\LogonService

copy /Y .\LogonService\LogonService.exe.config  .\LockScreen\bin\publish\arm64\LogonService
copy /Y .\LogonService\LogonService.exe.config  .\LockScreen\bin\publish\x64\LogonService
copy /Y .\LogonService\LogonService.exe.config  .\LockScreen\bin\publish\x86\LogonService