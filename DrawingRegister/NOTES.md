## 2026-11-04
- Search functionality
- note: document number may need to be a string, as curently search is only either query(string) or drawing number(int)

## 2026-11-03
- Refactoring drawing index page

## 2026-11-02
- Delete drawing operation

## 2026-10-30
- Edit drawing operation
- note: wrap SaveChanges in try/catch for DbUpdateException, Create and Edit. For edge case when two users saving the same number within milliseconds
```
try { await SaveChangesAsync(); redirect }
catch (DbUpdateException) { AddModelError on DocumentNumber, refill Projects, return View }
	
```

## 2026-10-29
- change JS fetch in the view model for server-side static. Use fetch in view model only for dependent dropdowns
- add attribute values in view model 
- add nullable for Drawing Number and other, so it wont show default values in the form, and also wont show in the error messages. In the DB the they props are still non-nullable.
- AddModelError for DB rules for Drawing Number
- use AnyAsync for other lookups instead of FindAsync as the second will get always by primary key

## 2026-10-26
- Add drawing operation

## 2026-10-22
- write DrawingController to show the list

## 2026-10-20
- create seed class for initial data 

## 2026-10-15: Setup the project
- Install pckg to both Data & Web: 
	- Microsoft.EntityFrameworkCore.Design
	- Microsoft.EntityFrameworkCore.SqlServer
	- Microsoft.EntityFrameworkCore.Tools
- Write DBContext class

```
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
    }
	
```
- add to dbContext public DbSet<> for each Entity class
- write connection string in appsettings.json
```
	 "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=DrawingRegister;Trusted_Connection=True;TrustServerCertificate=True"
    }
```
- register connection string in Program.cs
```
using DrawingRegister.Data.Context;
using Microsoft.EntityFrameworkCore;
...
// Registration for DbContext
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```
- Build Solution (if wont build Migration also won't work)
- Set as Startup Project: Web
- NuGet Console change Target Project: Data 
- Add Migration NameOfMigration
- Update DB

## 2026-10-14: Architecture
- Sketch app idea and the rough functionality
- Entities:  
  - **Project**: holds drawings related to the project, One-to-Many (one project - many drawings)
  - **Drawing**: One-to-Many (one drawing - many revisions) and Many-to-Many (many drawings - many tags)
  - **Revision**: Many-to-One 
  - **Tag**: Many-to-Many	
  - **Status** (Enum): holds status of the drawing
  - **Type** (Enum): holds type of the drawing

## 2026-10-12: Split into Web and Data project
- Entities, dbContext, Migrations live in DrawingRegister.Data - Target project. It holds the DbContext and entities, and it’s where the Migrations folder will be created.
- All the rest: Controllers, Views, JS live in DrawingRegister.Web - Startup project. It has Program.cs, the connection string and the registered context. EF starts this project to build your DbContext.