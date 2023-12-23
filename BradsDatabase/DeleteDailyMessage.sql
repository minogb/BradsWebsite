CREATE PROCEDURE [dbo].[DeleteDailyMessage]
	@id int
AS
	DELETE FROM [DailyMessage] WHERE [DailyMessage].Id = @id;