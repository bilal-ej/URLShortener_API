using NSubstitute;
using Assert = Xunit.Assert;
using UrlShortener.Repositories;
using URLShortener.Services;
using URLShortener.Models;
using Xunit;

namespace URLShortener.UnitTests
{
    public class UrlServiceTests
    {
        private readonly IUrlService _sut;
        private readonly IUrlRepository _urlRepository = Substitute.For<IUrlRepository>();

        public UrlServiceTests() 
        {
            _sut =  new UrlService(_urlRepository);
        }

        [Fact]
        public async Task MapUrlAsync_ValidUrl_ShouldReturnSuccess()
        {
            // Arrange
            string longUrl = "https://www.example.com";
            string shortUrlCode = "123abcd";
            var urlMap = new UrlMap { LongUrl = longUrl, ShortUrlCode = shortUrlCode };

            _urlRepository.GenerateShortUrlCode().Returns(shortUrlCode);
            _urlRepository.AddUrlMapAsync(Arg.Any<UrlMap>()).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.MapUrlAsync(longUrl);

            // Assert
            Assert.True(result.isSuccess);
            Assert.Equal("success", result.message);
            Assert.Equal(longUrl, result.urlObj.LongUrl);
            Assert.Equal(shortUrlCode, result.urlObj.ShortUrlCode);
            Assert.NotNull(result.urlObj);

            await _urlRepository.Received(1).AddUrlMapAsync(Arg.Is<UrlMap>(u => u.LongUrl == longUrl && u.ShortUrlCode == shortUrlCode));
        }

        [Fact]
        public async Task MapUrlAsync_InvalidUrl_ShouldReturnErrorMessage()
        {
            // Arrange
            string invalidUrl = "invalid-url";

            // Act
            var result = await _sut.MapUrlAsync(invalidUrl);

            // Assert
            Assert.False(result.isSuccess);
            Assert.Equal("Please enter a valid URL.", result.message);
            Assert.Null(result.urlObj);

            await _urlRepository.DidNotReceive().AddUrlMapAsync(Arg.Any<UrlMap>());
        }

        [Fact]
        public void ValidateUrl_ValidUrl_ShouldReturnValid()
        {
            // Arrange
            string validUrl = "https://www.example.com";

            // Act
            var result = _sut.ValidateUrl(validUrl);

            // Assert
            Assert.True(result.isValid);
            Assert.Equal("success", result.message);
        }

        [Fact]
        public void ValidateUrl_InvalidUrl_ShouldReturnInvalid()
        {
            // Arrange
            string invalidUrl = "www.invalidurlentered.com";

            // Act
            var result = _sut.ValidateUrl(invalidUrl);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Please enter a valid URL.", result.message);
        }
    }
}
