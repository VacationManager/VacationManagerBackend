CREATE PROCEDURE [dbo].[spInitialize]
	@initUserFirstName nvarchar(128),
	@initUserLastName nvarchar(128),
	@initUserMail nvarchar(128),
	@initUserPassword nvarchar(256),
	@initDepartmentName nvarchar(128),
	@defaultDayCount int
AS
	IF (EXISTS(SELECT TOP 1 1 FROM [viConfiguration]))
	BEGIN
		RETURN 0
	END

	DECLARE @depId INT
	EXEC @depId = [spCreateDepartment] @initDepartmentName

	EXEC [spSetUser] NULL, @depId, @initUserFirstName, @initUserLastName, @initUserMail, @initUserPassword, 1, 1, 28

	DECLARE @i INT = 1
	WHILE (@i <= 5)
	BEGIN
		INSERT INTO [BusinessDay] (
			[Weekday],
			[IsInUse]
		) VALUES (
			@i,
			1
		)

		SET @i = @i + 1
	END

	INSERT INTO [BusinessDay] (
			[Weekday],
			[IsInUse]
		) VALUES (
			6,
			0
		)

		INSERT INTO [BusinessDay] (
			[Weekday],
			[IsInUse]
		) VALUES (
			0,
			0
		)

	INSERT INTO [Configuration] (
		[DefaultDayCount]
	) VALUES (
		@defaultDayCount
	)

	RETURN 1
