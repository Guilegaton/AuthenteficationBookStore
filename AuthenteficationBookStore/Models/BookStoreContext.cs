using System.Data.Entity;
using BookStore.Models;

namespace AuthenteficationBookStore.Models
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext() : base("BookStoreContext") { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
    }
}
