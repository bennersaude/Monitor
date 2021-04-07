set /p versao=Informe a versão da imagem do Docker para o Monitor.Console: 

dotnet restore ..\..\Monitor.sln

del F:\Deploy\Monitor.Console\*.* /S/Q

dotnet publish -o F:\Deploy\Monitor.Console ..\..\Monitor.Console\Monitor.Console.csproj

copy console.Dockerfile F:\Deploy\Monitor.Console /Y

docker image build -f F:\Deploy\Monitor.Console\console.Dockerfile -t andraderodrigosilva/monitorsistemas.console:%versao% F:\Deploy\Monitor.Console

docker image push andraderodrigosilva/monitorsistemas.console:%versao%

PAUSE