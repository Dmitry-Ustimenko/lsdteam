﻿-- =================================
-- Populate the dbo.UserMessageType table.
-- =================================
declare @t table
(
	[Id]         int       not null primary key,
	[Name]       nvarchar(128) not null unique
);
--
insert into @t([Id], [Name])
values
(1, N'Входящие'),
(2, N'Отправленные'),
(3, N'Сохраненные')
--
merge into [dbo].[UserMessageType] as [target]
using @t as [source] on [target].[Id] = [source].[Id]
when matched then
	update set [target].[Name] = [source].[Name]
when not matched by target then
	insert ([Id], [Name])
	values ([source].[Id], [source].[Name]);
go