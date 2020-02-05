﻿CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[DepartmentId] INT NOT NULL,
	[FirstName] NVARCHAR(256) NOT NULL,
	[LastName] NVARCHAR(256) NOT NULL,
	[MailAddress] NVARCHAR(256) NOT NULL,
	[Password] NVARCHAR(512) NOT NULL,
	[IsManager] BIT NOT NULL DEFAULT 0,
	[IsAdmin] BIT NOT NULL DEFAULT 0,
	[VacationDayCount] INT NOT NULL,
	[CreationTime] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	[DeleteTime] DATETIME2 NULL,
	CONSTRAINT [FK_User_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [Department]([Id])
)
