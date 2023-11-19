CREATE PROCEDURE [dbo].[CreatePost]
	@UserID int,
	@Message NVARCHAR(MAX),
	@Links PostLinkDataType READONLY,
	@ParentId int = null
AS
	Declare @PostId int;
	Declare @Index int;
	Declare @Other int;
	DECLARE @Word NVARCHAR(MAX);
	Declare Words CURSOR FOR
		SELECT value from STRING_SPLIT(@Message,' ');

	INSERT INTO [Post] ([UserID],[Message]) values (@UserID,@Message);
	--TODO create tags and references
	Set @PostId = SCOPE_IDENTITY();
	IF @ParentId IS NOT NULL
		INSERT INTO [Post_Reply] (ParentPost,ReplyPost) VALUES(@ParentId,@PostId);
	--Create links:
	Set @Index =  0;
	While @Index < (SELECT COUNT(*) FROM @Links)
	BEGIN
		INSERT INTO [Post_Link] ([Link],[PostID]) VALUES((SELECT [Link] FROM @Links WHERE [RowID] = @Index),@PostId);
		Set @Index =@Index + 1;
	END
	OPEN Words
	FETCH NEXT FROM Words INTO @Word
	WHILE @@FETCH_STATUS  = 0
	BEGIN
		SET @Other = NULL;
		IF @Word LIKE '@%'
			SELECT @Other = [Id] FROM [User] WHERE UPPER([User].Name) = UPPER(RIGHT(@Word,LEN(@Word)-1));
			IF @Other IS NOT NULL
				INSERT INTO [Post_Reference] ([PostID],[UserID]) VALUES(@PostId, @Other);
		IF @Word LIKE '#%'
			INSERT INTO [Post_Tag] ([PostID],[Tag]) VALUES(@PostId, RIGHT(@Word,LEN(@Word)-1));
		FETCH NEXT FROM Words INTO @Word
	END

	CLOSE Words
	Deallocate Words
RETURN @PostId