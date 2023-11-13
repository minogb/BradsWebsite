CREATE PROCEDURE [dbo].[DeletePost]
	@PostID int = 0,
	@UserID int
AS
	UPDATE [Post] set [Post].Deleted = GETDATE() WHERE [Post].Id = @PostID AND [Post].UserID = @UserID;
RETURN @@ROWCOUNT