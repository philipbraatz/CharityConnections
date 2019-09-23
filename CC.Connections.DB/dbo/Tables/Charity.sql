CREATE TABLE [dbo].[Charity] (
    [Charity_ID]            INT        NOT NULL,
    [Charity_Contact_ID]    INT        NULL,
    [Charity_EIN]           CHAR (50)  NULL,
    [Charity_Deductibility] BIT        NULL,
    [Charity_URL]           CHAR (75)  NULL,
    [Charity_Cause]         CHAR (50)  NULL,
    [Charity_Email]         CHAR (75)  NULL,
    [Charity_Category_ID]   INT        NULL,
    [Location_ID]           INT        NULL,
    [Charity_Requirements]  CHAR (500) NULL,
    CONSTRAINT [PK_Charity_ID] PRIMARY KEY CLUSTERED ([Charity_ID] ASC)
);

