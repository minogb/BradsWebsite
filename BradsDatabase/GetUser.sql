CREATE PROCEDURE [dbo].[GetUser]
	@UserEmail NVARCHAR(255),
	@UserSecrete NVARCHAR(255)
AS
SELECT * FROM [User] where LOWER(Email) = LOWER(@UserEmail) AND Secrete = @UserSecrete;