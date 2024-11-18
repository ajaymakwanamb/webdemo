CREATE PROCEDURE InsertEmployee
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
    @Status NVARCHAR(10) = 'Active',
    @RowsAffected INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Employee (
        FirstName, LastName, Email, PhoneNumber, DateOfBirth, HireDate,
        Salary, DepartmentID, DesignationID, Address, City, State, Country, PostalCode, Status, CreatedAt
    )
    VALUES (
        @FirstName, @LastName, @Email, @PhoneNumber, @DateOfBirth, @HireDate,
        @Salary, @DepartmentID, @DesignationID, @Address, @City, @State, @Country, @PostalCode, @Status, GETDATE()
    );

       SET @RowsAffected = @@ROWCOUNT;
END;
GO
