CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Name] NCHAR(255) NOT NULL, 
    [Email] NCHAR(255) NOT NULL, 
    [Secrete] NCHAR(255) NOT NULL, 
    [Locked] DATETIME NULL, 
    [Disabled] DATETIME NULL, 
    [AccountType] NCHAR(10) NOT NULL, 
    [Confirmed] DATETIME NULL, 
    [Created] DATETIME NOT NULL default GETDATE()
    CONSTRAINT UserEmail UNIQUE (Email)
    CONSTRAINT UserName UNIQUE (Name)
)