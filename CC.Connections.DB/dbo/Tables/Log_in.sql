CREATE TABLE [dbo].[Log_in] (
    [ContactInfoEmail] nvarchar (50)  NOT NULL,
    [LogInMember_ID]   INT        NULL,
    [LogInPassword]    nvarchar (150) NULL,
    CONSTRAINT [PK_ContactInfoEmail] PRIMARY KEY CLUSTERED ([ContactInfoEmail] ASC)
);

