set /p versao=Informe a versão da imagem do Docker para o Monitor.Api: 

dotnet restore ..\..\Montor.sln

del F:\Deploy\Monitor.Api\*.* /S/Q

dotnet publish -o F:\Deploy\Monitor.Api ..\..\Monitor.Api\Monitor.Api.csproj

copy api.Dockerfile F:\Deploy\Monitor.Api /Y

docker image build -f F:\Deploy\Monitor.Api\api.Dockerfile -t andraderodrigosilva/monitorsistemas:%versao% F:\Deploy\Monitor.Api

docker image push andraderodrigosilva/monitorsistemas:%versao%

PAUSE