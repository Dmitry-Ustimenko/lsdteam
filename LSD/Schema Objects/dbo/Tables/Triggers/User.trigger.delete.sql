CREATE TRIGGER UserDeleteTrigger
	ON [dbo].[User]
	INSTEAD OF DELETE
AS

BEGIN

	DECLARE @Del table ( UserId int NOT NULL )

	INSERT INTO @Del
	SELECT [Id]
	FROM deleted

	UPDATE [dbo].[UserMessage]
	SET [RecipientId] = NULL
	WHERE [RecipientId] IN (SELECT [UserId] FROM @Del)

	UPDATE [dbo].[UserMessage]
	SET [SenderId] = NULL
	WHERE [SenderId] IN (SELECT [UserId] FROM @Del)

	DELETE FROM [dbo].[User]
	WHERE [Id] IN (SELECT [UserId] FROM @Del)

END

GO