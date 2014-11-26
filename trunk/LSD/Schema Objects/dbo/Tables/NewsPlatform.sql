create table [dbo].[NewsPlatform]
(
	[Id]			int		not null identity (1, 1),
	[NewsId] int NOT NULL, 
    [PlatformId] int NOT NULL, 
    constraint [PK_dboNewsPlatform] primary key clustered ([Id])
);

go
ALTER TABLE [dbo].[NewsPlatform]  WITH CHECK ADD  CONSTRAINT [FK_NewsPlatform_News] FOREIGN KEY([NewsId])
REFERENCES [dbo].[News] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsPlatform] CHECK CONSTRAINT [FK_NewsPlatform_News]
GO

go
ALTER TABLE [dbo].[NewsPlatform]  WITH CHECK ADD  CONSTRAINT [FK_NewsPlatform_Platform] FOREIGN KEY([PlatformId])
REFERENCES [dbo].[Platform] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsPlatform] CHECK CONSTRAINT [FK_NewsPlatform_Platform]
GO