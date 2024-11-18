CREATE PROCEDURE UpdateDesignation
    @DesignationID INT,
    @DesignationName NVARCHAR(100),
    @Description NVARCHAR(255) = NULL,
    @RowsAffected INT OUTPUT

AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Designation
    SET
        DesignationName = @DesignationName,
        Description = @Description,
        UpdatedAt = GETDATE()
    WHERE DesignationID = @DesignationID;
    SET @RowsAffected = @@ROWCOUNT;
END;
GO
