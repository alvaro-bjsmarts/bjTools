﻿CREATE TABLE [dbo].[Contacts]
(
	[ContactsId] [int] IDENTITY(1,1) NOT NULL,
	[First Name] nvarchar(50),
	[Last Name] nvarchar(50),
	[Job Title] nvarchar(50),
	[AccountsId] int,
	[Business Phone] nvarchar(50),
	[Home Phone] nvarchar(50),
	[Mobile Phone] nvarchar(50),
	[Fax] nvarchar(50),
	[Email] nvarchar(50),
	[StreeAddress1] nvarchar (250),
	[StreeAddress2] nvarchar (250),
	[City] nvarchar (50),
	[State/Province] nvarchar (50),
	[Zip/Postal Code] nvarchar (50),
	[CountryId] int, 
	[Country] nvarchar (50),
	[Description] nvarchar(250),
	[Department] nvarchar(50),
	[RoleId] int,
	[Manager] nvarchar(50),
	[Manager Phone] nvarchar(50),
	[GenderId] int,
	[CurrencyId] int,
	[Currency] nvarchar (50),
	[Notes] nvarchar(500),
	[OrganizationId] int,
	[Organization] nvarchar (50),
	[LeadId] int
)
