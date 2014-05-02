create table [dbo].[UserInfo]
(
	[Id]			int				not null identity (1, 1),
	[FirstName]		nvarchar(128)	null,
	[LastName]		nvarchar(128)	null,
	[Address]		nvarchar(1024)	null,
	[Activity]		nvarchar(1024)	null,
	[DateBirth]	datetime		null,
	[SexId]			int			null,
    [UserId] INT NOT NULL, 
    constraint [PK_dboUserInfo] primary key clustered ([Id])
);

go
ALTER TABLE [dbo].[UserInfo]  WITH CHECK ADD  CONSTRAINT [FK_UserInfo_Sex] FOREIGN KEY([SexId])
REFERENCES [dbo].[Sex] ([Id])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[UserInfo] CHECK CONSTRAINT [FK_UserInfo_Sex]
GO

ALTER TABLE [dbo].[UserInfo]  WITH CHECK ADD CONSTRAINT [FK_UserInfo_User] FOREIGN KEY([SexId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserInfo] CHECK CONSTRAINT [FK_UserInfo_User]
GO