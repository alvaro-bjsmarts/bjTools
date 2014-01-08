CREATE TABLE [dbo].[MaritalStatus]
(
	[MaritalStatusId] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(25),
	[Description] nvarchar(100),
	[Language] int,
	[Sort_Order] int
)
