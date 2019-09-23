CREATE TABLE [dbo].[Contact_Info] (
    [Contact_Info_ID]   INT       NOT NULL,
    [ContactInfo_FName] nvarchar (25) NULL,
    [ContactInfo_LName] nvarchar (50) NULL,
    [ContactInfo_Phone] nvarchar (12) NULL,
    [ContactInfo_Email] nvarchar (50) NULL,
    [Location_ID]       INT       NULL,
    [DateOfBirth]       DATE      NULL,
    CONSTRAINT [PK_Contact_Info_ID] PRIMARY KEY CLUSTERED ([Contact_Info_ID] ASC)
);

