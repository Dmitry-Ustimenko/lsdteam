create table [dbo].[UserToken]
(
	[Id]			int				not null identity(1,1),
	[CreateDate]	datetime		not null,
	[Token]			nvarchar(256)	not null,
	[UserId]		int				not null
);

go
alter table [dbo].[UserToken]
add constraint [PK_dboUserToken]
primary key clustered ([Id]);
go

alter table [dbo].[UserToken]
add constraint [FK_dboUserToken_dboUser] 
foreign key ([UserId])
references [dbo].[User] ([id])
on update cascade
on delete cascade;

go

create unique index [UQ_dboUserToken_token]
on [dbo].[UserToken]
(
	[Token]
)
include
(
	[UserId],
	[CreateDate]
);

go

create unique index [UQ_dboUserToken_user_id]
on [dbo].[UserToken]
(
	[UserId]
)
include
(
	[Token],
	[CreateDate]
);

go

