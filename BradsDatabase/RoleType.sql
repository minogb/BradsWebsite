﻿CREATE TABLE [dbo].[RoleType]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL
    CONSTRAINT RoleTypeName UNIQUE (Name)
)