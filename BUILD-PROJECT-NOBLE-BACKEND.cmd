@echo off
setlocal
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\build-project-noble.ps1"
if errorlevel 1 (
  echo.
  echo Project Noble backend build failed. Read the messages above.
  pause
  exit /b 1
)
echo.
echo Project Noble backend build completed.
pause
