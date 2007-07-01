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

;
; Generic Setup file imported by SDK and Runtime
;

#define public VERSION '2.10.1-1';

[Setup]
AppPublisher=ZaspeSharp
AppPublisherURL=http://code.google.com/p/zaspe-sharp/
AppSupportURL=http://code.google.com/p/zaspe-sharp/
AppUpdatesURL=http://code.google.com/p/zaspe-sharp/
DefaultGroupName=ZaspeSharp

;Enable bzip for testing, lzma/max for release
Compression=lzma/max
SolidCompression=true

DisableDirPage=false
DisableReadyPage=false
VersionInfoCompany=Milton Pividori
;InfoBeforeFile=ReleaseNotes.txt
;InfoAfterFile=GtkSharp_1.9.2\RightAfterInstall.txt
LicenseFile=License.txt
;ShowLanguageDialog=yes
;AlwaysRestart=yes
;WizardSmallImageFile=installerBannerSmall256_52x52.bmp
;WizardImageFile=new-installerBanner256.bmp
;UninstallDisplayIcon={app}\mono.ico
;SetupIconFile=mono.ico

[Files]
Source: gtk\*; DestDir: {app}; Flags: ignoreversion recursesubdirs
Source: gtk-sharp\*; DestDir: {app}; Flags: ignoreversion recursesubdirs
Source: gts\*; DestDir: {app}; Flags: ignoreversion recursesubdirs
Source: poppler\*; DestDir: {app}; Flags: ignoreversion recursesubdirs
; Apply hotfixes last, so that they override original files
Source: hotfixes\*; DestDir: {app}; Flags: ignoreversion recursesubdirs
Source: batandsh\gtk-sharp_msgac_install.bat; DestDir: {app}\bin; Flags: ignoreversion
Source: batandsh\gtk-sharp_msgac_uninstall.bat; DestDir: {app}\bin; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: Support\InstHlpr.dll; Flags: dontcopy

[Registry]
Root: HKLM; Subkey: SOFTWARE\Microsoft\.NETFramework\AssemblyFolders\Gtk-Sharp-2.0; ValueType: string; ValueData: {app}\lib\gtk-sharp-2.0; Flags: uninsdeletekey
Root: HKLM; Subkey: SYSTEM\CurrentControlSet\Control\Session Manager\Environment; ValueType: string; ValueName: GTK_BASEPATH; ValueData: {app}; Flags: uninsdeletevalue
Root: HKLM; SubKey: SOFTWARE\GTK\2.0; ValueType: string; ValueName: Path; ValueData: {app}; Flags: uninsdeletekey
Root: HKLM; SubKey: SOFTWARE\GTK\2.0; ValueType: string; ValueName: Version; ValueData: {#VERSION}

[UninstallRun]
Filename: {app}\bin\gtk-sharp_msgac_uninstall.bat; StatusMsg: Un-install Gtk# from the Microsoft GAC; Flags: runhidden

[Code]
//importing a custom DLL function
procedure SignalPathChange();
external 'SignalPathChange@files:InstHlpr.dll stdcall';

Function GetMonoBasePath(strParam1 : string): string;
var
    strMonoBaseDir: String;
    bRc: Boolean;
    strDefaultCLR: String;
begin
    // Get current CLR version
    bRc := RegQueryStringValue(HKLM, 'SOFTWARE\Novell\Mono', 'DefaultCLR', strDefaultCLR);
    If bRc = true Then
    begin
      // Get the registry value
      bRc := RegQueryStringValue(HKLM, 'SOFTWARE\Novell\Mono\' + strDefaultCLR, 'SdkInstallRoot',
      strMonoBaseDir
      );
    end;

    If bRc = true Then
    begin
        Result := strMonoBaseDir;
    end;
end;

Function SayMessage(const strMsg: String; const typMsgBox: TMsgBoxType) : Integer;
begin
  Result := MsgBox(strMsg, typMsgBox, MB_OK);
end;

Procedure ReplaceRootPathForBat(var strFileName: String; const strOrignal, strReplacement: String);
var
  strFileContents: String;
begin
  // Read the contents of the file into a string
  if LoadStringFromFile(strFileName, strFileContents) = true then
  begin
    // Perform search and replace
    StringChange(strFileContents, strOrignal, strReplacement);

    // Write the changed string back out to the file
    SaveStringToFile(strFileName, strFileContents, false);
  end;
end;

Procedure ReplaceRootPath(var strFileName: String; const strOrignal,
strOriginalForwardSlash, strReplacement, strReplacementForwardSlash: String);
var
  strFileContents: String;
begin
  // Read the contents of the file into a string
  if LoadStringFromFile(strFileName, strFileContents) = true then
  begin

    // Perform search and replace
    StringChange(strFileContents, strOrignal, strReplacement);
    StringChange(strFileContents, strOriginalForwardSlash, strReplacementForwardSlash);

    // Write the changed string back out to the file
    SaveStringToFile(strFileName, strFileContents, false);
  end;
end;
