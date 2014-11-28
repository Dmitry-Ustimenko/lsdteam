create table [dbo].[NewsCategory]
(
	[Id]			int				not null,
	[Name]			NVARCHAR(150)	NOT NULL, 
	[ShortName] NVARCHAR(150) NOT NULL DEFAULT 'category', 
    constraint [PK_dboNewsCategory] primary key clustered ([Id])
);