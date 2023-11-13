CREATE PROCEDURE [dbo].[CreateUser]
	@UserEmail varchar(255),
	@UserName varchar(255),
	@UserSecrete varchar(255)
AS
	if (SELECT Count(*) FROM [User] where LOWER(Email) = LOWER(@UserEmail)) > 0
		return 0;
	INSERT INTO [User] (Name,Email,Secrete,Locked,Disabled,AccountType,Confirmed) values (@UserName,@UserEmail,@UserSecrete,null,null,'USER',null);
RETURN 1