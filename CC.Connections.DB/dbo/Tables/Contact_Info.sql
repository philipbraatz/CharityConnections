CREATE TABLE [dbo].[Contact_Info] (
    [Contact_Info_ID]      INT       NOT NULL,
    [Contact_Info_FName]   CHAR (25) NULL,
    [Contact_Info_LName]   CHAR (50) NULL,
    [Contact_Info_Address] CHAR (50) NULL,
    [Contact_Info_City]    CHAR (25) NULL,
    [Contact_Info_State]   CHAR (2)  NULL,
    [Contact_Info_Zip]     CHAR (9)  NULL,
    [Contact_Info_Phone]   CHAR (12) NULL,
    [Contact_Info_Email]   CHAR (50) NULL,
    CONSTRAINT [PK_Contact_Info_ID] PRIMARY KEY CLUSTERED ([Contact_Info_ID] ASC)
);

