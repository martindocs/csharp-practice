using DrawingRegister.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingRegister.Data.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        // A DbSet represents the collection of all entities in the context, or that can be queried from the database, of a given type.
        public DbSet<Project> Projects { get; set; }
        public DbSet<Drawing> Drawings { get; set; }
        public DbSet<Revision> Revisions { get; set; }
        public DbSet<Tag> Tags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // means run the parent’s version too, then mine.
            /*
                Also base.OnModelCreating(modelBuilder) before own config , the order matters for a reason: the parent’s setup runs first, then yours is applied on top, so your settings can refine or override anything the parent did. If you called it last, the parent’s configuration could overwrite yours.
                
                Whether that matters depends on what the parent does in that method:

                Plain DbContext: the parent’s method is empty, so calling it or not makes no difference. You’re fine as is.
                IdentityDbContext: the parent’s method configures all the login tables (users, roles, and so on). If you forget the base call there, those tables are misconfigured and you get confusing errors.

                The usual habit is to put the call on the first line anyway. It costs nothing, and the day you switch to a different base class, your override keeps working. Since authentication is out of scope for this project, it’s optional now.
             */
            modelBuilder
                .Entity<Drawing>(entity =>
                {
                    // Value converters allow property values to be converted when reading from or writing to the database. Converting enum values to and from strings in the database.
                    entity.Property(e => e.DrawingStatus)
                    .HasConversion<string>()
                    .HasMaxLength(24);

                    entity.Property(e => e.DrawingType)
                    .HasConversion<string>()
                    .HasMaxLength(24);

                    // Add DeleteBehavior.Restrict to prevent Project to being deleted if contains any drawings
                    entity.
                    HasOne(p => p.Project) // This entity has a relationship with another entity
                    .WithMany(p => p.Drawings) // The other entity can be related to many of these entities.
                    .HasForeignKey(p => p.ProjectId) // This property stores the foreign key.
                    .OnDelete(DeleteBehavior.Restrict); // block the delete while children exist

                    // Drawing table, column DrawingStatus, max length 24.
                    entity.Property(p => p.DocumentNumber)
                    .HasMaxLength(50);

                    // Drawing table, make an index on DocumentNumber, no duplicates allowed.
                    entity
                    .HasIndex(p => p.DocumentNumber)
                    .IsUnique();  // Prevent duplicate values or combinations in that index.

                });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasIndex(p => p.Name)
                .IsUnique();
            });

            modelBuilder.Entity<Revision>(entity =>
            {
                /*
                    HasIndex() - is about how the database organises and searches data in a table. Without an index, the database might have to look through every row until it finds the matching document.
                 
                   EXAMPLES:
                   // Database, create an index on DocumentNumber, and make sure its value is unique across the table.
                   entity
                    .HasIndex                       -  Create an index on this column
                        (p => p.DocumentNumber)     - The column I want the index on is DocumentNumber.
                    .IsUnique();                    - No two records can have the same DocumentNumber.
                 

                   // Create an index using BOTH DrawingId and RevisionLetter together, and make sure that combination is unique.
                   entity
                    .HasIndex(p => new {p.DrawingId, p.RevisionLetter})
                    .IsUnique();


                   * By Default ID of the table has Index, but if we want to search quicker by other columns we need to specyfy which ones. 
                   * And Index is stored inside the database as a separate database structure, not as another column in your table.
                   * The index is persisted on disk as part of the database's storage. The database also uses memory to work with indexes.
                   * The Index is created when we configure in DBContext and when we create and apply a migration 
                   
                    IMPORTNAT - Indexes have a cost!
                    * They consume additional database storage.
                    * When you INSERT a record, the database also needs to update relevant indexes.
                    * When you UPDATE an indexed column, the index might need updating too.
                    * Too many indexes can slow down INSERT, UPDATE and DELETE operations.
                   
                  */
                entity
                .HasIndex(p => new {p.DrawingId, p.RevisionLetter})
                .IsUnique();
            });
        }
       
    }

    
}
