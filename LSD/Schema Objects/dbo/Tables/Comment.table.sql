create table [dbo].[Comment]
(
	[Id]			int				not null identity (1, 1),
    [Description] NVARCHAR(MAX) NOT NULL, 
    [CreateDate] DATETIME NOT NULL, 
    [WriterId] INT NOT NULL,
	[Rate] int not null,
    constraint [PK_dboComment] primary key clustered ([Id])
);