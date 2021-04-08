set /p versao=Informe a versão da imagem do Docker para o Monitor.Console: 
@SET IMAGE=andraderodrigosilva/monitorsistemas.console
@SET CONTAINERNAME=monitorsistemasBennerConsole
@docker run -d --env-file=settings-console.env --name %CONTAINERNAME% %IMAGE%:%versao%
@IF %ERRORLEVEL% NEQ 0 (
	@echo *** NÃO FOI POSSÍVEL INICIAR O CONTAINER!
	exit /b 1
)
@pause