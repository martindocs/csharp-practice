# Drawing Register: Project Spec

A small ASP.NET Core MVC learning project: search, filter, sort, paging and validation.

## Description

A register of CAD drawings and their revisions for an engineering team. You can search, filter, sort and page through it, and the data is protected by validation rules.

It's something an engineer would use, and it's small enough to finish.

## 1. Entities (3 only)

- **Project** (lookup): Id, Code, Name. It feeds the filter dropdown.
- **Drawing** (main entity): Id, DrawingNumber, Title, ProjectId, Status (enum: Draft, InReview, Released, Obsolete), Designer, Weight (number), CreatedDate, ReleasedDate (nullable).
- **DrawingRevision** (child of Drawing): Id, DrawingId, RevisionLetter, Description, Date.

Drawing has a text field, a lookup, an enum, dates and a number, so there's plenty to filter on. Revision gives you one parent-child relationship, a simpler version of what you did with dimensions.

## 2. Carried over from Sling Pattern Library (review, not new)

- CRUD with a ViewModel per screen
- A dropdown fed from the Project table
- `ModelState` checks, `TempData` messages, guard clauses
- Delete as a POST, with the cascade behavior checked before you write it
- Revision list shown on the Drawing details page (add and delete only, no editing yet)

## 3. New: search, filter, sort, paging

- Text search across DrawingNumber, Title and Designer
- Filters: Project, Status, date range
- Sort by clicking a column header
- Paging with page size and a total count
- The state lives in the URL through a GET form, so a page is bookmarkable and Back works
- Build the query with `IQueryable` and run it once at the end
- Seed 150 or more fake drawings so paging means something
- A result ViewModel: items, page, page size, total count, active filters

## 4. New: validation (five kinds, each teaching something different)

1. **Basic rules:** Required, StringLength, and Range on Weight.
2. **Format rule:** a DrawingNumber pattern like `ABC-1234` (regex first, then try a custom attribute).
3. **Uniqueness:** a controller check for a friendly message, plus a unique index in the database. Ask yourself why you need both.
4. **Cross-field rule:** ReleasedDate can't be before CreatedDate, using `IValidatableObject`.
5. **Business rule:** a drawing can't be set to Released unless it has at least one revision. This one needs a database look-up, so think about where it belongs.

Also check what happens with client-side validation switched off, and why the server must still catch it.

## 5. Out of scope (on purpose)

Authentication, file uploads, links to SolidWorks files, a service layer, unit tests, soft delete. Park these in the README as "next version" ideas.

## Suggested build order

1. Create the repo folder, with a README and `.gitignore`.
2. Entities, migration, seed data.
3. Basic list and CRUD for Drawing.
4. Search, then filters, then sort, then paging, one at a time and committing after each.
5. Validation rules, one at a time.
6. Revisions on the details page.
7. Polish and write up what you learned.

## Done means

I can search by text, filter by project and status, sort by column, and page through 150 seeded drawings. I can add and edit drawings with all five kinds of validation working, and I can add revisions to a drawing.

If you want to trim it, drop the date range filter and the business rule in validation #5. That still covers every concept.
