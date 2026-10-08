# Student API Layers

A .NET Web API organized into three projects:

- `TestAPI`: HTTP API and controllers
- `API Busness Layer`: student business logic
- `API_Data_LAyer`: SQL Server data access

## Requirements

- .NET 10 SDK
- SQL Server with the `StudentsDB` database and stored procedures used by `StudentData`

## Configure the database

Set `STUDENTS_DB_CONNECTION` to your SQL Server connection string before running the API. For example, in PowerShell:

```powershell
$env:STUDENTS_DB_CONNECTION = "Server=.;Database=StudentsDB;Trusted_Connection=True;TrustServerCertificate=True;"
dotnet run --project .\TestAPI\TestAPI.csproj
```

Keep real credentials out of source control. Use a local environment variable or .NET user secrets for development.
