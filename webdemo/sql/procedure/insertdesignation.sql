CREATE PROCEDURE InsertDesignation
    @DesignationName NVARCHAR(100),
    @Description NVARCHAR(255) = NULL,
    @RowsAffected INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Designation (
        DesignationName, Description, CreatedAt
    )
    VALUES (
        @DesignationName, @Description, GETDATE()
    );
    SET @RowsAffected = @@ROWCOUNT;
END;
GO
