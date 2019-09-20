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
DROP TABLE IF EXISTS dbo.Categories
DROP TABLE IF EXISTS dbo.Charity
DROP TABLE IF EXISTS dbo.CharityEvents
DROP TABLE IF EXISTS dbo.Contact_Info
DROP TABLE IF EXISTS dbo.Event_Requirements
DROP TABLE IF EXISTS dbo.Helping_Action
DROP TABLE IF EXISTS dbo.Log_in
DROP TABLE IF EXISTS dbo.Member_Charities
DROP TABLE IF EXISTS dbo.Member_Type
DROP TABLE IF EXISTS dbo.MemberCategories
DROP TABLE IF EXISTS dbo.Members
DROP TABLE IF EXISTS dbo.NeedsRequirements
DROP TABLE IF EXISTS dbo.Preferences
DROP TABLE IF EXISTS dbo.Roles