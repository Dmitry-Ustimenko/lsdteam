create table [dbo].[Comment]
(
	[Id]			int				not null identity (1, 1),
    [Description] NVARCHAR(MAX) NOT NULL, 
    [CreateDate] DATETIME NOT NULL, 
    [WriterId] INT NOT NULL,
	[Rate] int not null,
    [ModifierDate] DATETIME NOT NULL, 
    constraint [PK_dboComment] primary key clustered ([Id])
);

go
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_User] FOREIGN KEY([WriterId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE NO ACTION
ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_User]
GO