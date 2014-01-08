CREATE TABLE [dbo].[EmployeeAccounts]
(
	[EmployeeAccountsId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(25),
	[Description] nvarchar(100),
	[Sort_Order] int,
	[Language] int,
	[Organization] int,
	[Deleted] int
)
