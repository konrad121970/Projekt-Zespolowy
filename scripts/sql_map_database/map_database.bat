@echo off
echo - mapping database...

set edmx_source_dir=%cd%\EDMX
set entity_framework_dir=%cd%\EntityFramework

cd ..\..\
set edmx_target_dir=%cd%\libs\Network.Server\Database

del %edmx_target_dir% /F /Q
xcopy %edmx_source_dir%\DrocsidDbModel.* %edmx_target_dir%

cd %edmx_target_dir%
%windir%\Microsoft.NET\Framework\v4.0.30319\edmgen.exe /mode:fullgeneration /c:"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=Drocsid_DB; Integrated Security=SSPI" /project:DrocsidDbModel /entitycontainer:DrocsidDbContext /namespace:DrocsidDbModel /language:CSharp /pl

cd %edmx_source_dir%
start EDMX.exe %edmx_target_dir%\

cd %entity_framework_dir%
call text_transform.bat %edmx_target_dir% cs

del %edmx_target_dir%\DrocsidDbModel.csdl
del %edmx_target_dir%\DrocsidDbModel.msl
del %edmx_target_dir%\DrocsidDbModel.ssdl

del %edmx_target_dir%\DrocsidDbModel.ObjectLayer.cs
del %edmx_target_dir%\DrocsidDbModel.Views.cs

del %edmx_target_dir%\t4list.txt

echo - mapping finished
echo.