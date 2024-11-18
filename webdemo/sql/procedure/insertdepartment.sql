CREATE PROCEDURE InsertDepartment
    @DepartmentName NVARCHAR(100),
    @Description NVARCHAR(255) = NULL,
    @RowsAffected INT OUTPUT
AS
BEGIN
    INSERT INTO Department (DepartmentName, Description, CreatedAt)
    VALUES (@DepartmentName, @Description, GETDATE());

    SET @RowsAffected = @@ROWCOUNT;
END
