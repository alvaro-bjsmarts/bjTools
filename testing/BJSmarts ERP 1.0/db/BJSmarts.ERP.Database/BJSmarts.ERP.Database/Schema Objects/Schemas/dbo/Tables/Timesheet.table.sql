CREATE TABLE [dbo].[Timesheet]
(
	[TimeSheetId] [int] IDENTITY(1,1) NOT NULL, 
	[EmployeeAccount] nvarchar(50),
	[EmployeeTimeAccount] nvarchar(50),
	[WeekNumber] int,
	[day1] int,
	[day1Time] datetime,
	[day2] int,
	[day2Time] datetime,
	[day3] int,
	[day3Time] datetime,
	[day4] int,
	[day4Time] datetime,
	[day5] int,
	[day5Time] datetime,
	[day6] int,
	[day6Time] datetime,
	[day7] int,
	[day7Time] datetime
)
