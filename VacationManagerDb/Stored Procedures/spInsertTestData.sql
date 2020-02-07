CREATE PROCEDURE [dbo].[spInsertTestData]
AS
	DECLARE @depId int = 0;
	DECLARE @firstName NVARCHAR(64)
	DECLARE @lastName NVARCHAR(64)
	DECLARE @mail NVARCHAR(128)
	DECLARE @pw nvarchar(128)
	DECLARE @userId int
	DECLARE @holidays [HolidayList]


	EXEC [spCreateDepartment] 'Personalmanagement'
	SET @depId = @@IDENTITY

	SELECT @depId
	SET @firstName = 'Jens'
	SET @lastName = 'Baum'
	SET @mail = @firstName + '.' + @lastName + '@example.com'
	SET @pw = @lastName + '1'
	EXEC [spSetUser] NULL, @depId, @firstName, @lastName, @mail, @pw, 1, 1, 28

	SET @firstName = 'Max'
	SET @lastName = 'Mustermann'
	SET @mail = @firstName + '.' + @lastName + '@example.com'
	SET @pw = @lastName + '1'
	EXEC [spSetUser] NULL, @depId, @firstName, @lastName, @mail, @pw, 0, 0, 28

	SET @firstName = 'Brigitte'
	SET @lastName = 'Stroh'
	SET @mail = @firstName + '.' + @lastName + '@example.com'
	SET @pw = @lastName + '1'
	EXEC [spSetUser] NULL, @depId, @firstName, @lastName, @mail, @pw, 0, 0, 28




	EXEC [spCreateDepartment] 'Marketing'
	SET @depId = @@IDENTITY


	SET @firstName = 'Klaus'
	SET @lastName = 'Müller'
	SET @mail = @firstName + '.' + @lastName + '@example.com'
	SET @pw = @lastName + '1'
	EXEC [spSetUser] NULL, @depId, @firstName, @lastName, @mail, @pw, 1, 0, 28

	SET @firstName = 'Anette'
	SET @lastName = 'Schäfer'
	SET @mail = @firstName + '.' + @lastName + '@example.com'
	SET @pw = @lastName + '1'
	EXEC [spSetUser] NULL, @depId, @firstName, @lastName, @mail, @pw, 0, 0, 28

	SET @userId = @@IDENTITY
	EXEC [spCreateVacationRequest] @userId, '2021-01-02 00:00', '2021-01-12 12:00', null, @holidays



	EXEC [spCreateDepartment] 'Entwicklung'
	SET @depId = @@IDENTITY


	SET @firstName = 'Malte'
	SET @lastName = 'Bauer'
	SET @mail = @firstName + '.' + @lastName + '@example.com'
	SET @pw = @lastName + '1'
	EXEC [spSetUser] NULL, @depId, @firstName, @lastName, @mail, @pw, 1, 0, 28

	SET @firstName = 'Rainer'
	SET @lastName = 'Zufall'
	SET @mail = @firstName + '.' + @lastName + '@example.com'
	SET @pw = @lastName + '1'
	EXEC [spSetUser] NULL, @depId, @firstName, @lastName, @mail, @pw, 0, 0, 28

	SET @userId = @@IDENTITY
	EXEC [spCreateVacationRequest] @userId, '2020-02-06 00:00', '2020-02-12 12:00', null, @holidays

	SET @firstName = 'Paul'
	SET @lastName = 'Günter'
	SET @mail = @firstName + '.' + @lastName + '@example.com'
	SET @pw = @lastName + '1'
	EXEC [spSetUser] NULL, @depId, @firstName, @lastName, @mail, @pw, 0, 0, 28

	SET @userId = @@IDENTITY
	EXEC [spCreateVacationRequest] @userId, '2021-05-11 00:00', '2021-05-16 00:00', null, @holidays

	SET @firstName = 'Erika'
	SET @lastName = 'Mustermann'
	SET @mail = @firstName + '.' + @lastName + '@example.com'
	SET @pw = @lastName + '1'
	EXEC [spSetUser] NULL, @depId, @firstName, @lastName, @mail, @pw, 0, 0, 28

	SET @firstName = 'Elise'
	SET @lastName = 'Deutschmann'
	SET @mail = @firstName + '.' + @lastName + '@example.com'
	SET @pw = @lastName + '1'
	EXEC [spSetUser] NULL, @depId, @firstName, @lastName, @mail, @pw, 0, 0, 28

	SET @userId = @@IDENTITY
	EXEC [spCreateVacationRequest] @userId, '2020-04-06 00:00', '2020-04-12 12:00', null, @holidays
	EXEC [spCreateVacationRequest] @userId, '2020-08-06 00:00', '2020-08-12 14:00', 'Test', @holidays
