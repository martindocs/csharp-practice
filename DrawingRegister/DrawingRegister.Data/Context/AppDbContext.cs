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
                    HasOne(p => p.Project)
                    .WithMany(p => p.Drawings)
                    .HasForeignKey(p => p.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict); // block the delete while children exist

                    entity.
                    HasIndex(p => p.DocumentNumber)
                    .IsUnique(); 
                });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasIndex(p => p.Name)
                .IsUnique();
            });
        }
       
    }

    
}
