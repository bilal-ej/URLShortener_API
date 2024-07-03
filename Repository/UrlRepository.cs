using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Repositories;
using URLShortener.Models;

namespace URLShortener.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly UrlShortenerContext _context;
        private readonly ILogger<UrlRepository> _logger;

        public UrlRepository(UrlShortenerContext context, ILogger<UrlRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UrlMap?> GetUrlMapAsync(string shortUrlCode)
        {
            // retrieve from db
            var urlMap = await _context.UrlMaps.FirstOrDefaultAsync(um => um.ShortUrlCode == shortUrlCode);
            // log entry
            _logger.LogInformation("Retrieved URL map: {@UrlMap}", urlMap);
            return urlMap;
        }

        public async Task AddUrlMapAsync(UrlMap urlMap)
        {
            // save to db
            _context.UrlMaps.Add(urlMap);
            await _context.SaveChangesAsync();
            // log entry
            _logger.LogInformation("Added URL map: {@UrlMap}", urlMap);
        }

        public string GenerateShortUrlCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 7);
        }
    }
}