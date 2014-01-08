CREATE TABLE [dbo].[Campaign]
(
	[CampaignId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(50),
	[Description] nvarchar(250),
	[Total Cost of Campaign] Decimal,
	[Budget Allocated] Decimal,
	[Estimated Revenue] Decimal,
	[Proposed Begin Date] Datetime,
	[Proposed End Date] Datetime,
	[Actual Begin Date] Datetime,
	[Actual End Date] Datetime,
	[CampaignStatusId] int,
	[CampaignStatus] nvarchar (50),
	[CampaignTypeId] int, 
	[CampaignType] nvarchar (50),
	[OrganizationId] int,
	[Organization] nvarchar (50)
)
