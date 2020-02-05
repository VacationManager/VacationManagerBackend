CREATE PROCEDURE [dbo].[spCreateVacationRequest]
	@userId int,
	@startTime datetime2,
	@endTime datetime2,
	@annotation nvarchar(max)
AS
	IF (@endTime <= @startTime)
	BEGIN
		RAISERROR(N'StartTime must be smaller than EndTime', 10, 1)
	END
	IF (EXISTS(SELECT TOP 1 1 FROM [viVacationRequest] WHERE [UserId] = @userId AND [StartTime] >= @startTime AND [EndTime] < @endTime))
	BEGIN
		RETURN -1
	END

	DECLARE @vacationSlot TABLE (
		[Date] DATE
		,[DayTimeType] BIT
	)

	DECLARE @currentTime datetime2 = @startTime
	WHILE (@currentTime <= @endTime)
	BEGIN
		IF (EXISTS(SELECT TOP 1 1 FROM [viBusinessDay] WHERE [Weekday] = DATEPART(DW, @currentTime) - 1 AND [IsInUse] = 1))
		BEGIN
			INSERT INTO @vacationSlot (
				[Date]
				,[DayTimeType]
			) VALUES (
				@currentTime
				,CASE WHEN CAST(@currentTime AS TIME) < '12:00' THEN 0 ELSE 1 END
			)
		END
		SET @currentTime = DATEADD(DAY, 0.5, @currentTime)
	END

	DECLARE @startYearSlotCount int =  (SELECT DISTINCT COUNT([S].[Id])
															FROM [viVacationSlot] [S]
															LEFT JOIN [viVacationRequest] [R]
															ON [S].[VacationRequestId] = [R].[Id]
															WHERE [UserId] = @userId
															AND [RequestState] != 2
															GROUP BY [Date]
															HAVING YEAR([Date]) = YEAR(@startTime)) + (SELECT COUNT(1) FROM @vacationSlot GROUP BY [Date] HAVING YEAR([Date]) = YEAR(@startTime))

	DECLARE @endYearSlotCount int =  (SELECT DISTINCT COUNT([S].[Id])
													FROM [viVacationSlot] [S]
													LEFT JOIN [viVacationRequest] [R]
													ON [S].[VacationRequestId] = [R].[Id]
													WHERE [UserId] = @userId
													AND [RequestState] != 2
													GROUP BY [Date]
													HAVING YEAR([Date]) = YEAR(@endTime)) + (SELECT COUNT(1) FROM @vacationSlot GROUP BY [Date] HAVING YEAR([Date]) = YEAR(@endTime))

	DECLARE @slotsPerYear int = (SELECT [VacationDayCount] FROM [viUser] WHERE [Id] = @userId) * 2

	IF (@startYearSlotCount > @slotsPerYear)
	BEGIN
		RETURN 0
	END

	INSERT INTO [VacationRequest] (
		[UserId]
		,[StartTime]
		,[EndTime]
		,[Annotation]
	) VALUES (
		@userId
		,@startTime
		,@endTime
		,@annotation
	)
	DECLARE @requestId int = SCOPE_IDENTITY()

	INSERT INTO [VacationSlot] (
		[VacationRequestId]
		,[Date]
		,[DayTimeType]
	)
	SELECT  @requestId
			,[Date]
			,[DayTimeType]
		FROM @vacationSlot
RETURN 1
