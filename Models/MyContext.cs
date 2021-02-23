using Microsoft.EntityFrameworkCore;

namespace ProdsAndCats.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Association> Associations { get; set; }
    }
}