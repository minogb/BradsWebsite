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
	FROM [Post]
	LEFT JOIN [User] ON [User].Id = [Post].UserID
		where [Post].[Date] < @From and @@ROWCOUNT < 20
		and (@Tag IS NULL OR @Tag IN(SELECT [Post_Tag].Tag FROM [Post_Tag] WHERE Tag = @Tag))
		AND (@Parent IS null OR [Post].Id = @Parent OR [Post].Id IN (SELECT [Post_Reply].ReplyPost FROM [Post_Reply] WHERE [Post_Reply].ParentPost = @Parent))
		AND [User].Locked IS NULL AND [User].Disabled IS NULL
		AND [Post].Deleted IS NULL
	ORDER BY [Post].Date DESC;