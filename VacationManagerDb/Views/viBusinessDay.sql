CREATE VIEW [dbo].[viBusinessDay]
	AS SELECT * FROM [BusinessDay] WITH (NOLOCK) WHERE [DeleteTime] IS NULL
