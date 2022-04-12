@echo off
set dir=%cd%

cd %dir%\sql_create_database
call create_database.bat

cd %dir%\sql_map_database
call map_database.bat

pause