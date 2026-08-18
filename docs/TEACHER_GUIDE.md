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
| `TODO-PT-CREATE-01` | `Pages/Students/Create.cshtml.cs` — `ModelState` check | Invalid input redisplays without saving |
| `TODO-PT-CREATE-02` | `Pages/Students/Create.cshtml.cs` — `Add`/`SaveChanges` | Valid Student is saved |
| `TODO-PT-CREATE-03` | `_StudentForm.cshtml` — form groups | All required inputs bind correctly so CREATE can be tested |
| `TODO-PT-UPDATE-01` | `Pages/Students/Edit.cshtml.cs` — `OnGet` | Existing values load |
| `TODO-PT-UPDATE-02` | `Pages/Students/Edit.cshtml.cs` — `ModelState` check | Invalid changes do not save |
| `TODO-PT-UPDATE-03` | `Pages/Students/Edit.cshtml.cs` — find/copy/save | Changes persist |
| `TODO-PT-DELETE-01` | `Pages/Students/Delete.cshtml.cs` — `OnGet` | Confirmation loads without deletion |
| `TODO-PT-DELETE-02` | `Pages/Students/Delete.cshtml.cs` — `OnPost` | Confirmed record is removed |
| `TODO-PT-VALIDATE-02` | `_StudentForm.cshtml` — validation elements | Useful field messages display |

The student repository must contain hints and incomplete behavior only. Do not copy this repository's Git directory, branches, tags, commits, backup files, or completed handlers into it.

## Student laboratory pathway

The student repository separates prerequisite practice, setup, and assessed PT implementation:

- `PREREQUISITE_LABS.md` defines three separate individual practice laboratories plus Guided PT Lab 0 setup. Practice projects remain outside the SimpleSIS repository and do not reveal CRUD answers.
- `README.md` covers cloning/downloading, prerequisites, safe PostgreSQL configuration, the provided migration, first run, and setup troubleshooting.
- `GUIDED_LAB.md` begins only after setup succeeds and is organized as:

```text
READ → CREATE → UPDATE → DELETE → Validation → Final Testing
```

Each lab provides goals, concept reminders, TODO locations, guided tasks, non-solution hints, observable checkpoints, reasoning challenges, and troubleshooting. The guide names the required EF Core operations conceptually but does not include completed handler statements. Students should follow the guided lab first and use `PT_CHECKLIST.md` as the concise completion and scoring reference.

### Complete laboratory schedule

| Primary target | Recovery window | Laboratory | Role and sign-off evidence |
| --- | --- | --- | --- |
| August 20 | Preferably August 24 | Individual Lab 1 — .NET CLI and Project Structure | Prerequisite practice: console project restores, builds, runs; files explained |
| August 26 | August 27 or next PC block | Individual Lab 2 — First Razor Pages Web App | Prerequisite practice: local web app, URL/port, request/response, visible edit |
| September 1 | September 2–3 or next PC block | Guided PT Lab 0 — SimpleSIS Setup | Environment readiness: restore, migration, startup |
| September 2 | September 3 or before PT Lab 1 | Individual Lab 3 — Razor Page and PageModel Flow | Prerequisite practice: `OnGet` property renders through `@Model` |
| September 7 | Guided PT Lab 1 — READ Students | Cumulative PT | Seeds and five columns display |
| September 8 | Guided PT Lab 2 — CREATE a Student | Cumulative PT | Valid create persists after restart |
| September 9 | Guided PT Lab 3 — UPDATE a Student | Cumulative PT | Edited values persist after restart |
| September 9 | Guided PT Lab 4 — DELETE with Confirmation | Cumulative PT | Confirmation is safe; deletion persists |
| September 9 | Guided PT Lab 5 — Validation and Feedback | Cumulative PT | Invalid input is rejected and not saved |
| September 10 | Final PT Completion Lab | Integration | Full integrated matrix and first formal check |
| September 14 | Remediation and clean retest | Repair | Readiness recheck |
| September 15–16 | Individual demonstrations | Authentication of learning | Behavior and code explanation |
| September 17 | Final submission | Completion | Teacher-approved source submission |

All dates are primary targets. For a suspension, online shift without equitable PC access, unfinished prerequisite, or setup failure, move the affected laboratory to its recovery window or next available in-person PC block. Do not compress theory, skip a checkpoint, or require home-PC coding to preserve the target date.

For the announced August 19 suspension, move Formal Lecture Activity 1 and any unfinished Lesson 1.2 work to the next available teaching block. Conduct Individual Lab 1 on August 20 only if Lessons 1.1–1.4, the CLI demonstration, time, and PC access are all sufficient; otherwise use August 24 as the preferred recovery block.

The three prerequisite labs are formative readiness evidence and use disposable practice projects. The five PT labs produce SimpleSIS incrementally. Do not award duplicate full-product credit for prerequisite exercises or for the same PT behavior; the final PT grade also requires integration, persistence, testing, demonstration, and submission.

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
