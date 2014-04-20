create table [dbo].[Role]
(
	[Id]		int				not null,
	[Name]		nvarchar(128)	not null,
	constraint [PK_dboRole] primary key clustered ([Id])
);
go
create unique index [UQ_dboRole_Name] ON [dbo].[Role] ([Name]);
go