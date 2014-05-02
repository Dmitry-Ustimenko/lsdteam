create table [dbo].[UserPasswordResetToken]
(
	[Id]			int				not null identity(1,1),
	[CreateDate]	datetime		not null,
	[Token]			nvarchar(256)	not null,
	[UserId]		int				not null
);

go
alter table [dbo].[UserPasswordResetToken]
add constraint [PK_dboUserPasswordResetToken]
primary key clustered ([Id]);
go

alter table [dbo].[UserPasswordResetToken]
add constraint [FK_dboUserPasswordResetToken_dboUser] 
foreign key ([UserId])
references [dbo].[User] ([id])
on update cascade
on delete cascade;

go

create unique index [UQ_dboUserPasswordResetToken_token]
on [dbo].[UserPasswordResetToken]
(
	[Token]
)
include
(
	[UserId],
	[CreateDate]
);

go

create unique index [UQ_dboUserPasswordResetToken_user_id]
on [dbo].[UserPasswordResetToken]
(
	[UserId]
)
include
(
	[Token],
	[CreateDate]
);

go

