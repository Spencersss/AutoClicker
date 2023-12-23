# Spencer Auto Clicker

This is a small project that I had originally implemented to enable both myself and friends to auto-click a video game whilst the window was unfocused and running in the background.

The following were used to develop this application:
- Visual Studio 2022 Community
- .NET 6.0
- Windows Presentation Foundation (WPF)

Libraries & API(s):
- Ninject: http://www.ninject.org/
- SharpHook: https://github.com/TolikPylypchuk/SharpHook
- Win32 API: https://learn.microsoft.com/en-us/windows/win32/apiindex/windows-api-list

## Building Executable

1. Ensure the following dependencies are installed locally:
   1. .NET 6.0: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
2. Run the following command whilst in the root directory:
```
dotnet publish -p:PublishProfile=FolderProfile
```
1. Built executable will be located in `SpencerAutoClicker/Output/` if built with no errors.

