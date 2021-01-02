@echo.服务启动......  
@echo off  
@net start TimeService  
@sc config TimeService start= AUTO  
@echo off  
@echo.启动完毕！  
@pause