CREATE TABLE [dbo].[Event_Requirements] (
    [Event_Requirement_ID] INT NOT NULL,
    [NeedsRequirementsID]  INT NULL,
    [CharityEventID]       INT NULL,
    CONSTRAINT [PK_Event_Requirements_ID] PRIMARY KEY CLUSTERED ([Event_Requirement_ID] ASC)
);

