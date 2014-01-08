CREATE TABLE [dbo].[Departments]
(
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(50),
	[Description] nvarchar(50),
	[ManagerId] int,
	[StreeAddress1] nvarchar (250),
	[StreeAddress2] nvarchar (250),
	[City] nvarchar (50),
	[State/Province] nvarchar (50),
	[Zip/Postal Code] nvarchar (50),
	[Sort_Order] int,
	[OrganizationId] int,
	[Organization] nvarchar (50),
	[Deleted] int
)
