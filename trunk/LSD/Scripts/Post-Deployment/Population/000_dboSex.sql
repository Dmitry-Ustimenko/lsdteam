-- =================================
-- Populate the dbo.Sex table.
-- =================================
declare @t table
(
	[Id]         int       not null primary key,
	[Name]       nvarchar(128) not null unique
);
--
insert into @t([Id], [Name])
values
(1, N'Man'),
(2, N'Woman')
--
merge into [dbo].[Sex] as [target]
using @t as [source] on [target].[Id] = [source].[Id]
when matched then
	update set [target].[Name] = [source].[Name]
when not matched by target then
	insert ([Id], [Name])
	values ([source].[Id], [source].[Name]);
go
