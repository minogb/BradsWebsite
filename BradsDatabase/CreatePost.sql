CREATE PROCEDURE [dbo].[CreatePost]
	@UserID int,
	@Message NVARCHAR(MAX),
	@Links PostLinkDataType READONLY,
	@ParentId int = null
AS
	Declare @PostId int;
	Declare @Index int;
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
RETURN @PostId