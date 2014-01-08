CREATE TABLE [dbo].[Locations]
(
	[LocationId] [int] IDENTITY(1,1) NOT NULL, 
	[StreetAddress1] nvarchar(100),
	[StreetAddress2] nvarchar(100),
	[PostalCode] nvarchar(25),
	[City] nvarchar(25),
	[StateProvince] nvarchar(25),
	[CountryId] int,
	[Deleted] int
)
