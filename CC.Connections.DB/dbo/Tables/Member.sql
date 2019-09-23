CREATE TABLE [dbo].[Member] (
    [Member_ID]           INT NOT NULL,
    [MemberContact_ID]    INT NULL,
    [MemberPreference_ID] INT NULL,
    [MemberType_ID]       INT NULL,
    [Location_ID]         INT NULL,
    PRIMARY KEY CLUSTERED ([Member_ID] ASC)
);

