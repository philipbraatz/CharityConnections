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
:r .\DefaultData\Categories.sql
:r .\DefaultData\Charity.sql
:r .\DefaultData\CharityEvents.sql
:r .\DefaultData\ContactInfo.sql
:r .\DefaultData\EventRequirements.sql
:r .\DefaultData\HelpingActions.sql
:r .\DefaultData\Login.sql
:r .\DefaultData\MemberCategories.sql
:r .\DefaultData\MemberCharities.sql
:r .\DefaultData\Members.sql
:r .\DefaultData\MemberType.sql
:r .\DefaultData\NeedsRequirements.sql
:r .\DefaultData\Preferences.sql
:r .\DefaultData\Roles.sql