CREATE PROCEDURE [dbo].[spGetDuration]
	@userId int,
	@startTime datetime2,
	@endTime datetime2
AS
	DECLARE @vacationRequest TABLE(
		[Id] INT NOT NULL PRIMARY KEY,
		[StartTime] DATETIME2,
		[EndTime] DATETIME2,
		[Duration] INT
	)

	INSERT INTO @vacationRequest (
		[Id]
		,[StartTime]
		,[EndTime]
		,[Duration]
	)
	SELECT [Id]
		,[StartTime]
		,[EndTime]
		,[Duration]
	FROM [viVacationRequest]
	WHERE [UserId] = @userId
	AND [RequestState] != 2
	AND [StartTime] >= @startTime
	AND [EndTime] <= @endTime

	

RETURN 0
