
using GeoWalk_mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoWalk_mvc.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
           
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ContentBase> Contents { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Walk> Walks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<InfoContent>().ToTable("Contents");
            builder.Entity<QuestionAnswer>().ToTable("Contents");
            base.OnModelCreating(builder);
        }
    }
}
