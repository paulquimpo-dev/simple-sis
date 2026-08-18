# SimpleSIS Teacher Guide

## Architecture

```text
Browser
  → Razor Page and PageModel
  → AppDbContext / Entity Framework Core
  → Npgsql
  → PostgreSQL
```

The handlers use straightforward synchronous EF Core operations because `async`/`await` is not a required student prerequisite in the blueprint.

## Expected behavior

- **READ:** `/Students` retrieves and displays every persisted Student.
- **CREATE:** a valid form adds and saves a Student, then returns to the list.
- **UPDATE:** the selected Student loads into the form; valid changes are copied to the tracked record and saved.
- **DELETE:** GET only displays confirmation. The record is removed and saved only by the confirmation POST.
- **Validation:** invalid forms redisplay without calling `SaveChanges` and show field-level feedback.

## Planned student TODO-to-reference map

These stable IDs should be used when the separate student scaffold is independently generated.

| Student TODO | Reference implementation | Observable checkpoint |
| --- | --- | --- |
| `TODO-PT-READ-01` | `Pages/Students/Index.cshtml.cs` — `OnGet` | Seed Students are retrieved |
| `TODO-PT-READ-02` | `Pages/Students/Index.cshtml` — table body | All five visible fields display |
| `TODO-PT-CREATE-01` | `Pages/Students/Create.cshtml.cs` — `OnPost` | Valid Student is saved |
| `TODO-PT-CREATE-02` | `Pages/Students/Create.cshtml` and `_StudentForm.cshtml` | Form binds and displays feedback |
| `TODO-PT-UPDATE-01` | `Pages/Students/Edit.cshtml.cs` — `OnGet` | Existing values load |
| `TODO-PT-UPDATE-02` | `Pages/Students/Edit.cshtml.cs` — `OnPost` | Changes persist |
| `TODO-PT-DELETE-01` | `Pages/Students/Delete.cshtml.cs` — `OnGet` | Confirmation loads without deletion |
| `TODO-PT-DELETE-02` | `Pages/Students/Delete.cshtml.cs` — `OnPost` | Confirmed record is removed |
| `TODO-PT-VALIDATE-01` | Create/Edit `OnPost` handlers | Invalid `ModelState` prevents saving |
| `TODO-PT-VALIDATE-02` | `_StudentForm.cshtml` | Useful messages display |

The student repository must contain hints and incomplete behavior only. Do not copy this repository's Git directory, branches, tags, commits, backup files, or completed handlers into it.

## Required test matrix

- [ ] Application starts.
- [ ] READ displays both seed records.
- [ ] CREATE accepts a valid Student.
- [ ] Restart: the created Student remains.
- [ ] UPDATE changes that Student.
- [ ] Restart: the update remains.
- [ ] Opening DELETE confirmation does not delete the Student.
- [ ] Confirming DELETE removes the Student.
- [ ] Restart: the deleted Student remains absent.
- [ ] Empty Student Number is rejected.
- [ ] Empty Full Name is rejected.
- [ ] Grade Level below 7 or above 12 is rejected.
- [ ] Empty Section is rejected.
- [ ] Empty Strand is rejected.

For each rejected submission, confirm the page displays a useful message and no invalid database row is inserted or updated.

## Persistence test procedure

1. Start the application and create `2026-TEST` with valid field values.
2. Stop the server with Ctrl+C and run `dotnet run` again.
3. Confirm `2026-TEST` is still listed.
4. Edit its Section, stop the server, restart, and confirm the new Section remains.
5. Open its Delete page and confirm the row still exists before submitting.
6. Confirm deletion, stop the server, restart, and confirm the row remains absent.

These checks demonstrate PostgreSQL persistence rather than temporary in-memory storage.

## Common mistakes

- Forgetting to check `ModelState.IsValid` before saving.
- Calling `Add` but not `SaveChanges`.
- Editing only the form object without updating the stored entity.
- Deleting during GET instead of waiting for confirmation POST.
- Omitting `asp-validation-for` feedback from the form.
- Using the placeholder password unchanged.
- Running the app before applying the provided migration.

## Suggested oral questions

1. What job does the PageModel perform?
2. How does the form become a C# `Student` object?
3. Why must invalid `ModelState` return the same page?
4. What does `SaveChanges` do?
5. Why does stopping and restarting the app prove persistence?
6. Why must deletion happen on POST rather than when confirmation is displayed?

## Clean reset and reseed

Use this only against the designated local classroom database. It deletes all data in that database:

```powershell
dotnet ef database drop
dotnet ef database update
```

The second command recreates the schema and deterministic seed rows from the provided migration. Verify the configured database name before dropping it.

## Student work areas

Students should work only in the selected `Pages/Students` TODO locations identified by their scaffold and instructions. `Models/Student.cs`, `Data/AppDbContext.cs`, `Program.cs`, `.config/dotnet-tools.json`, and `Migrations/` are teacher-provided infrastructure.
