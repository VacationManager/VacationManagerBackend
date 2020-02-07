CREATE VIEW [dbo].[viVacationRequest]
	AS SELECT * FROM [VacationRequest] WITH (NOLOCK) WHERE [DeleteTime] IS NULL
