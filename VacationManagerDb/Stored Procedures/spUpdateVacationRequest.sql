CREATE PROCEDURE [dbo].[spUpdateVacationRequest]
	@userId int,
	@newState int,
	@requestId int
AS
	DECLARE @departmentId int = (SELECT [DepartmentId] FROM [viUser] WHERE [Id] = @userId AND [IsManager] = 1)
	IF (@departmentId IS NULL OR (SELECT [DepartmentId] FROM [viUser] u LEFT JOIN [viVacationRequest] r ON r.[UserId] = u.[Id] WHERE r.[Id] = @requestId) != @departmentId)
	BEGIN
		RETURN 0
	END

	UPDATE [VacationRequest]
	SET [RequestState] = @newState
	WHERE [Id] = @requestId

	IF (@newState = 2)
	BEGIN
		UPDATE [VacationSlot]
		SET [DeleteTime] = GETUTCDATE()
		WHERE [VacationRequestId] = @requestId
	END

	RETURN 1