CREATE PROCEDURE [dbo].[GetPostPage]
	@From DateTime,
	@Tag NVARCHAR = null,
	@Mention NVARCHAR = null,
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
		(@Tag IS NULL OR UPPER(@Tag) IN(SELECT UPPER([Post_Tag].Tag) FROM [Post_Tag] WHERE Tag = @Tag))
		AND (@Parent IS null OR [Post].Id = @Parent OR [Post].Id IN (SELECT [Post_Reply].ReplyPost FROM [Post_Reply] WHERE [Post_Reply].ParentPost = @Parent))
		AND (@Mention IS null
				OR [Post].Id IN (SELECT [Post_Reference].PostID FROM [Post_Reference] WHERE [Post_Reference].UserID IN (SELECT [Id] FROM [User] WHERE UPPER([User].Name) = UPPER(@Mention))) 
				OR UPPER(@Mention) IN (SELECT UPPER([Name]) FROM [User] WHERE [User].Id = [Post].UserID))
		AND [User].Locked IS NULL AND [User].Disabled IS NULL
		AND [Post].Deleted IS NULL
	ORDER BY [Post].Date DESC;