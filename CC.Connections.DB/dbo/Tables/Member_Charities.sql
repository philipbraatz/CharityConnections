CREATE TABLE [dbo].[Member_Charities]
(
	[Member_Charity_ID] INT NOT NULL PRIMARY KEY, 
    [Member_ID] INT NOT NULL, 
    [Charity_ID] INT NOT NULL,
)
