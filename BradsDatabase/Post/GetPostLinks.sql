CREATE PROCEDURE [Post].[GetPostLinks]
	@PostId int
AS
	SELECT [Link] FROM [Post_Link] WHERE [Post_Link].PostID = @PostId;
