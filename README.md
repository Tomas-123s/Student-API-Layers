# Student API Layers

A .NET Web API organized into three projects:

- `TestAPI`: HTTP API and controllers
- `API Busness Layer`: student business logic
- `API_Data_LAyer`: SQL Server data access

## Requirements

- .NET 10 SDK
- SQL Server

## Create the database

Run [`Database/StudentsDB-setup.sql`](Database/StudentsDB-setup.sql) in SQL Server Management Studio. It creates the `StudentsDB` database, the `Students` table, the stored procedures used by the API, and five synthetic sample rows. It contains no real student records.

## Configure the connection

Set `STUDENTS_DB_CONNECTION` to your SQL Server connection string before running the API. For example, in PowerShell:

```powershell
$env:STUDENTS_DB_CONNECTION = "Server=.;Database=StudentsDB;Trusted_Connection=True;TrustServerCertificate=True;"
dotnet run --project .\TestAPI\TestAPI.csproj
```

Keep real credentials out of source control. Use a local environment variable or .NET user secrets for development.

The data layer requires this environment variable and does not contain a database password.

## Run from Visual Studio

Choose **TestAPI - Swagger** from the startup profile dropdown, then press **F5**. This starts the TestAPI project using its HTTP launch profile, sets ASPNETCORE_ENVIRONMENT=Development, and opens /swagger in your browser.

