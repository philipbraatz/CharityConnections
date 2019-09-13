CREATE TABLE [dbo].[Log_in] (
    [Log_in_ID] INT        NOT NULL,
    [MemeberID] INT        NULL,
    [Password]  CHAR (150) NULL,
    CONSTRAINT [PK_Log_in_ID] PRIMARY KEY CLUSTERED ([Log_in_ID] ASC)
);

