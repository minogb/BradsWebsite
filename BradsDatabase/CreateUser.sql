CREATE PROCEDURE [dbo].[CreateUser]
	@UserEmail NVARCHAR(255),
	@UserName NVARCHAR(255),
	@UserSecrete NVARCHAR(255)
AS
	if (SELECT Count(*) FROM [User] where LOWER(Email) = LOWER(@UserEmail)) > 0
		return 0;
	INSERT INTO [User] (Name,Email,Secrete,Locked,Disabled,AccountType,Confirmed) values (@UserName,@UserEmail,@UserSecrete,null,null,'USER',null);
	DECLARE @UserId int;
    SET @UserId = SCOPE_IDENTITY();
    DECLARE @RoleCursor CURSOR;
    DECLARE @RoleId int;
    SET @RoleCursor = CURSOR FOR
    select [Id] FROM [RoleType]
    OPEN @RoleCursor 
    FETCH NEXT FROM @RoleCursor 
    INTO @RoleId
    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO [UserRole] ([Level],[User],[Role]) VALUES(0,@UserId,@RoleId);
    FETCH NEXT FROM @RoleCursor 
    INTO @RoleId 
    END; 
    CLOSE @RoleCursor;
    DEALLOCATE @RoleCursor;
RETURN 1