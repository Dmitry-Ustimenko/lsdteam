create table [dbo].[UserExternalInfo]
(
	[Id]			int				not null identity (1, 1),
	[ProviderName] NVARCHAR(50) NOT NULL, 
    [ProviderKey] NVARCHAR(50) NOT NULL, 
    constraint [PK_dboUserExternalInfo] primary key clustered ([Id])
);

go
create unique index [UQ_dboUserExternalInfo_ProviderName_ProviderKey] ON [dbo].[UserExternalInfo] ([ProviderName], [ProviderKey]);
go