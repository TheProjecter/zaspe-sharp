@ECHO OFF

REM
REM Builds SDK and Runtime installers
REM brad.taylor@medsphere.com
REM

SET BASE_PATH=C:\Installer
SET SDK_PATH=%BASE_PATH%\SDK
SET RUNTIME_PATH=%BASE_PATH%\Runtime
SET GTK_SDK_PATH=%SDK_PATH%\gtk
SET GTK_RUNTIME_PATH=%RUNTIME_PATH%\gtk

SET ISCC_PATH="C:\Archivos de programa\Inno Setup 5\Compil32.exe"
SET ISCC_FLAGS=/cc

ECHO Building SDK installer...
CD %SDK_PATH%
%ISCC_PATH% %ISCC_FLAGS% SDK.iss

ECHO Building Runtime installer...
CD %RUNTIME_PATH%
%ISCC_PATH% %ISCC_FLAGS% Runtime.iss

ECHO Done!
PAUSE
