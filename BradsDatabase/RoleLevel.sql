CREATE TABLE [dbo].[RoleLevel]
(
	[Level] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL,
    CONSTRAINT RoleLevelName UNIQUE (Name)
)
