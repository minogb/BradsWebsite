CREATE PROCEDURE [dbo].[CreateDailyMessage]
	@Category NVARCHAR(50),
	@Message nvarchar(MAX),
	@Start nvarchar(5) = NULL,
	@End nvarchar(5) = NULL
AS
	INSERT INTO [DailyMessage] ([Category],[Message],[Start],[End]) VALUES(UPPER(@Category),@Message,@Start,@End);

	SELECT SCOPE_IDENTITY()