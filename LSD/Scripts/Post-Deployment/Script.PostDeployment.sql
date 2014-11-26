/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Configure database.
alter authorization on database::[$(DatabaseName)] to [sa];
go

-- Populate tables.
:r .\Population\000_dboSex.sql
:r .\Population\001_dboRole.sql
:r .\Population\002_dboUser.sql
:r .\Population\003_dboUserExternalInfo.sql
:r .\Population\004_dboUserMessageType.sql
:r .\Population\005_dboGameCategory.sql
:r .\Population\006_dboPlatform.sql
:r .\Population\007_dboNewsCategory.sql