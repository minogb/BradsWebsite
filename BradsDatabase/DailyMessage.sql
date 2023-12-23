﻿CREATE TABLE [dbo].[DailyMessage]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Message] NVARCHAR(MAX) NOT NULL,
    [Start] NVARCHAR(5),
    [End] NVARCHAR(5), 
    [Category] NVARCHAR(50) NOT NULL  DEFAULT 'SAFETY'
)
