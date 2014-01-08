CREATE TABLE [dbo].[BankAccounts]
(
	[BankAccountId] [int] IDENTITY(1,1) NOT NULL,
	[BankName] nvarchar(50),
	[BankRouter] nvarchar(50),
	[BankAccountNumber] nvarchar(50),
	[BankAccountType] int, 
	[EmployeeId] int,
	[Deleted] int
)
