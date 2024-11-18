CREATE PROCEDURE UpdateDesignation
    @DesignationID INT,
    @DesignationName NVARCHAR(100),
    @Description NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Designation
    SET
        DesignationName = @DesignationName,
        Description = @Description,
        UpdatedAt = GETDATE()
    WHERE DesignationID = @DesignationID;
END;
GO
