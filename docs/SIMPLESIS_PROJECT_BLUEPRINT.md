# SimpleSIS Living Project Blueprint

**Document role:** Repository-specific source of truth and change record  
**Project:** SimpleSIS  
**Repository:** `https://github.com/paulquimpo-dev/simple-sis`  
**Authoritative implementation branch:** `main`  
**Document version:** 1.8
**Last updated:** August 18, 2026  
**Current implementation baseline:** commit `1f0e136`

## 1. Purpose

This living blueprint tracks the actual SimpleSIS implementation as it is built, tested, revised, and approved. It records project-specific decisions and changes that may be more detailed than, or not yet reflected in, the Term 1 Second-Half Master Blueprint.

This document will later provide the implementation evidence needed to align three downstream bodies of work:

1. the Performance Task sections of the master teaching blueprint;
2. the future `portfolio` branch of this repository; and
3. the independent `simple-sis-student-pt` student repository.

This file must be updated whenever a material SimpleSIS behavior, dependency, schema rule, page, test, repository decision, or downstream requirement changes.

## 2. Source-of-truth hierarchy

Use the following order when making SimpleSIS implementation decisions:

1. The teacher's latest explicit project decision.
2. The code, migration, and verified behavior on `SimpleSIS/main`.
3. This living SimpleSIS blueprint.
4. The v16.2 Term 1 Second-Half Master Blueprint.
5. Older planning documents and historical blueprint versions.

The curriculum master blueprint remains authoritative for teaching goals, assessment boundaries, and pedagogy. This file controls the concrete SimpleSIS implementation record. If the two documents conflict, record the conflict in Section 13 before changing either document.

## 3. Repository model

```text
SimpleSIS repository
├── main
│   └── Complete, simple Teacher/PT Reference
└── portfolio
    └── Future portfolio enhancements created only after main approval

Separate future repository
└── simple-sis-student-pt
    └── Student scaffold with clean, independent Git history
```

### `main` rules

- Remains the authoritative teacher/PT reference.
- Must be complete, readable, and suitable for Grade 12 instruction.
- Must remain intentionally small and aligned with the assessed concepts.
- Must not acquire portfolio-only features or unnecessary architecture.

### `portfolio` rules

- Do not create or develop it until `main` is complete, stabilized, tested, documented, and explicitly approved.
- Branch it from the approved `main` baseline.
- Track every enhancement and its relationship to the reference implementation in this document before or with its implementation.

### Student-repository rules

- Repository name: `simple-sis-student-pt`.
- It must represent the same application contract and project structure as the approved teacher version.
- It must be created independently from the specification with clean Git history.
- Never copy the teacher repository's `.git` directory, commits, branches, tags, patches, backups, commented answers, or hidden solutions.
- Student TODOs must map to the completed teacher implementation and observable tests documented here.

## 4. Product scope

SimpleSIS is an ASP.NET Core Razor Pages web application demonstrating one-entity Student CRUD with real PostgreSQL persistence.

### Required features

- Home page and Students navigation
- Student list (READ)
- Add Student (CREATE)
- Edit Student (UPDATE)
- Delete Student with a separate confirmation step (DELETE)
- Server-side and client-side validation feedback
- PostgreSQL persistence across application restarts
- Deterministic fictional seed data
- Safe local credential configuration

### Explicitly excluded from `main`

- Authentication, authorization, roles, or user accounts
- APIs or JavaScript-heavy SPA frameworks
- Repository pattern, service layer, CQRS, MediatR, or AutoMapper
- Docker or cloud deployment requirements
- Dashboard analytics, search, filtering, or pagination
- Enrollment, attendance, grades, subjects, schedules, or LMS features
- Student Number uniqueness unless later explicitly taught and approved

Items such as dashboard improvements, search/filtering, pagination, screenshots, and significant UI/UX enhancement are possible `portfolio` candidates, not current `main` requirements.

## 5. Fixed technical baseline

| Component | Current decision |
| --- | --- |
| Application type | ASP.NET Core Razor Pages web app |
| Project/assembly | `SimpleSIS` |
| Target framework | `.NET 9` / `net9.0` |
| EF Core design package | `Microsoft.EntityFrameworkCore.Design` 9.0.18 |
| PostgreSQL EF provider | `Npgsql.EntityFrameworkCore.PostgreSQL` 9.0.4 |
| Local EF tool | `dotnet-ef` 9.0.18 |
| Database | PostgreSQL |
| Default local database | `student_sis` |
| UI baseline | Default Razor Pages Bootstrap assets |
| Handler style | Direct, synchronous, beginner-readable EF Core |

Do not silently upgrade the target framework or package major versions. Any version change requires classroom-environment verification, migration review, a completed regression matrix, and an entry in Sections 12 and 14.

## 6. Student entity contract

```text
Student
├── Id             int     primary key
├── StudentNumber  string  required
├── FullName       string  required
├── GradeLevel     int     required; inclusive range 7–12
├── Section        string  required
└── Strand         string  required
```

Current validation messages:

- `Student Number is required.`
- `Full Name is required.`
- `Grade Level must be from 7 to 12.`
- `Section is required.`
- `Strand is required.`

`Email` is not part of the contract. `StudentNumber` does not have a unique database index in the reference implementation.

## 7. Persistence and seed baseline

Required architecture:

```text
Browser
  → Razor Page
  → PageModel
  → AppDbContext / EF Core
  → Npgsql
  → PostgreSQL
```

The provided `InitialCreate` migration creates the `Students` table and inserts two deterministic fictional records:

| Student Number | Full Name | Grade | Section | Strand |
| --- | --- | ---: | --- | --- |
| 2026-001 | Juan Dela Cruz | 12 | St. Paul | ICT |
| 2026-002 | Maria Santos | 12 | St. Paul | ICT |

Students apply the provided migration using `dotnet ef database update`. They do not design the schema, write raw SQL, create `AppDbContext`, configure Npgsql, or author the initial migration.

## 8. Current implementation map

| Responsibility | File |
| --- | --- |
| Package and framework pins | `SimpleSIS.csproj` |
| Local EF tool pin | `.config/dotnet-tools.json` |
| PostgreSQL registration | `Program.cs` |
| Student entity and validation | `Models/Student.cs` |
| DbContext and seed configuration | `Data/AppDbContext.cs` |
| Provided database schema | `Migrations/` |
| READ | `Pages/Students/Index.cshtml` and `.cshtml.cs` |
| CREATE | `Pages/Students/Create.cshtml` and `.cshtml.cs` |
| UPDATE | `Pages/Students/Edit.cshtml` and `.cshtml.cs` |
| DELETE | `Pages/Students/Delete.cshtml` and `.cshtml.cs` |
| Shared Student form | `Pages/Students/_StudentForm.cshtml` |
| Main navigation | `Pages/Shared/_Layout.cshtml` |
| Safe credential example | `appsettings.Development.example.json` |
| Teacher setup overview | `README.md` |
| Teacher assessment guidance | `docs/TEACHER_GUIDE.md` |

### CRUD behavior

- READ uses `context.Students.ToList()`.
- CREATE checks `ModelState.IsValid`, calls `Add` and `SaveChanges`, then redirects to the list.
- UPDATE loads the stored entity with `Find`, copies the five permitted fields, calls `SaveChanges`, then redirects.
- DELETE GET loads and displays the selected Student without deleting it.
- DELETE POST loads the record with `Find`, calls `Remove` and `SaveChanges`, then redirects.
- Missing IDs return HTTP 404 through `NotFound()`.

## 9. Safe configuration workflow

The committed repository contains only `appsettings.Development.example.json` with a placeholder connection string. The real `appsettings.Development.json` is ignored by Git.

Standard setup:

```powershell
dotnet restore
dotnet tool restore
# Copy the development settings example and enter local PostgreSQL values.
dotnet ef database update
dotnet run
```

Never place a real password in this document, source code, committed settings, test output, patch, issue, or screenshot.

## 10. Required regression matrix

Run this matrix after every material CRUD, model, validation, migration, package, or configuration change.

- [x] Application builds with zero warnings.
- [x] Application starts as a browser-based Razor Pages web app.
- [x] Provided migration applies to PostgreSQL.
- [x] READ displays both seed records.
- [x] CREATE accepts a valid Student.
- [x] Restart: the created Student remains.
- [x] UPDATE loads the selected Student and saves changes.
- [x] Restart: the update remains.
- [x] Opening DELETE confirmation does not delete the Student.
- [x] Confirmed DELETE removes the Student.
- [x] Restart: the deleted Student remains absent.
- [x] Empty StudentNumber is rejected.
- [x] Empty FullName is rejected.
- [x] GradeLevel below 7 or above 12 is rejected.
- [x] Empty Section is rejected.
- [x] Empty Strand is rejected.
- [x] Invalid submissions are not saved.
- [x] Real credentials are absent from committed files.
- [x] Local credential settings and generated build folders are ignored.
- [x] EF migration snapshot matches the current model.

Last complete matrix run: August 18, 2026, against local PostgreSQL 18.

## 11. Current progress and readiness

| Phase | Status | Evidence |
| --- | --- | --- |
| Git repository and `main` | Complete | GitHub remote and tracked `main` baseline |
| Razor Pages baseline | Complete | Build and browser startup verified |
| EF Core/Npgsql setup | Complete | Pinned packages and local tool manifest |
| Student model/validation | Complete | Model attributes and invalid-form tests |
| Migration and seed data | Complete | Migration applied; both seeds verified |
| READ | Complete | Seed list verified through HTTP |
| CREATE | Complete | Save and restart persistence verified |
| UPDATE | Complete | Save and restart persistence verified |
| DELETE | Complete | Confirmation and restart absence verified |
| Documentation | Complete baseline | README and teacher guide present |
| Final teacher review/approval | Complete for student-scaffold baseline | Teacher directed creation of student version |
| `portfolio` branch | Not started | Intentionally deferred |
| `simple-sis-student-pt` | Published with guided lab | Independent history, build/startup/TODO/leakage audits passed; guided lab aligned to all TODO IDs |

## 12. Decision log

| Date | Decision | Reason and impact |
| --- | --- | --- |
| 2026-08-18 | Use `SimpleSIS/main` as the teacher/PT reference. | Keeps the instructional answer implementation authoritative and simple. |
| 2026-08-18 | Reserve `portfolio` for later enhancements. | Prevents portfolio work from changing the assessed reference baseline. |
| 2026-08-18 | Create `simple-sis-student-pt` independently only after teacher approval. | Prevents solution leakage while preserving application alignment. |
| 2026-08-18 | Target .NET 9 and pin EF/tool versions. | Matches the verified development environment and avoids accidental upgrades. |
| 2026-08-18 | Use PostgreSQL through Npgsql only. | Meets the PT's real-persistence requirement. |
| 2026-08-18 | Validate GradeLevel from 7 through 12. | Resolves the range left open in earlier planning text. |
| 2026-08-18 | Prefer synchronous direct EF Core handlers. | Avoids making `async`/`await` or architectural abstractions hidden prerequisites. |
| 2026-08-18 | Share Student form markup through `_StudentForm.cshtml`. | Keeps Create/Edit consistent while remaining easy to explain. |
| 2026-08-18 | Freeze the current `main` behavior as the student-scaffold baseline before portfolio work. | Ensures students receive the same architecture and expected output as the completed teacher reference. |
| 2026-08-18 | Use stable TODO IDs across READ, CREATE, UPDATE, DELETE, and validation. | Makes checkpoints, teacher answers, tests, and rubric categories traceable. |
| 2026-08-18 | Provide a progressive guided lab without completed solution statements. | Gives beginners enough structure to proceed while preserving reasoning, implementation, persistence, and explanation challenges. |
| 2026-08-18 | Make the repository Guided Lab the cumulative formal PT laboratory sequence. | Eliminates unrelated lab products: READ is scheduled September 7, CREATE September 8, UPDATE/DELETE/Validation September 9, and final integration September 10. |
| 2026-08-18 | Require a Final PT Completion Lab after the five individual labs. | Completing component labs builds the application, while integration, persistence, testing, teacher checking, demonstration, and submission establish final PT completion. |

## 13. Open decisions and alignment issues

- Decide whether the unused default Privacy page should remain as harmless template content or be removed before freezing `main`.
- Before creating `portfolio`, define exactly which enhancements belong there and which must remain absent from the student PT.
- After `main` approval, compare this document against the master blueprint and update only the PT sections affected by the completed implementation.

## 14. Change log

Every material project revision must add an entry. Use one row per cohesive change and reference the related commit after it exists.

| Date | Blueprint version | Branch/commit | Change | Tests rerun | Downstream impact |
| --- | --- | --- | --- | --- | --- |
| 2026-08-18 | 1.0 | `main` / `eb446ce` | Recorded the completed teacher-reference baseline, exact technical choices, CRUD behavior, persistence evidence, and downstream repository rules. | Full matrix already passed | Establishes alignment source for master PT, portfolio, and student scaffold |
| 2026-08-18 | 1.1 | Student scaffold `f315fd7`; related teacher documentation update | Created the independent student scaffold with provided infrastructure, stable TODO gaps, student README/checklist/rubric, one clean commit, and no teacher Git history. | Restore/build zero warnings; migration compatibility; starter HTTP startup; TODO and leakage audits | Student repository is ready for GitHub publication and remote clean-clone verification |
| 2026-08-18 | 1.2 | Student remote `paulquimpo-dev/simple-sis-student-pt` / `f315fd7` | Published the student repository and audited a fresh remote clone. | One commit, one source branch, zero tags, 28 tracked TODO references, no teacher guide/living blueprint, restore and build with zero warnings | Student scaffold is technically ready for teacher student-view review |
| 2026-08-18 | 1.3 | Student remote commit `2ab19e7` | Added `GUIDED_LAB.md` and linked it from the student README. The lab covers setup, CRUD, validation, restart persistence, troubleshooting, reflection, and optional challenges without completed handler code. | Zero-warning build; all 11 code TODO IDs represented; local links verified; completed-handler leakage pattern scan passed | Student instructions now support a structured lab sequence while retaining assessed challenge |
| 2026-08-18 | 1.4 | Student remote commit `ecf5813` | Separated repository/environment setup from implementation work: README owns clone through first run; the guided lab begins at READ and continues through final testing. | Zero-warning build; all 11 code TODO IDs remain aligned; documentation links verified | Removes duplicated instructions and clearly distinguishes guided prerequisites from assessed completion work |
| 2026-08-18 | 1.5 | Student remote commit `82c9d85`; coordinated teacher/master revision | Reframed the student guide as five dated Guided PT Labs plus a Final PT Completion Lab and aligned the master calendar, teacher checkpoints, assessment policy, remediation, demonstrations, and submission sequence. | Five-pass cross-artifact audit passed; teacher/student builds zero warnings | Guided labs now cumulatively produce the PT without duplicating unrelated formal laboratory products |
| 2026-08-18 | 1.6 | Student remote commit `0d8d819`; coordinated teacher/master documentation revision | Moved completion of the four missing shared form controls from the later Validation lab to CREATE as `TODO-PT-CREATE-03`; Validation retains message rendering and invalid-input proof as `TODO-PT-VALIDATE-02`. | Teacher and student builds: zero warnings/errors; browser CRUD, five-message server validation, 404 handling, and create/update/delete restart persistence passed | Students can now complete and test CREATE in scheduled order without skipping ahead to Lab 5 |
| 2026-08-18 | 1.7 | Student remote commit `d1d0433`; coordinated teacher/master documentation revision | Converted the earlier guided practice into three named prerequisite individual laboratories on August 20, August 26, and September 2; named September 1 Guided PT Lab 0 setup; preserved September 7–10 as cumulative CRUD implementation. | Five-pass laboratory-pathway audit passed; both projects build with zero warnings/errors | Students now have a continuous hands-on laboratory pathway before formal PT coding without being asked to use untaught CRUD concepts |
| 2026-08-18 | 1.8 | Student remote commit `8118d60`; coordinated teacher/master documentation revision | Replaced fixed laboratory assumptions with primary targets, readiness gates, and recovery windows; recorded the announced August 19 suspension and protected students from forced home-PC catch-up. | Suspension-path terminology, links, TODO boundaries, and both zero-warning/error builds passed | Missed classes roll forward without skipping theory, losing lab eligibility, or destabilizing later PT checkpoints |

### Required entry format for future changes

```text
Date:
Blueprint version:
Branch/commit:
Change:
Reason:
Files affected:
Tests rerun and results:
Database/migration impact:
Student scaffold impact:
Portfolio impact:
Master blueprint impact:
```

## 15. Workflow for every future SimpleSIS revision

1. Record the proposed change and reason in Section 13 or the decision log.
2. Confirm whether it belongs on `main` or only on the future `portfolio` branch.
3. Implement the smallest beginner-readable change.
4. Update model/migration documentation when persistence changes.
5. Run the proportionate regression tests; run the complete matrix for material changes.
6. Update the implementation map, progress table, decisions, and change log in this file.
7. Commit the code and this blueprint together when they describe the same revision.
8. Record downstream effects on the master PT, portfolio plan, and student scaffold.

## 16. Main-completion alignment procedure

After the teacher declares `SimpleSIS/main` complete:

### Update the master blueprint

- Compare its PT entity, validation, packages, repository names, setup, migration, seed, page structure, CRUD behavior, and test matrix against this file.
- Replace outdated choices with the approved implementation baseline.
- Preserve curriculum and pedagogy that are not changed by implementation evidence.

### Plan the `portfolio` branch

- Branch from the exact approved `main` commit.
- List each enhancement and confirm it is portfolio-only.
- Keep the core Student contract and PostgreSQL behavior compatible unless an intentional portfolio migration is documented.
- Track portfolio-specific tests and screenshots without weakening the reference branch.

### Generate `simple-sis-student-pt`

- Build it independently from this specification and the approved master PT plan.
- Match the teacher version's target framework, packages, Student contract, migration, seed records, filenames, and expected outputs.
- Replace only approved student work areas with stable TODO IDs and conceptual hints.
- Ensure incomplete tasks cause incomplete behavior rather than unrelated build failures when practical.
- Clean-clone and follow the student README exactly.
- Audit every commit, branch, tag, file, comment, patch, backup, and generated artifact for solution leakage.
- Confirm TODO IDs map to the teacher guide, checklist, rubric, checkpoints, and observable tests.

## 17. Completion gates

### Freeze `main` only when

- [x] Teacher explicitly approves the implementation for student-scaffold generation.
- [x] Full CRUD/validation/persistence matrix passes.
- [x] Documentation matches code and migration.
- [x] No real credentials are committed.
- [x] Scope remains Grade-12 appropriate.
- [x] Remaining non-blocking `main` decisions are deliberately deferred.

### Begin `portfolio` only when

- [ ] `main` is frozen at a recorded commit.
- [ ] Portfolio goals and exclusions are documented.
- [ ] The branch is created from that exact baseline.

### Publish the student repository only when

- [x] The approved teacher baseline is recorded.
- [x] Student TODO/checklist/rubric mapping is complete.
- [x] Guided lab stages and challenge checkpoints align with every code TODO ID.
- [x] Guided lab dates align with the master September 7–10 development schedule.
- [x] Remote clean-clone restore and zero-warning build pass.
- [x] Provided migration compatibility and seed infrastructure pass.
- [x] Local Git-history and file-content leakage audits pass.
- [ ] Teacher performs a final student-view review before classroom distribution.

## 18. Five-pass cumulative-PT alignment audit

Completed August 18, 2026, before publishing blueprint version 1.5.

| Pass | Focus | Result |
| --- | --- | --- |
| 1 | Dates, sequence, and exact lab names across master calendar, README, Guided Lab, checklist, and teacher guide | Passed after standardizing README names to `Guided PT Lab 1–5` |
| 2 | Exact traceability of all code TODO IDs through Guided Lab, checklist, teacher answer map, and master blueprint | Passed: all 11 IDs match and each assigned code marker is unique |
| 3 | Assessment semantics, obsolete standalone labs, implementation start date, cumulative-credit policy, and duplicate grading prevention | Passed: old Course Information/Feedback formal products and September 8-only start conflict removed |
| 4 | Teacher/student builds, migration consistency, repository separation, credential safety, teacher-file leakage, backup/patch leakage, and solution statements in student guidance | Passed after placing repositories in independent directories; both builds produce zero warnings |
| 5 | README-versus-guide responsibility, technical pins, entity contract, dates, links, terminology, and final Git diffs | Passed after audit-path trust and whitespace cleanup |

Audit conclusion: README owns clone through first run; five scheduled Guided PT Labs cumulatively build the application; the Final PT Completion Lab establishes integrated functional readiness; remediation, individual demonstration, and submission complete the PT assessment. A subsequent student-view rehearsal found and corrected one ordering defect: the complete form is now built during CREATE, before its required persistence checkpoint.

## 19. Prerequisite laboratory pathway audit

Completed August 18, 2026, for blueprint version 1.7.

| Pass | Focus | Result |
| --- | --- | --- |
| 1 | Chronological sequence and 2026 weekdays | Passed: August 20, August 26, September 1, September 2, and September 7–17 are ordered correctly |
| 2 | Exact laboratory purpose across master, teacher guide, student README, prerequisite guide, Guided Lab, and checklist | Passed |
| 3 | Prerequisite boundaries and solution leakage | Passed: practice uses separate console/Razor projects and does not complete Student CRUD TODOs |
| 4 | Links, TODO traceability, Markdown whitespace, and repository separation | Passed: links resolve and all 11 PT TODO IDs remain aligned |
| 5 | Technical regression | Passed: teacher and student projects build with zero warnings and zero errors |

Audit conclusion: students receive meaningful individual laboratory work before September 7, Guided PT Lab 0 prepares SimpleSIS on September 1, and the cumulative CRUD implementation remains scheduled only after its prerequisite lessons.

## 20. Suspension and recovery rule

Laboratory dates are pacing targets, not unconditional deadlines. A laboratory proceeds only when its prerequisite instruction is complete and equitable school-PC access is available. Suspended or online-only PC blocks move to the documented recovery window or next available in-person block. No student is required to perform PC-dependent catch-up at home merely to protect the calendar.

The August 19, 2026 suspension moves unfinished Lesson 1.2 and Formal Lecture Activity 1 forward. Individual Lab 1 remains possible on August 20 only if the lesson gate, demonstration, time, and PC gate are satisfied; otherwise August 24 is the preferred recovery target.
