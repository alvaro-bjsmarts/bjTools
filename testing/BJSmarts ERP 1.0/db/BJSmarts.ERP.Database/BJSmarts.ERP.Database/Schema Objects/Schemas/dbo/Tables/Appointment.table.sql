CREATE TABLE [dbo].[Appointment]
(
	[AppointmentId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] nvarchar (30),
	[Location] nvarchar (30),
	[RegardingId] int,
	[Start Time] DateTime,
	[End Time] DateTime,
	[Notes] nvarchar (500)
)
