@echo off
chcp 65001 >nul
set PORT=5180
cd /d "%~dp0src\NongTrai.Api"
echo Dang chay server tai http://localhost:%PORT%  (dong cua so nay de tat server)
start "" http://localhost:%PORT%/swagger
dotnet run --no-launch-profile
if errorlevel 1 (
  echo.
  echo ===== Server khong chay duoc. Thong tin de gui cho Claude: =====
  echo -- Chuong trinh dang giu cong %PORT%:
  netstat -ano | findstr :%PORT%
  echo -- Cac khoang cong Windows giu san:
  netsh interface ipv4 show excludedportrange protocol=tcp
)
pause
