create table [dbo].[UserMessage]
(
	[Id]			int				not null identity (1, 1),
	[Title]		nvarchar(128)	not null,
	[Description]		nvarchar(MAX)	not null,
	[IsRead] bit not null,
	[CreateDate] datetime not null,
    [SenderId] INT NULL,
	[RecipientId] INT null,
    [IsSenderDeleted] BIT NOT NULL, 
    [IsRecipientDeleted] BIT NOT NULL, 
    [IsSenderSaved] BIT NOT NULL, 
    [IsRecipientSaved] BIT NOT NULL, 
    constraint [PK_dboUserMessage] primary key clustered ([Id])
);

go

ALTER TABLE [dbo].[UserMessage]  WITH CHECK ADD CONSTRAINT [FK_UserMessage_UserSender] FOREIGN KEY([SenderId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE NO ACTION
ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[UserMessage] CHECK CONSTRAINT [FK_UserMessage_UserSender]
GO

ALTER TABLE [dbo].[UserMessage]  WITH CHECK ADD CONSTRAINT [FK_UserMessage_UserRecipient] FOREIGN KEY([RecipientId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE NO ACTION
ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[UserMessage] CHECK CONSTRAINT [FK_UserMessage_UserRecipient]
GO