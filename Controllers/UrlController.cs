using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using UrlShortener.Repositories;
using URLShortener.Services;
namespace URLShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IUrlService _urlService;
        private readonly UrlShortenerContext _context;

        public UrlController(IUrlRepository urlRepository, IUrlService urlService, UrlShortenerContext context)
        {
            _urlRepository = urlRepository;
            _urlService = urlService;
            _context = context;
        }

        [HttpGet("shortUrl/{ShortUrl}")]
        public async Task<IActionResult> Get(string shortUrl)
        {
            // retrieve orginal URL with unique short URL code
            var urlMap = await _urlRepository.GetUrlMapAsync(shortUrl);

            if (urlMap == null)
            {
                return NotFound();
            }

            return Redirect(urlMap.LongUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string longUrl)
        {
            var response = await _urlService.MapUrlAsync(longUrl);
            if(response.isSuccess)
            return Ok(new { shortUrl = response.urlObj.ShortUrlCode });
            return BadRequest(response.message);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var urlMaps = _context.UrlMaps.ToList();
            return Ok(urlMaps);
        }
    }
}