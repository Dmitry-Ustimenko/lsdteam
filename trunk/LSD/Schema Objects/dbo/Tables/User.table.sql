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
    [PhotoPath] NVARCHAR(1024) NULL, 
    [RoleId] int NOT NULL DEFAULT 8, 
    [IsBanned] BIT NOT NULL DEFAULT 0
);

go
create unique index [UQ_dboUser_Name] ON [dbo].[User] ([UserName]);
go
create unique index [UQ_dboUser_Email] ON [dbo].[User] ([Email]);
go

ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
ON UPDATE CASCADE
ON DELETE No Action
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO