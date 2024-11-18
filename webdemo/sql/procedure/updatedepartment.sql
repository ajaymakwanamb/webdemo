CREATE PROCEDURE UpdateDepartment
    @DepartmentID INT,
    @DepartmentName NVARCHAR(100),
    @Description NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Department
    SET
        DepartmentName = @DepartmentName,
        Description = @Description,
        UpdatedAt = GETDATE()
    WHERE DepartmentID = @DepartmentID;
END;
GO