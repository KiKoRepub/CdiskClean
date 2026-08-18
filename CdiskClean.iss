; CdiskClean 安装包构建脚本（Inno Setup 6.x）
; 使用方法：将本文件放在项目根目录，然后用 Inno Setup 编译器（ISCC.exe）编译：
;   ISCC.exe CdiskClean.iss
; 注意：编译前请先执行一次自包含发布，生成 publish\win-x64 目录：
;   dotnet publish CdiskClean.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o publish\win-x64

#define MyAppName "CdiskClean"
#define MyAppVersion "1.0.0"
#define MyAppExeName "CdiskClean.exe"

[Setup]
; 注意：AppId 应保持唯一且稳定，卸载时依赖此 ID
AppId={{5C4351FD-F27E-4976-86EE-39D5EB5FD987}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher=CdiskClean
DefaultDirName={autopf}\CdiskClean
DisableProgramGroupPage=yes
; 按用户安装，不需要管理员权限（应用清单为 asInvoker）
PrivilegesRequired=lowest
; 应用为 x64 构建，仅允许 64 位系统安装
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
OutputDir=publish
OutputBaseFilename=CdiskClean-Setup-{#MyAppVersion}
SetupIconFile=hfGrandTheatre.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
; 最低支持 Windows 10 1607（.NET 8 WinForms 要求）
MinVersion=10.0.14393

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "chinesesimplified"; MessagesFile: "compiler:Languages\ChineseSimplified.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
; 打包自包含发布产物（含全部运行依赖），排除调试符号文件
Source: "publish\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Excludes: "*.pdb"

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
