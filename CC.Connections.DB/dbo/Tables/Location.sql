CREATE TABLE [dbo].[Location] (
    [Location_ID]        INT        NOT NULL,
    [ContactInfoAddress] nvarchar (50) NULL,
    [ContactInfoCity]    nvarchar (25) NULL,
    [ContactInfoState]   nvarchar (25) NULL,
    [ContactInfoZip]     nvarchar (10) NULL,
    PRIMARY KEY CLUSTERED ([Location_ID] ASC)
);

