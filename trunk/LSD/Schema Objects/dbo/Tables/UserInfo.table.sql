create table [dbo].[UserInfo]
(
	[Id]			int				not null identity (1, 1),
	[FirstName]		nvarchar(128)	null,
	[LastName]		nvarchar(128)	null,
	[Activity]		nvarchar(1024)	null,
	[DateBirth]	datetime		null,
	[Country] nvarchar(128)	null,
	[Town] nvarchar(128)	null,
	[Street] nvarchar(128)	null,
	[HomeNumber] nvarchar(5)	null,
	[SiteLink] nvarchar(128)	null,
	[ICQ] nvarchar(128)	null,
	[Skype] nvarchar(128)	null,
	[BattleLog] nvarchar(128)	null,
	[Steam] nvarchar(128)	null,
	[AboutMe] nvarchar(128)	null,
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

ALTER TABLE [dbo].[UserInfo]  WITH CHECK ADD CONSTRAINT [FK_UserInfo_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserInfo] CHECK CONSTRAINT [FK_UserInfo_User]
GO

create unique index [UQ_dboUserInfo_UserId] ON [dbo].[UserInfo] ([UserId]);

go