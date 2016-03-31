
set msBuildDir=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\
call %msBuildDir%\msbuild.exe .\src\SlackClient\SlackClient.fsproj /p:Configuration=Release /v:d 
call %msBuildDir%\msbuild.exe .\src\Serilog.Sinks.Slack\Serilog.Sinks.Slack.csproj /p:Configuration=Release /t:Rebuild /p:RunOctoPack=true /p:OctoPackNuGetProperties="configuration=Release" /p:OctoPackPublishPackageToHttp=https://packages.nuget.org/packages/ /p:OctoPackPublishApiKey=a38950da-c491-47b1-a8ed-8d0d9c74204d
exit /b
