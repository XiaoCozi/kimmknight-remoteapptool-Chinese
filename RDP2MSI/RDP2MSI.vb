Module RDP2MSI

    Sub Main()
        Dim cmdRdpPath = ""
        Dim cmdSwitches = ""

        LogText("RDP2MSI" & vbCrLf)
        If My.Application.CommandLineArgs.Count = 0 Then
            ShowUsage()
            End
        ElseIf My.Application.CommandLineArgs.Count = 1 Then
            cmdRdpPath = My.Application.CommandLineArgs(0)
        ElseIf My.Application.CommandLineArgs.Count = 2 Then
            If My.Application.CommandLineArgs(0).StartsWith("/") Then
                cmdSwitches = My.Application.CommandLineArgs(0)
                cmdRdpPath = My.Application.CommandLineArgs(1)
            Else
                cmdSwitches = My.Application.CommandLineArgs(1)
                cmdRdpPath = My.Application.CommandLineArgs(0)
            End If
        ElseIf My.Application.CommandLineArgs.Count > 2 Then
            LogText("错误：参数过多。", True)
        End If

        Dim relCmdRdpPath = System.IO.Directory.GetCurrentDirectory & "\" & cmdRdpPath
        If Not cmdRdpPath.Contains(":") Then cmdRdpPath = relCmdRdpPath

        If Not My.Computer.FileSystem.FileExists(cmdRdpPath) Then LogText("错误：找不到 RDP 文件：" & cmdRdpPath, True)

        If Not cmdSwitches.ToUpper.Contains("D") Then
            If Not cmdSwitches.ToUpper.Contains("S") Then
                cmdSwitches += "DS"
            End If
        End If


        PrepareRDP2MSI(cmdRdpPath, cmdSwitches.ToUpper)
    End Sub

    Sub ShowUsage()
        LogText("用法： rdp2msi.exe [/DSNATU] rdpfile.rdp")
        LogText("")
        LogText("开关：")
        LogText("")
        LogText("  /D     MSI 将部署桌面快捷方式。")
        LogText("  /S     MSI 将在开始菜单 > 程序 > (AppName) 中部署快捷方式。")
        LogText("  /N     需要 /S。MSI 不会在开始菜单 > 程序中创建子文件夹。")
        LogText("  /A     基于应用名称生成升级代码，否则随机生成。")
        LogText("  /T     不在部署的快捷方式上包含 (remote) 标签。")
        LogText("  /U     MSI 按用户安装。默认情况下，MSI 按机器安装。")
        LogText("")
        LogText("如果未指定开关，则默认使用 /DS。")
        LogText("")
        LogText("如果存在与 RDP 文件同名的 ICO 文件，将使用该文件。")
        LogText("")
    End Sub

    Private Sub PrepareRDP2MSI(rdpFilePath As String, Optional cmdSwitches As String = "DS")
        If Not My.Computer.FileSystem.FileExists(rdpFilePath) Then LogText("错误：找不到 RDP 文件：" & rdpFilePath, True)
        If Not rdpFilePath.ToLower.EndsWith(".rdp") Then LogText("错误：输入文件必须是 RDP 文件。", True)

        Dim RDP As New RDP2MSIlib.RDP
        RDP.rdpPath = rdpFilePath

        If Not RDP.WixInstalled Then LogText("错误：找不到 WiX 工具集。如果您刚安装它，请重启后再试。", True)

        LogText("RDP 文件：" & rdpFilePath)
        Dim RemoteAppFullName = RDP.ReadRDPProperty("remoteapplicationname")
        Dim RDPFullAddress = RDP.ReadRDPProperty("full address")
        If RDPFullAddress = "" Then LogText("错误：RDP 文件不包含远程桌面服务器地址。", True)
        If RemoteAppFullName = "" Then
            'Full desktop connection
            LogText("RDP 文件类型：完整桌面会话")
        Else
            'RemoteApp connection
            LogText("RDP 文件类型：RemoteApp")
            LogText("应用完整名称：" & RemoteAppFullName)
        End If
        LogText("命令行开关：" & cmdSwitches.ToUpper)


        RDP.ShortcutInStart = cmdSwitches.Contains("S")
        RDP.ShortcutOnDesktop = cmdSwitches.Contains("D")
        RDP.ShortcutSubfolderInStart = Not cmdSwitches.Contains("N")
        Dim ShortcutLocations As New List(Of String)
        If RDP.ShortcutInStart Then
            If RDP.ShortcutSubfolderInStart Then
                ShortcutLocations.Add("开始菜单（子文件夹）")
            Else
                ShortcutLocations.Add("开始菜单（顶层）")
            End If
        End If
        If RDP.ShortcutOnDesktop Then ShortcutLocations.Add("桌面")

        LogText("快捷方式位置：" & String.Join("、", ShortcutLocations))

        RDP.ProductUpgradeRandom = Not cmdSwitches.Contains("A")
        LogText("MSI 升级代码：{" & RDP.ProductUpgradeCode & "}")
        If cmdSwitches.Contains("T") Then RDP.ProductRemoteTag = ""

        RDP.PerUser = cmdSwitches.Contains("U")
        If RDP.PerUser Then
            LogText("MSI 安装上下文：用户")
        Else
            LogText("MSI 安装上下文：机器")
        End If

        Dim ShortcutTag = "无"
        If Not RDP.ProductRemoteTag = "" Then ShortcutTag = "(" & RDP.ProductRemoteTag & ")"
        LogText("快捷方式标签：" & ShortcutTag)

        LogText("")
        LogText("正在生成 MSI...")

        RDP.CreateMSI()



    End Sub


    Private Sub LogText(TheText As String, Optional DoExit As Boolean = False)
        Console.WriteLine(TheText)
        If DoExit = True Then
            'CleanupTempFiles()
            End
        End If
    End Sub

End Module


