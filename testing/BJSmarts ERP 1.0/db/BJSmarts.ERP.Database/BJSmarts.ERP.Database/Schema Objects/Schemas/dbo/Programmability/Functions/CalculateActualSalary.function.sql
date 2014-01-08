CREATE FUNCTION [dbo].[CalculateActualSalary]
(
	@EmployeeId int	
)
RETURNS MONEY
AS
BEGIN
	
	DECLARE @Salary as MONEY;
	DECLARE @Deductions as MONEY;

	select 	    
		@Salary = SUM(Salary) 
		from Employees 
		where Employees.EmployeeId = @EmployeeId

	select 
		@Deductions = SUM(Percentage) 
		from EmployeePayStub 
		where EmployeePayStub.EmployeeId = @EmployeeId	
	
	RETURN @Salary - ( ( @Salary * @Deductions ) / 100 )

END