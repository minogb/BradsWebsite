CREATE PROCEDURE [dbo].[GetDailyMessage]
	@id int = 0,
	@Category nvarchar(50) = null
AS
	
	DECLARE @cDate NVARCHAR(5);
	SET @cDate = FORMAT(GetDate(),'MM-dd');
	IF @id > 0
		SELECT [ID], [Message], [Start], [End], [Category] FROM [DailyMessage] WHERE @id = [Id];
	ELSE
		SELECT TOP 1 [ID], [Message], [Start], [End] FROM [DailyMessage] WHERE  ([Start] IS NULL OR ([Start] <= @cDate AND [End] >= @cDate)) AND [Category] = UPPER(@Category) ORDER BY NEWID();