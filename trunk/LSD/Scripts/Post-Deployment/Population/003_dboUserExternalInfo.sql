-- =================================
-- Populate the dbo.UserExternalInfo table.
-- =================================
declare @t table
(
	[ProviderName]       nvarchar(50) not null,
	[ProviderKey]       nvarchar(50) not null,
	[UserId]		int	not null
);

--

declare @user_id int
SET @user_id = (select [id] from [dbo].[User] where [Email] = N'lord.udv@gmail.com')

--
insert into @t([ProviderName],[ProviderKey],[UserId])
values
(N'Facebook',N'100005765525384',@user_id)
--
merge into [dbo].[UserExternalInfo] as [target]
using @t as [source] on [target].[ProviderName] = [source].[ProviderName] AND [target].[ProviderKey] = [source].[ProviderKey]
when matched then
	update set [target].[ProviderName] = [source].[ProviderName],
	[target].[ProviderKey] = [source].[ProviderKey],
	[target].[UserId] = [source].[UserId]
when not matched by target then
	insert ([ProviderName],[ProviderKey],[UserId])
	values ([source].[ProviderName],[source].[ProviderKey],[source].[UserId]);
go