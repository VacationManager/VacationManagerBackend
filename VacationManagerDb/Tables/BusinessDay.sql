﻿CREATE TABLE [dbo].[BusinessDay]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Weekday] INT NOT NULL,
	[IsInUse] BIT NOT NULL DEFAULT 1,
	[CreationTime] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	[DeleteTime] DATETIME2 NULL
)
