SET FOLDER="%CD%"
SET PORT=8181

SET IIS="C:\Program Files (x86)\IIS Express\iisexpress.exe"

%IIS% /port:%PORT% /path:%FOLDER% /clr:v4.0

REM PAUSE


