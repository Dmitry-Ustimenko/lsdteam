create table [dbo].[UserComment]
(
	[Id]			int	not null identity (1, 1),
	[UserId]		int	not null,
	[CommentId]		int	not null,
	[IsIncrement]	bit not null,
    constraint [PK_dboUserComment] primary key clustered ([Id])
);

go
create unique index [UQ_UserComment_UserId_CommentId] ON [dbo].[UserComment] ([UserId],[CommentId]);
go

ALTER TABLE [dbo].[UserComment]  WITH CHECK ADD CONSTRAINT [FK_UserComment_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserComment] CHECK CONSTRAINT [FK_UserComment_User]
GO

ALTER TABLE [dbo].[UserComment]  WITH CHECK ADD CONSTRAINT [FK_UserComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[Comment] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserComment] CHECK CONSTRAINT [FK_UserComment_Comment]
GO