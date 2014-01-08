CREATE TABLE [dbo].[Services]
(
	[ServicesId] [int] IDENTITY(1,1) NOT NULL,
	[Service Name] nvarchar(50),
	[Service Description] nvarchar(50),	
	[OrganizationId] int,
	[Organization] nvarchar (50),
	[Notes] nvarchar(250)
)
