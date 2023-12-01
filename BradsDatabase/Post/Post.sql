CREATE TABLE [Post].[Post]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [UserID] INT NOT NULL, 
    [Message] NVARCHAR(MAX) NOT NULL
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[User]([Id]), 
    [Date] DATETIME NOT NULL DEFAULT GetDate(), 
    [Deleted] DATETIME NULL
)
