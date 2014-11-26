create table [dbo].[Platform]
(
	[Id]			int				not null,
	[Name]			NVARCHAR(150)	NOT NULL, 
	constraint [PK_dboPlatform] primary key clustered ([Id])
);
