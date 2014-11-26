create table [dbo].[GameCategory]
(
	[Id]			int				not null,
	[Name]			NVARCHAR(150)	NOT NULL, 
	constraint [PK_dboGameCategory] primary key clustered ([Id])
);

