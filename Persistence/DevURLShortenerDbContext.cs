using DevUrlShortener.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevUrlShortener.Persistence
{
    public class DevURLShortenerDbContext : DbContext
    {
        private int _currentIndex = 1;
        public DevURLShortenerDbContext(DbContextOptions<DevURLShortenerDbContext> options) : base(options)
        {
            
        }

        public List<ShortenedCustomLink> Links { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder) 
        {
            builder.Entity<ShortenedCustomLink>( e => {
                e.HasKey(l => l.Id);
            });
        }

    }
}