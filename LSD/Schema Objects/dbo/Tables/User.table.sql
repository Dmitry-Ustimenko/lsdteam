create table [dbo].[User]
(
	[Id]			int				not null identity (1, 1),
	[UserName]		nvarchar(128)	not null,
	[Email]			nvarchar(50)	not null,
	[Password]		nvarchar(104)	not null,
	[IsActive]		bit				not null,
	[CreateDate] DATETIME NOT NULL, 
    [LastActivity] DATETIME NOT NULL
    constraint [PK_dboUser] primary key clustered ([Id]), 
    [ShowEmail] BIT NOT NULL default 0, 
    [PhotoPath] NVARCHAR(1024) NULL
);

go
create unique index [UQ_dboUser_Name] ON [dbo].[User] ([UserName]);
go
create unique index [UQ_dboUser_Email] ON [dbo].[User] ([Email]);
go