using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Entities;
using RestingMovies.Api.Persistence;

namespace RestingMovies.Api.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly MoviesDbContext _dbContext;

    public MovieRepository(MoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Movie>> GetAllMovies()
    {
        return await _dbContext.Movies.ToListAsync();
    }

    public async Task<Movie?> GetMovieById(int id)
    {
        return await _dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Movie>> GetMoviesByName(string name)
    {
        return await _dbContext.Movies.Where(x => x.Name.ToLower().Equals(name.ToLower())).ToListAsync();
    }

    public async Task SaveMovie(Movie movie)
    {
        await _dbContext.Movies.AddAsync(movie);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteMovie(Movie movie)
    {
        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync();
    }
}