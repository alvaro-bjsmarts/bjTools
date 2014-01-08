CREATE TABLE [dbo].[WeekNumber]
(
	[TimeSheetId] [int] IDENTITY(1,1) NOT NULL, 
	[WeekNumber] int, 
	[StartDate] datetime,
	[EndDate] datetime,
	[Year] nvarchar(10), 
	[Language] int,
)
