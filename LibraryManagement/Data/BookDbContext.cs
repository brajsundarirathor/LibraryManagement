using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<UserMaster> UserMaster { get; set; }

        public DbSet<UserOrder> UserOrders { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserMaster>()
                .Property(user => user.Role).HasDefaultValue("User");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

       
    }


}
