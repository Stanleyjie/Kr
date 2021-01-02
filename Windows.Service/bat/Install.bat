@echo off 
@title 安装windows服务
path %SystemRoot%\Microsoft.NET\Framework\v4.0.30319
echo==============================================================
echo=
echo         windows服务程序安装
echo=
echo==============================================================
@echo off 
InstallUtil.exe Windows.Service.exe
pause