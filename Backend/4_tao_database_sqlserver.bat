@echo off
chcp 65001 >nul
set SERVER=
sc query MSSQLSERVER | find "RUNNING" >nul && set SERVER=localhost
if not defined SERVER sc query "MSSQL$SQLEXPRESS" | find "RUNNING" >nul && set SERVER=.\SQLEXPRESS
if not defined SERVER (
  echo [LOI] Khong thay SQL Server dang chay. Mo Services, bat dich vu "SQL Server (MSSQLSERVER)".
  pause
  exit /b 1
)
where sqlcmd >nul 2>&1 || (
  echo [LOI] May chua co sqlcmd. Cach khac: mo SSMS, ket noi %SERVER%,
  echo       mo lan luot 3 file trong database\sqlserver\ va bam Execute.
  pause
  exit /b 1
)
echo Dung SQL Server: %SERVER%
cd /d "%~dp0database\sqlserver"
sqlcmd -S %SERVER% -E -C -b -f 65001 -i 01_create_database.sql || goto loi
sqlcmd -S %SERVER% -E -C -b -f 65001 -i 02_seed_demo.sql || goto loi
sqlcmd -S %SERVER% -E -C -b -f 65001 -i 03_reports.sql -o 03_reports_ketqua.txt || goto loi
echo.
echo Xong. Ket qua cac bao cao: database\sqlserver\03_reports_ketqua.txt
echo Tiep theo: doi "Provider": "SqlServer" trong src\NongTrai.Api\appsettings.json roi chay 2_chay_server.bat
pause
exit /b 0
:loi
echo.
echo [LOI] Xem thong bao phia tren va gui cho Claude.
pause
exit /b 1
