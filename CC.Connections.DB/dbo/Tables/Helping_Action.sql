CREATE TABLE [dbo].[Helping_Action] (
    [Helping_Action_ID]    INT       NOT NULL,
    [Helping_Action_CatID] INT       NULL,
    [Helping_Action_Desc]  CHAR (75) NULL,
    CONSTRAINT [PK_Helping_Action_ID] PRIMARY KEY CLUSTERED ([Helping_Action_ID] ASC)
);

