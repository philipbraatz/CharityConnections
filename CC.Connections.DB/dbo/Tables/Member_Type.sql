﻿CREATE TABLE [dbo].[Member_Type] (
    [MemberType_ID]         INT       NOT NULL,
    [MemberTypeDescription] CHAR (75) NULL,
    CONSTRAINT [PK_MemberType_ID] PRIMARY KEY CLUSTERED ([MemberType_ID] ASC)
);

