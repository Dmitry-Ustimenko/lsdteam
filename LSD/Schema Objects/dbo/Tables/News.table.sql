create table [dbo].[News]
(
	[Id]			int				not null identity (1, 1),
	[Title] NVARCHAR(150) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [CreateDate] DATETIME NOT NULL, 
    [CountViews] INT NOT NULL, 
    [NewsCategoryId] INT NOT NULL, 
    [WriterId] INT NOT NULL, 
    [ImagePath] NVARCHAR(250) NULL, 
    [Annotation] NVARCHAR(200) NOT NULL DEFAULT 'Annotation', 
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
ALTER TABLE [dbo].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_NewsCategory] FOREIGN KEY([NewsCategoryId])
REFERENCES [dbo].[NewsCategory] ([Id])
ON UPDATE CASCADE
ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[News] CHECK CONSTRAINT [FK_News_NewsCategory]
GO