using UrlShortener.Repositories;
using URLShortener.Models;

namespace URLShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        public UrlService(IUrlRepository urlRepository)
        {
           _urlRepository = urlRepository;
        }

        public async Task<(UrlMap? urlObj, bool isSuccess, string message)> MapUrlAsync(string longUrl)
        {
            var validationResult = ValidateUrl(longUrl);
            if (validationResult.isValid)
            {
                // generate Unique short code for short Url and map to Url Model
                var shortUrlcode = _urlRepository.GenerateShortUrlCode();
                var urlMap = new UrlMap { LongUrl = longUrl, ShortUrlCode = shortUrlcode };

                // save to db
                await _urlRepository.AddUrlMapAsync(urlMap);
                return (urlMap, true, "success");
            }
            return (null, validationResult.isValid, validationResult.message);
            
        }

        public (bool isValid, string message) ValidateUrl(string longUrl)
        {
            // validate Url
            if (!Uri.TryCreate(longUrl, UriKind.Absolute, out _))
                return (false, "Please enter a valid URL.");
            else
                return (true, "success");
        }

    }
}
