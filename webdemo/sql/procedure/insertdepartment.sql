CREATE PROCEDURE InsertDepartment
    @DepartmentName NVARCHAR(100),
    @Description NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Department (
        DepartmentName, Description, CreatedAt
    )
    VALUES (
        @DepartmentName, @Description, GETDATE()
    );
END;
GO
