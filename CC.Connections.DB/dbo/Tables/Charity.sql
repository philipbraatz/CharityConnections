CREATE TABLE [dbo].[Charities] (
    [Charity_ID]            INT        NOT NULL,
    [Charity_Contact_ID]    INT        NULL,
    [Charity_EIN]           nvarchar (50)  NULL,
    [Charity_Deductibility] BIT        NULL,
    [Charity_URL]           nvarchar (75)  NULL,
    [Charity_Cause]         nvarchar (50)  NULL,
    [Charity_Email]         nvarchar (75)  NULL,
    [Charity_Category_ID]   INT        NULL,
    [Location_ID]           INT        NULL,
    [Charity_Requirements]  nvarchar (500) NULL,
    CONSTRAINT [PK_Charity_ID] PRIMARY KEY CLUSTERED ([Charity_ID] ASC)
);

