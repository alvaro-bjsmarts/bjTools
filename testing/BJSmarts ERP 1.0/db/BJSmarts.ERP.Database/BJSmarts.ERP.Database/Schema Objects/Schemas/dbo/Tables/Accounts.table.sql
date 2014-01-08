﻿CREATE TABLE [dbo].[Accounts]
(
	[AccountsId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(50),
	[Description] nvarchar(500),
	[Account Number] nvarchar(50),
	[Main Phone] nvarchar(50),
	[Primary Contact] int,
	[Fax] nvarchar (50),
	[WebSite] nvarchar(50),
	[Email] nvarchar(50),
	[StreeAddress1] nvarchar (250),
	[StreeAddress2] nvarchar (250),
	[City] nvarchar (50),
	[State/Province] nvarchar (50),
	[Zip/Postal Code] nvarchar (50),
	[CountryId] int,
	[Country] nvarchar (50),
	[IndustryId] int,
	[Industry] nvarchar (50),
	[CurrencyId] int,
	[Currency] nvarchar (50),
	[AccountCategoryId] int,
	[AccountCategory] nvarchar (50),
	[Notes] nvarchar (500),
	[OrganizationId] int,
	[Organization] nvarchar (50),
	[LeadId] int
)