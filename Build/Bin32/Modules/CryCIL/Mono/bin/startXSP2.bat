@echo off
call C:\PROGRA~2\MONO-3~1.3\bin\setmonopath.bat
cd /D C:\PROGRA~2\MONO-3~1.3\lib\xsp\test
xsp2 --root . --port 8080 --applications /:.
