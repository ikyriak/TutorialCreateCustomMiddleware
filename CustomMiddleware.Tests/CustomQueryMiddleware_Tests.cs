using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace CustomMiddleware.Tests
{
    public class CustomQueryMiddleware_Tests
    {
        public CustomQueryMiddleware_Tests()
        {

        }

        private Task<IHost> StartTestWebHostAsync()
        {
            return new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                    // Here we can add the services that are needed from the middleware, for example:
                    // services.AddMyServices();
                })
                        .Configure(app =>
                        {
                            app.UseCustomQueryMiddleware();
                        });
                })
                .StartAsync();
        }

        /// <summary>
        /// Scenario:
        /// The "culture" query parameter contains an invalid culture name.
        /// 
        /// Action:
        /// An CultureNotFoundException is thrown.
        /// </summary>
        [Fact]
        public async Task AnCultureNotFoundExceptionIsThrown_WhenCultureIsInvalid()
        {
            // Arrange
            string invalidCulture = "xx-XXX";
            using var host = await StartTestWebHostAsync();

            // Act:
            // Send a request using HttpClient
            var exception = await Assert.ThrowsAsync<CultureNotFoundException>(() => host.GetTestClient().GetAsync($"/?culture={invalidCulture}"));

            // Assert
            Assert.Equal("Culture is not supported. (Parameter 'name')\r\nxx-XXX is an invalid culture identifier.", exception.Message);
        }


        /// <summary>
        /// Scenario:
        /// The "culture" query parameter contains an empty string.
        /// 
        /// Action:
        /// An CultureNotFoundException is thrown.
        /// </summary>
        [Fact]
        public async Task AnCultureNotFoundExceptionIsThrown_WhenCultureIsEmpty()
        {
            // Arrange
            string emptyCulture = string.Empty;
            using var host = await StartTestWebHostAsync();

            // Act:
            // Send a request using HttpClient
            var exception = await Assert.ThrowsAsync<CultureNotFoundException>(() => host.GetTestClient().GetAsync($"/?culture={emptyCulture}"));

            // Assert
            Assert.Equal("The query parameter 'culture' has no value.", exception.Message);
        }
    }
}
