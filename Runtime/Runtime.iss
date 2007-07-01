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
; Runtime installer
;
#include "../Base.iss"

[Setup]
AppName=Gtk# Runtime for Windows
AppVerName=Gtk# Runtime for Windows {#VERSION}
DefaultDirName={pf}\ZaspeSharp\GtkRun

OutputBaseFilename=gtksharp-runtime-{#VERSION}

AppID={{DE88B637-C288-44B0-B79C-3DD94F79903F}

[Icons]
Name: {group}\Gtk# Runtime\Theme Selector; Filename: {app}\bin\gtkthemeselector.exe; Comment: GTK+ Theme Selector
Name: {group}\Gtk# Runtime\{cm:UninstallProgram,Gtk# for Windows}; Filename: {uninstallexe}

[Registry]
Root: HKLM; Subkey: SOFTWARE\ZaspeSharp\Gtk-Sharp\Runtime; Flags: uninsdeletekeyifempty
Root: HKLM; Subkey: SOFTWARE\ZaspeSharp\Gtk-Sharp\Runtime; ValueType: string; ValueName: FrameworkAssemblyDirectory; ValueData: {app}\lib; Flags: uninsdeletevalue
Root: HKLM; Subkey: SOFTWARE\ZaspeSharp\Gtk-Sharp\Runtime; ValueType: string; ValueName: SdkInstallRoot; ValueData: {app}; Flags: uninsdeletevalue
Root: HKLM; Subkey: SOFTWARE\ZaspeSharp\Gtk-Sharp\Runtime; ValueType: dword; ValueName: GtkSharpIsInstalled; ValueData: 1; Flags: uninsdeletevalue
Root: HKLM; Subkey: SOFTWARE\ZaspeSharp\Gtk-Sharp\Runtime; ValueType: string; ValueName: Version; ValueData: {#VERSION}; Flags: uninsdeletevalue
Root: HKLM; Subkey: SYSTEM\CurrentControlSet\Control\Session Manager\Environment; ValueType: string; ValueName: GTK_BASEPATH; ValueData: {app}; Flags: uninsdeletevalue
Root: HKLM; Subkey: SYSTEM\CurrentControlSet\Control\Session Manager\Environment; ValueType: expandsz; ValueName: Path; ValueData: {code:AddBinToPath}

[Code]
// Checks to see if the .NET Framework SDK 2.0 is Installed
Function IsMsNetSDK20Installed() : Boolean;
begin
    Result := RegValueExists(HKLM, 'SOFTWARE\Microsoft\.NETFramework', 'sdkInstallRootv2.0');
end;

// Checks to see if the .NET Framework Runtime 2.0 is Installed
Function IsMsNetRuntime20Installed() : Boolean;
begin
    Result := RegValueExists(HKLM, 'SOFTWARE\Microsoft\.NETFramework\policy\v2.0', '50727');
end;

// Checks to see if Gtk# Installer for MS .NET SDK is Installed
Function IsCurrentlyInstalled() : Boolean;
begin
	Result := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{DE88B637-C288-44B0-B79C-3DD94F79903F}_is1');
end;

Function InitializeSetup : Boolean;
begin
	// Check requirements before Installation
	If IsCurrentlyInstalled() = true Then
	begin
	 Result := false;
	 SayMessage('Gtk# Runtime seems to be installed.' + #13 + #10 + 'Please uninstall it and run this setup again.', mbError);
	 exit;
	end
	If IsMsNetRuntime20Installed() = false Then
	begin
	 Result := false;
	 SayMessage('Microsft .NET Framework Runtime 2.0 does not seem to be installed.' + #13 + #10 + 'Please install it and run this setup again.', mbError);
	 exit;
	end
	Result := true;
end;

function AddBinToPath(Param: String): String;
var
    strOrigRegVal, strBinDir: String;
    bRc: Boolean;
begin
    // Get the value for the BinDir
    strBinDir := ExpandConstant('{app}') + '\bin';

    // Get the registry value
    bRc := RegQueryStringValue(HKLM, 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', 'Path', strOrigRegVal);
    // Concatenate Bin and Lib paths to the Original Value
    If bRc = true Then
    begin
        if Pos(strBinDir, strOrigRegVal) < 1 Then
        begin
            // Add the BinDir string if is not already there
            strOrigRegVal :=  strBinDir + ';' + strOrigRegVal;
        end;
        Result := strOrigRegVal;
    end;

end;

procedure RemoveBinFromPath();
var
    strOrigRegVal, strBinDir: String;
    bRc: Boolean;
begin
    // Get the value for the BinDir
    strBinDir := ExpandConstant('{app}') + '\bin';

    // Get the registry value
    bRc := RegQueryStringValue(HKLM, 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', 'Path', strOrigRegVal);
    // Concatenate Bin and Lib paths to the Original Value
    If bRc = true Then
    begin
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
	If IsMsNetRuntime20Installed() = true Then
	begin
        bRc := RegQueryStringValue(HKLM, 'SOFTWARE\Microsoft\.NETFramework', 'sdkInstallRootv2.0', strSDKBaseDir);
        strFilePath := RemoveBackslash(cstrBasePath) + '\bin\gtk-sharp_msgac_install.bat'
        ReplaceRootPathForBat(strFilePath, 'C:\Program Files\Microsoft.NET\SDK\v1.1\', strSDKBaseDir);
        strFilePath := RemoveBackslash(cstrBasePath) + '\bin\gtk-sharp_msgac_uninstall.bat'
        ReplaceRootPathForBat(strFilePath, 'C:\Program Files\Microsoft.NET\SDK\v1.1\', strSDKBaseDir);
	end
    else IF IsMsNetSDK20Installed() = true Then // Base on SDK 1.0
	begin
        bRc := RegQueryStringValue(HKLM, 'SOFTWARE\Microsoft\.NETFramework', 'sdkInstallRootv2.0', strSDKBaseDir);
        strFilePath := RemoveBackslash(cstrBasePath) + '\bin\gtk-sharp_msgac_install.bat'
        ReplaceRootPathForBat(strFilePath, 'C:\Program Files\Microsoft Visual Studio .NET 2003\SDK\v1.1\', strSDKBaseDir);
        strFilePath := RemoveBackslash(cstrBasePath) + '\bin\gtk-sharp_msgac_uninstall.bat'
        ReplaceRootPathForBat(strFilePath, 'C:\Program Files\Microsoft Visual Studio .NET 2003\SDK\v1.1\', strSDKBaseDir);
	end;
end;

Procedure WriteRootPath(const cstrBasePath, cstrBasePathForwardSlash: String);
var strFilePath: String;
begin
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
	// Batch Files
	strFilePath := RemoveBackslash(cstrBasePath) + '\bin\gtk-sharp_msgac_install.bat'
	ReplaceRootPathForBat(strFilePath, 'C:\Target', cstrBasePath);
	strFilePath := RemoveBackslash(cstrBasePath) + '\bin\gtk-sharp_msgac_uninstall.bat'
	ReplaceRootPathForBat(strFilePath, 'C:\Target', cstrBasePath);
end;

Procedure CurStepChanged(CurStep: TSetupStep);
var
 strBasePath: String;
 strShortPath, strBasePathForwardSlash: String;
 strGacInstallBat: String;
 strReconfigBat: String;
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

    // Run the reconfigure script
    strReconfigBat := RemoveBackslash(strBasePath) + '\bin\reconfig.bat'
    Exec(
      strReconfigBat, '',
      '',
      0,
      ewWaitUntilTerminated,
      nRc);
  end;
end;

Procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
    If CurUninstallStep = usUninstall Then
    begin
        RemoveBinFromPath();
    end;
end;

