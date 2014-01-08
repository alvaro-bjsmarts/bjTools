CREATE TABLE [dbo].[PhoneCall]
(
	[PhoneCallId] [int] IDENTITY(1,1) NOT NULL,
	[SenderId] int,
	[RecipientId] int,
	[PHone Number] nvarchar(30),
	[Subject] nvarchar (30),
	[Description] nvarchar(500),
	[OwnerId] int,
	[Due] DateTime,
	[PriorityId] int,
	[Notes] nvarchar (500)
)
