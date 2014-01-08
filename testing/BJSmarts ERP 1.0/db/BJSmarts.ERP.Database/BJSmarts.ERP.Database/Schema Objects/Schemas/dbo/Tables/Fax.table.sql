CREATE TABLE [dbo].[Fax]
(
	[FaxId] [int] IDENTITY(1,1) NOT NULL,
	[SenderId] int,
	[Recipient] int,
	[Fax Number] nvarchar (30),
	[Subject] nvarchar (30),
	[Description] nvarchar(500),
	[RegardingId] int,
	[OwnerId] int,
	[PriorityId] int,
	[Due] DateTime,
	[Notes] nvarchar (500)
)
