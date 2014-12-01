create table [dbo].[NewsComment]
(
	[Id]			int		not null identity (1, 1),
	[NewsId] int NOT NULL, 
    [CommentId] int NOT NULL, 
    constraint [PK_dboNewsComment] primary key clustered ([Id])
);

go
create unique index [UQ_dboNewsComment_NewsId_CommentId] ON [dbo].[NewsComment] ([NewsId], [CommentId]);
go

go
ALTER TABLE [dbo].[NewsComment]  WITH CHECK ADD  CONSTRAINT [FK_NewsComment_News] FOREIGN KEY([NewsId])
REFERENCES [dbo].[News] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsComment] CHECK CONSTRAINT [FK_NewsComment_News]
GO

go
ALTER TABLE [dbo].[NewsComment]  WITH CHECK ADD  CONSTRAINT [FK_NewsComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[Comment] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsComment] CHECK CONSTRAINT [FK_NewsComment_Comment]
GO