@echo off
:: Ga naar de map waar dit batch-bestand staat
cd /d "%~dp0"

:: Voer het JS-bestand uit met Node.js
:: Zorg dat 'node' is geïnstalleerd en in je PATH staat
node server.js

:: Optioneel: houd het venster open bij fouten
if %errorlevel% neq 0 pause