CREATE TABLE [dbo].[Members] (
    [Member_ID]     INT NOT NULL,
    [ContactID]     INT NULL,
    [Role_ID]       INT NULL,
    [MemeberTypeID] INT NULL,
    CONSTRAINT [PK_Members_ID] PRIMARY KEY CLUSTERED ([Member_ID] ASC)
);

