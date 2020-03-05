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


DROP TABLE IF EXISTS dbo.EventAttendance
DROP TABLE IF EXISTS dbo.[LogIn]
DROP TABLE IF EXISTS dbo.Member
DROP TABLE IF EXISTS dbo.MemberAction
DROP TABLE IF EXISTS dbo.MemberType
DROP TABLE IF EXISTS dbo.Preference

DROP TABLE IF EXISTS dbo.ContactInfo
DROP TABLE IF EXISTS dbo.CharityEvent

DROP TABLE IF EXISTS dbo.[Location]
DROP TABLE IF EXISTS dbo.Categories
DROP TABLE IF EXISTS dbo.Members
DROP TABLE IF EXISTS dbo.Charities
DROP TABLE IF EXISTS dbo.PreferredCategories
DROP TABLE IF EXISTS dbo.PreferredCharity
DROP TABLE IF EXISTS dbo.HelpingAction