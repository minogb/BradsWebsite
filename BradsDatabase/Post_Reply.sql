CREATE TABLE [dbo].[Post_Reply]
(
	[ParentPost] INT NOT NULL PRIMARY KEY, 
    [ReplyPost] INT NOT NULL
    FOREIGN KEY ([ParentPost]) REFERENCES [Post]([Id])
    FOREIGN KEY ([ReplyPost]) REFERENCES [Post]([Id])
)
