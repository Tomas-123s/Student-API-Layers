/*
  Creates the StudentsDB schema and stored procedures required by StudentData.cs.
  All rows below are synthetic examples. No production/student records are included.
  Run this script in SQL Server Management Studio or with sqlcmd.
*/
IF DB_ID(N'StudentsDB') IS NULL
    CREATE DATABASE StudentsDB;
GO

USE StudentsDB;
GO

IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Students
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Students PRIMARY KEY,
        Name nvarchar(100) NOT NULL,
        Grade int NOT NULL,
        Age int NULL
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_GetAllStudents
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, Age, Grade FROM dbo.Students;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_GetPassedStudents
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, Age, Grade FROM dbo.Students WHERE Grade >= 50;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_GetAverageGrade
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AVG(CAST(Grade AS float)) FROM dbo.Students;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_GetStudentById
    @StudentId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, Age, Grade FROM dbo.Students WHERE Id = @StudentId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_AddStudent
    @Name nvarchar(100),
    @Grade int,
    @NewStudentId int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Students (Name, Grade) VALUES (@Name, @Grade);
    SET @NewStudentId = CONVERT(int, SCOPE_IDENTITY());
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_UpdateStudent
    @Name nvarchar(100),
    @Grade int,
    @StudentId int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Students SET Name = @Name, Grade = @Grade WHERE Id = @StudentId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_DeleteStudent
    @StudentId int
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Students WHERE Id = @StudentId;
    SELECT @@ROWCOUNT;
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Students)
BEGIN
    INSERT INTO dbo.Students (Name, Grade, Age) VALUES
        (N'Sample Student 1', 88, 20),
        (N'Sample Student 2', 74, 21),
        (N'Sample Student 3', 92, 19),
        (N'Sample Student 4', 45, 22),
        (N'Sample Student 5', 67, 20);
END;
GO
