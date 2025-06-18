@echo off
echo Closing Visual Studio processes...
taskkill /f /im devenv.exe 2>nul
taskkill /f /im ServiceHub.* 2>nul
taskkill /f /im MSBuild.exe 2>nul
taskkill /f /im VBCSCompiler.exe 2>nul

echo Adding files to Git...
git add .

echo Committing changes...
git commit -m "Updated project with UI improvements"

echo Done!
pause