using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BookStore_Final_Project
{
    public class DbBookStore: DbContext
    {
        public DbBookStore() : base("DbBookStore")
        {
            Database.SetInitializer<DbBookStore>(new MyInitializer());
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
