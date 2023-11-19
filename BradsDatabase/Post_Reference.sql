CREATE TABLE [dbo].[Post_Reference]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [PostID] INT NOT NULL, 
    [UserID] INT NOT NULL
    FOREIGN KEY ([PostID]) REFERENCES [Post]([Id])
    FOREIGN KEY ([UserID]) REFERENCES [User]([Id])
)
