CREATE TABLE [dbo].[Helping_Action] (
    [Helping_Action_ID]        INT       NOT NULL,
    [HelpingActionCategory_ID] INT       NULL,
    [HelpingActionDescription] nvarchar (75) NULL,
    CONSTRAINT [PK_Helping_Action_ID] PRIMARY KEY CLUSTERED ([Helping_Action_ID] ASC)
);

