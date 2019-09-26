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


DROP TABLE IF EXISTS dbo.Event_Attendance
DROP TABLE IF EXISTS dbo.Log_in
DROP TABLE IF EXISTS dbo.Member
DROP TABLE IF EXISTS dbo.Member_Action
DROP TABLE IF EXISTS dbo.Member_Type
DROP TABLE IF EXISTS dbo.Preference

DROP TABLE IF EXISTS dbo.Contact_Info
DROP TABLE IF EXISTS dbo.Charity_Event

DROP TABLE IF EXISTS dbo.Location
DROP TABLE IF EXISTS dbo.Category
DROP TABLE IF EXISTS dbo.Member
DROP TABLE IF EXISTS dbo.Charity
DROP TABLE IF EXISTS dbo.Preferred_Category
DROP TABLE IF EXISTS dbo.Preferred_Charity
DROP TABLE IF EXISTS dbo.Helping_Action


/*DROP TABLE IF EXISTS dbo.Helping_Action*/