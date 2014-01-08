@ECHO OFF
@SET SITE="http://localhost:34308"

SET SITE=
SET /P SITE=Enter the SharePoint Site Url : %=%

ECHO Installing Site Solution . . . .

powershell.exe .\DeploySolution.ps1 %SITE%