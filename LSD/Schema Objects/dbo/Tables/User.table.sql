create table [dbo].[User]
(
	[Id]			int				not null identity (1, 1),
	[UserName]		nvarchar(128)	not null,
	[Email]			nvarchar(50)	not null,
	[Password]		nvarchar(104)	not null,
	[IsActive]		bit				not null,
	[CreateDate] DATETIME NOT NULL, 
    [LastActivity] DATETIME NOT NULL, 
    [UserExternalInfoId] INT NULL, 
    constraint [PK_dboUser] primary key clustered ([Id])
);

go
create unique index [UQ_dboUser_Name] ON [dbo].[User] ([UserName]);
go
create unique index [UQ_dboUser_Email] ON [dbo].[User] ([Email]);
go

ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_UserExternalInfo] FOREIGN KEY([UserExternalInfoId])
REFERENCES [dbo].[UserExternalInfo] ([Id])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_UserExternalInfo]
GO