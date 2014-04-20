create table [dbo].[User]
(
	[Id]			int				not null identity (1, 1),
	[UserName]		nvarchar(128)	not null,
	[FirstName]		nvarchar(128)	null,
	[LastName]		nvarchar(128)	null,
	[Email]			nvarchar(50)	not null,
	[Password]		nvarchar(104)	not null,
	[Address]		nvarchar(1024)	null,
	[IsActive]		bit				not null,
	[Activity]		nvarchar(1024)	null,
	[DateBirth]	datetime		not null,
	[SexId]			int			null,
	constraint [PK_dboUser] primary key clustered ([Id])
);

go
create unique index [UQ_dboUser_Name] ON [dbo].[User] ([UserName]);
go
create unique index [UQ_dboUser_Email] ON [dbo].[User] ([Email]);
go
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Sex] FOREIGN KEY([SexId])
REFERENCES [dbo].[Sex] ([Id])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Sex]
GO