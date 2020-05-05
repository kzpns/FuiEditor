@echo off

rem https://qiita.com/tera1707/items/018e8390207c5b2212b2
rem thx!

rem まず開発者用コマンドプロンプトを起動してから
call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"

rem MSBuildでビルドする(C#)
cd %~dp0

echo Building...
MSBuild ".\FuiEditor.sln" /t:clean;rebuild /p:Configuration=Release;Platform="Any CPU"
if %ERRORLEVEL% neq 0 (
    echo ErrorLevel:%ERRORLEVEL%
    echo ビルド失敗
)

pause