-- =================================
-- Populate the dbo.Platform table.
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
(1, N'PC', N'PC'),
(2, N'Xbox 360', N'Xbox360'),
(3, N'Xbox One', N'XboxOne'),
(4, N'PlayStation 3', N'PS3'),
(5, N'PlayStation 4', N'PS4'),
(6, N'Wii U', N'WiiU')
--
merge into [dbo].[Platform] as [target]
using @t as [source] on [target].[Id] = [source].[Id]
when matched then
	update set [target].[Name] = [source].[Name], [target].[ShortName] = [source].[ShortName]
when not matched by target then
	insert ([Id], [Name], [ShortName])
	values ([source].[Id], [source].[Name], [source].[ShortName]);
go