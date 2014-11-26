create table [dbo].[GamePlatform]
(
	[Id]			int		not null identity (1, 1),
	[GameId] int NOT NULL, 
    [PlatformId] int NOT NULL, 
    constraint [PK_dboGamePlatform] primary key clustered ([Id])
);

go
create unique index [UQ_dboGamePlatform_GameId_PlatformId] ON [dbo].[GamePlatform] ([GameId], [PlatformId]);
go

go
ALTER TABLE [dbo].[GamePlatform]  WITH CHECK ADD  CONSTRAINT [FK_GamePlatform_Game] FOREIGN KEY([GameId])
REFERENCES [dbo].[Game] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GamePlatform] CHECK CONSTRAINT [FK_GamePlatform_Game]
GO

go
ALTER TABLE [dbo].[GamePlatform]  WITH CHECK ADD  CONSTRAINT [FK_GamePlatform_Platform] FOREIGN KEY([PlatformId])
REFERENCES [dbo].[Platform] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GamePlatform] CHECK CONSTRAINT [FK_GamePlatform_Platform]
GO