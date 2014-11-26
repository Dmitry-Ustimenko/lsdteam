-- =================================
-- Populate the dbo.Platform table.
-- =================================
declare @t table
(
	[Id]         int       not null primary key,
	[Name]       nvarchar(128) not null unique
);
--
insert into @t([Id], [Name])
values
(1, N'PC'),
(2, N'Xbox 360'),
(3, N'Xbox One'),
(4, N'PlayStation 3'),
(5, N'PlayStation 4'),
(6, N'Wii U')
--
merge into [dbo].[Platform] as [target]
using @t as [source] on [target].[Id] = [source].[Id]
when matched then
	update set [target].[Name] = [source].[Name]
when not matched by target then
	insert ([Id], [Name])
	values ([source].[Id], [source].[Name]);
go