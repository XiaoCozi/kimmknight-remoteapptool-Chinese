Module HelpSystem
    Public Sub SetupTips(TheForm As System.Windows.Forms.Form)
        Dim toolTip1 As New ToolTip With {
            .AutoPopDelay = 10000,
            .InitialDelay = 500,
            .ReshowDelay = 500
        }

        Dim HelpString As String

        For Each Control As Control In TheForm.Controls
            For Each SubControl As Control In Control.Controls
                For Each SubSubControl In SubControl.Controls
                    For Each SubSubSubControl In SubSubControl.Controls
                        HelpString = GetTipString(Control.Parent.Name, SubSubSubControl.Name)
                        If Not HelpString = "" Then toolTip1.SetToolTip(SubSubSubControl, HelpString)
                    Next
                    HelpString = GetTipString(Control.Parent.Name, SubSubControl.Name)
                    If Not HelpString = "" Then toolTip1.SetToolTip(SubSubControl, HelpString)
                Next
                HelpString = GetTipString(Control.Parent.Name, SubControl.Name)
                If Not HelpString = "" Then toolTip1.SetToolTip(SubControl, HelpString)
            Next
            HelpString = GetTipString(Control.Parent.Name, Control.Name)
            If Not HelpString = "" Then toolTip1.SetToolTip(Control, HelpString)
        Next

    End Sub

    Private Function GetTipString(FormName As String, ControlName As String) As String
        Dim TipString As String = ""

        Dim TipText As String = GetTipFile()
        Dim TipArray = TipText.Split(ControlChars.CrLf.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)

        For Each TipLine As String In TipArray
            Dim TipLineArray = TipLine.Split("|")
            If TipLineArray(0) = FormName And TipLineArray(1) = ControlName Then TipString = TipLineArray(2)
        Next

        TipString = TipString.Replace("\r", vbCr)
        TipString = TipString.Replace("\n", vbLf)

        Return TipString
    End Function

    Private Function GetTipFile() As String
        Dim TipFile As String = ""

        If My.Computer.FileSystem.FileExists("tips.txt") Then
            TipFile = My.Computer.FileSystem.ReadAllText("tips.txt")
        Else
            TipFile = GetBuiltInTips()
        End If

        Return TipFile
    End Function

    Private Function GetBuiltInTips() As String
        Dim Tips As String = ""

        Tips += "RemoteAppMainWindow|CreateButton|添加新的 RemoteApp。" & vbCrLf
        Tips += "RemoteAppMainWindow|DeleteButton|移除选中的 RemoteApp。" & vbCrLf
        Tips += "RemoteAppMainWindow|EditButton|编辑选中 RemoteApp 的属性。" & vbCrLf
        Tips += "RemoteAppMainWindow|CreateClientConnection|为选中的 RemoteApp 创建 RDP 文件或 MSI 安装程序。" & vbCrLf

        Tips += "RemoteAppEditWindow|SaveButton|保存更改并关闭。" & vbCrLf
        Tips += "RemoteAppEditWindow|FTAButton|设置此 RemoteApp 的文件类型关联。" & vbCrLf
        Tips += "RemoteAppEditWindow|CancelEditButton|放弃更改并关闭。" & vbCrLf
        Tips += "RemoteAppEditWindow|BrowseIconPath|为 RemoteApp 选择图标。" & vbCrLf
        Tips += "RemoteAppEditWindow|IconPathCopyButton|将""路径""字段的值复制到""图标路径""字段。" & vbCrLf
        Tips += "RemoteAppEditWindow|BrowsePath|浏览应用程序。" & vbCrLf
        Tips += "RemoteAppEditWindow|IconResetButton|将图标重置为此应用程序的默认值。" & vbCrLf

        'IconResetButton

        Tips += "RemoteAppCreateClientConnection|RDPRadioButton|创建 RDP 文件。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|MSIRadioButton|创建 MSI 文件。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|EditAfterSave|编辑 RDP 连接文件。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|CreateRAWebIcon|为应用程序和任何文件类型关联生成图标。\r\n与 RAWeb 配合使用。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|FTAButton|设置此 RemoteApp 的文件类型关联。\r\n此处更改仅影响此客户端连接，不会被保存。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|DisabledFTACheckBox|不在此客户端连接中包含文件类型关联。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|SaveButton|保存窗口设置供下次使用。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|ResetButton|将窗口设置重置为默认值。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|CancelEditButton|关闭并返回主窗口。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|CreateButton|创建客户端连接并选择保存位置。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|UseRDGatewayCheckBox|使用远程桌面网关连接到主机。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|AttemptDirectCheckBox|先尝试直接连接，如果失败则使用远程桌面网关。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|ShortcutTagCheckBox|在每个快捷方式标题末尾附加一些文本。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|PerMachineRadioButton|快捷方式是为所有用户安装（每机器），还是仅为当前登录用户安装（每用户）？" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|PerUserRadioButton|快捷方式是为所有用户安装（每机器），还是仅为当前登录用户安装（每用户）？" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|CheckBoxSignRDPEnabled|对 RDP 文件进行数字签名。" & vbCrLf
        Tips += "RemoteAppCreateClientConnection|CheckBoxCreateSignedAndUnsigned|同时生成签名和未签名的 RDP 文件副本。" & vbCrLf

        Tips += "RemoteAppFileTypeAssociation|CreateButton|创建新的文件类型关联。" & vbCrLf
        Tips += "RemoteAppFileTypeAssociation|DeleteButton|删除选中的文件类型关联。" & vbCrLf
        Tips += "RemoteAppFileTypeAssociation|EditButton|更改选中文件类型关联的图标。" & vbCrLf
        Tips += "RemoteAppFileTypeAssociation|SetAssociationButton|在当前系统上创建或移除选中的文件类型关联。" & vbCrLf
        Tips += "RemoteAppFileTypeAssociation|CloseButton|保存更改并关闭。" & vbCrLf

        Tips += "RemoteAppIconPicker|BrowseButton|浏览包含图标的文件。" & vbCrLf
        Tips += "RemoteAppIconPicker|CancelEditButton|放弃更改并关闭。" & vbCrLf
        Tips += "RemoteAppIconPicker|OKButton|选择选中的图标。" & vbCrLf

        Return Tips
    End Function

End Module
