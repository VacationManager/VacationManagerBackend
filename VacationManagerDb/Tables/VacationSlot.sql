CREATE TABLE [dbo].[VacationSlot]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[VacationRequestId] INT NOT NULL,
	[Date] DATE NOT NULL,
	[DayTimeType] BIT NOT NULL,
	[CreationTime] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	[DeleteTime] DATETIME2 NULL,
	CONSTRAINT [FK_VacationSlot_VacationRequest] FOREIGN KEY ([VacationRequestId]) REFERENCES [VacationRequest]([Id])
)
