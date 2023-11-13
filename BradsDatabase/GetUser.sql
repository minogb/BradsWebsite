CREATE PROCEDURE [dbo].[GetUser]
	@UserEmail varchar(255),
	@UserSecrete varchar(255)
AS
SELECT * FROM [User] where LOWER(Email) = LOWER(@UserEmail) AND Secrete = @UserSecrete;