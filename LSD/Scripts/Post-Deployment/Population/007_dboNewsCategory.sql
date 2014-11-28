-- =================================
-- Populate the dbo.NewsCategory table.
-- =================================
declare @t table
(
	[Id]         int       not null primary key,
	[Name]       nvarchar(128) not null unique,
	[ShortName]       nvarchar(128) not null unique
);
--
insert into @t([Id], [Name], [ShortName])
values
(1, N'Игры', N'Games'),
(2, N'Железо', N'Hardware'),
(3, N'Индустрия', N'Industry'),
(4, N'ПК', N'PC'),
(5, N'Консоли', N'Console'),
(6, N'Новости сайта', N'Sitenews')
--
merge into [dbo].[NewsCategory] as [target]
using @t as [source] on [target].[Id] = [source].[Id]
when matched then
	update set [target].[Name] = [source].[Name], [target].[ShortName] = [source].[ShortName]
when not matched by target then
	insert ([Id], [Name], [ShortName])
	values ([source].[Id], [source].[Name], [source].[ShortName]);
go