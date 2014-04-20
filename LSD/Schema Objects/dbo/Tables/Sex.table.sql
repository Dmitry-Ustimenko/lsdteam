create table [dbo].[Sex]
(
	[Id]	int		not null,
	[Name]	nvarchar(128)	not null,
	constraint [PK_dboSex] primary key clustered ([Id])
);
go
create unique index [UQ_dboSex_Name] ON [dbo].[Sex] ([Name]);
go