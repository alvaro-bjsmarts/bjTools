CREATE TABLE [dbo].[Task]
(
	[TaskId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] nvarchar (500),
	[RegardingId] int,
	[OwnerId] int,
	[Due] DateTime,
	[PriorityId] int,
	[Notes] nvarchar (500)
)
