create table [dbo].[Game]
(
	[Id]			int		not null identity (1, 1),
	[Name] NVARCHAR(150) NOT NULL, 
    [GameCategoryId] int NOT NULL, 
    [ReleaseDate] DATETIME NOT NULL, 
    [Developer] nvarchar(100) NOT NULL, 
    [Publisher] nvarchar(100) NOT NULL, 
	[RecommendedOS] nvarchar(50) null,
	[RecommendedCPU] nvarchar(50) null,
	[RecommendedGPU] nvarchar(50) null,
	[RecommendedRAM] nvarchar(50) null,
	[RecommendedHDD] nvarchar(50) null,
    constraint [PK_dboGame] primary key clustered ([Id])
);

go
ALTER TABLE [dbo].[Game]  WITH CHECK ADD  CONSTRAINT [FK_Game_GameCategory] FOREIGN KEY([GameCategoryId])
REFERENCES [dbo].[GameCategory] ([Id])
ON UPDATE CASCADE
ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[Game] CHECK CONSTRAINT [FK_Game_GameCategory]
GO