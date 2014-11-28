create table [dbo].[Platform]
(
	[Id]			int				not null,
	[Name]			NVARCHAR(150)	NOT NULL, 
	[ShortName] NVARCHAR(150) NOT NULL DEFAULT 'platform', 
    constraint [PK_dboPlatform] primary key clustered ([Id])
);
