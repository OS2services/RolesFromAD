#define AppId "{{937e5a02-8368-40bc-9826-a1228a844ef3}"
#define AppSourceDir "..\Source\bin\Debug"
#define AppName "AulaDagtilbudADIntegration"
#define AppVersion "1.0.0"
#define AppPublisher "Digital Identity"
#define AppURL "http://digital-identity.dk/"
#define AppExeName "AulaDagtilbudADIntegration"

[Setup]
AppId={#AppId}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
DefaultDirName={pf}\{#AppPublisher}\{#AppName}
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
OutputBaseFilename={#AppExeName}
Compression=lzma
SolidCompression=yes
SourceDir={#AppSourceDir}
OutputDir=..\..\..\Installer

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "*.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "*.config"; DestDir: "{app}"; Flags: ignoreversion onlyifdoesntexist
Source: "*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "*.pdb"; DestDir: "{app}"; Flags: ignoreversion

[Run]
Filename: "{app}\AulaDagtilbud AD Integration.exe"; Parameters: "install" 

[UninstallRun]
Filename: "{app}\AulaDagtilbud AD Integration.exe"; Parameters: "uninstall"
