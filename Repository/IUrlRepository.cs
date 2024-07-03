using URLShortener.Models;

namespace UrlShortener.Repositories
{
    public interface IUrlRepository
    {
        Task<UrlMap?> GetUrlMapAsync(string shortUrl);
        Task AddUrlMapAsync(UrlMap urlMap);
        string GenerateShortUrlCode();
    }
}