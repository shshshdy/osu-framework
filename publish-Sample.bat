cd /d %~dp0
dotnet publish  SampleGame.Desktop/SampleGame.Desktop.csproj -c Release -r win-x64 -p:PublishAot=true -o ./out/win-x64