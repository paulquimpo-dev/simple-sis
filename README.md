# SimpleSIS — Teacher/PT Reference

SimpleSIS is the complete teacher-reference implementation for the Grade 12 .NET Performance Task. It intentionally demonstrates only beginner-level Student CRUD with ASP.NET Core Razor Pages, Entity Framework Core, Npgsql, and PostgreSQL.

The future `simple-sis-student-pt` repository will use the same architecture, model, migration, pages, and expected behavior, with selected implementation areas replaced by student TODOs. It must be created independently with clean Git history so this completed solution is never exposed.

## Technology

- .NET 9 / ASP.NET Core Razor Pages
- Entity Framework Core 9.0.18 design tools
- Npgsql Entity Framework Core provider 9.0.4
- Repository-local `dotnet-ef` 9.0.18
- PostgreSQL

## Student data contract

| Field | Rule |
| --- | --- |
| `Id` | Integer primary key |
| `StudentNumber` | Required |
| `FullName` | Required |
| `GradeLevel` | Integer from 7 through 12 |
| `Section` | Required |
| `Strand` | Required |

`StudentNumber` is intentionally not unique because uniqueness is outside the blueprint's required scope.

## Local setup

Prerequisites: .NET 9 SDK and PostgreSQL.

1. Restore the pinned dependencies:

   ```powershell
   dotnet restore
   dotnet tool restore
   ```

2. Copy `appsettings.Development.example.json` to `appsettings.Development.json`.
3. Replace the placeholder values in `DefaultConnection` with local PostgreSQL values. Never commit this file.
4. Create or select the empty `student_sis` database.
5. Apply the provided migration and run the application:

   ```powershell
   dotnet ef database update
   dotnet run
   ```

6. Open the localhost URL printed in the terminal and select **Students**.

The migration creates the `Students` table and two fictional records:

- `2026-001 | Juan Dela Cruz | 12 | St. Paul | ICT`
- `2026-002 | Maria Santos | 12 | St. Paul | ICT`

## Project structure

```text
Data/AppDbContext.cs          EF Core context and deterministic seed definitions
Models/Student.cs             Student entity and validation attributes
Migrations/                   Provided PostgreSQL migration
Pages/Students/Index.*        READ
Pages/Students/Create.*       CREATE
Pages/Students/Edit.*         UPDATE
Pages/Students/Delete.*       DELETE confirmation and deletion
Pages/Students/_StudentForm   Shared form fields and validation feedback
Program.cs                    Razor Pages and PostgreSQL registration
```

See [docs/TEACHER_GUIDE.md](docs/TEACHER_GUIDE.md) for checking, persistence tests, planned student TODO mapping, and reset guidance.

## Scope

This reference deliberately excludes authentication, APIs, repositories/services, CQRS, AutoMapper, Docker, dashboards, enrollment, attendance, grades, subjects, and other non-blueprint features.
