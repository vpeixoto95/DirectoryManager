Download and run ConsoleApp.exe for Windows.

The code can be found in folder ConsoleApp, file DirectoryManager.cs, and was developed in Visual Studio using C#.

To build the executable, use the following command:
```cmd
dotnet publish -c Release -r <OS> --self-contained -p:PublishSingleFile=true
```

Replace `<OS>` with the appropriate operating system:
- `win-x64` for Windows
- `linux-x64` for Linux
- `osx-x64` for MacOS

Once this command is executed, you can find the executable in the following path:
```cmd
ConsoleApp/bin/Release/net6.0/<OS>/publish
```
