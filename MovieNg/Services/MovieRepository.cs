using MovieNg.Models;
using MovieNg.Utility;
using System.Text.Json;

namespace MovieNg.Services
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public OMDbSettings _omdbSettings { get; set; }

        public MovieRepository(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public async Task<List<Movie>> GetMovieByTitle(string title)
        {
            _omdbSettings = _configuration.GetSection("OMDb").Get<OMDbSettings>();

            var baseUrl = "http://www.omdbapi.com/";
            var apiUrl = $"{baseUrl}?t={title}&apikey={_omdbSettings.ApiKey}";

            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync(apiUrl);


            if (response.IsSuccessStatusCode)
            {
                // Read the content of the response as a stream
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    // Deserialize the JSON response into a JsonDocument
                    var jsonDocument = await JsonDocument.ParseAsync(stream);

                    // Extract the root JSON element
                    var root = jsonDocument.RootElement;

                    // Deserialize the JSON data into a single MovieDetails object
                    var movie = new Movie
                    {
                        Title = root.GetProperty("Title").GetString(),
                        Year = root.GetProperty("Year").GetString(),
                        Genre = root.GetProperty("Genre").GetString(),
                        Poster = root.GetProperty("Poster").GetString(),
                        ImdbRating = root.GetProperty("imdbRating").GetString(),
                        Type = root.GetProperty("Type").GetString(),
                        Language = root.GetProperty("Type").GetString(),
                        Released = root.GetProperty("Released").GetString(),
                        Runtime = root.GetProperty("Runtime").GetString(),
                        Country = root.GetProperty("Country").GetString(),
                        Director = root.GetProperty("Director").GetString(),

                    };

                    // Return a list containing the single MovieDetails object
                    return new List<Movie> { movie };
                }
            }
            else
            {
                // Handle the case where the request was not successful
                return null;
            }

        }
    }
}
