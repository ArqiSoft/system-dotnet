[Environment]::SetEnvironmentVariable("MSBUILDDIR", "C:\Windows\Microsoft.NET\Framework64\v4.0.30319", "Machine")

nuget.exe restore Sds.Core.sln

%MSBUILDDIR%\msbuild build.proj /p:Configuration=Debug
