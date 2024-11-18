CREATE PROCEDURE InsertDesignation
    @DesignationName NVARCHAR(100),
    @Description NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Designation (
        DesignationName, Description, CreatedAt
    )
    VALUES (
        @DesignationName, @Description, GETDATE()
    );
END;
GO
