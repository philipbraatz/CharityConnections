CREATE TABLE [dbo].[Charity_Event] (
    [CharityEvent_ID]            INT        NOT NULL,
    [CharityEventName]           CHAR (75)  NULL,
    [CharityEventLocation_ID]    INT        NULL,
    [CharityEventCharity_ID]     INT        NULL,
    [CharityEventContactInfo_ID] INT        NULL,
    [CharityEventStartDate]      DATE       NULL,
    [CharityEventEndDate]        DATE       NULL,
    [CharityEventTime]           DATETIME   NULL,
    [CharityEventStatus]         CHAR (10)  NULL,
    [CharityEventRequirements]   CHAR (500) NULL,
    CONSTRAINT [PK_CharityEvent_ID] PRIMARY KEY CLUSTERED ([CharityEvent_ID] ASC)
);

