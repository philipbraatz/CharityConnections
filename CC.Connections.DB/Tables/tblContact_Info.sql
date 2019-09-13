CREATE TABLE [dbo].[tblContact_Info]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [First Name] VARCHAR(20) NULL, 
    [Last Name] VARCHAR(20) NULL, 
    [Address] NCHAR(10) NULL, 
    [City] VARCHAR(50) NULL, 
    [State] VARCHAR(2) NULL, 
    [Zip] VARCHAR(9) NULL, 
    [Phone] VARCHAR(12) NULL, 
    [Email] VARCHAR(50) NULL
)
