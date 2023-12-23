CREATE PROCEDURE [dbo].[CreateDailyMessage]
	@Category NVARCHAR(50),
	@Message nvarchar(MAX),
	@Start DATE = NULL,
	@End DATE = NULL
AS
	DECLARE @sDate NVARCHAR(5);
	DECLARE @eDate NVARCHAR(5);
	SET @sDate = NULL;
	SET @eDate = NULL;
	IF @Start IS NOT NULL AND @End IS NOT NULL
	BEGIN
		SET @sDate = FORMAT(@Start,'MM-dd');
		SET @eDate = FORMAT(@End,'MM-dd');
	END;
	INSERT INTO [DailyMessage] ([Category],[Message],[Start],[End]) VALUES(UPPER(@Category),@Message,@Start,@End);

	SELECT SCOPE_IDENTITY()