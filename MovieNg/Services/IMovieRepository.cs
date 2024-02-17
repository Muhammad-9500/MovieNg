using MovieNg.Models;

namespace MovieNg.Services
{
    public interface IMovieRepository
    {
        public Task<List<Movie>> GetMovieByTitle(string title);
    }
}
