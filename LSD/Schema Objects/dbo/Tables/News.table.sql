create table [dbo].[News]
(
	[Id]			int				not null identity (1, 1),
	[Title] NVARCHAR(150) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [CreateDate] DATETIME NOT NULL, 
    [CountViews] INT NOT NULL, 
    [GameCategoryId] INT NULL, 
    [WriterId] INT NOT NULL, 
    constraint [PK_dboNews] primary key clustered ([Id])
);

go
ALTER TABLE [dbo].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_User] FOREIGN KEY([WriterId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[News] CHECK CONSTRAINT [FK_News_User]
GO

go
ALTER TABLE [dbo].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_GameCategory] FOREIGN KEY([GameCategoryId])
REFERENCES [dbo].[GameCategory] ([Id])
ON UPDATE CASCADE
ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[News] CHECK CONSTRAINT [FK_News_GameCategory]
GO