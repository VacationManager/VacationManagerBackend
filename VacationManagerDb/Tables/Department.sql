﻿CREATE TABLE [dbo].[Department]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[DepartmentName] NVARCHAR(256)  NOT NULL,
	[CreationTime] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	[DeleteTime] DATETIME2 NULL
)
