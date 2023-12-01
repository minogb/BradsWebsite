CREATE PROCEDURE [Post].[GetPostPage]
	@From DateTime,
	@Tag NVARCHAR(MAX) = null,
	@Mention NVARCHAR(MAX) = null,
	@Parent int = null
AS
--TODO: Mentions
--todo get links
	SELECT
       [Post].[UserID] As UserId
	  ,[User].Name As UserName
	  ,[Post].[Id] As PostId
      ,[Post].[Message]
      ,[Post].[Date]
	  ,(SELECT COUNT(*) FROM [Post_Reply] WHERE [Post_Reply].ParentPost = [Post].Id) As Replies
	  ,(SELECT [Post_Reply].ParentPost FROM [Post_Reply] WHERE [Post_Reply].ReplyPost = [Post].Id) As ParentPost
	FROM [Post]
		LEFT JOIN [User] ON [User].Id = [Post].UserID
	WHERE
		--TODO [Post].[Date] < @From and @@ROWCOUNT < 20 and
		(@Tag IS NULL OR [Post].Id IN(SELECT [Post_Tag].PostID FROM [Post_Tag] WHERE UPPER([Post_Tag].Tag) = UPPER(@Tag)))
		AND (@Parent IS null OR [Post].Id = @Parent OR [Post].Id IN (SELECT [Post_Reply].ReplyPost FROM [Post_Reply] WHERE [Post_Reply].ParentPost = @Parent))
		AND (@Mention IS null
				OR [Post].Id IN (SELECT [Post_Reference].PostID FROM [Post_Reference] WHERE [Post_Reference].UserID IN (SELECT [User].Id FROM [dbo].[User] WHERE UPPER([User].Name) = UPPER(@Mention))) 
				OR UPPER(@Mention) IN (SELECT UPPER([User].Name) FROM [dbo].[User] WHERE [User].Id = [Post].UserID))
		AND [User].Locked IS NULL AND [User].Disabled IS NULL
		AND [Post].Deleted IS NULL
	ORDER BY [Post].Date DESC;