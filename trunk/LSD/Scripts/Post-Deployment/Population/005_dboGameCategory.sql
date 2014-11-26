-- =================================
-- Populate the dbo.GameCategory table.
-- =================================
declare @t table
(
	[Id]         int       not null primary key,
	[Name]       nvarchar(128) not null unique
);
--
insert into @t([Id], [Name])
values
(1, N'Action'),
(2, N'RPG'),
(3, N'RTS'),
(4, N'Race')
--
merge into [dbo].[GameCategory] as [target]
using @t as [source] on [target].[Id] = [source].[Id]
when matched then
	update set [target].[Name] = [source].[Name]
when not matched by target then
	insert ([Id], [Name])
	values ([source].[Id], [source].[Name]);
go