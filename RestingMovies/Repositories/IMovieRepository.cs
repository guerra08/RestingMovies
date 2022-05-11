using RestingMovies.Api.Entities;

namespace RestingMovies.Api.Repositories;

public interface IMovieRepository
{
    public Task<IEnumerable<Movie>> GetAllMovies();
    public Task<Movie?> GetMovieById(int id);
    public Task<IEnumerable<Movie>> GetMoviesByName(string name);
    public Task SaveMovie(Movie movie);
    public Task DeleteMovie(Movie movie);
}