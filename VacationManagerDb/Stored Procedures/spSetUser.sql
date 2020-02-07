CREATE PROCEDURE [dbo].[spSetUser]
	@Id INT = NULL,
	@DepartmentId INT = NULL,
	@FirstName NVARCHAR(256) = NULL,
	@LastName NVARCHAR(256) = NULL,
	@MailAddress NVARCHAR(256) = NULL,
	@Password NVARCHAR(512) = NULL,
	@IsManager BIT = NULL,
	@IsAdmin BIT = NULL,
	@VacationDayCount INT = NULL,
	@Delete BIT = 0
AS
	IF (@Id IS NULL)
	BEGIN
		INSERT INTO [User](
			DepartmentId,
			FirstName,
			LastName,
			MailAddress,
			[Password],
			IsManager,
			IsAdmin,
			VacationDayCount
		)
		VALUES(
			@DepartmentId,
			@FirstName,
			@LastName,
			@MailAddress,
			@Password,
			@IsManager,
			@IsAdmin,
			@VacationDayCount
		);

		RETURN @@IDENTITY
	END

	ELSE
	BEGIN
		IF (@Delete = 0)
		BEGIN
			UPDATE [User]
			SET	DepartmentId = ISNULL(@DepartmentId, DepartmentId),
				FirstName = ISNULL(@FirstName, FirstName),
				LastName = ISNULL(@LastName, LastName),
				MailAddress = ISNULL(@MailAddress, MailAddress),
				[Password] = ISNULL(@Password, [Password]),
				IsManager = ISNULL(@IsManager, IsManager),
				VacationDayCount = ISNULL(@VacationDayCount, VacationDayCount)
			WHERE Id = @Id
		END

		ELSE
		BEGIN
			UPDATE [User]
			SET DeleteTime = GETUTCDATE()
			WHERE Id = @Id
		END
	END
RETURN 0
