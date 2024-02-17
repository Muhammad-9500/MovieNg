using Microsoft.AspNetCore.Mvc;
using MovieNg.Services;

namespace MovieNg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMovieRepository _movieRepository;
        public MovieController(IHttpClientFactory clientFactory, IMovieRepository movieRepository)
        {
            _clientFactory = clientFactory;
            _movieRepository = movieRepository;
        }

        // POST: api/movie?title=
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([FromQuery]string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest("Title is required.");
            }

            var searchResults = await _movieRepository.GetMovieByTitle(title);
            return Ok(searchResults);
        }

        [HttpGet]
        public async Task<IActionResult> GetData(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest("Title is required.");
            }

            try
            {
                var searchResults = await _movieRepository.GetMovieByTitle(title);
                return Ok(searchResults);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Movie not found.");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        
    }
}
