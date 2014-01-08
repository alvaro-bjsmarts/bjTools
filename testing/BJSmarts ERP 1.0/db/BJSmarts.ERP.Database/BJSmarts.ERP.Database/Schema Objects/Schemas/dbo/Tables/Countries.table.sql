CREATE TABLE [dbo].[Countries]
(
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(50),
	[Description] nvarchar(50),
	[Sort_Order] int,
	[Language] int,
	[Deleted] int
)
