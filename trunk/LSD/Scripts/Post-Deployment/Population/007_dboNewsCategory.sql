-- =================================
-- Populate the dbo.NewsCategory table.
-- =================================
declare @t table
(
	[Id]         int       not null primary key,
	[Name]       nvarchar(128) not null unique
);
--
insert into @t([Id], [Name])
values
(1, N'Игры'),
(2, N'Железо'),
(3, N'Индустрия'),
(4, N'ПК'),
(5, N'Консоли'),
(6, N'Новости сайта')
--
merge into [dbo].[NewsCategory] as [target]
using @t as [source] on [target].[Id] = [source].[Id]
when matched then
	update set [target].[Name] = [source].[Name]
when not matched by target then
	insert ([Id], [Name])
	values ([source].[Id], [source].[Name]);
go