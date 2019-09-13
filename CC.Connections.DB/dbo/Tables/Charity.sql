CREATE TABLE [dbo].[Charity] (
    [Charity_ID]            INT       NOT NULL,
    [Charity_ContactID]     INT       NULL,
    [Charity_EIN]           CHAR (50) NULL,
    [Charity_Deductibility] BIT       NULL,
    [Charity_URL]           CHAR (75) NULL,
    [Charity_Cause]         CHAR (50) NULL,
    [Charity_Email]         CHAR (75) NULL,
    CONSTRAINT [PK_Charity_ID] PRIMARY KEY CLUSTERED ([Charity_ID] ASC)
);

