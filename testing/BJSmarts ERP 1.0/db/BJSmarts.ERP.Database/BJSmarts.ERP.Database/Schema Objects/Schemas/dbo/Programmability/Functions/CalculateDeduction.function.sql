CREATE FUNCTION [dbo].[CalculateDeduction]
(
	@EmployeeId int, 
	@Percentage decimal (18, 4) 
)
RETURNS INT
AS
BEGIN
	DECLARE @Salary as MONEY;
	
	select 
	    @Salary = Salary 
		from Employees 
		where EmployeeId = @EmployeeId 		

	RETURN @Salary * @Percentage / 100.00
END