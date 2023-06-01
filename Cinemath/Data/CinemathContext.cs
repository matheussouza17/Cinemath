using Microsoft.EntityFrameworkCore;
using Cinemath.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinemath.Services;
//roomMovie.Id = (await _context.RoomMovie.MaxAsync(e => (int?)e.Id) ?? 0 + 1) + 1;
namespace Cinemath.Data
{
    public class CinemathContext : DbContext
    {
        public CinemathContext(DbContextOptions<CinemathContext> options)
            : base(options)
        {
        }

        public DbSet<Cinemath.Models.Movies> Movies { get; set; } = default!;

        

        public DbSet<Cinemath.Models.Employee> Employee { get; set; } = default!;
       
        public DbSet<BlogPostsCount> BlogPostCounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<BlogPostsCount>(
                    eb =>
                    {
                        eb.HasNoKey();
                        eb.ToView("View_BlogPostCounts");
                        eb.Property(v => v.BlogName).HasColumnName("Name");
                    });
        }

    }
}
 