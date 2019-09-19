CREATE TABLE [dbo].[Members] (
    [Member_ID]     INT NOT NULL,
    [Contact_ID]     INT NULL,
    [Role_ID]       INT NULL,
    [Memeber_Type_ID] INT NULL,

    [Preference_ID] INT NULL, 
    CONSTRAINT [PK_Members_ID] PRIMARY KEY CLUSTERED ([Member_ID] ASC)
);

