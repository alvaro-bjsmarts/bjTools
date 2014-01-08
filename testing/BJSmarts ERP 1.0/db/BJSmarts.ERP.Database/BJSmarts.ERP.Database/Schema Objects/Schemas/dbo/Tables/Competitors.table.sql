CREATE TABLE [dbo].[Competitors]
(
	[CompetitorId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(50),
	[Description] nvarchar(250),
	[WebSite] nvarchar (50),
	[StreeAddress1] nvarchar (250),
	[StreeAddress2] nvarchar (250),
	[City] nvarchar (50),
	[State/Province] nvarchar (50),
	[Zip/Postal Code] nvarchar (50),
	[CountryId] int, 
	[Country] nvarchar (50),
	[CurrencyId] int,
	[Currency] nvarchar (50),
	[Notes] nvarchar (500)
)
