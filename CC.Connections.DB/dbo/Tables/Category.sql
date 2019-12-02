CREATE TABLE [dbo].[Categories] (
    [Category_ID]   INT       NOT NULL,
    [Category_Desc] NVARCHAR(70) NULL,
    [Category_Color] NCHAR(6) NULL, 
    CONSTRAINT [PK_Category_ID] PRIMARY KEY CLUSTERED ([Category_ID] ASC)
);

