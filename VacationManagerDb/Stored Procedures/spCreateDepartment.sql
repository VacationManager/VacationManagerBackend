CREATE PROCEDURE [dbo].[spCreateDepartment]
	@name nvarchar(256)
AS
	INSERT INTO [Department] (
		[DepartmentName]
	) VALUES (
		@name
	)

	SELECT SCOPE_IDENTITY()
