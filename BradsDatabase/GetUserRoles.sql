CREATE PROCEDURE [dbo].[GetUserRoles]
	@UserId int
AS
	SELECT Role FROM [RoleView] WHERE [User] = @UserId;
