create table [dbo].[UserExternalInfo]
(
	[Id]			int				not null identity (1, 1),
	[ProviderName] NVARCHAR(50) NOT NULL, 
    [ProviderKey] NVARCHAR(50) NOT NULL, 
    [UserId] INT NOT NULL, 
    constraint [PK_dboUserExternalInfo] primary key clustered ([Id])
);

go
create unique index [UQ_dboUserExternalInfo_ProviderName_ProviderKey] ON [dbo].[UserExternalInfo] ([ProviderName], [ProviderKey]);
go

ALTER TABLE [dbo].[UserExternalInfo]  WITH CHECK ADD  CONSTRAINT [FK_UserExternalInfo_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserExternalInfo] CHECK CONSTRAINT [FK_UserExternalInfo_User]
GO