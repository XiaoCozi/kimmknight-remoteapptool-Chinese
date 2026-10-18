' =====================================================================
' 单文件发布支持模块（ApplicationEvents.vb）
' ---------------------------------------------------------------------
' 作用：
'   程序运行时如果需要某个依赖 DLL（例如 RemoteAppLib.dll），
'   .NET 默认会去 exe 所在的磁盘文件夹里找。
'   但我们已经把这些 DLL 当作“资源”打包进了 exe 内部，
'   所以这里通过 AssemblyResolve 事件告诉 .NET：
'   “磁盘上找不到也没关系，直接从 exe 内部把 DLL 读出来加载即可。”
'
' 效果：最终只需要分发“RemoteApp Tool.exe”这一个文件，
'       双击就能运行，不需要再附带任何 DLL。
' =====================================================================

Imports System.Reflection

Namespace My

    ' MyApplication 是 VB.NET WinForms 项目自动生成的应用程序类（部分类）。
    ' 这里补充它的启动事件处理代码。
    Partial Friend Class MyApplication

        ''' <summary>
        ''' 应用程序启动事件：在主窗口创建之前触发，是注册“程序集解析器”的最佳时机。
        ''' </summary>
        Private Sub MyApplication_Startup(
                sender As Object,
                e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs
            ) Handles Me.Startup

            ' 注册事件：当 .NET 在磁盘上找不到需要的 DLL 时，就会调用我们的方法。
            AddHandler AppDomain.CurrentDomain.AssemblyResolve,
                       AddressOf ResolveEmbeddedAssembly
        End Sub

        ''' <summary>
        ''' 当 .NET 找不到某个程序集（DLL）时触发此方法。
        ''' </summary>
        ''' <param name="sender">触发事件的 AppDomain（应用程序域）</param>
        ''' <param name="args">包含 .NET 想要加载的程序集名称等信息</param>
        ''' <returns>从 exe 内部资源加载好的程序集；找不到则返回 Nothing</returns>
        Private Function ResolveEmbeddedAssembly(sender As Object, args As ResolveEventArgs) As Assembly

            ' args.Name 的完整格式形如：
            '   "LockChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
            ' 用 AssemblyName 解析后，只取逗号前面的简单名称，例如 "LockChecker"。
            Dim requestedName As String = New AssemblyName(args.Name).Name

            ' 获取当前正在运行的主 exe 程序集（DLL 就嵌入在这里面）。
            Dim mainAssembly As Assembly = Assembly.GetExecutingAssembly()

            ' 遍历主 exe 中所有嵌入资源的名称。
            ' 资源名称类似：RemoteApp_Tool.EmbeddedDeps.LockChecker.dll
            For Each resourceName As String In mainAssembly.GetManifestResourceNames()

                ' 用结尾匹配，避免依赖固定的命名空间前缀（更健壮）。
                ' StringComparison.OrdinalIgnoreCase 表示忽略大小写。
                If resourceName.EndsWith("." & requestedName & ".dll",
                                         StringComparison.OrdinalIgnoreCase) Then

                    ' 打开该资源（即 exe 内部的 DLL 文件内容）。
                    Using resourceStream As IO.Stream =
                          mainAssembly.GetManifestResourceStream(resourceName)

                        ' 一次性把 DLL 的全部字节读到内存数组中。
                        Dim assemblyData(CInt(resourceStream.Length) - 1) As Byte
                        resourceStream.Read(assemblyData, 0, assemblyData.Length)

                        ' 直接从内存字节加载程序集。
                        ' 好处：不需要把 DLL 释放到磁盘，真正做到单文件运行，
                        '       也不会在临时目录留下文件。
                        Return Assembly.Load(assemblyData)
                    End Using
                End If
            Next

            ' 没有在嵌入资源中找到，返回 Nothing，让 .NET 按默认逻辑继续处理。
            Return Nothing
        End Function

    End Class

End Namespace
