create table [dbo].[NewsCategory]
(
	[Id]			int				not null,
	[Name]			NVARCHAR(150)	NOT NULL, 
	constraint [PK_dboNewsCategory] primary key clustered ([Id])
);