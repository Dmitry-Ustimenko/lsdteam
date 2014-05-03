create table [dbo].[UserResetToken]
(
	[Id]			int				not null identity(1,1),
	[CreateDate]	datetime		not null,
	[Token]			nvarchar(256)	not null,
	[UserId]		int				not null
);

go
alter table [dbo].[UserResetToken]
add constraint [PK_dboUserResetToken]
primary key clustered ([Id]);
go

alter table [dbo].[UserResetToken]
add constraint [FK_dboUserResetToken_dboUser] 
foreign key ([UserId])
references [dbo].[User] ([id])
on update cascade
on delete cascade;

go

create unique index [UQ_dboUserResetToken_token]
on [dbo].[UserResetToken]
(
	[Token]
)
include
(
	[UserId],
	[CreateDate]
);

go

create unique index [UQ_dboUserResetToken_user_id]
on [dbo].[UserResetToken]
(
	[UserId]
)
include
(
	[Token],
	[CreateDate]
);

go

