CREATE TABLE [dbo].[EmployeePayStub]
(
	[EmployeePayStubId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(25),
	[Description] nvarchar(100),	
	[Deduction Type] nvarchar(25),
	[EmployeeId] int,	
	[Percentage] decimal(18, 2), 	 	
	[Deleted] int
)
