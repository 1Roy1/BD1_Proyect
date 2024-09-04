@echo off
setlocal

REM Obtener la fecha y hora actual en formato YYYY-MM-DD_HH-MM-SS
for /f "tokens=2 delims==" %%I in ('"wmic os get localdatetime /value"') do set datetime=%%I
set fecha=%datetime:~0,4%-%datetime:~4,2%-%datetime:~6,2%
set hora=%datetime:~8,2%-%datetime:~10,2%-%datetime:~12,2%

REM Definir el nombre del archivo de respaldo
set nombre_archivo=respaldo_%fecha%_%hora%.sql

REM Definir la ruta de destino
set ruta_destino=\\ROY\Compartir\respaldo\%nombre_archivo%

REM realizar el respaldo
mysqldump -u root -proot proyecto > "%ruta_destino%"

endlocal