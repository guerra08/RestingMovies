using RestingMovies.Api.Entities;

namespace RestingMovies.Api.Repositories;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllMovies();
    Task<Movie?> GetMovieById(int id);
    Task SaveMovie(Movie movie);
    Task<List<Movie>> GetMoviesByName(string name);
    Task DeleteMovie(Movie movie);
}