﻿CREATE TABLE [dbo].[LeadSource]
(
	[LeadSourceId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(50),
	[Description] nvarchar(250),
	[Sort_Order] int, 
	[Language] int,
	[Deleted] int
)
