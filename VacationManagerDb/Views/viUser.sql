﻿CREATE VIEW [dbo].[viUser]
	AS SELECT * FROM [User] WITH (NOLOCK) WHERE [DeleteTime] IS NULL
