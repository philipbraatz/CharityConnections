CREATE TABLE [dbo].[tblCharityEvents]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(75) NULL, 
    [Location] VARCHAR(75) NULL, 
    [CharityID] INT NULL, 
    [ContactInfoID] INT NULL, 
    [StartDate] DATETIME NULL, 
    [EndDate] DATETIME NULL, 
    [Time] DATETIME NULL, 
    [Email] VARCHAR(75) NULL
)
