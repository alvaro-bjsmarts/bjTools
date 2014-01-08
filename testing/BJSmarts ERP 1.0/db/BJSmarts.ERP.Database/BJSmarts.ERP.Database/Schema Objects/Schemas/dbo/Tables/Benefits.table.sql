CREATE TABLE [dbo].[Benefits]
(
	[BenefitsId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(25),
	[Description] nvarchar(100),
	[AccruedHours] decimal,
	[Sort_Order] int, 
	[EmployeeId] int, 
	[Language] int,
	[Deleted] int
)
