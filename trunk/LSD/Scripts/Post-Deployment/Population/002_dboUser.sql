-- =================================
-- Populate the dbo.User table.
-- =================================
declare @t table
(
	[UserName]       nvarchar(128) not null unique,
	[Email]       nvarchar(50) not null unique,
	[Password]		nvarchar(104)	not null,
	[IsActive]		bit		not null,
	[CreateDate] DATETIME NOT NULL,
    [LastActivity] DATETIME NOT NULL,
    [ShowEmail] BIT NOT NULL,
    [PhotoPath] NVARCHAR(1024) NULL, 
    [RoleId] int NOT NULL,
    [IsBanned] BIT NOT NULL
);
--
insert into @t([UserName],[Email],[Password],[IsActive],[CreateDate],[LastActivity],[ShowEmail],[RoleId],[IsBanned])
values
(N'Admin',N'lord.udv@gmail.com',N'+co6/6k=Jd603H+kwKYK7kAAlzM5VXQlRKJyVxOlSFT2hobvrK4=',1,GetDate(),GetDate(),0,1,0)
--
merge into [dbo].[User] as [target]
using @t as [source] on [target].[UserName] = [source].[UserName] OR [target].[Email] = [source].[Email]
when matched then
	update set [target].[UserName] = [source].[UserName],
	[target].[Email] = [source].[Email],
	[target].[Password] = [source].[Password],
	[target].[IsActive] = [source].[IsActive],
	[target].[CreateDate] = [source].[CreateDate],
	[target].[LastActivity] = [source].[LastActivity],
	[target].[ShowEmail] = [source].[ShowEmail],
	[target].[RoleId] = [source].[RoleId],
	[target].[IsBanned] = [source].[IsBanned]
when not matched by target then
	insert ([UserName],[Email],[Password],[IsActive],[CreateDate],[LastActivity],[ShowEmail],[RoleId],[IsBanned])
	values ([source].[UserName],[source].[Email],[source].[Password],[source].[IsActive],[source].[CreateDate],[source].[LastActivity],[source].[ShowEmail],[source].[RoleId],[source].[IsBanned]);
go