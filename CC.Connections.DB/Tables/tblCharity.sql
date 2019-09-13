CREATE TABLE [dbo].[tblCharity]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [ContactID] INT NULL, 
    [EIN] VARCHAR(50) NULL, 
    [Deductibility] BIT NULL, 
    [URL] VARCHAR(75) NULL, 
    [Cause] VARCHAR(50) NULL, 
    [Email] VARCHAR(75) NULL
)
