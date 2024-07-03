using URLShortener.Models;

namespace URLShortener.Services
{
    public interface IUrlService
    {
        Task<(UrlMap? urlObj, bool isSuccess, string message)> MapUrlAsync(string longUrl);
        (bool isValid, string message) ValidateUrl(string longUrl);
    }
}
