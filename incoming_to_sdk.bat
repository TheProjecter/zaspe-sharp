@ECHO OFF

REM
REM Convert Ivan's installers (to be placed in C:\Installer\Incoming) to our
REM SDK sources
REM

SET BASE_PATH=C:\Installer
SET INCOMING=C:\GTK
SET GTK_SDK_PATH=%BASE_PATH%\SDK\gtk

CD %INCOMING%

ECHO Copying binaries...
XCOPY %INCOMING%\bin %GTK_SDK_PATH%\bin /E /Y /I

ECHO Deleting unneccessary binaries...
CD %GTK_SDK_PATH%\bin
DEL gtkthemeselector.exe

CD %INCOMING%

ECHO Copying libraries...
XCOPY %INCOMING%\lib %GTK_SDK_PATH%\lib /E /Y /I

ECHO Copying include headers...
XCOPY %INCOMING%\include %GTK_SDK_PATH%\include /E /Y /I

ECHO Copying etc files...
XCOPY %INCOMING%\etc %GTK_SDK_PATH%\etc /E /Y /I

ECHO Copying share files...
XCOPY %INCOMING%\share %GTK_SDK_PATH%\share /E /Y /I

CD %GTK_SDK_PATH%\share
RMDIR /S /Q gtkthemeselector

ECHO Done!
PAUSE
