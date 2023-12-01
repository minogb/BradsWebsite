CREATE TABLE [Post].[Post_Reference]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [PostID] INT NOT NULL, 
    [UserID] INT NOT NULL
    FOREIGN KEY ([PostID]) REFERENCES [Post].[Post]([Id])
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[User]([Id])
)
