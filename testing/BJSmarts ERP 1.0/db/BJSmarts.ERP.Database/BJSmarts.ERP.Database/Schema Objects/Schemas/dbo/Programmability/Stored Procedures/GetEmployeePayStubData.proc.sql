CREATE PROCEDURE [dbo].[GetEmployeePayStubData]
	
AS
	DECLARE @SQL NVARCHAR(4000)
	DECLARE @cols NVARCHAR(2000) 

	select @cols = STUFF(( SELECT DISTINCT TOP 100 PERCENT   
	'],[' + name   
	FROM employeepaystub    
	ORDER BY '],[' + name 
	FOR XML PATH('')   
	), 1, 2, '') + ']'  from employeepaystub
	
	SET @SQL = 'select * from
	(
	select 
		Employees.EmployeeId,
			Employees.Email,
			Employees.FirstName, 
			Employees.LastName,
			Employees.ResourceType,
			Employees.Department,   
			Employees.Salary,  		    
			EmployeePayStub.Name,
			(select dbo.CalculateDeduction(Employees.EmployeeId, EmployeePayStub.Percentage) ) as [Amount], 
			(select dbo.CalculateActualSalary(Employees.EmployeeId) ) as [Net Pay]
	    	
	from [EmployeePayStub], [Employees] 
	where [EmployeePayStub].EmployeeId = [Employees].EmployeeId 
	)t 
	PIVOT (SUM(Amount) FOR name 
	IN (' + @cols + ' ) ) as pvt'

	EXECUTE(@SQL);
RETURN 0