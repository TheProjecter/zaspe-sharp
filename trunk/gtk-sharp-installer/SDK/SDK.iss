;
; win32-gtk#-installer
; Copyright (C) 2004-2006 Francisco G. Martinez.
; Copyright (C) 2007 Medsphere Systems Corporation.
; Copyright (C) 2007 Milton Pividori.
;
; This file is part of win32-gtk#-installer.
;
; win32-gtk#-installer is free software; you can redistribute it and/or modify
; it under the terms of the GNU General Public License as published by the Free
; Software Foundation; either version 2 of the License, or (at your option) any
; later version.  win32-gtk#-installer is distributed in the hope that it will
; be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
; MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General
; Public License for more details.

; You should have received a copy of the GNU General Public License along with
; win32-gtk#-installer; if not, write to the Free Software Foundation, Inc.,
; 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

;
; SDK installer
;
#include "../Base.iss";

#define REGISTRY_PATH 'SOFTWARE\Medsphere\Gtk-Sharp\SDK';

[Setup]
AppName=Gtk# SDK for Windows
AppVerName=Gtk# SDK for Windows {#VERSION}
DefaultDirName={pf}\ZaspeSharp\GtkSDK

OutputBaseFilename=gtksharp-sdk-{#VERSION}

AppID={{CAEE0352-BB68-4A47-99E3-911A7D09C450}

;Associate the Document Type .glade with glade-2.exe
ChangesAssociations=yes

[Files]
Source: samples\*; DestDir: {app}\samples; Flags: ignoreversion recursesubdirs
Source: docs\*; DestDir: {app}\docs; Flags: ignoreversion recursesubdirs
Source: templates\csharp\*; DestDir: {code:GetVSCSharpPrjPath}; Flags: ignoreversion recursesubdirs
Source: templates\vb\*; DestDir: {code:GetVB7PrjPath}; Flags: ignoreversion recursesubdirs
Source: wizards\csharp\*; DestDir: {code:GetVSCSharpWizardsPath}; Flags: ignoreversion recursesubdirs
Source: wizards\vb\*; DestDir: {code:GetVB7WizardsPath}; Flags: ignoreversion recursesubdirs

[INI]
Filename: {app}\GtkPlus.url; Section: InternetShortcut; Key: URL; String: http://www.gtk.org
Filename: {app}\GtkSharp.url; Section: InternetShortcut; Key: URL; String: http://gtk-sharp.sourceforge.net/index.html
Filename: {app}\MonoDocWeb.url; Section: InternetShortcut; Key: URL; String: http://www.go-mono.com/docs/index.aspx

[Icons]
Name: {group}\Gtk# SDK\{cm:ProgramOnTheWeb,GTK+}; Filename: {app}\GtkPlus.url
Name: {group}\Gtk# SDK\{cm:ProgramOnTheWeb,Gtk#}; Filename: {app}\GtkSharp.url
Name: {group}\Gtk# SDK\Docs\Mono Documentation on the Web; Filename: {app}\MonoDocWeb.url
Name: {group}\Gtk# SDK\Docs\Glade# Quick Start Tutorial; Filename: {app}\docs\glade\index.html
Name: {group}\Gtk# SDK\Applications\Glade; Filename: {app}\bin\glade-2.exe; Comment: GTK+ GUI designer
Name: {group}\Gtk# SDK\Applications\ThemeSeletor; Filename: {app}\bin\gtkthemeselector.exe; Comment: GTK+ Theme Selector
Name: {group}\Gtk# SDK\Samples\Samples Directory; Filename: {app}\samples; Comment: Gtk# Samples
Name: {group}\Gtk# SDK\{cm:UninstallProgram,Gtk# for Windows}; Filename: {uninstallexe}

[Registry]
Root: HKLM; Subkey: {#REGISTRY_PATH}; Flags: uninsdeletekeyifempty
Root: HKLM; Subkey: {#REGISTRY_PATH}; ValueType: string; ValueName: FrameworkAssemblyDirectory; ValueData: {app}\lib; Flags: uninsdeletevalue
Root: HKLM; Subkey: {#REGISTRY_PATH}; ValueType: string; ValueName: SdkInstallRoot; ValueData: {app}; Flags: uninsdeletevalue
Root: HKLM; Subkey: {#REGISTRY_PATH}; ValueType: dword; ValueName: GtkSharpIsInstalled; ValueData: 1; Flags: uninsdeletevalue
Root: HKLM; Subkey: {#REGISTRY_PATH}; ValueType: string; ValueName: Version; ValueData: {#VERSION}; Flags: uninsdeletevalue
Root: HKLM; Subkey: {#REGISTRY_PATH}; ValueType: dword; ValueName: GtkPlusDevIsInstalled; ValueData: 1; Flags: uninsdeletevalue
Root: HKLM; Subkey: SYSTEM\CurrentControlSet\Control\Session Manager\Environment; ValueType: expandsz; ValueName: Path; ValueData: {code:AddBinAndLibToPath}

; Glade as default editor in VS
Root: HKCU; Subkey: SOFTWARE\Microsoft\VisualStudio\7.1\Default Editors; Flags: uninsdeletekeyifempty
Root: HKCU; Subkey: SOFTWARE\Microsoft\VisualStudio\7.1\Default Editors\glade; ValueType: string; ValueName: Custom; ValueData: glade-2.exe; Flags: uninsdeletevalue
Root: HKCU; Subkey: SOFTWARE\Microsoft\VisualStudio\7.1\Default Editors\glade; ValueType: dword; ValueName: Type; ValueData: 2; Flags: uninsdeletevalue
Root: HKCU; Subkey: SOFTWARE\Microsoft\VisualStudio\7.1\Default Editors\glade\glade-2.exe; ValueType: string; ValueData: {app}\bin\glade-2.exe; Flags: uninsdeletevalue
Root: HKLM; SubKey: SOFTWARE\Microsoft\VisualStudio\7.1\AssemblyFolders\Atk; ValueType: string; ValueData: {app}\lib\gtk-sharp-2.0\atk; Flags: uninsdeletekey
Root: HKLM; SubKey: SOFTWARE\Microsoft\VisualStudio\7.1\AssemblyFolders\Gdk; ValueType: string; ValueData: {app}\lib\gtk-sharp-2.0\gdk; Flags: uninsdeletekey
Root: HKLM; SubKey: SOFTWARE\Microsoft\VisualStudio\7.1\AssemblyFolders\Glade; ValueType: string; ValueData: {app}\lib\gtk-sharp-2.0\glade; Flags: uninsdeletekey
Root: HKLM; SubKey: SOFTWARE\Microsoft\VisualStudio\7.1\AssemblyFolders\Glib; ValueType: string; ValueData: {app}\lib\gtk-sharp-2.0\glib; Flags: uninsdeletekey
Root: HKLM; SubKey: SOFTWARE\Microsoft\VisualStudio\7.1\AssemblyFolders\Gtk; ValueType: string; ValueData: {app}\lib\gtk-sharp-2.0\gtk; Flags: uninsdeletekey
Root: HKLM; SubKey: SOFTWARE\Microsoft\VisualStudio\7.1\AssemblyFolders\Gtk.Net; ValueType: string; ValueData: {app}\lib\gtk-sharp-2.0\gtk-dotnet; Flags: uninsdeletekey
Root: HKLM; SubKey: SOFTWARE\Microsoft\VisualStudio\7.1\AssemblyFolders\Pango; ValueType: string; ValueData: {app}\lib\gtk-sharp-2.0\pango; Flags: uninsdeletekey
Root: HKLM; SubKey: SOFTWARE\Microsoft\VisualStudio\7.1\AssemblyFolders\Mono.Cairo; ValueType: string; ValueData: {app}\lib; Flags: uninsdeletekey

; Associate the Document Type .glade with Glade.exe
Root: HKCR; Subkey: .glade; ValueType: string; ValueName: ; ValueData: GladeFile; Flags: uninsdeletevalue
Root: HKCR; Subkey: GladeFile; ValueType: string; ValueName: ; ValueData: Glade File; Flags: uninsdeletekey
Root: HKCR; Subkey: GladeFile\DefaultIcon; ValueType: string; ValueName: ; ValueData: {app}\bin\glade-2.exe,0; Flags: uninsdeletekey
Root: HKCR; Subkey: GladeFile\shell\open\command; ValueType: string; ValueName: ; ValueData: """{app}\bin\glade-2.exe"" ""%1"""; Flags: uninsdeletekey

[UninstallRun]
Filename: {app}\bin\gtk-sharp_msgac_uninstall.bat; StatusMsg: Un-install Gtk# from the Microsoft GAC; Flags: runhidden

[UninstallDelete]
Type: files; Name: {app}\GtkPlus.url
Type: files; Name: {app}\GtkSharp.url
Type: files; Name: {app}\MonoDocWeb.url

[Code]
// Checks to see if Visual C# 2003 is installed
Function IsMsVSCSInstalled() : Boolean;
begin
    Result := RegValueExists(HKLM,
    'SOFTWARE\Microsoft\VisualStudio\7.1\Projects\{FAE04EC0-301F-11d3-BF4B-00C04F79EFBC}',
    'ProjectTemplatesDir'
    );
end;

// Checks to see if the .NET Framework SDK 2.0 is Installed
Function IsMsNetSDK20Installed() : Boolean;
begin
    Result := RegValueExists(HKLM, 'SOFTWARE\Microsoft\.NETFramework', 'sdkInstallRootv2.0');
end;

// Checks to see if Gtk# Installer for MS .NET SDK is Installed
Function IsCurrentlyInstalled() : Boolean;
begin
	Result := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{F972701B-F524-44EE-9566-2CA628766EEC}_is1');
end;

Function InitializeSetup : Boolean;
begin
	// Check requirements before Installation
	If IsCurrentlyInstalled() = true Then
	begin
	 Result := false;
	 SayMessage('Gtk# for MS .NET Framework SDK seems to be installed.' + #13 + #10 + 'Please uninstall it and run this setup again.', mbError);
	 exit;
	end
	If IsMsNetSDK20Installed() = false Then
	begin
	 Result := false;
	 SayMessage('Microsoft .NET Framework SDK 2.0 does not seem to be installed.' + #13 + #10 + 'Please install it and run this setup again.', mbError);
	 exit;
	end
	Result := true;
end;

// Obtain the path to the Wizards Templates folder used by
// Visual C# 2003
function GetVSCSharpWizardsPath(Param: String): String;
var
    strWizardsTemplatesDir: String;
    bRc: Boolean;
begin
    // Get the registry value
    bRc := RegQueryStringValue(
    HKLM,
    'SOFTWARE\Microsoft\VisualStudio\7.1\Projects\{FAE04EC0-301F-11d3-BF4B-00C04F79EFBC}',
    'WizardsTemplatesDir',
    strWizardsTemplatesDir
    );

    If bRc = true Then
    begin
        Result := strWizardsTemplatesDir;
    end;
end;

// Obtain the path to the Project Template folder used by
// Visual C# 2003
function GetVSCSharpPrjPath(Param: String): String;
var
    strProjectTemplatesDir: String;
    bRc: Boolean;
begin
    // Get the registry value
    bRc := RegQueryStringValue(
    HKLM,
    'SOFTWARE\Microsoft\VisualStudio\7.1\Projects\{FAE04EC0-301F-11d3-BF4B-00C04F79EFBC}',
    'ProjectTemplatesDir',
    strProjectTemplatesDir
    );

    If bRc = true Then
    begin
        Result := strProjectTemplatesDir;
    end;
end;

// Obtain the path to the Wizards Templates folder used by
// Visual Basic .NET
function GetVB7WizardsPath(Param: String): String;
var
    strWizardsTemplatesDir: String;
    bRc: Boolean;
begin
    // Get the registry value
    bRc := RegQueryStringValue(
    HKLM,
    'SOFTWARE\Microsoft\VisualStudio\7.1\Projects\{F184B08F-C81C-45f6-A57F-5ABD9991F28F}',
    'WizardsTemplatesDir',
    strWizardsTemplatesDir
    );

    If bRc = true Then
    begin
        Result := strWizardsTemplatesDir;
    end;
end;

// Obtain the path to the Project Template folder used by
// Visual Basic .NET
function GetVB7PrjPath(Param: String): String;
var
    strProjectTemplatesDir: String;
    bRc: Boolean;
begin
    // Get the registry value
    bRc := RegQueryStringValue(
    HKLM,
    'SOFTWARE\Microsoft\VisualStudio\7.1\Projects\{F184B08F-C81C-45f6-A57F-5ABD9991F28F}',
    'ProjectTemplatesDir',
    strProjectTemplatesDir
    );

    If bRc = true Then
    begin
        Result := strProjectTemplatesDir;
    end;
end;

function AddBinAndLibToPath(Param: String): String;
var
    strOrigRegVal, strBinDir, strLibDir: String;
    bRc: Boolean;
begin
    // Get the value for the LibDir
    strLibDir := ExpandConstant('{app}') + '\lib';
    // Get the value for the BinDir
    strBinDir := ExpandConstant('{app}') + '\bin';

    // Get the registry value
    bRc := RegQueryStringValue(HKLM, 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', 'Path', strOrigRegVal);
    // Concatenate Bin and Lib paths to the Original Value
    If bRc = true Then
    begin
        if Pos(strLibDir, strOrigRegVal) < 1 Then
        begin
            // Add the LibDir string if is not already there
            strOrigRegVal := strOrigRegVal + ';' + strLibDir;
        end;
        if Pos(strBinDir, strOrigRegVal) < 1 Then
        begin
            // Add the BinDir string if is not already there
            strOrigRegVal := strOrigRegVal + ';' + strBinDir;
        end;
        Result := strOrigRegVal;
    end;
end;

procedure RemoveBinAndLibToPath();
var
    strOrigRegVal, strBinDir, strLibDir: String;
    bRc: Boolean;
begin
    // Get the value for the LibDir
    strLibDir := ExpandConstant('{app}') + '\lib';
    // Get the value for the BinDir
    strBinDir := ExpandConstant('{app}') + '\bin';

    // Get the registry value
    bRc := RegQueryStringValue(HKLM, 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', 'Path', strOrigRegVal);
    // Concatenate Bin and Lib paths to the Original Value
    If bRc = true Then
    begin
        // Remove the LibDir
        StringChange(strOrigRegVal, ';' + strLibDir, '');
        // Remove the BinDir
        StringChange(strOrigRegVal, ';' + strBinDir, '');
        bRc := RegWriteStringValue (HKLM, 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', 'Path', strOrigRegVal);
    end;
end;

// Utility function to configure the batchfile use for
// registering Gtk# in the MS GAC
Procedure ConfigureGacInstallBat(const cstrBasePath: String);
var
    strFilePath, strSDKBaseDir: String;
    bRc: Boolean;
begin
	// Get Path of the sdkvars.bat
	// Base on SDK 1.1
	If IsMsNetSDK20Installed() = true Then
	begin
        bRc := RegQueryStringValue(HKLM, 'SOFTWARE\Microsoft\.NETFramework', 'sdkInstallRootv2.0', strSDKBaseDir);
        strFilePath := RemoveBackslash(cstrBasePath) + '\bin\gtk-sharp_msgac_install.bat'
        ReplaceRootPathForBat(strFilePath, 'C:\Program Files\Microsoft.NET\SDK\v1.1\', strSDKBaseDir);
        strFilePath := RemoveBackslash(cstrBasePath) + '\bin\gtk-sharp_msgac_uninstall.bat'
        ReplaceRootPathForBat(strFilePath, 'C:\Program Files\Microsoft.NET\SDK\v1.1\', strSDKBaseDir);
	end;
end;

Procedure WriteRootPath(const cstrBasePath, cstrBasePathForwardSlash: String);
var strFilePath: String;
begin
    // Shell Scripts
	strFilePath := RemoveBackslash(cstrBasePath) + '\bin\cert2spc.'
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	// etc files
	strFilePath := RemoveBackslash(cstrBasePath) + '\etc\gtk-2.0\gdk-pixbuf.loaders.'
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\etc\gtk-2.0\gtk.immodules.'
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\etc\gtk-2.0\gtkrc.'
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\etc\pango\pango.modules.'
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
    // Config files in bin
	strFilePath := RemoveBackslash(cstrBasePath) + '\bin\freetype-config.'
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\bin\libpng-config.'
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\bin\libpng12-config.'
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\bin\xml2-config.'
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
    // Pkg-config files
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gecko-sharp.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gecko-sharp-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\art-sharp-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gapi-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\glade-sharp-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gtk-dotnet-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gtk-sharp-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\rsvg-sharp-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\art-sharp.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gapi.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gdkglext-1.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gdkglext-win32-1.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gtk-engines-2.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gtkglext-1.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gtkglext-win32-1.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\libart-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\libgsf-1.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\libiconv.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\librsvg-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\atk.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\fontconfig.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\freetype2.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gdk-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gdk-pixbuf-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gdk-win32-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\glib-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gmodule-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gobject-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gthread-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gtk+-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\gtk+-win32-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\libglade-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\libintl.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\libpng.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\libpng12.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\libxml-2.0.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\mint.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\mono.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\pango.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\pangoft2.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	strFilePath := RemoveBackslash(cstrBasePath) + '\lib\pkgconfig\pangowin32.pc';
	ReplaceRootPath(strFilePath, 'C:\Target', 'C:/Target', cstrBasePath, cstrBasePathForwardSlash);
	// Batch Files
	strFilePath := RemoveBackslash(cstrBasePath) + '\bin\gtk-sharp_msgac_install.bat'
	ReplaceRootPathForBat(strFilePath, 'C:\Target', cstrBasePath);
	strFilePath := RemoveBackslash(cstrBasePath) + '\bin\gtk-sharp_msgac_uninstall.bat'
	ReplaceRootPathForBat(strFilePath, 'C:\Target', cstrBasePath);
end;

Procedure RemoveTemplateFromCSharpVsdirFile(const strCSharpVsdir: String; strLineForRemoval: String);
var
  strFileContents: String;
begin
  // Read the contents of the file into a string
  if LoadStringFromFile(strCSharpVsdir, strFileContents) = true then
  begin
    // Perform search and replace
    StringChange(strFileContents, (#13 + #10 + strLineForRemoval), '');

    // Write the changed string back out to the file
    SaveStringToFile(strCSharpVsdir, strFileContents, false);
  end;
end;

Procedure RemoveTemplateFromVB7VsdirFile(const strVB7Vsdir: String; strLineForRemoval: String);
var
  strFileContents: String;
begin
  // Read the contents of the file into a string
  if LoadStringFromFile(strVB7Vsdir, strFileContents) = true then
  begin
    // Perform search and replace
    StringChange(strFileContents, (#13 + #10 + strLineForRemoval), '');

    // Write the changed string back out to the file
    SaveStringToFile(strVB7Vsdir, strFileContents, false);
  end;
end;

Procedure AddTemplateToCSharpVsdirFile(const strCSharpVsdir: String; strAddLine: String);
var
    strText : String;
begin
    // Read the contents of the file into a string
    if LoadStringFromFile(strCSharpVsdir, strText) = true then
    begin
        if Pos((#13 + #10 + strAddLine), strText) < 1 then
        begin
            strText := strText + #13 + #10 + strAddLine;
            SaveStringToFile(strCSharpVsdir, strText, False);
        end
    end
end;

Procedure AddTemplateToVB7VsdirFile(const strVB7Vsdir: String; strAddLine: String);
var
    strText : String;
begin
    // Read the contents of the file into a string
    if LoadStringFromFile(strVB7Vsdir, strText) = true then
    begin
        if Pos((#13 + #10 + strAddLine), strText) < 1 then
        begin
            strText := strText + #13 + #10 + strAddLine;
            SaveStringToFile(strVB7Vsdir, strText, False);
        end
    end
end;

Procedure CurStepChanged(CurStep: TSetupStep);
var
 strBasePath, strSelectedComponents: String;
 strShortPath, strBasePathForwardSlash: String;
 strCSharpVsdirFile: String;
 strVB7VsdirFile: String;
 strGacInstallBat: String;
 nRc: Integer;
begin
  If CurStep = ssDone THEN
  Begin
    // Signal System that the environment has changed
    // Through the use of my external function/DLL
    SignalPathChange();
  End
  if CurStep = ssPostInstall THEN
  begin
    strBasePath := ExpandConstant('{app}');
    if Pos(' ', strBasePath) <> 0 THEN
    begin
      strShortPath := GetShortName(strBasePath);
      strBasePath := strShortPath;
    end;
    strBasePathForwardSlash := strBasePath;
    StringChange(strBasePathForwardSlash, '\', '/');
    WriteRootPath(strBasePath, strBasePathForwardSlash);
    ConfigureGacInstallBat(strBasePath);
    strGacInstallBat := RemoveBackslash(strBasePath) + '\bin\gtk-sharp_msgac_install.bat'
    Exec(
     strGacInstallBat,'',
     '',
     0,
     ewWaitUntilTerminated,
     nRc);
    strSelectedComponents := WizardSelectedComponents(false);
    // C# Template addition
    if Pos('gtk1\vscsharptemplates', strSelectedComponents) > 0 then
    begin
        strCSharpVsdirFile := GetVSCSharpPrjPath('') + '\CSharp.vsdir';
        AddTemplateToCSharpVsdirFile(strCSharpVsdirFile,
        'GladeSharp.vsz|{FAE04EC1-301F-11d3-BF4B-00C04F79EFBC}|Glade# Project|2|Glade# Project Template|{FAE04EC1-301F-11d3-BF4B-00C04F79EFBC}|4554| |GladeSharp'
        );
        AddTemplateToCSharpVsdirFile(strCSharpVsdirFile,
        'GtkSharp.vsz|{FAE04EC1-301F-11d3-BF4B-00C04F79EFBC}|Gtk# Project|3|Gtk# Project Template|{FAE04EC1-301F-11d3-BF4B-00C04F79EFBC}|4554| |GtkSharp'
        );
    end;
    // VB.NET Template addition
    if Pos('gtk1\vb7templates', strSelectedComponents) > 0 then
    begin
        strVB7VsdirFile := GetVB7PrjPath('') + '\projects_std.vsdir';
        AddTemplateToVB7VsdirFile(strVB7VsdirFile,
        'GladeSharpApplication.vsz|{164B10B9-B200-11D0-8C61-00A0C91E29D5}|Glade# Project|1|Glade# Project Template|{164B10B9-B200-11D0-8C61-00A0C91E29D5}|4507| |GladeSharpApplication'
        );
    end;
  end;
end;

Procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
 strCSharpVsdirFile: String;
 strVB7VsdirFile: String;
begin
    If CurUninstallStep = usUninstall Then
    begin
        RemoveBinAndLibToPath();
        strCSharpVsdirFile := GetVSCSharpPrjPath('') + '\CSharp.vsdir';
        RemoveTemplateFromCSharpVsdirFile(strCSharpVsdirFile,
        'GladeSharp.vsz|{FAE04EC1-301F-11d3-BF4B-00C04F79EFBC}|Glade# Project|2|Glade# Project Template|{FAE04EC1-301F-11d3-BF4B-00C04F79EFBC}|4554| |GladeSharp'
        );
        RemoveTemplateFromCSharpVsdirFile(strCSharpVsdirFile,
        'GtkSharp.vsz|{FAE04EC1-301F-11d3-BF4B-00C04F79EFBC}|Gtk# Project|3|Gtk# Project Template|{FAE04EC1-301F-11d3-BF4B-00C04F79EFBC}|4554| |GtkSharp'
        );
        strVB7VsdirFile := GetVB7PrjPath('') + '\projects_std.vsdir';
        RemoveTemplateFromVB7VsdirFile(strVB7VsdirFile,
        'GladeSharpApplication.vsz|{164B10B9-B200-11D0-8C61-00A0C91E29D5}|Glade# Project|1|Glade# Project Template|{164B10B9-B200-11D0-8C61-00A0C91E29D5}|4507| |GladeSharpApplication'
        );
    end;
end;
