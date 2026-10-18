<meta name="google-site-verification" content="MGrMbBi28Ut57MeqKI6srDSmlrprH0lug6rwNMIi0_A" /># RemoteApp 工具

（使用AI进行了汉化处理）


借助微软 RemoteApp 技术，你可以流畅使用运行在另一台电脑上的应用程序。

RemoteApp 工具是一款实用工具，用于在 Windows 系统（7、8、10、11、XP 以及服务器系统）上创建和管理 RemoteApp，还可为客户端生成 RDP 和 MSI 文件。

如果你希望 RemoteApp 显示在客户端的开始菜单，或是通过网页界面访问，请查看 [RAWeb](https://github.com/kimmknight/raweb)！

如果你对 RemoteApp 工具存在疑问、意见或建议，请前往 [讨论区](https://github.com/kimmknight/remoteapptool/discussions)。

## 功能特性

- 在 Windows 桌面系统与服务器上创建、管理 RemoteApp
- 生成 RDP 文件
- 生成 MSI 安装包（需要 WiX 工具集）
- 使用远程桌面网关
- 设置会话超时等选项
- 为应用程序选择图标
- 为已部署的应用设置文件类型关联
- 对 RDP 文件进行签名
- 备份 RemoteApp

## 运行要求

- Microsoft .Net Framework 4
- https://wixtoolset.org/docs/wix3/ （如需制作 MSI 安装包，安装完成后重启电脑）
- **受支持版本**的 Windows XP、7、8、10 或服务器系统。详见 https://github.com/kimmknight/remoteapptool/wiki/Windows-Compatibility。

**注意：** 如果你尝试在不兼容的 Windows 版本（例如 Windows 10 家庭版）托管 RemoteApp，工具虽然可以运行，但 RemoteApp**无法正常工作**。RDP 客户端会显示正在连接，随后直接消失，不会弹出任何错误提示。

## 下载

**最新版本：**

[RemoteApp Tool 6.1.0.0 Installer](https://github.com/kimmknight/remoteapptool/releases/download/v6.1.0.0/RemoteApp.Tool.6100.msi)

[RemoteApp Tool 6.1.0.0 Zip](https://github.com/kimmknight/remoteapptool/releases/download/v6.1.0.0/RemoteApp.Tool.6100.zip)

请注意：最新版安装包不再支持 Windows XP，XP 用户请使用压缩包版本。

其他近期版本可在 [此处](https://github.com/kimmknight/remoteapptool/releases) 获取，久远的旧版本在 [这里](https://github.com/kimmknight/remoteapptool/wiki/Version-history-(pre-GitHub))。

## 使用指南

https://github.com/kimmknight/remoteapptool/wiki/Create-a-RemoteApp-and-use-it-on-another-computer

## Screenshots

![Screenshot 1](https://raw.githubusercontent.com/wiki/kimmknight/remoteapptool/images/screenshots/ss1.png)


![Screenshot 2](https://raw.githubusercontent.com/wiki/kimmknight/remoteapptool/images/screenshots/ss2.png)


![Screenshot 3](https://raw.githubusercontent.com/wiki/kimmknight/remoteapptool/images/screenshots/ss3.png)


![Screenshot 4](https://raw.githubusercontent.com/wiki/kimmknight/remoteapptool/images/screenshots/ss5.png)


![Screenshot 4](https://raw.githubusercontent.com/wiki/kimmknight/remoteapptool/images/screenshots/ss4.png)
