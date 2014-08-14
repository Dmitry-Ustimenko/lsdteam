create table [dbo].[UserMessage]
(
	[Id]			int				not null identity (1, 1),
	[Title]		nvarchar(128)	not null,
	[Description]		nvarchar(MAX)	null,
	[IsRead] bit not null,
	[TypeId]	int		not	null,
    [SenderId] INT NOT NULL,
	[RecipientId] INT null,
    constraint [PK_dboUserMessage] primary key clustered ([Id])
);

go
ALTER TABLE [dbo].[UserMessage]  WITH CHECK ADD  CONSTRAINT [FK_UserMessage_UserMessageType] FOREIGN KEY([TypeId])
REFERENCES [dbo].[UserMessageType] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserMessage] CHECK CONSTRAINT [FK_UserMessage_UserMessageType]
GO

ALTER TABLE [dbo].[UserMessage]  WITH CHECK ADD CONSTRAINT [FK_UserMessage_UserSender] FOREIGN KEY([SenderId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
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