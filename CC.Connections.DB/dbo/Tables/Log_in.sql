CREATE TABLE [dbo].[Log_in] (
    [ContactInfoEmail] CHAR (50)  NOT NULL,
    [LogInMember_ID]   INT        NULL,
    [LogInPassword]    CHAR (150) NULL,
    CONSTRAINT [PK_ContactInfoEmail] PRIMARY KEY CLUSTERED ([ContactInfoEmail] ASC)
);

