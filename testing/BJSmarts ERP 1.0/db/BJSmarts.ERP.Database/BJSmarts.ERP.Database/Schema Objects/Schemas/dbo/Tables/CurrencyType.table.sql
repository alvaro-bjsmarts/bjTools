﻿CREATE TABLE [dbo].[CurrencyType]
(
	[CurrencyTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(25),
	[Description] nvarchar(100),
	[Sort_Order] int,
	[Language] int,
	[Deleted] int
)
