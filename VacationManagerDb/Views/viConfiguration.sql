CREATE VIEW [dbo].[viConfiguration]
	AS SELECT * FROM [Configuration] WITH (NOLOCK) WHERE [DeleteTime] IS NULL
