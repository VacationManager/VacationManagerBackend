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
RETURN 0
