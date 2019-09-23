CREATE TABLE [dbo].[Preferred_Category] (
    [PreferredCategory_ID]  INT NOT NULL,
    [MemberCat_Category_ID] INT NULL,
    [MemberCat_Member_ID]   INT NULL,
    CONSTRAINT [PK_PreferredCategory_ID] PRIMARY KEY CLUSTERED ([PreferredCategory_ID] ASC)
);

