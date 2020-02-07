CREATE PROCEDURE [dbo].[spDeleteVacationRequest]
	@Id INT = NULL,
	@UserId INT = NULL
AS
	UPDATE VacationRequest
	SET DeleteTime = GETUTCDATE()
	WHERE (@Id > 0 AND Id = @Id OR @Id IS NULL)
	AND (@UserId > 0 AND UserId = @UserId OR @UserId IS NULL)
RETURN 0
