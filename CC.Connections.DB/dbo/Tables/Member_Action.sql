CREATE TABLE [dbo].[Member_Action] (
    [MemberAction_ID]       INT NOT NULL,
    [MemberActionMember_ID] INT NULL,
    [MemberActionAction_ID] INT NULL,
    PRIMARY KEY CLUSTERED ([MemberAction_ID] ASC),
    FOREIGN KEY ([MemberActionAction_ID]) REFERENCES [dbo].[Helping_Action] ([Helping_Action_ID])
);

