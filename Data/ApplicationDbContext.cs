using Labb_4_My_Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Labb_4_My_Library.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrow> Borrows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship
            modelBuilder.Entity<Borrow>()
                .HasKey(b => new { b.CustomerId, b.BookId });

            modelBuilder.Entity<Borrow>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Borrows)
                .HasForeignKey(b => b.CustomerId);

            modelBuilder.Entity<Borrow>()
                .HasOne(b => b.Book)
                .WithMany(bk => bk.Borrows)
                .HasForeignKey(b => b.BookId);

            //Seed example data
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Alice Johnson", Email = "alice@example.com", PhoneNumber = "123-456-7890" },
                new Customer { Id = 2, Name = "Bob Smith", Email = "bob@example.com", PhoneNumber = "098-765-4321" },
                new Customer { Id = 3, Name = "Carol Williams", Email = "carol@example.com", PhoneNumber = "456-123-7890" },
                new Customer { Id = 4, Name = "David Brown", Email = "david@example.com", PhoneNumber = "321-654-9870" },
                new Customer { Id = 5, Name = "Emily Davis", Email = "emily@example.com", PhoneNumber = "789-012-3456" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "1984", Author = "George Orwell", IsReturned = true },
                new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", IsReturned = false },
                new Book { Id = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", IsReturned = true },
                new Book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen", IsReturned = false },
                new Book { Id = 5, Title = "The Catcher in the Rye", Author = "J.D. Salinger", IsReturned = true },
                new Book { Id = 6, Title = "The Hobbit", Author = "J.R.R. Tolkien", IsReturned = false },
                new Book { Id = 7, Title = "Moby-Dick", Author = "Herman Melville", IsReturned = true },
                new Book { Id = 8, Title = "War and Peace", Author = "Leo Tolstoy", IsReturned = false },
                new Book { Id = 9, Title = "The Odyssey", Author = "Homer", IsReturned = true },
                new Book { Id = 10, Title = "Brave New World", Author = "Aldous Huxley", IsReturned = false }
            );

        }
    }
}

