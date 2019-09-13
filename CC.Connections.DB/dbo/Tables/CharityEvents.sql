CREATE TABLE [dbo].[CharityEvents] (
    [CharityEvent_ID]       INT       NOT NULL,
    [CharityEventName]      CHAR (75) NULL,
    [CharityEventLocation]  CHAR (75) NULL,
    [CharityEventCharityID] INT       NULL,
    [ContactInfoID]         INT       NULL,
    [CharityEventStartDate] DATETIME  NULL,
    [CharityEventEndDate]   DATETIME  NULL,
    [CharityEventTime]      DATETIME  NULL,
    [CharityEvent_Email]    CHAR (75) NULL,
    CONSTRAINT [PK_CharityEvents_ID] PRIMARY KEY CLUSTERED ([CharityEvent_ID] ASC)
);

