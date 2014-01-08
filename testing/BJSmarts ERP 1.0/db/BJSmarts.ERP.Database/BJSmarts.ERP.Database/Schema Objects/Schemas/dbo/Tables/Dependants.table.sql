CREATE TABLE [dbo].[Dependants]
(
	[DependantId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] nvarchar(25),
	[LastName] nvarchar(25),
	[Email] nvarchar(25),
	[PhoneNumber] nvarchar(25),
	[DateOfBirth] date,	
	[GenderId] int, 
	[EmployeeId] int,
	[LocationId] int, 
	[Deleted] int	
)
