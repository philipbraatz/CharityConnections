CREATE TABLE [dbo].[MemberCategories] (
    [MemberCat_ID] INT NOT NULL,
    [CategoriesID] INT NULL,
    [MemberID]     INT NULL,
    CONSTRAINT [PK_MemberCategories_ID] PRIMARY KEY CLUSTERED ([MemberCat_ID] ASC)
);

