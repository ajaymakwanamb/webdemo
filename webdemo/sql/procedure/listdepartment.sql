CREATE PROCEDURE ListDepartments
    @DepartmentID INT = NULL 
AS
BEGIN
    SET NOCOUNT ON;

    IF @DepartmentID IS NULL
    BEGIN
        SELECT DepartmentID, DepartmentName, Description, CreatedAt, UpdatedAt
        FROM Department;
    END
    ELSE
    BEGIN
        SELECT DepartmentID, DepartmentName, Description, CreatedAt, UpdatedAt
        FROM Department
        WHERE DepartmentID = @DepartmentID;
    END
END;
