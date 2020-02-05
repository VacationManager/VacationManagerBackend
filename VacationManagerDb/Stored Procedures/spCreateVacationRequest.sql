﻿CREATE TYPE [HolidayList] AS TABLE(
	[Day] DATE
)
GO
CREATE PROCEDURE [dbo].[spCreateVacationRequest]
	@userId int,
	@startTime datetime2,
	@endTime datetime2,
	@annotation nvarchar(max),
	@holidays [HolidayList] READONLY
AS
	IF (@endTime <= @startTime)
	BEGIN
		RAISERROR(N'StartTime must be smaller than EndTime', 10, 1)
	END
	IF (EXISTS(SELECT TOP 1 1 FROM [viVacationRequest] WHERE [UserId] = @userId AND [StartTime] >= @startTime AND [EndTime] < @endTime AND [RequestState] != 2))
	BEGIN
		RETURN -1
	END
	DECLARE @duration DECIMAL(10,1) = 0
	DECLARE @currentTime datetime2 = @startTime
	WHILE (@startTime <= @endTime)
	BEGIN
		IF (EXISTS(SELECT TOP 1 1 FROM [viBusinessDay] WHERE [Weekday] = DATEPART(DW, @currentTime) - 1 AND [IsInUse] = 1) AND NOT EXISTS(SELECT TOP 1 1 FROM @holidays WHERE [Day] = CAST(@currentTime AS date)))
		BEGIN
			SET @duration = @duration + 0.5
		END
		SET @currentTime = DATEADD(HOUR, 0.5, @currentTime)
	END

	INSERT INTO [VacationRequest] (
		[UserId]
		,[StartTime]
		,[EndTime]
		,[Duration]
		,[Annotation]
	) VALUES (
		@userId
		,@startTime
		,@endTime
		,@duration
		,@annotation
	)
RETURN 1