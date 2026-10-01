@echo off
cd /d "%~dp0"
git add -A -- . ":(exclude)push.bat"
git commit -m "done main menu"
git push origin main
echo.
echo Xong. Ban co the xoa file push.bat nay.
pause
