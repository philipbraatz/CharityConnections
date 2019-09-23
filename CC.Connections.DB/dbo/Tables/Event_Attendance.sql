CREATE TABLE [dbo].[Event_Attendance] (
    [EventAttendance_ID] INT        NOT NULL,
    [Event_ID]           INT        NULL,
    [Member_ID]          INT        NULL,
    [Status]             NCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([EventAttendance_ID] ASC)
);

