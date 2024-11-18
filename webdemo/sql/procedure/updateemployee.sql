CREATE PROCEDURE UpdateEmployee
    @EmployeeID INT,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(15) = NULL,
    @DateOfBirth DATE = NULL,
    @HireDate DATE,
    @Salary DECIMAL(15, 2),
    @DepartmentID INT,
    @DesignationID INT,
    @Address NVARCHAR(255) = NULL,
    @City NVARCHAR(100) = NULL,
    @State NVARCHAR(100) = NULL,
    @Country NVARCHAR(100) = NULL,
    @PostalCode NVARCHAR(10) = NULL,
    @Status NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Employee
    SET
        FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        PhoneNumber = @PhoneNumber,
        DateOfBirth = @DateOfBirth,
        HireDate = @HireDate,
        Salary = @Salary,
        DepartmentID = @DepartmentID,
        DesignationID = @DesignationID,
        Address = @Address,
        City = @City,
        State = @State,
        Country = @Country,
        PostalCode = @PostalCode,
        Status = @Status,
        UpdatedAt = GETDATE()
    WHERE EmployeeID = @EmployeeID;
END;
GO
