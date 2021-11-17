# Force UTF-8 2022

适用于 Visual Studio 2022 的强制 UTF-8 with BOM 编码拓展。

### 安装步骤
- 下载Release中的.vsix文件。
- 双击安装即可。

### 编译步骤
- 使用VS2022打开`ForceUtf8 2022.sln`。  
- 切换编译目标为`x64`。  
- 生成项目。

### 保存为 UTF-8 without BOM
- 打开`ForceUtf8_2022Package.cs`。
- 修改`private bool withBOM = true;`为`private bool withBOM = false;`。
- 参考`编译步骤`进行编译。
