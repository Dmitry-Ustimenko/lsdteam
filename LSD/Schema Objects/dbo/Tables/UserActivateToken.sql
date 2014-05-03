create table [dbo].[UserActivateToken]
(
	[Id]			int				not null identity(1,1),
	[Token]			nvarchar(256)	not null,
	[UserId]		int				not null
);

go
alter table [dbo].[UserActivateToken]
add constraint [PK_dboUserActivateToken]
primary key clustered ([Id]);
go

alter table [dbo].[UserActivateToken]
add constraint [FK_dboUserActivateToken_dboUser] 
foreign key ([UserId])
references [dbo].[User] ([id])
on update cascade
on delete cascade;

go

create unique index [UQ_dboUserActivateToken_token]
on [dbo].[UserActivateToken]
(
	[Token]
)
include
(
	[UserId]
);

go

create unique index [UQ_dboUserActivateToken_user_id]
on [dbo].[UserActivateToken]
(
	[UserId]
)
include
(
	[Token]
);

go