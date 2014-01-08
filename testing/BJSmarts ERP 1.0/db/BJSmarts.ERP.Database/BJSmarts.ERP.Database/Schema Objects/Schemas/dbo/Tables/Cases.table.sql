CREATE TABLE [dbo].[Cases]
(
	[CasesId] [int] IDENTITY(1,1) NOT NULL,
	[Title] nvarchar (50),
	[Description] nvarchar (500),
	[CustomerId] int,
	[SubjectId] int,
	[CaseTypeId] int,
	[CaseType] nvarchar (50),
	[CaseOriginId] int,
	[SatisfactionId] int,
	[OwnerId] int,
	[CaseStatusId] int,
	[CaseStatus] nvarchar (50),
	[OrganizationId] int,
	[Organization] nvarchar (50),
	[PriorityId] int
)
