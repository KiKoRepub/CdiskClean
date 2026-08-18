# CdiskClean 打包指南

本文档说明如何将 CdiskClean 打包成一个可执行安装包（`setup.exe`）。
安装包内包含运行所需的全部依赖（.NET 运行时、AntdUI、SQLite 等），**不含任何源代码文件**。

## 一、产出物

| 文件 | 说明 |
|------|------|
| `publish\win-x64\` | 自包含发布产物（运行所需的全部文件） |
| `publish\CdiskClean-Setup-1.0.0.exe` | 最终安装包（约 50 MB，LZMA2 压缩） |
| `CdiskClean.iss` | Inno Setup 构建脚本（可复用） |

## 二、前置条件

1. **.NET SDK**（≥ 8.0，用于发布）
2. **Inno Setup 6**（安装器编译器）
   - 官方下载：https://jrsoftware.org/isdl.php
   - 安装后 `ISCC.exe` 位于 `C:\Program Files (x86)\Inno Setup 6\ISCC.exe`

## 三、打包步骤

### 第 1 步：自包含发布

```bash
# 注意：必须发布 csproj 而非 sln！
# 解决方案里还有 TestForDisk 项目，发布 sln 会把它的 exe 混进产物，且会报
# MSB3190（ClickOnce 不支持 requireAdministrator）导致整体失败。
dotnet publish CdiskClean.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o publish\win-x64
```

说明：
- `--self-contained true`：把 .NET 8 桌面运行时一并打进产物，**目标机器无需安装任何运行时**。
- `-r win-x64`：程序为 x64 构建（csproj 中 `PlatformTarget=x64`），只支持 64 位 Windows。

### 第 2 步：编译安装包

```bash
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" CdiskClean.iss
```

脚本会读取 `publish\win-x64\`，输出安装包到 `publish\CdiskClean-Setup-1.0.0.exe`。

## 四、安装包特性

- **免管理员**：按用户安装（`PrivilegesRequired=lowest`），安装到 `%LocalAppData%\Programs\CdiskClean`，无 UAC 弹窗
- **双语安装界面**：简体中文 / 英文，跟随系统语言
- **自动创建**：开始菜单快捷方式 + 桌面快捷方式（可选）
- **自带卸载程序**：控制面板"应用和程序"中可正常卸载，卸载时清除全部文件
- **最低系统要求**：Windows 10 1607 及以上
- **排除项**：`*.pdb`（调试符号）不进入安装包

## 五、升级版本时需修改

发布新版本时，编辑 `CdiskClean.iss` 顶部：

```ini
#define MyAppVersion "1.0.0"     ; ← 改成新版本号
```

- 输出文件名会自动变为 `CdiskClean-Setup-<版本号>.exe`
- **`AppId` 不要改**（`5C4351FD-...`），卸载/升级依赖它识别旧版本
- 旧版直接运行新版安装包即可覆盖升级，用户数据（SQLite 数据库）保留

## 六、注意事项

1. **必须先发布再编译**：`ISCC.exe` 不会自动执行 `dotnet publish`，每次打包前确保 `publish\win-x64` 是最新产物（建议删除旧目录后重新发布）。
2. **不要发布解决方案（sln）**：见第 1 步中的注释，直接发布 `CdiskClean.csproj`。
3. **版本号三处一致**：`CdiskClean.iss` 的 `AppVersion`、程序集版本、以及 README 中的说明应同步更新。
4. **签名**：目前安装包未做代码签名，Windows SmartScreen 可能提示"未知发布者"。如要正式分发，可购买代码签名证书，在 `[Setup]` 段加 `SignTool=signtool $f` 并配置 `[Files]` 后处理。
5. **应用启动后会在自身目录生成 `CdiskClean.db`**（SQLite 数据），属正常现象；卸载时一并删除。
6. **安装器本身不包含源代码**：打包内容只有发布产物，`.cs` / `.csproj` / `docs` / `claude-prompt.txt` 等均不在其中（已验证）。

## 七、命令行静默安装（可选）

```bash
CdiskClean-Setup-1.0.0.exe /VERYSILENT /SUPPRESSMSGBOXES /NORESTART
```

用于批量部署；`/DIR="C:\自定义路径"` 可指定安装位置，`/LOG=setup.log` 记录日志。
