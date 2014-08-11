create table [dbo].[UserMessageType]
(
	[Id]		int				not null,
	[Name]		nvarchar(128)	NOT null,
    constraint [PK_dboUserMessageType] primary key clustered ([Id])
);

GO

create unique index [UQ_dboUserMessageType_Name] ON [dbo].[UserMessageType] ([Name]);

GO