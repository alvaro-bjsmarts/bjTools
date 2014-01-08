CREATE TABLE [dbo].[Companies]
(
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(25),
	[Description] nvarchar(100),
	[LocationId] int, 
	[Sort_Order] int, 
	[Deleted] int	
)
