@SET IMAGE=andraderodrigosilva/monitorsistemas.console
@SET CONTAINERNAME=monitorsistemasBennerConsole
@IF [%1] == [] (
	@echo *** INFORME A TAG A SER UTILIZADA PARA A IMAGEM %IMAGE%!
	exit /b 1
)
@REM @call settings docker %2
@REM @IF %ERRORLEVEL% NEQ 0 (
@REM 	@echo *** NÃO FOI POSSÍVEL CONFIGURAR AS SETTINGS!
@REM 	exit /b 1
@REM )
@docker run -d --env-file=settings-console.env --name %CONTAINERNAME% %IMAGE%:%1
@IF %ERRORLEVEL% NEQ 0 (
	@echo *** NÃO FOI POSSÍVEL INICIAR O CONTAINER!
	exit /b 1
)
@pause