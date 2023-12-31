CREATE TABLE [dbo].[UserRole]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [User] INT NOT NULL, 
    [Role] INT NOT NULL,
    [Level] INT NOT NULL
    CONSTRAINT RoleUser UNIQUE ([User], [Role], [Level])
)
