@echo off
chcp 65001 >nul
echo ==== .NET SDK (can ban 8.x) ====
dotnet --list-sdks
if errorlevel 1 echo [THIEU] Chua cai .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
echo.
echo ==== SQL Server ====
set FOUND=
sc query MSSQLSERVER | find "STATE" && set FOUND=1 && echo   - Ban mac dinh (MSSQLSERVER): Server=localhost
sc query "MSSQL$SQLEXPRESS" | find "STATE" && set FOUND=1 && echo   - Ban Express (SQLEXPRESS): Server=.\SQLEXPRESS
sqllocaldb info >nul 2>&1 && set FOUND=1 && echo   - Co LocalDB: Server=(localdb)\MSSQLLocalDB
if not defined FOUND echo (chua thay SQL Server - van chay duoc bang SQLite)
echo.
echo ==== sqlcmd (de chay script SQL tu dong) ====
where sqlcmd
if errorlevel 1 echo (khong co sqlcmd - mo cac file .sql bang SSMS va bam Execute cung duoc)
echo.
pause
