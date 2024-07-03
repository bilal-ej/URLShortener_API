using Microsoft.EntityFrameworkCore;
using URLShortener.Models;

namespace UrlShortener.Data
{
    public class UrlShortenerContext : DbContext
    {
        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : base(options) { }

        public DbSet<UrlMap> UrlMaps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UrlMap>(builder =>
            {
                builder.HasKey(um => um.ShortUrlCode);
                builder.HasIndex(um => um.ShortUrlCode).IsUnique();
                builder.Property(um => um.ShortUrlCode).HasMaxLength(7);
            });
        }
    }
}
