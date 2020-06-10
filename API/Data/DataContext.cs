using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data {
    public class DataContext : DbContext {
        public DataContext (DbContextOptions<DataContext> options) : base (options) { }

        public DbSet<Donation> Donations { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}