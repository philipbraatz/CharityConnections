CREATE TABLE [dbo].[Location] (
    [Location_ID]        INT        NOT NULL,
    [ContactInfoAddress] NCHAR (50) NULL,
    [ContactInfoCity]    NCHAR (25) NULL,
    [ContactInfoState]   NCHAR (25) NULL,
    [ContactInfoZip]     NCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([Location_ID] ASC)
);

