@echo off
setlocal
cd /d "%~dp0"
title Reach Community Client - Windows Builder
powershell.exe -NoLogo -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\build-windows.ps1"
set "BUILD_EXIT=%ERRORLEVEL%"
echo.
if not "%BUILD_EXIT%"=="0" (
  echo BUILD FAILED. Read the message above, fix the issue, and run this file again.
) else (
  echo Your Windows x64 release ZIP is in the artifacts folder.
)
echo.
pause
exit /b %BUILD_EXIT%
