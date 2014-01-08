﻿CREATE TABLE [dbo].[Quotes]
(
	[QuoteId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(50),
	[Description] nvarchar(50),
	[Potential Customer] int,
	[CurrencyId] int,
	[Currency] nvarchar (50),
	[Price List] decimal, 
	[Detail Amount] decimal,
	[Quota Discount] decimal,
	[Total Tax] decimal, 
	[Total Amount] decimal, 
	[Effective From] DateTime,
	[Effective To] DateTime,
	[Requested Delivery Date] DateTime,
	[Due By] DateTime,
	[Bill StreeAddress1] nvarchar (250),
	[Bill StreeAddress2] nvarchar (250),
	[Bill City] nvarchar (50),
	[Bill State/Province] nvarchar (50),
	[Bill Zip/Postal Code] nvarchar (50),
	[Bill CountryId] int,
	[Bill PHone] nvarchar (50),
	[Bill Address Contact] nvarchar (50),
	[Ship StreeAddress1] nvarchar (250),
	[Ship StreeAddress2] nvarchar (250),
	[Ship City] nvarchar (50),
	[ShipState/Province] nvarchar (50),
	[Ship Zip/Postal Code] nvarchar (50),
	[Ship CountryId] int,
	[Ship Phone] nvarchar (50),
	[Ship Address Contact] nvarchar (50),
	[OwnerId] int,
	[OpportunityId] int,
	[CampaignId] int, 
	[OrganizationId] int,
	[Organization] nvarchar (50),
	[Notes] nvarchar (500)
)