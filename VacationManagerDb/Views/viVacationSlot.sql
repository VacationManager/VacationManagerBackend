CREATE VIEW [dbo].[viVacationSlot]
	AS SELECT * FROM [VacationSlot] WITH (NOLOCK) WHERE [DeleteTime] IS NULL
