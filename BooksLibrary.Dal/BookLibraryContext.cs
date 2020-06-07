using System.Reflection;
using BooksLibrary.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.Dal
{
    public class BookLibraryContext : DbContext
    {
        public BookLibraryContext()
        {
        }

        public BookLibraryContext(DbContextOptions<BookLibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BooksAuthors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: Move connection string to configuration
            optionsBuilder.UseSqlite(
                "Filename=C:\\Users\\stani\\RiderProjects\\BookLibraryRepository\\BooksLibrary.Dal\\Books.db",
                options => { options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>()
                .HasKey(o => new {o.AuthorId, o.BookId});
        }
    }
}