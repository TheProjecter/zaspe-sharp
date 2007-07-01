@ECHO OFF

REM
REM Convert SDK sources to Runtime sources
REM brad.taylor@medsphere.com
REM

SET BASE_PATH=C:\Installer
SET SDK_PATH=%BASE_PATH%\SDK
SET RUNTIME_PATH=%BASE_PATH%\Runtime
SET GTK_SDK_PATH=%SDK_PATH%\gtk
SET GTK_RUNTIME_PATH=%RUNTIME_PATH%\gtk

CD %GTK_SDK_PATH%

ECHO Copying binaries...
XCOPY %GTK_SDK_PATH%\bin %GTK_RUNTIME_PATH%\bin /E /Y

ECHO Deleting SDK-only binaries...
CD %GTK_RUNTIME_PATH%\bin
DEL csslint-0.6.exe
DEL envsubst.exe
DEL fc-cache.exe
DEL fc-list.exe
DEL gdk-pixbuf-csource.exe
DEL glib-genmarshal.exe
DEL gobject-query.exe
DEL gspawn-win32-helper-console.exe
DEL gtk-update-icon-cache.exe
DEL gtkaio.sh
DEL glade-2.exe
DEL gladeloader.exe
DEL gettext.exe
DEL gettext.exe
DEL gettext.sh
DEL gsf.exe
DEL gsf-office-thumbnailer.exe
DEL gsf-vba-dump.exe
DEL gtkautogen
DEL gtkconfigure
DEL gtk-demo.exe
DEL iconv.exe
DEL msgattrib.exe
DEL msgcat.exe
DEL msgcmp.exe
DEL msgcomm.exe
DEL msgconv.exe
DEL msgen.exe
DEL msgexec.exe
DEL msgfilter.exe
DEL msgfmt.exe
DEL msggrep.exe
DEL msginit.exe
DEL msgmerge.exe
DEL msgunfmt.exe
DEL msguniq.exe
DEL ngettext.exe
DEL pdffonts.exe
DEL pdfimages.exe
DEL pdfinfo.exe
DEL pdftohtml.exe
DEL pdftoppm.exe
DEL pdftops.exe
DEL pdftotext.exe
DEL pkg-config.exe
DEL test-poppler-glib.exe
DEL xgettext.exe
DEL xmlcatalog.exe
DEL xmllint.exe
DEL libpoppler-1.dll
DEL libpoppler-glib-1.dll

CD %GTK_SDK_PATH%

ECHO Copying libraries...
XCOPY %GTK_SDK_PATH%\lib %GTK_RUNTIME_PATH%\lib /E /Y

CD %GTK_RUNTIME_PATH%\lib

ECHO Deleting SDK-only libraries...
RMDIR /S /Q pkgconfig
RMDIR /S /Q locale
DEL charset.alias

CD pango\1.5.0\modules
DEL *.a

REM Yeah, I know, this looks bad, but really, all the dlls are stored in bin/,
REM and you only need the libs to actually compile an app against it

CD %GTK_RUNTIME_PATH%\lib

DEL *.lib
DEL *.def
DEL *.a
DEL *.sh

ECHO Deleting include headers...
RMDIR /S /Q glib-2.0
RMDIR /S /Q gtk-2.0\include
RMDIR /S /Q gtkglext-1.0
RMDIR /S /Q libglade

CD %GTK_SDK_PATH%

ECHO Copying etc files...
XCOPY %GTK_SDK_PATH%\etc %GTK_RUNTIME_PATH%\etc /E /Y

ECHO Copying shared files...
XCOPY %GTK_SDK_PATH%\share %GTK_RUNTIME_PATH%\share /E /Y

CD %GTK_RUNTIME_PATH%\share

ECHO Deleting documentation...
RMDIR /S /Q gtk-doc
RMDIR /S /Q doc
RMDIR /S /Q glade-2
RMDIR /S /Q glib-2.0
RMDIR /S /Q pixmaps
RMDIR /S /Q aclocal

ECHO Done!
PAUSE
