CREATE TABLE [dbo].[Charity_Event] (
    [CharityEvent_ID]            INT        NOT NULL,
    [CharityEventName]           nvarchar (75)  NULL,
    [CharityEventLocation_ID]    INT        NULL,
    [CharityEventCharity_ID]     INT        NULL,
    [CharityEventContactInfo_ID] INT        NULL,
    [CharityEventStartDate]      DATETIME       NULL,
    [CharityEventEndDate]        DATETIME       NULL,
    [CharityEventStatus]         nvarchar (10)  NULL,
    [CharityEventRequirements]   nvarchar (500) NULL,
    CONSTRAINT [PK_CharityEvent_ID] PRIMARY KEY CLUSTERED ([CharityEvent_ID] ASC)
);

