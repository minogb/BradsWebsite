CREATE TABLE [Post].[Post_Reply]
(
	[ParentPost] INT NOT NULL PRIMARY KEY, 
    [ReplyPost] INT NOT NULL
    FOREIGN KEY ([ParentPost]) REFERENCES [Post].[Post]([Id])
    FOREIGN KEY ([ReplyPost]) REFERENCES [Post].[Post]([Id])
)
