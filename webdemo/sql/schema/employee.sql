CREATE TABLE Employee (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(15),
    DateOfBirth DATE,
    HireDate DATE NOT NULL,
    Salary DECIMAL(15, 2) CHECK (Salary > 0),
    DepartmentID INT NOT NULL,
    DesignationID INT NOT NULL,
    Address NVARCHAR(255),
    City NVARCHAR(100),
    State NVARCHAR(100),
    Country NVARCHAR(100),
    PostalCode NVARCHAR(10),
    Status NVARCHAR(10) DEFAULT 'Active' CHECK (Status IN ('Active', 'Inactive')),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Employee_Department FOREIGN KEY (DepartmentID) REFERENCES Department(DepartmentID) ON DELETE CASCADE,
    CONSTRAINT FK_Employee_Designation FOREIGN KEY (DesignationID) REFERENCES Designation(DesignationID) ON DELETE CASCADE
);