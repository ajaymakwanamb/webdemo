CREATE PROCEDURE ListDesignations
    @DesignationID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @DesignationID IS NULL
    BEGIN
        SELECT DesignationID, DesignationName, Description, CreatedAt, UpdatedAt
        FROM Designation;
    END
    ELSE
    BEGIN
        SELECT DesignationID, DesignationName, Description, CreatedAt, UpdatedAt
        FROM Designation
        WHERE DesignationID = @DesignationID;
    END
END;
