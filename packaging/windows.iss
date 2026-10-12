#ifndef AppVersion
  #define AppVersion "2.0.0-preview.1"
#endif
#ifndef RuntimeId
  #define RuntimeId "win-x64"
#endif
[Setup]
AppId={{AEB2B1C2-DF1A-4B0A-B312-3455DD782810}
AppName=DigestingDuck
AppVersion={#AppVersion}
DefaultDirName={localappdata}\Programs\DigestingDuck
DefaultGroupName=DigestingDuck
PrivilegesRequired=lowest
#if RuntimeId == "win-arm64"
ArchitecturesAllowed=arm64
ArchitecturesInstallIn64BitMode=arm64
#else
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
#endif
OutputDir=..\artifacts
OutputBaseFilename=DigestingDuck-{#AppVersion}-{#RuntimeId}-setup
Compression=lzma2
SolidCompression=yes
UninstallDisplayIcon={app}\DigestingDuck.Desktop.exe
[Files]
Source: "..\publish\{#RuntimeId}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
[Icons]
Name: "{group}\DigestingDuck"; Filename: "{app}\DigestingDuck.Desktop.exe"
[Run]
Filename: "{app}\DigestingDuck.Desktop.exe"; Description: "Abrir DigestingDuck"; Flags: nowait postinstall skipifsilent
