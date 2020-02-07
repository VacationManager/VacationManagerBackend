CREATE PROCEDURE [dbo].[spCreateDepartment]
	@name nvarchar(256)
AS
	INSERT INTO [Department] (
		[DepartmentName]
	) VALUES (
		@name
	)

	DECLARE @depId INT = SCOPE_IDENTITY()

	RETURN @depId
