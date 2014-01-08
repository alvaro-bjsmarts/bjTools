CREATE TABLE [dbo].[Organizations]
(
	[OrganizationId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(25),
	[Description] nvarchar(100),
	[SiteId] nvarchar(100),
	[SiteUrl] nvarchar(100),
	[CurrencyId] int,
	[IndustryId] int,
	[Industry] nvarchar(100),
	[Language] int,
	[Deleted] int
)
