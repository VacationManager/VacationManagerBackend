CREATE PROCEDURE [dbo].[spDeleteVacationSlot]
	@UserId INT = NULL
AS
	UPDATE vs
	SET vs.DeleteTime = GETUTCDATE()
	FROM VacationSlot AS vs
	INNER JOIN VacationRequest AS vr
	ON vr.Id = vs.VacationRequestId
	WHERE vr.UserId = @UserId
RETURN 0
