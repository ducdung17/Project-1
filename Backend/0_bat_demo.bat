@echo off
chcp 65001 >nul
title Nong Trai - Bat demo
set PORT=5180
set ROOT=%~dp0

echo ============================================================
echo   NONG TRAI CUA BE - BAT MOI THU DE DEMO
echo ============================================================
echo.

echo [1/4] Kiem tra SQL Server...
sc query MSSQLSERVER | find "RUNNING" >nul
if errorlevel 1 goto sql_start
echo       OK - SQL Server dang chay.
goto sql_db

:sql_start
echo       SQL Server chua chay, dang bat...
net start MSSQLSERVER >nul 2>&1
sc query MSSQLSERVER | find "RUNNING" >nul
if not errorlevel 1 goto sql_started
echo       [LOI] Khong bat duoc SQL Server (can quyen Administrator).
echo       Cach sua: chuot phai file nay - Run as administrator,
echo       hoac mo services.msc - "SQL Server (MSSQLSERVER)" - Start.
pause
exit /b 1
:sql_started
echo       OK - Da bat SQL Server.

:sql_db
echo [2/4] Kiem tra database NongTrai...
set KIDS=
for /f "usebackq tokens=1" %%n in (`sqlcmd -S localhost -E -C -r1 -h -1 -W -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM NongTrai.dbo.Children" 2^>nul`) do set KIDS=%%n
if not defined KIDS goto db_missing
echo       OK - Database co %KIDS% be.
goto server

:db_missing
echo       Chua co database NongTrai (hoac chua co sqlcmd).
echo       Dang tao database + du lieu demo...
call "%ROOT%4_tao_database_sqlserver.bat"

:server
echo [3/4] Bat server API tai http://localhost:%PORT% ...
curl -s -o nul http://localhost:%PORT%/api/health
if not errorlevel 1 goto server_ready_already
start "NongTrai Server - DONG CUA SO NAY LA TAT SERVER" cmd /k "cd /d "%ROOT%src\NongTrai.Api" && dotnet run --no-launch-profile"

set /a TRIES=0
:wait_loop
timeout /t 2 /nobreak >nul
curl -s -o nul http://localhost:%PORT%/api/health
if not errorlevel 1 goto server_ready
set /a TRIES+=1
if %TRIES% lss 45 goto wait_loop
echo       [LOI] Server chua len sau 90 giay. Xem thong bao trong cua so "NongTrai Server".
pause
exit /b 1

:server_ready_already
echo       Server da chay san tu truoc.
goto open
:server_ready
echo       OK - Server da san sang.

:open
echo [4/4] Mo trang phu huynh...
start "" http://localhost:%PORT%/

echo.
echo ============================================================
echo   SAN SANG DEMO
echo   - Server: cua so "NongTrai Server" (de nguyen, dong la tat)
echo   - Web   : http://localhost:%PORT%/  (trang phu huynh, tu cap nhat 15 giay)
echo   - API   : http://localhost:%PORT%/swagger
echo   - Game  : mo Unity bam Play, hoac chay ban .exe
echo   - SQL   : mo SSMS, ket noi localhost, database NongTrai
echo             Cau hay dung: EXEC dbo.sp_ChildReport @ChildId = N'demo-na';
echo ============================================================
echo.
pause
