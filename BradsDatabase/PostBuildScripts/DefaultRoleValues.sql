IF (SELECT COUNT(*) FROM RoleLevel) = 0
    BEGIN
        INSERT INTO RoleLevel ([Level], [Name]) VALUES(0,'USER');
        INSERT INTO RoleLevel ([Level], [Name]) VALUES(1,'MOD');
        INSERT INTO RoleLevel ([Level], [Name]) VALUES(2,'ADMIN');
        INSERT INTO RoleLevel ([Level], [Name]) VALUES(3,'OWNER');
    END;
IF (SELECT COUNT(*) FROM RoleType) = 0
    BEGIN
        INSERT INTO RoleType ([Name]) VALUES('USER');
        INSERT INTO RoleType ([Name]) VALUES('POST');
        INSERT INTO RoleType ([Name]) VALUES('PYRAMID');
        INSERT INTO RoleType ([Name]) VALUES('RESOURCE');
        INSERT INTO RoleType ([Name]) VALUES('DAILY');
        DECLARE @UserCursor CURSOR;
        DECLARE @UserId int;
        SET @UserCursor = CURSOR FOR
        select [Id] FROM [User] WHERE [Id] NOT IN (SELECT [User] FROM [UserRole])
        OPEN @UserCursor 
        FETCH NEXT FROM @UserCursor 
        INTO @UserId
        WHILE @@FETCH_STATUS = 0
        BEGIN
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
          FETCH NEXT FROM @UserCursor 
          INTO @UserId 
        END; 
        CLOSE @UserCursor;
        DEALLOCATE @UserCursor;
    END;