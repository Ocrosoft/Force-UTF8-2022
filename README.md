# 提示
最新版本 Visual Studio 2022 已经不再需要安装此插件。<br/>
请前往：工具-选项-环境-文档，将“使用特定编码保存文件”设置为“Unicode（UTF-8 无签名）”或“Unicode（UTF-8 带签名）”。

# Force UTF-8 2022

适用于 Visual Studio 2022 的强制 UTF-8 编码拓展。

### 安装步骤（下载 .vsix）
- 下载 Release 中的 Force.UTF-8.with.BOM.2022.vsix 或 Force.UTF-8.2022.vsix 文件。
- 运行 vsix 安装。

### 安装步骤（Visual Studio Marketplace）
- 在 Visual Studio 2022 的扩展窗口，搜索 utf。
- 安装需要的版本即可。

### 编译步骤（With BOM）
- 使用 VS2022 打开 `ForceUtf8 2022.sln`。  
- 切换编译目标为`x64`。  
- 生成项目。

### 编译步骤（No BOM）
- 打开 `ForceUtf8_2022Package.cs`。
- 修改 `private bool withBOM = true;` 为 `private bool withBOM = false;`。
- 切换编译目标为`x64`。
- 生成项目。

### 参考
https://github.com/jz5/vs-force-utf8-2017
