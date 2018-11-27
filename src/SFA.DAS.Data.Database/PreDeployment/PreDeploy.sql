/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--Stop event loading for data locks
UPDATE [Data_Load].DAS_LoadedEvents 
SET LastProcessedEventId = convert(nvarchar(50), 999999999999)
WHERE EventFeed = 'DataLockEvent'

--Clear rows from data locks tables
DELETE 
FROM [Data_Load].[DAS_DataLock_Errors] 

DELETE 
FROM [Data_Load].[DAS_DataLocks] 
