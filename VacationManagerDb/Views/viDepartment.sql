CREATE VIEW [dbo].[viDepartment]
	AS SELECT * FROM [Department] WITH (NOLOCK) WHERE [DeleteTime] IS NULL
