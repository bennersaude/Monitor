set /p versao=Informe a versão da imagem do Docker para o Monitor.Api: 
@SET IMAGE=andraderodrigosilva/monitorsistemas
@SET CONTAINERNAME=monitorsistemasBenner
@docker run -d -p 9080:9090 --env-file=settings-api.env --name %CONTAINERNAME% %IMAGE%:%versao%
@IF %ERRORLEVEL% NEQ 0 (
	@echo *** NÃO FOI POSSÍVEL INICIAR O CONTAINER!
	exit /b 1
)
@timeout 5
@start http://localhost:9080
@pause
