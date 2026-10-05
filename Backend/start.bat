@echo off
chcp 65001 >nul
title Nong Trai - Server
set PORT=5180
set ROOT=%~dp0

echo [1/3] Kiem tra SQL Server...
sc query MSSQLSERVER | find "RUNNING" >nul
if not errorlevel 1 goto server
echo       SQL Server chua chay, dang bat...
net start MSSQLSERVER >nul 2>&1
sc query MSSQLSERVER | find "RUNNING" >nul
if not errorlevel 1 goto server
echo       [LOI] Khong bat duoc SQL Server (can quyen Administrator).
echo       Chuot phai file nay - Run as administrator,
echo       hoac mo services.msc - "SQL Server (MSSQLSERVER)" - Start.
pause
exit /b 1

:server
echo [2/3] Bat server API tai http://localhost:%PORT% ...
curl -s -o nul http://localhost:%PORT%/api/health
if not errorlevel 1 goto open
start "NongTrai Server - DONG CUA SO NAY LA TAT SERVER" cmd /k "cd /d "%ROOT%src\NongTrai.Api" && dotnet run --no-launch-profile"

set /a TRIES=0
:wait_loop
timeout /t 2 /nobreak >nul
curl -s -o nul http://localhost:%PORT%/api/health
if not errorlevel 1 goto open
set /a TRIES+=1
if %TRIES% lss 45 goto wait_loop
echo       [LOI] Server chua len sau 90 giay. Xem cua so "NongTrai Server".
pause
exit /b 1

:open
echo [3/3] Mo trang phu huynh...
start "" http://localhost:%PORT%/
echo.
echo   Web : http://localhost:%PORT%/
echo   API : http://localhost:%PORT%/swagger
echo.
pause
