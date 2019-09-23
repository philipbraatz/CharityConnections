CREATE TABLE [dbo].[Preferred_Charity] (
    [MemberCharity_ID] INT NOT NULL,
    [Member_ID]        INT NULL,
    [Charity_ID]       INT NULL,
    PRIMARY KEY CLUSTERED ([MemberCharity_ID] ASC)
);

