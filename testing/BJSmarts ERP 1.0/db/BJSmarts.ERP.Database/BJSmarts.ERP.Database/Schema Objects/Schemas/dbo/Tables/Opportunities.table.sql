CREATE TABLE [dbo].[Opportunities]
(
	[OpportunitiesId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] nvarchar(50),
	[Description] nvarchar(500),
	[Potential CustomerId] int,
	[ExpectedRevenue] decimal,
	[ExpectedCloseDate] DateTime,
	[Percent] decimal,
	[RatingId] int,
	[OwnerId] int,
	[LeadId] int,
	[CampaignId] int,
	[OrganizationId] int,
	[Organization] nvarchar (50),
	[Deleted] int
)
