CREATE PROCEDURE ListEmployees
    @EmployeeID INT = NULL 
AS
BEGIN
    SET NOCOUNT ON;

    IF @EmployeeID IS NULL
    BEGIN
        SELECT EmployeeID, FirstName, LastName, Email, PhoneNumber, DepartmentID, DesignationID, Salary, HireDate, CreatedAt, UpdatedAt
        FROM Employee;
    END
    ELSE
    BEGIN
        SELECT EmployeeID, FirstName, LastName, Email, PhoneNumber, DepartmentID, DesignationID, Salary, HireDate, CreatedAt, UpdatedAt
        FROM Employee
        WHERE EmployeeID = @EmployeeID;
    END
END;
