# SimpleSIS — Teacher/PT Reference

SimpleSIS is the complete teacher-reference implementation for the Grade 12 .NET Performance Task. It intentionally demonstrates only beginner-level Student CRUD with ASP.NET Core Razor Pages, Entity Framework Core, Npgsql, and PostgreSQL.

The independent `simple-sis-student-pt` repository uses the same architecture, model, migration, pages, and expected behavior, with selected implementation areas replaced by student TODOs. Its clean Git history remains separate so this completed solution is never exposed.

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

## Project and database setup

### 1. Install the prerequisites

- Git
- .NET 9 SDK
- PostgreSQL, including either pgAdmin or the `psql` command-line tool
- VS Code with the C# Dev Kit extension is recommended but not required

Confirm the command-line tools are available:

```powershell
git --version
dotnet --version
psql --version
```

The school baseline is the .NET 9 SDK. A newer SDK may also build the project when the .NET 9 targeting pack is available. If `psql` is not on `PATH`, PostgreSQL may still be managed through pgAdmin.

### 2. Clone and enter the project

```powershell
git clone https://github.com/paulquimpo-dev/simple-sis.git
cd simple-sis
```

If the repository is already open locally, run the remaining commands from its root folder—the folder containing `SimpleSIS.csproj`.

### 3. Restore the project and EF Core tool

```powershell
dotnet restore
dotnet tool restore
dotnet build
```

The repository-local tool manifest supplies the expected `dotnet-ef` version. Do not create a new migration for initial setup.

### 4. Start PostgreSQL and create the local database

Make sure the PostgreSQL service is running. Create an empty database named `student_sis`, or use the database name assigned by the teacher.

Using pgAdmin:

1. Connect to the local PostgreSQL server.
2. Right-click **Databases** and choose **Create → Database**.
3. Enter `student_sis` as the database name and save.

Using `psql`:

```powershell
psql -U postgres
```

Then run:

```sql
CREATE DATABASE student_sis;
```

Exit with:

```text
\q
```

If the assigned database already exists, do not create it again. The application migration creates the table and seed rows; students and teachers should not manually create the `Students` table.

### 5. Configure the private connection string

Create the ignored development settings file:

```powershell
Copy-Item appsettings.Development.example.json appsettings.Development.json
```

Open `appsettings.Development.json` and set `DefaultConnection` using local values:

```text
Host=localhost;Port=5432;Database=student_sis;Username=postgres;Password=YOUR_LOCAL_PASSWORD
```

- Change the database, username, port, or host if the local setup differs.
- Replace `YOUR_LOCAL_PASSWORD`; never commit or share the real password.
- `appsettings.Development.json` is ignored by Git. Confirm with `git status` before committing.

### 6. Apply the provided migration

```powershell
dotnet ef migrations list
dotnet ef database update
```

The update creates the schema and inserts the two fictional seed records. A successful repeated `database update` is normally safe because EF Core applies only pending migrations.

### 7. Run and verify the web application

```powershell
dotnet run
```

Open the localhost URL printed in the terminal and select **Students**. Confirm that Juan Dela Cruz and Maria Santos appear. Stop the web server with Ctrl+C.

### Setup troubleshooting

- **Database connection refused:** start the PostgreSQL service and verify host and port.
- **Password authentication failed:** verify the PostgreSQL username and local password.
- **Database does not exist:** create the configured database or correct its name.
- **`dotnet ef` is unavailable:** run `dotnet tool restore` from the repository root.
- **Migration fails after partial experimentation:** check the configured database before using the teacher-only clean-reset procedure in `docs/TEACHER_GUIDE.md`.
- **Port already in use:** stop the other local web server or use another URL printed by ASP.NET Core.

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

See [docs/SIMPLESIS_PROJECT_BLUEPRINT.md](docs/SIMPLESIS_PROJECT_BLUEPRINT.md) for the living implementation record, decisions, progress, revisions, and downstream alignment plan.

See [docs/TEACHER_GUIDE.md](docs/TEACHER_GUIDE.md) for checking, persistence tests, planned student TODO mapping, and reset guidance.

## Scope

This reference deliberately excludes authentication, APIs, repositories/services, CQRS, AutoMapper, Docker, dashboards, enrollment, attendance, grades, subjects, and other non-blueprint features.
