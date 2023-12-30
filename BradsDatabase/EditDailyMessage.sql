CREATE PROCEDURE [dbo].[EditDailyMessage]
	@id int,
	@Category NVARCHAR(50),
	@Message nvarchar(MAX),
	@Start nvarchar(5) = NULL,
	@End nvarchar(5) = NULL
AS
	UPDATE [DailyMessage] SET [Category] = UPPER(@Category),[Message] = @Message,[Start] =@Start,[End] = @End WHERE @id = Id